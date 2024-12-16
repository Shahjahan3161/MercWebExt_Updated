using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MercWebExt.Models.ViewModels
{
    public partial class ViewLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public virtual ICollection<UsersUserRole> UserRoles { get; set; }
        //public UsersUserRole UsersUserRole { get; set; }
    }
}
