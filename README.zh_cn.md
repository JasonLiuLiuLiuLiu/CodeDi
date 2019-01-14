# CodeDi 　　　　　　　　　　　　　　　　　　　　　　[English](https://github.com/liuzhenyulive/CodeDi/blob/master/README.md)
[![Build status](https://ci.appveyor.com/api/projects/status/eeo8aua4n8r5fnce?svg=true)](https://ci.appveyor.com/project/liuzhenyulive/codedi)
[![NuGet](https://img.shields.io/badge/nuget-1.0.1-blue.svg)](https://www.nuget.org/packages/CodeDI/)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/liuzhenyulive/codedi/master/LICENSE)

CodeDi是一个基于 .Net Standard的工具库,它能帮助我们自动地在Asp .net core或者 .net core项目中完成服务的注册.

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

CodeDi 是 Code Dependency Injection的意思,不知道您是否遇到和我一样的问题,在系统中有大量的接口对应的实现需要注册到ServiceCollection中,或者某个接口有多个实现,我们在构造函数中获取某个实现不是那么方便.在上次我在看了由依乐祝写的[<.NET Core中的一个接口多种实现的依赖注入与动态选择看这篇就够了>](https://www.cnblogs.com/yilezhu/p/10236163.html ".NET Core中的一个接口多种实现的依赖注入与动态选择看这篇就够了")后,我想如果写一个工具帮助我们自动地完成服务的注册,以及把服务获取方法做一个封装,不是很好?所以这个工具就诞生了.



## Getting Started

### 安装Nuget包

CodeDi的Nuget包已经发布到了nuget.org,您可以通过以下指令在您的项目中安装CodeDi

```
PM> Install-Package CodeDi
```
### ConfigureServices中的配置
#### 方法 1
您可以在`Startuo`的`ConfigureService`方法中添加AddCodeDi完成对CodeDi的调用.服务注册的人恶恶CodeDi会自动为您完成.
```
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCoreDi();
            services.AddMvc();
        }
```
#### 方法 2
您也可以在AddCodeDi方法中传入一个`Action<CodeDiOptions>`参数,在这个action中,您可以对CodeDiOptions的属性进行配置.
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
#### 方法 3
当然您也可以直接给`AddCodeDi()`方法直接传入一个`CodeDiOptions`实例.
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
你也可以在`appsetting.json`文件中配置`CodeDiOptions`的信息,并通过`Configuration.Bind("CodeDiOptions", options)`把配置信息绑定到一个`CodeDiOptions`实例.

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
ConfigureService方法
```
        public void ConfigureServices(IServiceCollection services)
        {
            var options=new CodeDiOptions();
            Configuration.Bind("CodeDiOptions", options);
            services.AddCoreDi(options);
            services.AddMvc();
        }
```

### CodeDiOptions详解
| 属性名称  | 属性描述  | 数据类型 | 默认值  |
| :------------: | :------------: | :------------: | :------------: |
| AssemblyPaths  | 在指定目录下加载Dll程序集  |  string[] | Bin目录  |
| AssemblyNames  | 选择要加载的程序集名称 (支持通配符)  | string[]  |  * |
| IgnoreAssemblies | 忽略的程序集名称 (支持通配符)  |   string[]|  null |
| IncludeSystemAssemblies  |  是否包含系统程序集(当为false时,会忽略含有System,Microsoft,CppCodeProvider,WebMatrix,SMDiagnostics,Newtonsoft关键词和在App_Web,App_global目录下的程序集) |  bool | false  |
| IgnoreInterface  |  忽略的接口 (支持通配符) | string[]  |  null |
| InterfaceMappings  | 接口对应的服务 (支持通配符) ,当一个接口有多个实现时,如果不进行配置,则多个实现都会注册到SerciceCollection中 | Dictionary<string, string>  |  null |
| DefaultServiceLifetime  |  默认的服务生命周期 | ServuceLifetime( Singleton,Scoped,Transient)  |  ServiceLifetime.Scope |
| ServiceLifeTimeMappings  | 指定某个接口的服务生命周期,不指定为默认的生命周期  |  Dictionary<string, ServiceLifetime> | null  |



如果 `ISay` 接口有`SayInChinese` 和`SayInEnglish` 两个实现,我们只想把SayInEnglish注册到`ServiceCollection`中

```
 public interface ISay
    {
        string Hello();
    }

      public class SayInChinese:ISay
    {
        public string Hello()
        {
            return "ä½ å¥½";
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
那么我们可以这样配置`InterfaceMappings`.

options.InterfaceMappings=new Dictionary<string, string>(){{ "*Say", "*Chinese" } }

如果我们希望ISay接口的服务的生命周期为`Singleton`,我们可以这样配置`ServiceLifeTimeMappings`.

options.ServiceLifeTimeMappings = new Dictionary<string, ServiceLifetime>(){{"*Say",ServiceLifetime.Singleton}};

关于ServiceLifetime: https://github.com/aspnet/DependencyInjection/blob/master/src/DI.Abstractions/ServiceLifetime.cs



### 获取服务实例
当然, 您可以和之前一样,直接在构造函数中进行服务的注册,但是当某个接口有多个实现而且都注册到了ServiceCollection中,获取就没有那么方便了,您可以用`ICodeDiServiceProvider` 来帮助您获取服务实例.


例如,当 `ISay` 接口有 `SayInChinese` 和 `SayInEnglish`两个实现, 我们我们如何获取我们想要的服务实例呢?

```
 public interface ISay
    {
        string Hello();
    }

      public class SayInChinese:ISay
    {
        public string Hello()
        {
            return "您好";
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
参数中的Name支持通配符.

### Engoy it!

加入了CodeDi后,当系统中添加了新的接口以及对应的服务实现后,我们只要进行一次配置,就不用再去一个个地Add到ServiceCollection中了,快到您的项目中试试吧!


### License

[MIT](https://raw.githubusercontent.com/liuzhenyulive/codedi/master/LICENSE)
