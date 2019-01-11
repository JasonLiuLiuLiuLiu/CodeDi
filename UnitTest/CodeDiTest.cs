using System;
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
        private CoderDiOptions _options;

        [SetUp]
        public void Setup()
        {
            _serviceCollection = new ServiceCollection();
            _options = new CoderDiOptions();
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

        private Type[] GetService<T>()
        {
            var serviceDescriptor = new ServiceDescriptor[_serviceCollection.Count];
            _serviceCollection.CopyTo(serviceDescriptor, 0);
            return serviceDescriptor.Where(u => u.ServiceType == typeof(T)).Select(u => u.ImplementationType).ToArray();
        }
    }
}