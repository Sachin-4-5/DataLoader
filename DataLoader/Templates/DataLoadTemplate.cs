using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader.Templates
{
    public class DataLoadTemplate
    {
        public string InputFilePath { get; set; }
        public string InputFilePattern { get; set; }
        public string StoredProcedureName { get; set; }
        public string IterationXPath { get; set; }
        public string InputType { get; set; }
        public string DbName { get; set; }
        public bool ReInsert { get; set; }
        public bool FileNameAsParam { get; set; }
        public string ArchivePath { get; set; }
       

        public List<ColumnMapping> Columns { get; set; } = new();
    }
}