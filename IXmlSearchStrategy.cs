using FacultyArchiveXml.Models;

namespace FacultyArchiveXml.Strategies;


public interface IXmlSearchStrategy
{
    List<Material> Search(string filePath, SearchCriteria criteria);
}
