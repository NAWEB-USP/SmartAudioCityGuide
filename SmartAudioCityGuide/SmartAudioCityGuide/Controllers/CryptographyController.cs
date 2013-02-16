using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;

namespace SmartAudioCityGuide.Controllers
{
    public class CryptographyController : Controller
    {
        /// <summary>
        /// Funcao que transforma uma string em outra string com a criptografia MD5
        /// </summary>
        /// <param name="entrada">string a ser transformada pelo MD5 </param>
        /// <returns>string MD5 da string entrada</returns>
        public string getMD5Hash(string enter)
        {
            MD5 md5 = MD5.Create();

            byte[] bytesImbutidos = Encoding.ASCII.GetBytes(enter);
            byte[] hash = md5.ComputeHash(bytesImbutidos);

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// A random string with a size
        /// </summary>
        /// <param name="size">The size of the random string</param>
        /// <returns>The random string</returns>
        public string randomString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            StringBuilder stringBuilder = new StringBuilder();
            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                stringBuilder.Append(ch);
            }

            return stringBuilder.ToString();
        }
    }
}
