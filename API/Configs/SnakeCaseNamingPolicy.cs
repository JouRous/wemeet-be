using System.Text.Json;
using API.Extensions;

namespace API.Configs
{
  public class SnakeCaseNamingPolicy : JsonNamingPolicy
  {
    public override string ConvertName(string name) => name.ToSnakeCase();
  }

}