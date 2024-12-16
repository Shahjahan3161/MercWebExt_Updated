using MercWebExt.Models.DataBase;
using System;
using System.Collections.Generic;

namespace MercWebExt.Models.ViewModels
{
    public partial class ViewCareer
    {
        public ViewCareer()
        { }
        
        // Career List Model
        public ViewCareer(List<Career> careerList)
        {
            this.careerList = careerList;
        }

        public List<Career> careerList {get;set; }
    }
}
