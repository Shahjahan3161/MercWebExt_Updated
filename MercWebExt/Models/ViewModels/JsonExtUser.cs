using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MercWebExt.Models.Induction
{
    public partial class JsonExtUser
    {
        public int Iid { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DriverEmail { get; set; }
        public string DriverMobile { get; set; }
        public string RegoNumber { get; set; }
        public string UserID { get; set; }

        // View Model for Answers 
        public string qr_1 { get; set; }
        public string qr_2 { get; set; }
        public string qr_3 { get; set; }
        public string qr_4 { get; set; }
        public string qr_5 { get; set; }
        public string qr_6 { get; set; }
        public string qr_7 { get; set; }
        public string qr_8 { get; set; }
        public string qr_9 { get; set; }
        public string qr_10 { get; set; }
        public string qr_11 { get; set; }
        public string qr_12 { get; set; }
        public string qr_13 { get; set; }
        public string qr_14 { get; set; }
        public string qr_15 { get; set; }
        public string qr_16 { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
