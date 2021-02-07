using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Conversion;

namespace Train2d.Main.Serialization
{

  public class XmlGuidListConverter : IConverter<List<Guid>>
  {
    public bool IsSatisfiedBy(TypeInfo parameter)
    {
      return typeof(List<Guid>).GetTypeInfo().IsAssignableFrom(parameter);
    }

    public List<Guid> Parse(string data)
    {
      return ParseGuidList(data);
    }

    public string Format(List<Guid> instance)
    {
      return FormatGuidList(instance);
    }

    public static List<Guid> ParseGuidList(string data)
    {
      var items = data.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      var result = items.Select(item => Guid.Parse(item)).ToList();
      return result;
    }

    public static string FormatGuidList(List<Guid> instance)
    {
      var items = instance.Select(item => item.ToString()).ToArray();
      var result = string.Join(" ", items);
      return result;
    }
  }
}
