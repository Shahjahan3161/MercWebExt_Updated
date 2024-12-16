using MercWebExt.Models.DataBase;
using Microsoft.AspNetCore.Http;
using System;

namespace MercWebExt.Models.ViewModels
{
    public partial class ViewApplication
    {
        public ViewApplication(Career career)
        {
            this.Career = career;
            this.JobTitle = career.JobTitle;
        }

        public ViewApplication()
        {}

        public Career Career { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAdd { get; set; }        
        public string JobTitle { get; set; }

        public DateTime AvailableDate { get; set; }

        public IFormFile Resume { get; set; }
    }
}
