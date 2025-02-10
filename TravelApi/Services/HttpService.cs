using Newtonsoft.Json;
using Shared.Data;
using Shared.Data;
using RestSharp;
using System.ComponentModel.Design;
using System.Net;
using System.Runtime;
using TravelApi.Data;
using TravelApi.Interfaces;
using static Shared.Data.Enumerations;

namespace TravelApi.Services
{
  public class HttpService : IHttpService
  {

    private readonly ApiSettings _settings;
    private static ILogger _logger = LoggingConfiguration.GetLogger();
    public HttpService(ApiSettings settings)
    {
      _settings = settings;
    }



    public async Task<TestData> GetData(Country country)
    {
      TestData routesData = new TestData();

      RestClient restClient = new RestClient();

      //It is no clear about request, is it POST or Get, how send eesnimi ja pass ...  Mikk does not know
      RestRequest request = new RestRequest(_settings.NovaterData.Url, Method.Post);

      _settings.NovaterData.Pass = CreateMD5(_settings.NovaterData.Login);

      // Fake request while no infor about rules of request. Hearder??? 
      request.AddParameter("loginid", _settings.NovaterData.Login);
      request.AddParameter("passwd", _settings.NovaterData.Pass);

      RestResponse response = await restClient.ExecuteAsync(request);

      _logger.LogInformation($"Novater response content: '{response.Content}'. Response status is '{(int)response.StatusCode}' ");

      // If response bad - just for test take data from file
      if (response.StatusCode != HttpStatusCode.OK)
      {
        string path = Directory.GetCurrentDirectory() + "\\test.json";

        try
        {
          using StreamReader reader = new(path);
          string json = reader.ReadToEnd();

          if (!string.IsNullOrEmpty(json))
          {
            routesData = JsonConvert.DeserializeObject<TestData>(json);
          }
        }
        catch (Exception ex)
        {
          string exc = ex.Message;
        }
      }
      else
      {
        routesData = JsonConvert.DeserializeObject<TestData>(response.Content);
      }

      return routesData;
    }


    public string CreateMD5(string input)
    {
      // Use input string to calculate MD5 hash
      using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
      {
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes); 

      }
    }
  }
}
