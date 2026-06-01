using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public static class Utils
    {
        public static string GenerateMD5Hash(string clave)
        {
            MD5 md5 = MD5.Create();
            byte[] cadenaBytes = Encoding.UTF8.GetBytes(clave);
            byte[] hash = md5.ComputeHash(cadenaBytes);

            StringBuilder sb = new StringBuilder();

            foreach (byte car in hash)
            {
                sb.Append(car.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
