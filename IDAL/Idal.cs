using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
	/// <summary>
	/// 数据库访问接口
	/// </summary>
	public interface Idal
	{
		/// <summary>
		/// 查询并绑定单个模型。
		/// </summary>
		/// <typeparam name="T">模型类</typeparam>
		/// <param name="QueryCriteria">查询条件</param>
		/// <returns></returns>
		T SelectModel<T>(string QueryCriteria) where T : class;
		/// <summary>
		/// 查询并绑定多个模型。
		/// </summary>
		/// <typeparam name="T">模型类</typeparam>
		/// <param name="QueryCriteria">查询条件</param>
		/// <returns></returns>
		List<T> SelectModels<T>(string QueryCriteria) where T : class;
		/// <summary>
		/// 对数据库执行增删改操作。
		/// </summary>
		/// <param name="sql">sql语句</param>
		/// <returns></returns>
		bool DbModify(string sql);
		/// <summary>
		/// 执行SQL语句，返回DataTable。
		/// </summary>
		/// <param name="sql">sql语句</param>
		/// <returns></returns>
		DataTable GetDataTable(string sql);
		/// <summary>
		/// 向数据库添加一个模型数据。
		/// </summary>
		/// <typeparam name="T">模型类</typeparam>
		/// <param name="model">数据库模型</param>
		/// <returns></returns>
		bool AddModelToDb<T>(T model) where T : class;
		/// <summary>
		/// 向数据库更新一个模型的数据。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="model"></param>
		/// <returns></returns>
		bool UpdateModelToDb<T>(T model) where T : class;

	}
	
}
