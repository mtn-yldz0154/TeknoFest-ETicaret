using ElektronikWebUI.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElektronikWebUI.Models
{
	public class RoleModel
	{
		[Required]
		public string RoleName { get; set; }
	}
    public class RoleDetails
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<Customer> Members { get; set; }
        public IEnumerable<Customer> NonMembers { get; set; }
    }

    public class RoleEditModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
