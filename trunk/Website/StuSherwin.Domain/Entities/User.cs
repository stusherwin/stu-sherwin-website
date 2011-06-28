using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace StuSherwin.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public bool IsValidPassword(string password)
        {
            byte[] utfBytes = UTF8Encoding.ASCII.GetBytes(password);
            var hashBytes = SHA1CryptoServiceProvider.Create().ComputeHash(utfBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString == PasswordHash;
        }
    }
}
