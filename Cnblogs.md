###  为什么做这么一个工具
因为我们的系统往往时面向接口编程的,所以在开发Asp .net core项目的时候,一定会有大量大接口及其对应的实现要在`ConfigureService`方法中添加到`ServiceCollection`中,传统的做法是加了一个服务,我们就要注册一次(`service.AddService()`),又比如,当一个接口有多个实现,在构造函数中获取服务也不是很友好,而据我所知, .Net Core目前是没有什么自带的库或者方法解决这些问题,当然,如果引入第三方容器如AutoFac这些问题时能迎刃而解的,但是如何在不引入第三方容器来解决这个问题呢?
所以我就设计了这样的一个轻量级工具.

首先,放上该项目的Github地址(记得Star哦!!)

https://github.com/liuzhenyulive/CodeDi

[![Build status](https://ci.appveyor.com/api/projects/status/eeo8aua4n8r5fnce?svg=true)](https://ci.appveyor.com/project/liuzhenyulive/codedi)
[![NuGet](https://img.shields.io/badge/nuget-1.0.1-blue.svg)](https://www.nuget.org/packages/CodeDI/)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/liuzhenyulive/codedi/master/LICENSE)

CodeDi是一个基于 .Net Standard的工具库,它能帮助我们自动地在Asp .net core或者 .net core项目中完成服务的注册.


## Overview

CodeDi 是 Code Dependency Injection的意思,在上次我在看了由依乐祝写的[<.NET Core中的一个接口多种实现的依赖注入与动态选择看这篇就够了>](https://www.cnblogs.com/yilezhu/p/10236163.html ".NET Core中的一个接口多种实现的依赖注入与动态选择看这篇就够了")后,回想起我之前遇到的那些问题,感觉拨云见日,所以,我就开始着手写这个工具了.



## 如何使用CodeDi

### 安装Nuget包

CodeDi的Nuget包已经发布到了 nuget.org,您可以通过以下指令在您的项目中安装CodeDi

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


#### InterfaceMappings

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

#### ServiceLifeTimeMappings

如果我们希望ISay接口的服务的生命周期为`Singleton`,我们可以这样配置`ServiceLifeTimeMappings`.

options.ServiceLifeTimeMappings = new Dictionary<string, ServiceLifetime>(){{"*Say",ServiceLifetime.Singleton}};

关于ServiceLifetime: https://github.com/aspnet/DependencyInjection/blob/master/src/DI.Abstractions/ServiceLifetime.cs



### 获取服务实例

当然, 您可以和之前一样,直接在构造函数中进行依赖的注入,但是当某个接口有多个实现而且都注册到了ServiceCollection中,获取就没有那么方便了,您可以用`ICodeDiServiceProvider` 来帮助您获取服务实例.


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

### 如何实现
既然时一个轻量级工具,那么实现起来自然不会太复杂,我来说说比较核心的代码.

```
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
```
GetInterfaceMapping通过反射机制,首先获取程序集中的所有接口`allInterfaces`,然后编辑`allInterfaces`找到该接口对应的实现,最终,该方法返回接口和实现的匹配关系,为Dictionary<Type, List<Type>>类型的数据.

```
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
```

该方法要判断CodeDiOptions中是否忽略了该接口,同时,是否指定实现映射关系.
什么叫实现映射关系呢?参见[InterfaceMappings](#interfacemappings)
如果指定了,那么就按指定的来实现,如果没指定,就会把每个实现都注册到ServiceCollection中.

```
        private readonly IServiceCollection _service;
        private readonly CodeDiOptions _options;
        private readonly ServiceDescriptor[] _addedService;

        public CodeDiService(IServiceCollection service, CodeDiOptions options)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _options = options ?? new CodeDiOptions();
            _addedService = new ServiceDescriptor[service.Count];
            service.CopyTo(_addedService, 0);
            //在构造函数中,我们通过这种方式把Service中已经添加的服务读取出来
            //后面进行服务注册时,会进行判断,避免重复添加
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
```
AddToService中,要判断有没有对接口的生命周期进行配置,参见[ServiceLifeTimeMappings](#servicelifetimemappings),如果没有配置,就按DefaultServiceLifetime进行配置,DefaultServiceLifetime如果没有修改的情况下时ServiceLifetime.Scoped,即每个Request创建一个实例.

### Enjoy it

只要进行一次简单的CodeDi配置,以后系统中添加了新的接口以及对应的服务实现后,就不用再去一个个地Add到IServiceCollection中了.

如果有问题,欢迎Issue,欢迎PR.
最后,赏个Star呗! [前往Star](https://github.com/liuzhenyulive/CodeDi) 

 