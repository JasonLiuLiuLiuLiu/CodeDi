# CodeDi
[![Build status](https://ci.appveyor.com/api/projects/status/eeo8aua4n8r5fnce?svg=true)](https://ci.appveyor.com/project/liuzhenyulive/codedi)
[![NuGet](https://img.shields.io/badge/nuget-1.0.1-blue.svg)](https://www.nuget.org/packages/CodeDI/)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/liuzhenyulive/codedi/master/LICENSE)

CodeDi is a tool help us add service to service collection in Asp.net core or .net core project.

## Overview

CodeDi means .Net Core Dependency Injection .I design this tool to help us auto-registration Interface and its corresponding implementation to service collection.



## Getting Started

### Install NuGet Package

You can run the following command to install the CodeDi in your project.

```
PM> Install-Package CodeDi
```
### Add CodeDi to ConfigureServices
Call the AddCodeDi method in the ConfigureService method of Startup to register the corresponding implementation of the interface in the system to the ServiceCollection.
```
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCoreDi();
            services.AddMvc();
        }
```
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


### License

[MIT](https://raw.githubusercontent.com/liuzhenyulive/codedi/master/LICENSE)
