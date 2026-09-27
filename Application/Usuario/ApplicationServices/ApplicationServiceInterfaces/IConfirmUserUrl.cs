using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public interface IConfirmUserUrl
    {
        string GetEmailConfirmationUrl(
            string userId,
            string token);
    }
}
