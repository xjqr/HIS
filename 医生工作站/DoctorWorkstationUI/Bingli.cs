using DoctorWorkstationBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoctorWorkstationUI
{
	public partial class Bingli : Form
	{
		public static DataTable Binglitable;
		public Bingli()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "")
			{
				try
				{
					Binglitable = new Bll().SelectBingli(int.Parse(textBox1.Text));




					foreach (DataRow row in Bingli.Binglitable.Rows)
					{
						dataGridView1.Rows.Add(row["drd_time"].ToString(), row["doctor_name"].ToString(), row["department_name"].ToString(), "查看");
					}
				}
				catch
				{

				}
			}
			else
			{
				MessageBox.Show("请输入诊疗卡号", "提示");
			}
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 3)
			{
				new Bingliinfo(e.RowIndex).ShowDialog();
			}
		}
	}
}
