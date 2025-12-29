using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLoader.Templates
{
    public class TemplateReader
    {
        public DataLoadTemplate ReadTemplate(string templatePath)
        {
            var doc = XDocument.Load(templatePath);
            var attributes = doc.Root.Element("ATTRIBUTES");
            var template = new DataLoadTemplate
            {
                InputFilePath = attributes.Element("INPUT_FILE_PATH")?.Value,
                InputFilePattern = attributes.Element("INPUT_FILE_PATTERN")?.Value,
                StoredProcedureName = attributes.Element("SP_NAME")?.Value,
                IterationXPath = attributes.Element("ITERATION_LEVEL_XPATH")?.Value,
                InputType = attributes.Element("INPUT_TYPE")?.Value,
                DbName = attributes.Element("DB_NAME")?.Value
            };

            var columns = doc.Root.Element("COLUMNS").Elements("COLUMN");

            foreach (var col in columns)
            {
                template.Columns.Add(new ColumnMapping
                {
                    ParamName = col.Attribute("PARAM_NAME")?.Value,
                    Name = col.Attribute("NAME")?.Value,
                    XPath = col.Attribute("XPATH")?.Value,
                    SqlDataType = col.Attribute("SQLDATATYPE")?.Value,
                    DefaultValue = col.Attribute("DEFAULT_VALUE")?.Value
                });
            }
            return template;
        }
    }
}