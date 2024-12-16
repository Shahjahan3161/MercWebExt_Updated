using MercWebExt.Models.DataBase;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace MercWebExt.Models.ViewModels
{
    public partial class ViewPolicies
    {
        public ViewPolicies()
        { }

        public List<Policies> policyList { get;set; }
        public Policies policy { get; set; }
        public IFormFile file { get; set; }
    }
}
