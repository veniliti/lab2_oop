using System.Xml.Linq;
using FacultyArchiveXml.Models;
using FacultyArchiveXml.Strategies;

namespace FacultyArchiveXml.Services;

//Strategy template
public class XmlProcessor
{
    private IXmlSearchStrategy? _strategy;

    public void SetStrategy(IXmlSearchStrategy strategy)
    {
        _strategy = strategy;
    }

    public List<Material> Search(string filePath, SearchCriteria criteria)
    {
        if (_strategy == null)
            throw new InvalidOperationException("Спочатку оберіть спосіб пошуку (SAX, DOM або LINQ).");

        return _strategy.Search(filePath, criteria);
    }


    public List<string> GetAvailableAttributes(string filePath)
    {
        List<string> names = new List<string>();

        XDocument doc = XDocument.Load(filePath);
        foreach (XElement material in doc.Descendants("Material"))
        {
            foreach (XAttribute attribute in material.Attributes())
            {
                string name = attribute.Name.LocalName;
                if (names.Contains(name) == false)
                    names.Add(name);
            }
        }

        names.Add("AuthorFullName");
        names.Add("Title");
        return names;
    }

    
    public List<string> GetAttributeValues(string filePath, string attributeName)
    {
        List<string> values = new List<string>();

        XDocument doc = XDocument.Load(filePath);
        foreach (XElement material in doc.Descendants("Material"))
        {
            string? value = (string?)material.Attribute(attributeName);
            if (value == null)
                value = (string?)material.Element(attributeName);

            if (value != null && values.Contains(value) == false)
                values.Add(value);
        }

        values.Sort();
        return values;
    }
}
