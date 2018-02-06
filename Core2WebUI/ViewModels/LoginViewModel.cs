using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core2WebUI.ViewModels
{
    public class LoginViewModel
    {
        

        [Required(ErrorMessage = "BoxLengthRequired")]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
