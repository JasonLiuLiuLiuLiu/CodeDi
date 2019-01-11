using System;
using System.Collections.Generic;
using System.Linq;
using CodeDi;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using UnitTest.TestService;

namespace UnitTest
{
    [TestFixture]
    public class CodeDiTest
    {
        private IServiceCollection _serviceCollection;

        [SetUp]
        public void Setup()
        {
            _serviceCollection = new ServiceCollection();
        }

        [Test]
        public void AssemblyNamesTest()
        {
            _serviceCollection.AddCoreDi(options => { options.AssemblyNames = new[] { "*CodeDi*" }; });
            var servieTypes = GetService<SayInEnglish>();
            Assert.AreEqual(servieTypes.Length, 0);
        }
        [Test]
        public void AssemblyNamesTest2()
        {
            _serviceCollection.AddCoreDi(options => { options.AssemblyNames = new[] { "*Test" }; });
            var servieTypes = GetService<ISay>();
            Assert.AreNotEqual(servieTypes.Length, 0);
        }
        [Test]
        public void IgnoreAssembliesTest()
        {
            _serviceCollection.AddCoreDi(options => { options.IgnoreAssemblies = new[] { "*CodeDi" }; });
            var serviceTypes = GetService<ICodeDiServiceProvider>();
            Assert.AreEqual(serviceTypes.Length, 0);
        }

        [Test]
        public void IgnoreAssembliesTest2()
        {
            _serviceCollection.AddCoreDi(options => { options.IgnoreAssemblies = new[] { "*Test" }; });
            var serviceTypes = GetService<ICodeDiServiceProvider>();
            Assert.AreNotEqual(serviceTypes.Length, 0);
        }
        [Test]
        public void IncludeSystemAssembliesTest()
        {
            _serviceCollection.AddCoreDi(options => { options.IncludeSystemAssemblies = true; });
            var serviceTypes = GetService<IServiceCollection>();
            Assert.AreEqual(serviceTypes.Length, 1);
        }

        [Test]
        public void IncludeSystemAssembliesTest2()
        {
            _serviceCollection.AddCoreDi();
            var serviceTypes = GetService<IServiceCollection>();
            Assert.AreEqual(serviceTypes.Length, 0);
        }
        [Test]
        public void IgnoreInterfaceTest()
        {
            _serviceCollection.AddCoreDi(options => { options.IgnoreInterface = new[] {"*Say"};});
            var serviceTypes = GetService<ISay>();
            Assert.AreEqual(serviceTypes.Length, 0);
        }
        [Test]
        public void IgnoreInterfaceTest2()
        {
            _serviceCollection.AddCoreDi( );
            var serviceTypes = GetService<ISay>();
            Assert.AreEqual(serviceTypes.Length, 2);
        }
        [Test]
        public void InterfaceMappingsTest()
        {
            _serviceCollection.AddCoreDi(options => { options.InterfaceMappings=new Dictionary<string, string>(){{ "*Say", "*English" } }; });
            var serviceTypes = GetService<ISay>();
            Assert.AreEqual(serviceTypes.Length, 1);
        }

        [Test]
        public void DefaultServiceLifetimeTest()
        {
            _serviceCollection.AddCoreDi(options => { options.DefaultServiceLifetime = ServiceLifetime.Transient; });
            var service = GetServiceLifeTime<ISay>();
            Assert.AreEqual(service, ServiceLifetime.Transient);
        }

        [Test]
        public void DefaultServiceLifetimeTest2()
        {
            _serviceCollection.AddCoreDi();
            var service = GetServiceLifeTime<ISay>();
            Assert.AreEqual(service, ServiceLifetime.Scoped);
        }

        [Test]
        public void ServiceLifeTimeMappingsTest()
        {
            _serviceCollection.AddCoreDi(options => { options.ServiceLifeTimeMappings = new Dictionary<string, ServiceLifetime>(){{"*Say",ServiceLifetime.Singleton}}; });
            var service = GetServiceLifeTime<ISay>();
            Assert.AreEqual(service, ServiceLifetime.Singleton);
        }


        private Type[] GetService<T>()
        {
            var serviceDescriptor = new ServiceDescriptor[_serviceCollection.Count];
            _serviceCollection.CopyTo(serviceDescriptor, 0);
            return serviceDescriptor.Where(u => u.ServiceType == typeof(T)).Select(u => u.ImplementationType).ToArray();
        }

        private ServiceLifetime? GetServiceLifeTime<T>()
        {
            var serviceDescriptor = new ServiceDescriptor[_serviceCollection.Count];
            _serviceCollection.CopyTo(serviceDescriptor, 0);
            return serviceDescriptor.FirstOrDefault(u => u.ServiceType == typeof(T))?.Lifetime;
        }
    }
}