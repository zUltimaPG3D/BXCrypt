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
			m_Key = key;
			m_IV = iv;
			m_plainTextHead = head;
		}
		public static string Encrypt(string uid)
		{
			StringBuilder sb = new StringBuilder(m_plainTextHead);
			sb.Append(uid);
			using (Rijndael rijn = RijndaelManaged.Create())
			{
				rijn.Padding = PaddingMode.Zeros;
				rijn.Mode = CipherMode.CBC;
				rijn.KeySize = 256;
				rijn.BlockSize = 128;
				ICryptoTransform encryptor = rijn.CreateEncryptor(Encoding.UTF8.GetBytes(m_Key), Encoding.UTF8.GetBytes(m_IV));
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
				ICryptoTransform decryptor = rijn.CreateDecryptor(Encoding.UTF8.GetBytes(m_Key), Encoding.UTF8.GetBytes(m_IV));
				MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(src));
				CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
				StreamReader streamReader = new StreamReader(cryptoStream);
				string decrypted = streamReader.ReadToEnd();
				if (decrypted.IndexOf(m_plainTextHead, StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					decrypted = decrypted.Substring(m_plainTextHead.Length);
					decrypted = Regex.Replace(decrypted, "[\u0000-\u0019+/]", "");
				}
				memoryStream.Dispose();
				streamReader.Dispose();
				cryptoStream.Dispose();
				return decrypted;
			}
		}
		public static string m_Key = "AehiP1hohphe4ith6ievoh4saht3aeca";
		public static string m_IV = "eiGadaegungoo0gu";
		public static string m_plainTextHead = "bx70test1";
	}
}