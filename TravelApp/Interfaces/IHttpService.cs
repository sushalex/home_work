using TravelApi.Data;
using Shared.Data;

namespace TravelApp.Interfaces
{
  public interface IHttpService
  {
    Task<TestData> GetData(string json);
  }
}
