using Microsoft.Extensions.Logging;

namespace Banking
{
    public class TestRunService
    {
        private readonly ILogger<TestRunService> _logger;

        public TestRunService(ILogger<TestRunService> logger)
        {
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("Start Test Run Service");


            _logger.LogInformation("End Test Run Service");
        }
    }
}
