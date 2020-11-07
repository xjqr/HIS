using System.Text;
using LogicMODEL;
using IDAL;
using DAL;
using System.Security.Cryptography;

namespace LogicBLL
{
	public class Bll
	{
		/// <summary>
		/// 32位MD5加密
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public  string Md5Hash(string input)
		{
			MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}
		/// <summary>
		/// 验证登录。
		/// </summary>
		/// <param name="user">用户类</param>
		/// <returns></returns>
		public bool TryLogin(User user)
		{
			    Idal idal = new Dal();
			   var result = idal.SelectModel<User>($" user_account='{user.user_account}' and user_password='{user.user_password}'");
				if (result.user_account == null||result.user_type=="病人")
				{
					return false;
				}
				else
				{
				    
					return true;
				}	
		}
		/// <summary>
		/// 获取登录用户的权限。
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public string  GetUserType(User user)
		{
			Idal idal = new Dal();
			return idal.GetDataTable($@"select * from his.user where user_account='{user.user_account}' and user_password='{user.user_password}'").Rows[0]["user_type"].ToString();
		}
	
	}
}
