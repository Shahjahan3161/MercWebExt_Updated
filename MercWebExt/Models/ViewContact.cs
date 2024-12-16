using MercWebExt.Models.DataBase;
using System.Collections.Generic;

namespace MercWebExt.Models.ViewModels
{
    public class DashboardViewContact
    {
        public AboutUs AboutUs { get; set; }
        public Certified Certified { get; set; }
        public List<Categories> Categories { get; set; }
        public List<Categories2> Categories2 { get; set; }
        public List<Categories3> Categories3 { get; set; }
    }
}
