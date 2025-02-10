using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Data;
using System.Reflection.Metadata;
using System.Text;
using TravelApi.Interfaces;
using static Shared.Data.Enumerations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TravelApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TravelApiController : ControllerBase
{
  private static readonly string[] Summaries = new[]
  {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

  private readonly ILogger<TravelApiController> _logger;
  private IHttpService _httpService;

  public TravelApiController(ILogger<TravelApiController> logger, IHttpService httpService)
  {
    _logger = logger;
    _httpService = httpService;
  }

  //[HttpGet]
  //public IEnumerable<WeatherForecast> Get()
  //{
  //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
  //    {
  //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
  //        TemperatureC = Random.Shared.Next(-20, 55),
  //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
  //    })
  //    .ToArray();
  //}

  [HttpPost]
  public async Task<IActionResult> GetTravelDataByCountry()
  {
    _logger.LogInformation($"GetTravelDataByCountry. Request IP: {Request.HttpContext.Connection.RemoteIpAddress.ToString()}.");       
    TestData data = null;
    Country cType = Country.NotDefined;

    try
    {
      //Get JSON from POST 
      using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
      {
        string requestJson = await reader.ReadToEndAsync();

        ApiRequest requestObj = JsonConvert.DeserializeObject<ApiRequest>(requestJson);

        Enum.TryParse(requestObj.Country, true, out cType);
      }
      data = new TestData();
      data = await _httpService.GetData(cType);
    }
    catch (Exception ex)
    {
      _logger.LogError($"GetRisk throw exeption '{ex.Message}'");
    }

    return new ContentResult
    {
      ContentType = "application/json",
      Content = JsonConvert.SerializeObject(data),
      StatusCode = 200
    };
  }
}
