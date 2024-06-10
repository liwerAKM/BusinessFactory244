using System;

namespace EncryptionKeyCore
{

	public class KeyData
	{
		public static string AESKEY(string ID)
		{
			string result;
			if (string.IsNullOrEmpty(ID) || ID.Trim() == "")
			{
				result = "";
			}
			else
			{
				string text = ID.Trim().ToUpper();
				string text2 = "";
				if (text != null)
				{
					if (text == "JSQH")
					{
						text2 = "8478CEFB711D4C00294CBD9BD76D4DFD";
					}
					else
                    {
						text2 = MD5Helper.Md5(ID.Trim().ToUpper() + "J@WK0");
                    }
				}
				//text2 = FormsAuthentication.HashPasswordForStoringInConfigFile(ID.Trim().ToUpper() + "J@WK0", "MD5");
				result = text2;
			}
			return result;
		}

		public static bool AESKEYCheck(string ID, string encryptString, string signature)
		{
			bool result;
			if (string.IsNullOrEmpty(ID) || ID.Trim() == "" || string.IsNullOrEmpty(encryptString))
			{
				result = false;
			}
			else
			{
				string str = KeyData.AESKEY(ID);
				//result = FormsAuthentication.HashPasswordForStoringInConfigFile(encryptString + str, "MD5").Equals(signature);
				result = MD5Helper.Md5(encryptString + str).Equals(signature);
			}
			return result;
		}
	}
}
