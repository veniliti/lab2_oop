using System.Xml;
using FacultyArchiveXml.Models;

namespace FacultyArchiveXml.Strategies;

// SAX
public class SaxSearchStrategy : IXmlSearchStrategy
{
    public List<Material> Search(string filePath, SearchCriteria criteria)
    {
        List<Material> result = new List<Material>();

       
        string author = "";
        string title = "";
        string faculty = "";
        string department = "";
        string type = "";
        int volume = 0;
        string date = "";

        string currentElement = ""; 

        using (XmlReader reader = XmlReader.Create(filePath))
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    currentElement = reader.Name;

                    if (reader.Name == "Material")
                    {
                        
                        author = "";
                        title = "";
                        faculty = reader.GetAttribute("Faculty") ?? "";
                        department = reader.GetAttribute("Department") ?? "";
                        type = reader.GetAttribute("MaterialType") ?? "";
                        date = reader.GetAttribute("CreationDate") ?? "";

                        string volumeText = reader.GetAttribute("Volume") ?? "";
                        int.TryParse(volumeText, out volume);
                    }
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    
                    if (currentElement == "AuthorFullName")
                        author = reader.Value;
                    else if (currentElement == "Title")
                        title = reader.Value;
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "Material")
                    {
                      
                        Material m = new Material(author, title, faculty, department, type, volume, date);
                        if (criteria.IsMatch(m))
                            result.Add(m);
                    }
                    currentElement = "";
                }
            }
        }

        return result;
    }
}
