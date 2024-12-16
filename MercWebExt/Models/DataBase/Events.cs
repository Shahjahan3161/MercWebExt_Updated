using System;
using System.Collections.Generic;

namespace MercWebExt.Models.DataBase
{
    public partial class Events
    {
        public int EventId { get; set; }
        public string EventBy { get; set; }
        public string EventType { get; set; }
        public string EventDesc { get; set; }
        public DateTime EventDate { get; set; }
    }
}
