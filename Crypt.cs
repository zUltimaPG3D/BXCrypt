using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

// IDA-Accurate Crypt recreation
namespace BX.App.Util
{
	public static class Crypt
	{
		public static void Initialize(string key, string iv, string head)
		{
			Key = key;
			IV = iv;
			plainTextHead = head;
		}
		public static string Encrypt(string uid)
		{
			StringBuilder sb = new StringBuilder(plainTextHead);
			sb.Append(uid);
			using (Rijndael rijn = RijndaelManaged.Create())
			{
				rijn.Padding = PaddingMode.Zeros;
				rijn.Mode = CipherMode.CBC;
				rijn.KeySize = 256;
				rijn.BlockSize = 128;
				ICryptoTransform encryptor = rijn.CreateEncryptor(Encoding.UTF8.GetBytes(Key), Encoding.UTF8.GetBytes(IV));
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
				cryptoStream.Write(Encoding.UTF8.GetBytes(sb.ToString()));
				cryptoStream.FlushFinalBlock();
				return Convert.ToBase64String(memoryStream.ToArray());
			}
		}
		public static string Decrypt(string src)
		{
			using (Rijndael rijn = RijndaelManaged.Create())
			{
				rijn.Padding = PaddingMode.Zeros;
				rijn.Mode = CipherMode.CBC;
				rijn.KeySize = 256;
				rijn.BlockSize = 128;
				ICryptoTransform decryptor = rijn.CreateDecryptor(Encoding.UTF8.GetBytes(Key), Encoding.UTF8.GetBytes(IV));
				MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(src));
				CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
				StreamReader streamReader = new StreamReader(cryptoStream);
				string decrypted = streamReader.ReadToEnd();
				if (decrypted.IndexOf(plainTextHead, StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					decrypted = decrypted.Substring(plainTextHead.Length);
					decrypted = Regex.Replace(decrypted, "[\u0000-\u0019+/]", "");
				}
				memoryStream.Dispose();
				streamReader.Dispose();
				cryptoStream.Dispose();
				return decrypted;
			}
		}
		public static string Key = "AehiP1hohphe4ith6ievoh4saht3aeca";
		public static string IV = "eiGadaegungoo0gu";
		public static string plainTextHead = "bx70test1";
	}
}