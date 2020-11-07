using IDAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace DAL
{
	public class Dal : Idal
	{
		public bool AddModelToDb<T>(T model) where T : class
		{
			try
			{
				Type type = model.GetType();//获取模型的类型。
				string columns = "";//定义sql插入语句的字段
				string values = "";//定义sql插入语句对应的值
				foreach (var prop in type.GetProperties())
				{
					if (prop.Name.ToLower() == type.Name.ToLower() + "_id" || prop.GetValue(model) is null)//跳过模型对象的主键和模型对象空值属性。
					{
						continue;
					}
					columns += $@"`{prop.Name.ToLower()}`,";
					values += $@"'{prop.GetValue(model)}',";

				}
				columns = columns.Substring(0, columns.Length - 1);
				values = values.Substring(0, values.Length - 1);
				string sql = $@"INSERT INTO `his`.`{type.Name.ToLower()}` ({columns}) VALUES ({values});";
				MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
				con.Open();
				MySqlCommand cmd = new MySqlCommand(sql, con);
				cmd.ExecuteNonQuery();
				con.Close();
				return true;

			}
			catch
			{
				throw;
			}
		}

		public bool DbModify(string sql)
		{
			try
			{
				MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
				con.Open();
				MySqlCommand cmd = new MySqlCommand(sql, con);
				cmd.ExecuteNonQuery();
				con.Close();
			}
			catch
			{
				throw;
			}
			return true;
		}

		public DataTable GetDataTable(string sql)
		{
			try
			{
				DataTable table = new DataTable();
				DataRow row;
				MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
				con.Open();
				MySqlCommand cmd = new MySqlCommand(sql, con);
				MySqlDataReader reader = cmd.ExecuteReader();
				for (int i = 0; i < reader.FieldCount; i++)//遍历获取MySqlDataReader所有字段名称并添加为Datatable的列名。
				{
					table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
				}
				while (reader.Read())//读取MySqlDataReader添加到Datatable中。
				{
					row = table.NewRow();
					for (int j = 0; j < reader.FieldCount; j++)
					{
						row[j] = reader[j];
					}
					table.Rows.Add(row);
				}
				con.Close();
				return table;

			}
			catch
			{
				throw;
			}

		}

		public T SelectModel<T>(string QueryCriteria) where T : class
		{
			try
			{
				Type type = typeof(T);
				string columnStrings = String.Join(",", type.GetProperties().Select(p => $"{p.Name}"));//构造sql查询的字段
				string sql = $@"select {columnStrings} from {type.Name.ToLower()} where {QueryCriteria}";//构造查询的sql语句。
				MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
				con.Open();
				MySqlCommand cmd = new MySqlCommand(sql, con);
				MySqlDataReader reader = cmd.ExecuteReader();
				T result = (T)Activator.CreateInstance(type);//创建需返回的类型的实例。
				//为返回的实例赋值。
				if (reader.Read())
				{
					foreach (var prop in type.GetProperties())
					{
						prop.SetValue(result, reader[prop.Name] is DBNull ? null : reader[prop.Name], null);
					}
				}
				con.Close();
				return result;
			}
			catch
			{
				return (T)Activator.CreateInstance(typeof(T));
			}
		}

		public List<T> SelectModels<T>(string QueryCriteria) where T : class
		{
			try
			{
				Type type = typeof(T);
				string columnStrings = String.Join(",", type.GetProperties().Select(p => $"{p.Name}"));
				string sql= $@"select {columnStrings} from {type.Name.ToLower()} {QueryCriteria}";			
				MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
				con.Open();
				MySqlCommand cmd = new MySqlCommand(sql, con);
				MySqlDataReader reader = cmd.ExecuteReader();
				List<T> results = new List<T>();
				while (reader.Read())
				{
					T t = (T)Activator.CreateInstance(type);
					foreach (var prop in type.GetProperties())
					{
						prop.SetValue(t, reader[prop.Name] is DBNull ? null : reader[prop.Name], null);
					}
					results.Add(t);
				}
				con.Close();
				return results;
			}
			catch
			{
				return new List<T>();
			}
		}

		public bool UpdateModelToDb<T>(T model) where T : class
		{
			try
			{
				Type type = model.GetType();
				int id = 0;
				string updatekeyvalues = "";
				foreach (var prop in type.GetProperties())
				{
					if (prop.Name.ToLower() == type.Name.ToLower() + "_id")
					{
						id = (int)prop.GetValue(model);
						continue;
					}
					if (prop.GetValue(model) is null) { continue; }
					updatekeyvalues += $@"`{prop.Name.ToLower()}`='{prop.GetValue(model)}',";
				}
				updatekeyvalues = updatekeyvalues.Substring(0, updatekeyvalues.Length - 1);
				string sql = $@"SET FOREIGN_KEY_CHECKS=0;Update  `his`.`{type.Name.ToLower()}` SET {updatekeyvalues} WHERE ({type.Name.ToLower()}_id='{id}');SET FOREIGN_KEY_CHECKS=1;";
				MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
				con.Open();
				MySqlCommand cmd = new MySqlCommand(sql, con);
				cmd.ExecuteNonQuery();
				con.Close();
				return true;

			}
			catch
			{
				return false;
			}
		}
	}
}
