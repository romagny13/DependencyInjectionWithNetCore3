Install NuGet Package:
* Microsoft.Extensions.Hosting


AppSettings:

Create json file "appsettings.json", define as content in property window

Define settings

{
  "AppSettings": {
    "MySetting": "My Value"
  }
}


Create AppSettings model

public class AppSettings
{
    public string MySetting { get; set; }
}


Configure settings

host = Host.CreateDefaultBuilder()
.ConfigureAppConfiguration((context, builder) =>
{
    builder.AddJsonFile("appsettings.local.json", true); // <=
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

+

services.Configure<AppSettings>(configuration.GetSection("AppSettings"));



Logging

Install 
* Microsoft.Extensions.Logging.Console  for example