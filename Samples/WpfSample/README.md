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
