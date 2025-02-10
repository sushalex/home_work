using TravelApi.Data;
using TravelApi.Interfaces;
using TravelApi.Services;

namespace TravelApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddControllers();
      builder.Services.AddHttpClient();
      builder.Services.AddDistributedMemoryCache();
      builder.Services.AddHttpContextAccessor();

      // ---------------
      ApiSettings apiSettings = new ApiSettings();
      builder.Configuration.Bind("ApiSettings", apiSettings);

      builder.Services.ConfigureLogging(builder.Configuration);
      builder.Services.AddHttpClient();
      builder.Services.AddSingleton(apiSettings);
      builder.Services.AddSingleton<IHttpService, HttpService>();
      // -----
      var app = builder.Build();

      // Configure the HTTP request pipeline.

      app.UseHttpsRedirection();
      app.UseAuthorization();

      app.MapControllers();

      app.Run();
    }
  }
}
