using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BandR
{

    //public class XMLConsts
    //{

    //    public static XNamespace s = "http://schemas.microsoft.com/sharepoint/soap/";
    //    public static XNamespace rs = "urn:schemas-microsoft-com:rowset";
    //    public static XNamespace z = "#RowsetSchema";

    //    public static string AsmxSuffix_Webs = "_vti_bin/Webs.asmx";
    //    public static string AsmxSuffix_Lists = "_vti_bin/Lists.asmx";
    //    public static string AsmxSuffix_Views = "_vti_bin/Views.asmx";

    //}

    public class XmlSerialization
    {

        /// <summary>
        /// </summary>
        public static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }

            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;

            using (var textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlReaderSettings();

            using (var textReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

    }

    public static class XMLExtensions
    {


        public static XElement GetXElement(this XmlNode node)
        {
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }


        public static XmlNode GetXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }


        public static string ToStringAlignAttributes(this XElement element)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;
            StringBuilder stringBuilder = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
                element.WriteTo(xmlWriter);
            return stringBuilder.ToString();
        }


    }
}
