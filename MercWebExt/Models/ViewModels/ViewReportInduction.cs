using MercWebExt.Models.DataBase;
using System;
using System.Collections.Generic;

namespace MercWebExt.Models.ViewModels
{
    public partial class ViewReportInduction
    {
        public ViewReportInduction()
        { }

        public ViewReportInduction(List<InductionInduction> driverList)
        { 
            this.driverList = driverList;    
        }

        public List<InductionInduction> driverList { get;set;}
        public DateTime selectDate { get;set; }
        public string selectOption { get;set; }

    }
}
