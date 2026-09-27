
using Application.Usuario.DataTransferObjets;

namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public interface ITokenService
    {
        TokenResult GenerateToken(UserTokenData user);
    }
}


