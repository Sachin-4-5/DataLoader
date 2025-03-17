using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailer;

namespace DataLoader
{
    public class DataLoadInfo
    {
        public int TotalRecords { get; set; }
        public int FailedRecords { get; set; }
        public int RecordsLoaded { get; set; }
        public int LoadedObjects { get; set; }
        public bool IsArchived { get; set; }
        public string ArchivePath { get; set; }
        public string LogPath { get; set; }
    }
}
