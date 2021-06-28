using System.Collections.Generic;
using Newtonsoft.Json;

namespace API.Utils
{
  public static class ConvertDictToObj<T>
  {
    public static T Convert(object obj)
    {
      var objSerialize = JsonConvert.SerializeObject(obj);
      var convertedObj = JsonConvert.DeserializeObject<T>(objSerialize);

      return convertedObj;
    }
  }
}