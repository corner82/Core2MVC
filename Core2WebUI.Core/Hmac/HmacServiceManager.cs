using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core2WebUI.Core.Hmac
{
    public class HmacServiceManager : HmacServiceManagerBase
    {
        //private const string _alg = "HmacSHA256"; deprecated
        private const string _salt = "rz8LuOtFBXphj9WQfvFh"; // Generated at https://www.random.org/strings
        private const int _expirationMinutes = 10;

        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public void test()
        {

        }

        public static string GenerateToken(string username, string password, string ip, string userAgent, long ticks)
        {
            string hash = string.Join(":", new string[] { username, ip, userAgent, ticks.ToString() });
            string hashLeft = "";
            string hashRight = "";
            //using (HMAC hmac = HMACSHA256.Create(_alg)) deprecated
            using (HMACSHA256 hmac = new HMACSHA256())
            {
                hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(password));
                byte[] hashLeftByte = hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                hashLeft = Convert.ToBase64String(hashLeftByte);
                hashRight = string.Join(":", new string[] { username, ticks.ToString() });
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }

        public static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });
            //using (HMAC hmac = HMACSHA256.Create(_alg)) deprecated
            using (HMACSHA256 hmac = new HMACSHA256())
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                //hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                byte[] hashPasswordSalt = hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hashPasswordSalt);
            }
        }

        public static bool IsTokenValid(string token, string ip, string userAgent)
        {
            bool result = false;
            try
            {
                // Base64 decode the string, obtaining the token:username:timeStamp.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    string username = parts[1];
                    long ticks = long.Parse(parts[2]);
                    DateTime timeStamp = new DateTime(ticks);
                    // Ensure the timestamp is valid.
                    bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
                    if (!expired)
                    {
                        //
                        // Lookup the user's account from the db.
                        //
                        if (username == "john")
                        {
                            string password = "password";
                            // Hash the message with the key to generate a token.
                            string computedToken = GenerateToken(username, password, ip, userAgent, ticks);
                            // Compare the computed token with the one supplied and ensure they match.
                            result = (token == computedToken);
                        }
                    }
                }
            }
            catch
            {
            }
            return result;
        }


    }
}
