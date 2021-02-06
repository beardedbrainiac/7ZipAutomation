using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7ZipAutomation.Models
{
    public class ArchiveModel
    {
        public int Id { get; set; }

        [DisplayName("Folder Path")]
        public string FolderPath { get; set; }

        [DisplayName("Archive Path")]
        public string ArchivePath { get; set; }
        
        public bool Archived { get; set; }
        
        public bool Deleted { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
