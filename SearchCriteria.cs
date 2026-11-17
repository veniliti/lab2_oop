namespace FacultyArchiveXml.Models;

public class SearchCriteria
{
    public string AttributeName { get; set; }
    public string SearchValue { get; set; }

    public SearchCriteria(string attributeName, string searchValue)
    {
        AttributeName = attributeName;
        SearchValue = searchValue;
    }

    // Перевіряє, чи підходить матеріал під критерій.
    // Автор і назва: шукаємо входження слова. Решта атрибутів: точний збіг.
    public bool IsMatch(Material m)
    {
        string value = SearchValue.Trim().ToLower();
        if (AttributeName == "")
        {
            return m.AuthorFullName.ToLower().Contains(value)
                || m.Title.ToLower().Contains(value)
                || m.Faculty.ToLower().Contains(value)
                || m.Department.ToLower().Contains(value)
                || m.MaterialType.ToLower().Contains(value)
                || m.Volume.ToString().Contains(value)
                || m.CreationDate.ToLower().Contains(value);
        }

        if (AttributeName == "AuthorFullName")
            return m.AuthorFullName.ToLower().Contains(value);
        if (AttributeName == "Title")
            return m.Title.ToLower().Contains(value);
        if (AttributeName == "Faculty")
            return m.Faculty.ToLower() == value;
        if (AttributeName == "Department")
            return m.Department.ToLower() == value;
        if (AttributeName == "MaterialType")
            return m.MaterialType.ToLower() == value;
        if (AttributeName == "Volume")
            return m.Volume.ToString() == value;
        if (AttributeName == "CreationDate")
            return m.CreationDate.ToLower() == value;

        return false; // невідомий атрибут
    }
}
