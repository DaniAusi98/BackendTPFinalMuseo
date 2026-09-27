using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usuario.DataTransferObjets
{
    public class PasswordValidationResultDto
    {
        
            public bool Succeeded { get; set; }

            public bool IsLockedOut { get; set; }

            public bool RequiresTwoFactor { get; set; }
        
    }
}
