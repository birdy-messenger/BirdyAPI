using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.DataBaseModels
{
    public class ConfirmToken
    {
        [Key]
        public string Email { get; set; }
        public Guid Token { get; set; }
        public DateTime TokenDate { get; set; }
    }
}
