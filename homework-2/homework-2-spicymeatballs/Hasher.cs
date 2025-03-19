using System;
using System.Security.Cryptography;
using System.Text;

namespace homework_2_spicymeatballs;

public static class Hasher
{
    public static string Hashed(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2")); // Convert byte to hex string
            }
            return builder.ToString();
        }

    }
}