using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Models;
using CodeDi;
using Microsoft.Extensions.Configuration;
using Sample.Service;

namespace Sample.Controllers
{
    public class HomeController : Controller
    {
        private ICodeDiServiceProvider _serviceProvider;

        public HomeController(ICodeDiServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string Index()
        {
            var say=_serviceProvider.GetService<ISay>("*English");
            return say.Hello();
        }
    }
}
