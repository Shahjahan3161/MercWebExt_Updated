using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MercWebExt.Models.ViewModels
{
    public partial class ViewContact
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string IsValid { get; set; }
    }
}
