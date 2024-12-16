using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MercWebExt.Models.ViewModels
{
    public partial class ExportList
    {   
        public string[] arrId { get; set; }
        public string[] arrData { get; set; }
        public string[] arrText { get; set; }
        public string roleId { get; set; }
    }
}
