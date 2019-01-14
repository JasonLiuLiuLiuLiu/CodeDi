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
        private readonly ISay _say;

        public HomeController(ICodeDiServiceProvider serviceProvider)
        {
            _say = serviceProvider.GetService<ISay>("*Chinese");
        }

        public string Index()
        {
            return _say.Hello();
        }
    }
}
