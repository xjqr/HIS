using RegisteredBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegisteredUI
{
	public partial class Qvxiaoguahao : Form
	{
		private static DataTable table;
		public Qvxiaoguahao()
		{
			InitializeComponent();
		}


		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex== 6&&e.RowIndex<dataGridView1.Rows.Count)
			{
				if (table != null)
				{
					if (table.Rows[e.RowIndex]["registrationrecord_enable"].ToString() == "1")
					{
						if(new Bll().CannelRegister(table.Rows[e.RowIndex]))
						{ 
							MessageBox.Show("取消挂号成功!", "操作结果");
							dataGridView1.Rows.Clear();
							table = new Bll().GetRegistrationrecords(textBox1.Text);
							foreach (DataRow row in table.Rows)
							{
								if (row["registrationrecord_enable"].ToString() != "0")
								{
									DateTime time = Convert.ToDateTime(row["registrationrecord_time"].ToString());
									dataGridView1.Rows.Add(row["registrationrecord_id"].ToString(), row["doctor_name"], row["department_name"], row["registrationrecord_num"], time.ToShortDateString(), row["registrationrecord_workday"], "取消");

								}
							}
						}
                      
					  
					}
					else
					{
						MessageBox.Show("不符合条件无法取消挂号。", "操作结果");
					}
				}
				

			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
			table= new Bll().GetRegistrationrecords(textBox1.Text);
			foreach (DataRow row in table.Rows)
			{
				if (row["registrationrecord_enable"].ToString() != "0")
				{
					DateTime time = Convert.ToDateTime(row["registrationrecord_time"].ToString());
					dataGridView1.Rows.Add(row["registrationrecord_id"].ToString(), row["doctor_name"], row["department_name"], row["registrationrecord_num"], time.ToShortDateString(), row["registrationrecord_workday"], "取消");

				}
							}
		}
	}
}
