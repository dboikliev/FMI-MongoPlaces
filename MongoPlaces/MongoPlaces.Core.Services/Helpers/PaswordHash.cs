using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MongoPlaces.Core.Services.Helpers
{
    internal class PasswordHash
    {
        internal byte[] PasswordBytes { get; private set;  }
        internal byte[] SaltBytes { get; private set; }

        internal PasswordHash(string password)
        {
            var rfc2898 = new Rfc2898DeriveBytes(password, 16, 1000);
            PasswordBytes = rfc2898.GetBytes(64);
            SaltBytes = rfc2898.Salt;
        }

        internal PasswordHash(string password, byte[] salt)
        {
            var rfc2898 = new Rfc2898DeriveBytes(password, salt, 1000);
            PasswordBytes = rfc2898.GetBytes(64);
            SaltBytes = rfc2898.Salt;
        }
    }
}
