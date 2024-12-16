using System;
using System.Collections.Generic;

namespace MercWebExt.Models.DataBase
{
    public partial class Career
    {
        public int Cid { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string JobDescription { get; set; }
        public bool? IsActivated { get; set; }
        public bool? IsDisplayed { get; set; }
        public DateTime? DateClosing { get; set; }
        public DateTime DateCreated { get; set; }
        public bool? IsNonExpired { get; set; }
    }
}
