using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace FileNewProject.Commom
{
    [Serializable]
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "USERNAME")]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Vaitro { get; set; }
    }
}