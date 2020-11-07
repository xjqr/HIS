using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL;
using IDAL;
using UserManageMODEL;
namespace UserManageBLL
{
	public class Bll
	{
		public List<DataTable> GetUserslist()
		{
			Idal idal = new Dal();
			List<DataTable> result = new List<DataTable>();
			result.Add(idal.GetDataTable($"select Registrars_name as 姓名,user_password as 密码,user_type as 权限,user_account as 账号 from his.v_registrars_user"));
			result.Add(idal.GetDataTable($"select doctor_name as 姓名,user_password as 密码,user_type as 权限,user_account as 账号 from his.v_doctor_user"));
			result.Add(idal.GetDataTable($"select Pharmacysstaff_name as 姓名,user_password as 密码,user_type as 权限,user_account as 账号 from his.v_pharmacysstaff_user"));
			result.Add(idal.GetDataTable($"select ChargedPersonnel_name as 姓名,user_password as 密码,user_type as 权限,user_account as 账号 from his.v_chargedpersonnel_user"));
			return result;
		}
		public bool Adduser(string account,string password,string type,string name)
		{
			try
			{Idal idal = new Dal();
			User user = new User { user_account = account, user_password = Md5Hash(password), user_enable = 1, user_type = type };
			idal.AddModelToDb(user);
			int id = int.Parse(idal.GetDataTable($"select * from his.user where user_account='{user.user_account}' and user_password='{user.user_password}'").Rows[0]["user_id"].ToString());
			switch (type)
			{
				case "挂号员":
					{
						int tableid = int.Parse(idal.GetDataTable($"select * from his.registrars where registrars_name='{name}'").Rows[0]["registrars_id"].ToString());
						idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.registrars set registrars_userid='{id}' where registrars_id='{tableid}';SET FOREIGN_KEY_CHECKS=1;");	
					}break;
				case "医生":
					{
						int tableid = int.Parse(idal.GetDataTable($"select * from his.doctor where doctor_name='{name}'").Rows[0]["doctor_id"].ToString());
						idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.doctor set doctor_userid='{id}' where doctor_id='{tableid}';SET FOREIGN_KEY_CHECKS=1;");
					}break;
				case "药房人员":
					{
						int tableid = int.Parse(idal.GetDataTable($"select * from his.pharmacysstaff where Pharmacysstaff_name='{name}'").Rows[0]["Pharmacysstaff_id"].ToString());
						idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.pharmacysstaff set Pharmacysstaff_userid='{id}' where Pharmacysstaff_id='{tableid}';SET FOREIGN_KEY_CHECKS=1;");
					}break;
				case "收费员":
					{
						int tableid = int.Parse(idal.GetDataTable($"select * from his.chargedpersonnel where chargedpersonnel_name='{name}'").Rows[0]["chargedpersonnel_id"].ToString());
						idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.chargedpersonnel set chargedpersonnel_userid='{id}' where chargedpersonnel_id='{tableid}';SET FOREIGN_KEY_CHECKS=1;");
					}break;
			}
				return true;
			}
			catch
			{
				return false;
			}
			

		}
		public bool Checkaccount(string account)
		{
			Idal idal = new Dal();
			if(idal.GetDataTable($@"select * from his.user where user_account='{account}'").Rows.Count == 0) { return true; }
			else { return false; }

		}
		/// <summary>
		/// 32位MD5加密
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public string Md5Hash(string input)
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
		public bool DeletelUser(string account,string password)
		{	
			Idal idal = new Dal();
			return idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;delete from his.user where user_account='{account}' and user_password='{password}';SET FOREIGN_KEY_CHECKS=1;");

		}
		public DataTable GetNamelist(string tablename)
		{
			Idal idal = new Dal();
		
			switch (tablename)
			{
				case "挂号员":
					{
						return idal.GetDataTable($@"select registrars_name as 姓名 from his.registrars");
					}

				case "医生":
					{
						return idal.GetDataTable($@"select doctor_name as 姓名 from his.doctor");
					}
	
				case "药房人员":
					{
						return  idal.GetDataTable($@"select Pharmacysstaff_name as 姓名 from his.pharmacysstaff");
					}
		
				case "收费员":
					{
						return idal.GetDataTable($@"select chargedpersonnel_name as 姓名 from his.chargedpersonnel");
					}
				
			}

			return new DataTable();
		}
	}
}
