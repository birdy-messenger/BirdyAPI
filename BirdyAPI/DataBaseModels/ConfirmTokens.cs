using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.DataBaseModels
{
    public class ConfirmTokens
    {
        [Key]
        public string Email { get; set; }
        public Guid ConfirmToken { get; set; }
        public DateTime TokenDate { get; set; }
    }
}
