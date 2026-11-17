using System.Xml;
using FacultyArchiveXml.Models;

namespace FacultyArchiveXml.Strategies;

// DOM
public class DomSearchStrategy : IXmlSearchStrategy
{
    public List<Material> Search(string filePath, SearchCriteria criteria)
    {
        List<Material> result = new List<Material>();

        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);

        
        XmlNodeList? nodes = doc.SelectNodes("//Material");
        if (nodes == null)
            return result;

        foreach (XmlNode node in nodes)
        {
            
            string faculty = GetAttribute(node, "Faculty");
            string department = GetAttribute(node, "Department");
            string type = GetAttribute(node, "MaterialType");
            string date = GetAttribute(node, "CreationDate");

            int volume = 0;
            int.TryParse(GetAttribute(node, "Volume"), out volume);

            
            string author = GetChildText(node, "AuthorFullName");
            string title = GetChildText(node, "Title");

            Material m = new Material(author, title, faculty, department, type, volume, date);
            if (criteria.IsMatch(m))
                result.Add(m);
        }

        return result;
    }

    
    private string GetAttribute(XmlNode node, string name)
    {
        if (node.Attributes == null)
            return "";

        XmlAttribute? attribute = node.Attributes[name];
        if (attribute == null)
            return "";
        return attribute.Value;
    }

    
    private string GetChildText(XmlNode node, string name)
    {
        XmlNode? child = node.SelectSingleNode(name);
        if (child == null)
            return "";
        return child.InnerText;
    }
}
