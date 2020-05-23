using Microsoft.Extensions.Logging;
using System;

namespace WpfSample.Services
{
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
}
