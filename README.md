# CodeDi
[![Build status](https://ci.appveyor.com/api/projects/status/eeo8aua4n8r5fnce?svg=true)](https://ci.appveyor.com/project/liuzhenyulive/codedi)
[![NuGet](https://img.shields.io/badge/nuget-1.0.1-blue.svg)](https://www.nuget.org/packages/CodeDI/)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/liuzhenyulive/codedi/master/LICENSE)

CodeDi is a tool help us add service to service collection in Asp.net core or .net core project.

- [CodeDi](#codedi)
- [Overview](#overview)
- [Getting Started](#getting-started)
  - [Install NuGet Package](#install-nuget-package)
  - [Add CodeDi to ConfigureServices](#add-codedi-to-configureservices)
    - [Options 1](#options-1)
    - [Options 2](#options-2)
    - [Options 3](#options-3)
  - [CodeDiOptions](#codedioptions)
- [License](#license)



## Overview

CodeDi means Code Dependency Injection.Are you as many as I have before the system need to manually add to the ServiceCollection one by one, or multiple implementations of an interface in the system is not convenient to get the service instance in the constructor? I design this tool to help us auto-registration Interface and its corresponding implementation to service collection and make it is easy to get a service instance with multiple implementations of an interface.



## Getting Started

### Install NuGet Package

You can run the following command to install the CodeDi in your project.

```
PM> Install-Package CodeDi
```
### Add CodeDi to ConfigureServices
#### Options 1
Call the AddCodeDi method in the ConfigureService method of Startup to register the corresponding implementation of the interface in the system to the ServiceCollection.
```
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCoreDi();
            services.AddMvc();
        }
```
#### Options 2
You can also call the AddCodeDi method with the Action<CodeDiOptions> parameter, which you can set value to the CodeDiOptions property in this action.
```
       public void ConfigureServices(IServiceCollection services)
        {
            services.AddCoreDi(options =>
            {
                options.DefaultServiceLifetime = ServiceLifetime.Scoped;

            });
            services.AddMvc();
        }
```
#### Options 3
Of course, you can also pass a CodeDiOptions parameter to AddCodeDi as follows.
```
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCoreDi(new CodeDiOptions()
            {
                DefaultServiceLifetime = ServiceLifetime.Scoped
            });
            services.AddMvc();
        }
```
You can also configure the Options information into the appsettings.json file and then bind the data to the CodeDiOptions parameter.

appsetting.json file
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "CodeDiOptions": {
    "DefaultServiceLifetime": 1,
    "AssemblyNames": [
      "*CodeDi"
    ],
    "AssemblyPaths": [
      "C:\\MyBox\\Github\\CodeDI\\CodeDI\\bin\\Debug\\netstandard2.0"
    ],
    "IgnoreAssemblies": [
      "*Test"
    ],
    "IncludeSystemAssemblies": false,
    "IgnoreInterface": [
      "*Say"
    ],
    "InterfaceMappings": {
      "*Say": "*English"
    },
    "ServiceLifeTimeMappings": {
      "*Say": 0
    }
  }
}

```
ConfigureService method
```
        public void ConfigureServices(IServiceCollection services)
        {
            var options=new CodeDiOptions();
            Configuration.Bind("CodeDiOptions", options);
            services.AddCoreDi(options);
            services.AddMvc();
        }
```

### CodeDiOptions
| Attribute name  | Attribute Description  | Data Type |Default Value  |
| :------------: | :------------: | :------------: | :------------: |
| AssemblyPaths  | Load dll under the specified path  |  string[] | Bin directory  |
| AssemblyNames  | Load the specified assembly (supports wildcards)  | string[]  |  * |
| IgnoreAssemblies | Ignore the specified assembly (wildcards are supported)  |   string[]|  null |
| IncludeSystemAssemblies  |  Whether to include system assemblies |  bool | false  |
| IgnoreInterface  |  Ignore the specified Interface (wildcards are supported) | string[]  |  null |
| InterfaceMappings  | Interface and match to the corresponding implementation (support wildcards)  | Dictionary<string, string>  |  null |
| DefaultServiceLifetime  |  Default service life time | ServuceLifetime( Singleton,Scoped,Transient)  |  ServiceLifetime.Scope |
| ServiceLifeTimeMappings  | Specify the life time of a particular interface  |  Dictionary<string, ServiceLifetime> | null  |

ServiceLifetime: https://github.com/aspnet/DependencyInjection/blob/master/src/DI.Abstractions/ServiceLifetime.cs

### Get Service
Of course, you can do the injection of dependencies in the constructor as before. But when I have multiple implementations of an interface, we may not be well handled in the constructor. I designed an `ICodeDiServiceProvider` interface. Help you get a service instance.

If the `ISay` interface has `SayInChinese` and `SayInEnglish` two implementations, and both implementations are registered in the ServiceCollection, how do we get the service instance we want?

```
 public interface ISay
    {
        string Hello();
    }

      public class SayInChinese:ISay
    {
        public string Hello()
        {
            return "你好";
        }
    }

        public class SayInEnglish:ISay
    {
        public string Hello()
        {
            return "Hello";
        }
    }

```

```
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

```
`ICodeDiServiceProvider.GetService<T>(string name=null)`
The Name supports wildcards,


### License

[MIT](https://raw.githubusercontent.com/liuzhenyulive/codedi/master/LICENSE)
