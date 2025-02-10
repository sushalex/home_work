using Newtonsoft.Json;
using Shared.Data;
using RestSharp;
using System.Net;

using TravelApp.Data;
using TravelApp.Interfaces;
using TravelApp;


namespace TravelApi.Services
{
  public class HttpService : IHttpService
  {

    private readonly AppSettings _settings;
    private static ILogger _logger = LoggingConfiguration.GetLogger();
    public HttpService(AppSettings settings)
    {
      _settings = settings;
    }



    public async Task<TestData> GetData(string json)
    {
      TestData routesData = new TestData();

      RestClient restClient = new RestClient();

      RestRequest apirequest = new RestRequest(_settings.TravelApiUrl + "GetTravelDataForEstonia/", Method.Post);
      apirequest.AddParameter("application/json", json, ParameterType.RequestBody);
      apirequest.AddHeader("Content-Type", "application/json; charset=utf-8");


      RestResponse response = await restClient.ExecuteAsync(apirequest);

      _logger.LogInformation($"TravelApi response content: '{response.Content}'. Response status is '{(int)response.StatusCode}' ");

      if (response.StatusCode == HttpStatusCode.OK)
      {
        string path = Directory.GetCurrentDirectory() + "\\test.json";

        try
        {
          using StreamReader reader = new(path);
          string tempJson = reader.ReadToEnd();

          if (!string.IsNullOrEmpty(json))
          {
            routesData = JsonConvert.DeserializeObject<TestData>(tempJson);
          }
        }
        catch(Exception ex){
          string exc = ex.Message;
        }
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

        return Convert.ToHexString(hashBytes); // .NET 5 +

        // Convert the byte array to hexadecimal string prior to .NET 5
        // StringBuilder sb = new System.Text.StringBuilder();
        // for (int i = 0; i < hashBytes.Length; i++)
        // {
        //     sb.Append(hashBytes[i].ToString("X2"));
        // }
        // return sb.ToString();
      }
    }
  }
}
