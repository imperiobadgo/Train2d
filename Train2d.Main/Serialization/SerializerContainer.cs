using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Train2d.Main.Serialization
{
  public class SerializerContainer
  {
    public static IExtendedXmlSerializer GetExtendedSerializer()
    {
      var configuration = new ConfigurationContainer().
            UseOptimizedNamespaces().EnableMemberExceptionHandling().
            UseAutoFormatting().
            Type<List<Guid>>().Register().Converter().Using(new XmlGuidListConverter());
      //UseEncryptionAlgorithm(New Base64Encryption())
      //Type(of ExportProgram).
      //Member(Function(e) e.ProgrammText).
      //Encrypt().
      //Type(Of Int32Collection).Register().Converter().Using(New XmlInt32CollectionConverter).
      //Type(Of Color).Register().Converter().Using(New XmlColorConverter).
      //Type(Of Vector3D).Register().Converter().Using(New XmlVector3DConverter).
      //Type(of Point3D).Register().Converter().Using(New XmlPoint3DConverter).
      //Type(Of Matrix3D).Register().Converter().Using(New XmlMatrix3DConverter).
      //Type(Of Vector).Register().Converter().Using(New XmlVectorConverter).
      //Type(Of Point).Register().Converter().Using(New XmlPointConverter).
      //Type(Of Matrix).Register().Converter().Using(New XmlMatrixConverter).
      //Type(Of List(Of Guid)).Register().Converter().Using(New XmlGuidListConverter)
      return configuration.Create();
    }



  }
}
