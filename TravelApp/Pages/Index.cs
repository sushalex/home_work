using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using Shared.Data;
using TravelApp.Data;

namespace TravelApp.Pages
{
  public partial class Index
  {
    private static readonly ILogger _logger = LoggingConfiguration.GetLogger();

    [Inject]
    IConfiguration Configuration { get; set; }

    [Inject]
    RestClient RestClient { get; set; }

    private AppSettings _settings;
    private TestData _tData;

    private bool _showTable;

    protected override async Task OnInitializedAsync()
    {
      _settings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
      _tData = new TestData();
    }


    private async Task GetData(string cntCode)
    {
      if (!string.IsNullOrEmpty(cntCode))
      {

        ApiRequest request = new ApiRequest() { Country = cntCode };
        string requestJson = JsonConvert.SerializeObject(request);

        RestRequest apiRequest = new RestRequest(_settings.TravelApiUrl + "GetTravelDataByCountry/", Method.Post);
        apiRequest.AddParameter("application/json", requestJson, ParameterType.RequestBody);
        apiRequest.AddHeader("Content-Type", "application/json; charset=utf-8");

        RestResponse apiresponse = await RestClient.ExecuteAsync(apiRequest);

        _logger.LogInformation($"Request from GetRisk. Response status '{apiresponse.StatusCode}' and content '{apiresponse.Content}'");
        if (apiresponse != null && !string.IsNullOrEmpty(apiresponse.Content))
        {
          _tData = JsonConvert.DeserializeObject<TestData>(apiresponse.Content);
          _showTable = true;


          StateHasChanged();
        }
      }
    }
  }
}
