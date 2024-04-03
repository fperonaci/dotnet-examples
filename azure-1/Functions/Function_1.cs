using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Functions;

public class Function_1
{
  [FunctionName("TimeForACupOfTea")]
  public void RunFunction_1(
    [TimerTrigger("*/10 * * * * *")] TimerInfo timerInfo,
    ILogger logger)
  {
    logger.LogInformation(
      $"Timer trigger function executed at: {DateTime.Now}");
  }
}
