using MercWebExt.Models.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MercWebExt.Models.ViewModels
{
    public partial class ViewInduction
    {
        public List<InductionQuestion> InductionQuestions { get; set; }
        
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string DriverEmail { get; set; }
        [Required(ErrorMessage = "The phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2-3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string DriverMobile { get; set; }
        [Required(ErrorMessage = "The phone number is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Please Check your Rego number")]
        public string RegoNumber { get; set; }
        
        // View Model for Answers 
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_1 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_2 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_3 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_4 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_5 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_6 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_7 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_8 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_9 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_10 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_11 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_12 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_13 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_14 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_15 { get; set; }
        [Required(ErrorMessage = "Pleae choose one of answers")]
        public string qr_16 { get; set; }
    }
}
