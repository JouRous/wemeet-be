using System.Collections.Generic;
using API.Types;

namespace API.Utils
{
  public static class QueryBuilder<T>
  {
    public static Query<T> Build(object paginationParams, object filter, object sort)
    {
      var s = (Dictionary<string, string>)sort;
      return new Query<T>
      {
        paginationParams = ConvertDictToObj<PaginationParams>.Convert(paginationParams),
        filter = ConvertDictToObj<T>.Convert(filter),
        sort = s.GetValueOrDefault("sort")
      };
    }
  }
}