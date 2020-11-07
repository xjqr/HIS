using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegisteredBLL;

namespace RegisteredUI
{
	public partial class Guahaochaxun : Form
	{
		public Guahaochaxun()
		{
			InitializeComponent();
			
		}



		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "" || comboBox1.Text != "" || comboBox2.Text != "")
			{
				DataTable table = new Bll().SelectIntegrated(comboBox2.Text, comboBox1.Text,textBox1.Text);
				dataGridView1.DataSource = table;
			}
			else
			{
				MessageBox.Show("请输入条件", "提示");
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			dataGridView1.DataSource = new Bll().SelectByDatetime(dateTimePicker1.Value, dateTimePicker2.Value);
		}

		private void Guahaochaxun_Load(object sender, EventArgs e)
		{
			dateTimePicker1.MaxDate = DateTime.Now.AddDays(-1);
			dateTimePicker2.MinDate = DateTime.Now;

			dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";

			dateTimePicker1.Format = DateTimePickerFormat.Custom;

			comboBox2.Items.Clear();
			DataTable dt = new Bll().GetDepartment();
			foreach(DataRow row in dt.Rows)
			{
				comboBox2.Items.Add(row["department_name"].ToString());
			}
			dt = new Bll().GetDoctorName();
			comboBox1.Items.Clear();
			foreach(DataRow dataRow in dt.Rows)
			{
				comboBox1.Items.Add(dataRow["doctor_name"].ToString());
			}
			dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";

			dateTimePicker2.Format = DateTimePickerFormat.Custom;
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox2.Text != "")
			{
				comboBox1.Items.Clear();
				DataTable table = new Bll().GetDoctorName(comboBox2.Text);
				foreach(DataRow dataRow in table.Rows)
				{
					comboBox1.Items.Add(dataRow["doctor_name"].ToString());
				}
			}
			

		}
	}
}
