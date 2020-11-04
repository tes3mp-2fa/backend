using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq.Expressions;
using System.Text.Json;

namespace tes3mp_verifier.Data.Converters
{
  public class JsonValueConverter<T>: ValueConverter<T, string>
  {
    public JsonValueConverter(ConverterMappingHints mappingHints = null)
      : base(serialize, deserialize, mappingHints)
    { }

    private static Expression<Func<T, string>> serialize = x => Serialize(x);
    private static Expression<Func<string, T>> deserialize = x => Deserialize<T>(x);

    public static string Serialize(T data)
    {
      return JsonSerializer.Serialize(data);
    }

    public static T Deserialize<T>(string json)
    {
      return JsonSerializer.Deserialize<T>(json);
    }
  }
}
