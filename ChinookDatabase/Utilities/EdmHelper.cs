using System.Xml;
using System.Xml.XPath;

namespace ChinookDatabase.Utilities
{
    public class EdmHelper
    {
        public static string GetSsdlFromEdmx(string filename)
        {
            var document = new XPathDocument(filename);
            var navigator = document.CreateNavigator();

            if (navigator.NameTable == null) return string.Empty;

            var manager = new XmlNamespaceManager(navigator.NameTable);
            manager.AddNamespace("edmx", "http://schemas.microsoft.com/ado/2009/11/edmx");
            var nodes = navigator.Select("/edmx:Edmx/edmx:Runtime/edmx:StorageModels", manager);

            if (nodes.MoveNext())
            {
                if (nodes.Current != null)
                    return nodes.Current.InnerXml;
            }

            return string.Empty;
        }
    }
}
