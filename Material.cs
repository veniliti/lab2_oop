namespace FacultyArchiveXml.Models;

public class Material
{
    public string AuthorFullName { get; set; }
    public string Title { get; set; }
    public string Faculty { get; set; }
    public string Department { get; set; }
    public string MaterialType { get; set; }
    public int Volume { get; set; }
    public string CreationDate { get; set; }

    public Material(string authorFullName, string title, string faculty, string department,
                    string materialType, int volume, string creationDate)
    {
        AuthorFullName = authorFullName;
        Title = title;
        Faculty = faculty;
        Department = department;
        MaterialType = materialType;
        Volume = volume;
        CreationDate = creationDate;
    }
}
