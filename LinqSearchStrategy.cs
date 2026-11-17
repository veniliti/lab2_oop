using System.Xml.Linq;
using FacultyArchiveXml.Models;

namespace FacultyArchiveXml.Strategies;

// LINQ to XML
public class LinqSearchStrategy : IXmlSearchStrategy
{
    public List<Material> Search(string filePath, SearchCriteria criteria)
    {
        XDocument doc = XDocument.Load(filePath);

       
        var all = from el in doc.Descendants("Material")
                  select new Material(
                      (string?)el.Element("AuthorFullName") ?? "",
                      (string?)el.Element("Title") ?? "",
                      (string?)el.Attribute("Faculty") ?? "",
                      (string?)el.Attribute("Department") ?? "",
                      (string?)el.Attribute("MaterialType") ?? "",
                      ToInt((string?)el.Attribute("Volume")),
                      (string?)el.Attribute("CreationDate") ?? "");

        
        return all.Where(m => criteria.IsMatch(m)).ToList();
    }

    private int ToInt(string? text)
    {
        int number = 0;
        if (text != null)
            int.TryParse(text, out number);
        return number;
    }
}
