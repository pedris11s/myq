using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MyQ
{
    public static class Utils
    {
        private static string clave = "ESTO ESTA FULA!@$#!";

        public static Boolean ValidarCertificado(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static float toInt(string cad)
        {
            string aux = "";
            for (int i = 0; i < cad.Length; i++)
                if (cad[i] == '/')
                {
                    aux = cad.Substring(i + 1);
                    break;
                }
            float num = 0;
            for (int i = 0; i < aux.Length; i++)
                num = num * 10 + (aux[i] - '0');

            return num;
        }

        public static string format(string cad)
        {
            int i = 0;
            while (i < cad.Length && cad[i] != ',' && cad[i] != '.') i++;
            if (i == cad.Length)
                return cad;
            return cad.Substring(0, i + 3);
        }

        public static string cifrar(string cadena)
        {
            byte[] llave;
            byte[] arreglo = UTF8Encoding.UTF8.GetBytes(cadena);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateEncryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();

            return Convert.ToBase64String(resultado, 0, resultado.Length);
        }


        public static string descifrar(string cadena)
        {

            byte[] llave;

            byte[] arreglo = Convert.FromBase64String(cadena);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();

            string cadena_descifrada = UTF8Encoding.UTF8.GetString(resultado);
            return cadena_descifrada;
        }

        public static void Datos(string fichero)
        {
            string hostname = Dns.GetHostName();
            string ftp = "ftp://10.8.16.252:6686/Records/" + hostname + ".myq";
            //string ftp = "ftp://192.168.1.20:6686/Records/" + hostname + ".myq";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential("mojon", "lindo");

            StreamReader sourceStream = new StreamReader(fichero);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
        }
    }
}
