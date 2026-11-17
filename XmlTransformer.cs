using System.Xml.Xsl;

namespace FacultyArchiveXml.Services;

public class XmlTransformer
{
    // to html transform
    public void TransformToHtml(string xmlPath, string xslPath, string outputHtmlPath)
    {
        XslCompiledTransform xslt = new XslCompiledTransform();
        xslt.Load(xslPath);
        xslt.Transform(xmlPath, outputHtmlPath);
    }
}
