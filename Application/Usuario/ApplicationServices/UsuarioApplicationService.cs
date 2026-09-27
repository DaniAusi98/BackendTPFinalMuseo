/*using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.Repositories;
using Domain.CommonDomain.ValueObjets;
using Domain.Usuarios.Entities.UsuarioVisitante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usuario.ApplicationServices
{
    public class UsuarioVisitanteApplicationService(
       IRepositorioUsuarioVisitante context,
       IPasswordHasher hasher,
       IUsuarioRegistradoEmailSender _emailSender,
       ITokenService _tokenService) : IUsuarioApplicationService
    {
        private readonly IRepositorioUsuarioVisitante _context = context ?? throw new ArgumentNullException(nameof(context));

        private readonly IPasswordHasher _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
        private readonly IUsuarioRegistradoEmailSender _emailSender = _emailSender ?? throw new ArgumentNullException(nameof(_emailSender));
        private readonly ITokenService _tokenService = _tokenService ?? throw new ArgumentNullException(nameof(_tokenService));
        public async Task<bool> VerificarEmailUnicoAsync(Email email)
        {
            return await _context.ExistsByEmailAsync(email);
        }

        public string Hash(string password) => _hasher.Hash(password);

        public bool Verify(string password, string hash) => _hasher.Verify(password, hash);

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await _emailSender.SendEmailAsync(to, subject, body);
        }

        public TokenResult GenerateToken(Visitante usuario) => _tokenService.GenerateToken(usuario);


    }
}
*/
