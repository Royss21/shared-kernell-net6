
namespace SharedKernell.Helpers
{
    using System.Security.Cryptography;
    using System.Text;
    public static class StringHelper
    {
        private const string PasswordHash = "pass75dc@avz10";
        private const string SaltKey = "s@lAvz10";
        private const string ViKey = "@1B2c3D4e5F6g7H8";
        private const int KeySize = 128;

        public static string ToPascalCase(this string text)
        {
            if (text == null) return null;
            if (text.Length < 2) return text.ToUpper();

            var words = text.Split(
                new char[] {},
                StringSplitOptions.RemoveEmptyEntries);

            var result = "";
            foreach (var word in words)
            {
                result +=$"{word.Substring(0, 1).ToUpper()}{word.Substring(1)}";
            }

            return result;
        }
        public static string Encrypt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return default;

            var plainTextBytes = Encoding.UTF8.GetBytes(value);

            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(KeySize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(ViKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }

                memoryStream.Close();
            }

            return Convert.ToBase64String(cipherTextBytes);
        }
        public static string Decrypt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return default;

            var cipherTextBytes = Convert.FromBase64String(value);
            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(KeySize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(ViKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];

            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}