using System.Xml.Linq;

namespace Commons.Extensions
{
    public static class XElementExtensions
    {
        public static string GetValue(this XElement xElement)
        {
            if (xElement == null) { return null; }
            return xElement.Value;
        }

        public static List<XElement> GetElements(this XElement xElement, string elementName)
        {
            if (xElement == null) { return null; }
            return xElement.Elements().Where(e => e.Name.LocalName.Equals(elementName)).ToList();
        }

        public static XElement GetElement(this XElement xElement, string elementName)
        {
            if (xElement == null) { return null; }
            return xElement.Elements().FirstOrDefault(atr => atr.Name.LocalName.Equals(elementName));
        }

        public static string AttributeValue(this XElement xElement, string attributeName)
        {
            if (xElement == null) { return null; }
            XAttribute xAttribute = xElement.Attributes().FirstOrDefault(atr => atr.Name.LocalName.Equals(attributeName));
            return xAttribute?.Value;
        }

        public static string ElementValue(this XElement xElement, string elementName)
        {
            if (xElement == null) { return null; }
            XElement firstMatch = xElement.Elements().FirstOrDefault(atr => atr.Name.LocalName.Equals(elementName));
            return firstMatch?.Value;
        }

        public static string XElementToString(this XElement xElement)
        {
            if (xElement == null) { return null; }
            var reader = xElement.CreateReader();
            reader.MoveToContent();
            return reader.ReadOuterXml();
        }
    }
}
