# Wpf Sample

## NuGet Package

Install **Microsoft.Extensions.Hosting**

## AppSettings

Create a **json file** "appsettings.json" and define this file **as content in property window**

Sample:

```xml
{
  "AppSettings": {
    "MySetting": "My Value"
  }
}
```
Create a **Model**:

```cs
public class AppSettings
{
    public string MySetting { get; set; }
}
```

**Configuration**:

```cs
host = Host.CreateDefaultBuilder()
.ConfigureAppConfiguration((context, builder) =>
{
   // here
    builder.AddJsonFile("appsettings.local.json", true); 
})
.ConfigureServices((context, services) =>
{
    ConfigureContainer();
    RegisterServices(context.Configuration, services);
})
.ConfigureLogging(logging =>
{

})
.Build();
```

And 

```cs
services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
```


## Logging

Install **Microsoft.Extensions.Logging** and **Microsoft.Extensions.Logging.Debug** for example


```cs
host = Host.CreateDefaultBuilder()
.ConfigureAppConfiguration((context, builder) =>
{
   // here
    builder.AddJsonFile("appsettings.local.json", true); 
})
.ConfigureServices((context, services) =>
{
    ConfigureContainer();
    RegisterServices(context.Configuration, services);
})
.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddDebug();
})
.Build();
```

Inject and use the logger

```cs
public class ServiceA : IService
{
    private readonly ILogger<ServiceA> logger;

    public ServiceA(ILogger<ServiceA> logger)
    {
        this.logger = logger;
    }

    public string GetTime()
    {
        logger.LogInformation($"ServiceA GetTime used, Timestamp:{DateTime.Now:u}");

        return DateTime.Now.ToLongTimeString();
    }
}
```