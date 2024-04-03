using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Functions;

public class Function_2
{
  [FunctionName("GetMeSomeCoffee")]
  public IActionResult RunFunction_2(
    [HttpTrigger(AuthorizationLevel.Anonymous,
                 "get",
                 "post",
                 Route = null)] HttpRequest httpRequest,
    ILogger logger)
  {
    logger.LogInformation(
      $"Http trigger function executed at: {DateTime.Now}");

    return new OkObjectResult("Here is a cup of coffee");
  }
}
