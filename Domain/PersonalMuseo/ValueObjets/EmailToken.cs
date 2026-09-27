using Domain.Common.ValueObjets;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PersonalMuseo.ValueObjets
{
    public class EmailToken : ValueObject
    {
        public string Token { get; }
        public DateTime ExpiracionUtc { get; }

        protected EmailToken() { } // Para EF Core

        public EmailToken(string token, DateTime expiracionUtc)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("El token no puede estar vacío.", nameof(token));

            if (expiracionUtc <= DateTime.UtcNow)
                throw new ArgumentException("La expiración debe ser futura.", nameof(expiracionUtc));

            Token = token;
            ExpiracionUtc = DateTime.SpecifyKind(expiracionUtc, DateTimeKind.Utc);
        }

        public bool Expirado() => ExpiracionUtc <= DateTime.UtcNow;

        public void Validar(string token)
        {
            if (Token != token)
                throw new InvalidOperationException("El token no coincide.");

            if (Expirado())
                throw new InvalidOperationException("El token ya expiró.");
        }

        public override bool Equals(object obj)
        {
            if (obj is not EmailToken other) return false;
            return Token == other.Token && ExpiracionUtc == other.ExpiracionUtc;
        }

        public override int GetHashCode() => HashCode.Combine(Token, ExpiracionUtc);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
