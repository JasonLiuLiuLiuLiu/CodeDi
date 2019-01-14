﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using CodeDi;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDi
{
    class CodeDiService
    {
        private readonly IServiceCollection _service;
        private readonly CodeDiOptions _options;
        private readonly ServiceDescriptor[] _addedService;

        public CodeDiService(IServiceCollection service, CodeDiOptions options)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _options = options ?? new CodeDiOptions();
            _addedService = new ServiceDescriptor[service.Count];
            service.CopyTo(_addedService, 0);
        }

        public IServiceCollection AddService()
        {
            var interfaceMappings = GetInterfaceMapping(AssemblyLoader.LoadAssembly(_options));

            AddToService(interfaceMappings);

            return _service;
        }

        private void AddToService(Dictionary<Type, List<Type>> interfaceMappings)
        {
            foreach (var mapping in interfaceMappings)
            {
                if (mapping.Key.FullName == null || (_options.IgnoreInterface != null &&
                   _options.IgnoreInterface.Any(i => mapping.Key.FullName.Matches(i))))
                    continue;

                if (mapping.Key.FullName != null && _options.InterfaceMappings != null &&
                    _options.InterfaceMappings.Any(u => mapping.Key.FullName.Matches(u.Key)))
                {
                    foreach (var item in mapping.Value.Where(value => value.FullName != null).
                        Where(value => value.FullName.Matches(_options.InterfaceMappings.FirstOrDefault(u => mapping.Key.FullName.Matches(u.Key)).Value)))
                    {
                        AddToService(mapping.Key, item);
                    }
                    continue;
                }

                foreach (var item in mapping.Value)
                {
                    AddToService(mapping.Key, item);
                }
            }
        }

        private void AddToService(Type serviceType, Type implementationType)
        {
            ServiceLifetime serviceLifetime;
            try
            {
                serviceLifetime = _options.DefaultServiceLifetime;
                if (_options.ServiceLifeTimeMappings != null && serviceType.FullName != null)
                {
                    var lifeTimeMapping =
                        _options.ServiceLifeTimeMappings.FirstOrDefault(u => serviceType.FullName.Matches(u.Key));

                    serviceLifetime = lifeTimeMapping.Key != null ? lifeTimeMapping.Value : _options.DefaultServiceLifetime;

                }
            }
            catch
            {
                throw new Exception("Service Life Time Only Can be set in range of 0-2");
            }

            if (_addedService.Where(u => u.ServiceType == serviceType).Any(u => u.ImplementationType == implementationType))
                return;
            _service.Add(new ServiceDescriptor(serviceType, implementationType, serviceLifetime));
        }



        private Dictionary<Type, List<Type>> GetInterfaceMapping(IList<Assembly> assemblies)
        {
            var mappings = new Dictionary<Type, List<Type>>();
            var allInterfaces = assemblies.SelectMany(u => u.GetTypes()).Where(u => u.IsInterface);
            foreach (var @interface in allInterfaces)
            {
                mappings.Add(@interface, assemblies.SelectMany(a =>
                        a.GetTypes().
                            Where(t =>
                                t.GetInterfaces().Contains(@interface)
                            )
                    )
                    .ToList());
            }
            return mappings;
        }
    }
}
