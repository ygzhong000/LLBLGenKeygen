using System;
using System.IO;
using System.Xml.Serialization;

namespace LLBLGenKeygen
{
    public static class XmlHelper
    {

        public static void SaveToXml(string filePath, object sourceObj, Type type, string xmlRootName)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
            {
                type = type != null ? type : sourceObj.GetType();

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = string.IsNullOrWhiteSpace(xmlRootName) ?
                        new System.Xml.Serialization.XmlSerializer(type) :
                        new System.Xml.Serialization.XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                    xmlSerializer.Serialize(writer, sourceObj);
                }
            }
        }

        public static string ToXml(this object sourceObj, string xmlRootName="")
        {
            return XmlHelper.ObjectToXml(sourceObj,xmlRootName);
        }

        public static string ObjectToXml(object sourceObj, string xmlRootName="")
        {
            if (sourceObj == null)
                throw new ArgumentNullException(nameof(sourceObj));
            var type = sourceObj.GetType();
            using (MemoryStream writer = new MemoryStream())
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = string.IsNullOrWhiteSpace(xmlRootName) ?
                    new System.Xml.Serialization.XmlSerializer(type) :
                    new System.Xml.Serialization.XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                xmlSerializer.Serialize(writer, sourceObj);
                byte[] b = writer.ToArray();
                return System.Text.Encoding.UTF8.GetString(b, 0, b.Length);
            }
        }

        public static object LoadFromXml(string filePath, Type type)
        {
            object result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
                    result = xmlSerializer.Deserialize(reader);
                }
            }
            return result;
        }

        public static object LoadFromXml<T>(string filePath, Type type) where T:class
        {
            object result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
                    result = xmlSerializer.Deserialize(reader);
                }
            }
            return (T)result;
        }
    }

}
