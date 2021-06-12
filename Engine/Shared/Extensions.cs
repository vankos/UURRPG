using System;
using System.Xml;

namespace Engine.Shared
{
    public static class Extensions
    {
        public static T GetXMLAttributeValue<T>(this XmlNode xmlNode, string attributeName)
        {
            XmlAttribute xmlAttribute = xmlNode.Attributes?[attributeName];
            if (xmlAttribute == null)
                throw new ArgumentException($"The attribute {attributeName} is not found for {xmlNode.Name} node");

            return (T)Convert.ChangeType(xmlAttribute.Value, typeof(T));
        }
    }
}
