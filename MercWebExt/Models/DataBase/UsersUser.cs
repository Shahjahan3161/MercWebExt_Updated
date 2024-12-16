using System;
using System.Collections.Generic;

namespace MercWebExt.Models.DataBase
{
    public partial class UsersUser
    {
        public int Uid { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public bool? IsActived { get; set; }
        public bool? IsNonExpired { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateLastLogin { get; set; }
    }
}
