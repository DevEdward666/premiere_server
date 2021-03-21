using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPremiere.Parameters
{
    public class PUsers
    {
        public class PGetUsers
        {
            [Required]
            [Display(Name = "Offset")]
            public short offset { get; set; }
        }
        public class PGetUsersInfo
        {
            [Required]
            [StringLength(30)]
            [Display(Name = "Premid")]
            public string premid { get; set; }
        }
        public class PGetUsersImage
        {
            [Required]
            [StringLength(255)]
            [Display(Name = "Username")]
            public string username { get; set; }
        }
    }
}
