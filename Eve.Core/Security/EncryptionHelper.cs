using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Eve.Core.Security {
	public static class EncryptionHelper {
		public static string ComputePasswordHash(string password, int salt) {
			if (String.IsNullOrEmpty(password))
				throw new ArgumentNullException("password", "Password string can't be null!");

			var utf8Encoder = new System.Text.UTF8Encoding();

			// Create byte arrays
			byte[] passwordBytes = utf8Encoder.GetBytes(password);
			byte[] saltBytes = {
				(byte)(salt >> 24), (byte)(salt >> 16), (byte)(salt >> 8), (byte)salt
			};

			// Combine two byte arrays
			byte[] bytes = new byte[passwordBytes.Length + saltBytes.Length];
			Array.Copy(passwordBytes, 0, bytes, 0, passwordBytes.Length);
			Array.Copy(saltBytes, 0, bytes, passwordBytes.Length, saltBytes.Length);

			// Encrypt byte array
			byte[] hash = (SHA512Managed.Create()).ComputeHash(bytes);

			return utf8Encoder.GetString(hash);
		}

		public static int CreateRandomSalt() {
			// Fill array of bytes with random values
			Byte[] saltBytes = new Byte[4];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(saltBytes);

			// Combine bytes into integer
			return (saltBytes[0] << 24) + (saltBytes[1] << 16) + (saltBytes[2] << 8) + saltBytes[3];
		}
	}
}
