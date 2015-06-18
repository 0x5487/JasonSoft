using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace JasonSoft.XML
{
    public static class XMLExtension
    {
        public static string ToXHTML(this String source)
        {
            if (String.IsNullOrEmpty(source)) throw new ArgumentNullException("source");

            //SgmlReader reader = new SgmlReader();
            //reader.DocType = "HTML";
            //reader.InputStream = new StringReader(source);

            //StringWriter sw = new StringWriter();
            //XmlTextWriter w = new XmlTextWriter(sw);
            //reader.Read();

            //while (!reader.EOF)
            //{
            //    w.WriteNode(reader, true);
            //}
            //w.Flush();
            //w.Close();
            //return sw.ToString();


            using (SgmlReader reader = new SgmlReader())
            {
                reader.DocType = "HTML";
                reader.InputStream = new StringReader(source);
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlTextWriter writer = new XmlTextWriter(stringWriter))
                    {
                        reader.WhitespaceHandling = WhitespaceHandling.None;
                        writer.Formatting = Formatting.Indented;
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);
                        if (doc.DocumentElement == null)
                        {
                            return string.Empty;
                        }
                        else
                        {
                            doc.DocumentElement.WriteContentTo(writer);
                        }
                        writer.Close();
                        string xhtml = stringWriter.ToString();
                        return xhtml;
                    }
                }
            }
        }

        public static String RemoveHTMLTag(this String source)
        {
            return Regex.Replace(source, @"<(.|\n)*?>", string.Empty);
        }

        public static String FormatXML(this String source, Formatting format)
        {
            StringReader stringReader = new StringReader(source);

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(stringWriter))
                {
                    writer.Formatting = format;
                    XmlDocument doc = new XmlDocument();
                    doc.Load(stringReader);
                    if (doc.DocumentElement == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        doc.DocumentElement.WriteContentTo(writer);
                    }
                    writer.Close();
                    return stringWriter.ToString();
                }
            }
        }

        public static string ToXML(this Object source)
        {
            XmlSerializer s = new XmlSerializer(source.GetType());
            using (StringWriter writer = new StringWriter())
            {
                s.Serialize(writer, source);
                return writer.ToString();
            }
        }

        public static T FromXML<T>(this String source)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(source))
            {
                object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }
    }

}

