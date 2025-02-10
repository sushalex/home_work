using Shared.Data;
using static Shared.Data.Enumerations;

namespace TravelApi.Interfaces
{
  public interface IHttpService
  {
    Task<TestData> GetData(Country country);
  }
}
