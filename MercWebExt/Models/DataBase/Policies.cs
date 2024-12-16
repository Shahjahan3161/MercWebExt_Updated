using System;
using System.Collections.Generic;

namespace MercWebExt.Models.DataBase
{
    public partial class Policies
    {
        public int Pid { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExt { get; set; }
        public decimal? FileSize { get; set; }
        public string Owner { get; set; }
        public bool? IsUsed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
