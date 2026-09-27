using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public interface IConfirmEmailService
    {
        Task<bool> ExecuteAsync(
            string userId,
            string token,
            CancellationToken cancellationToken = default);
    }
}
