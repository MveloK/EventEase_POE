using Microsoft.AspNetCore.Identity.UI;
using System.ComponentModel.DataAnnotations;
namespace ST10076452_POE_PART1_EventEase.Models
{
    public class UserModel
    {

  
            [Key] // Primary Key
            public int EmployeeID { get; set; }

            [Required]
            [StringLength(50)]
            public string EmpName { get; set; }

            [Required]
            [StringLength(50)]
            public string EmpSName { get; set; }

            [Required]
            [StringLength(15)]
            public string EmpPassword { get; set; } 
        }
    }



