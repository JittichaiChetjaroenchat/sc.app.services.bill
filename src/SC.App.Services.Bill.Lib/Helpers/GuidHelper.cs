using System;
using System.Security.Cryptography;
using System.Text;

namespace SC.App.Services.Bill.Lib.Helpers
{
    public static class GuidHelper
    {
        public static Guid Generate(string phase)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(phase));
                return new Guid(hash);
            }
        }
    }
}