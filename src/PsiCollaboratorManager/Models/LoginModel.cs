using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Ingrese el usuario.")]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ingrese la contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}