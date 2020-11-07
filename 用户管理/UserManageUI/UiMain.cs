using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserManageBLL;

namespace UserManageUI
{
	public partial class UiMain : Form
	{
		public UiMain()
		{
			InitializeComponent();
		}

		private void UiMain_Load(object sender, EventArgs e)
		{
			UpdateUserlist();
		}
		private void UpdateUserlist()
		{
			List<DataTable> list = new Bll().GetUserslist();
			dataGridView1.Rows.Clear();
			for (int i = 0; i < 4; i++)
			{
				foreach (DataRow row in list[i].Rows)
				{
					dataGridView1.Rows.Add(row["姓名"].ToString(), row["账号"].ToString(), row["密码"].ToString(), row["权限"].ToString());
				}
			}

		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateUserlist();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox2.Text != "" && textBox3.Text != "" && comboBox1.Text != "" && comboBox2.Text != "")
			{
				if (!new Bll().Checkaccount(textBox2.Text)) { MessageBox.Show("账号已存在！", "提示"); return; }
				if (new Bll().Adduser(textBox2.Text, textBox3.Text, comboBox1.Text, comboBox2.Text))
				{
					MessageBox.Show("更改成功成功", "提示");
					UpdateUserlist();
				}
				else { MessageBox.Show("更改成功失败", "提示"); }
			}
			else
			{
				MessageBox.Show("请填写完整用户账号信息！", "提示");
			}
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			if (comboBox1.Text == "") { comboBox2.Enabled = false; }
			else 
			{
			    comboBox2.Enabled = true;
		    }
				
	}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 4)
			{
				if(new Bll().DeletelUser(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()))
				{
					UpdateUserlist();
				}
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBox2.Items.Clear();
			DataTable dataTable = new Bll().GetNamelist(comboBox1.Text);
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					comboBox2.Items.Add(dataRow["姓名"].ToString());
				}

			}
		}

	
	}
}
