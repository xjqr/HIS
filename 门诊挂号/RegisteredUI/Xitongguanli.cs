using RegisteredBLL;
using RegisteredMODEL;
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
	public partial class Xitongguanli : Form
	{
		private static DataTable doctorsScheduletable;
		private static DataTable doctorInfo;
		public Xitongguanli()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != ""&&textBox1.Text.StartsWith("科室/"))
			{
				Bll bll = new Bll();
				bll.AddDepartment(textBox1.Text);
				treeView1.Nodes.Clear();
				bll.LoadTree(treeView1);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (textBox2.Text != "" && textBox2.Text.StartsWith("科室/"))
			{
				Bll bll = new Bll();
				bll.DeleteDepartment(textBox2.Text);
				treeView1.Nodes.Clear();
				bll.LoadTree(treeView1);
			}
		}

		private void Xitongguanli_Load(object sender, EventArgs e)
		{
			Bll bll = new Bll();
			bll.LoadTree(treeView1);
			doctorsScheduletable = bll.Loaddoctorsschedule();
			foreach(DataRow row in doctorsScheduletable.Rows)
			{
				dataGridView1.Rows.Add(row["星期"].ToString(), row["时段"].ToString(), row["系统总号数"].ToString(), row["网站总号数"].ToString(), row["网站已挂号数"].ToString(), row["医生姓名"].ToString(), row["总剩余号数"].ToString(), row["系统可加号数"].ToString());
			}
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
			dataGridViewComboBoxColumn.HeaderText = "科室";
			dataGridViewComboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
			DataTable dataTable = bll.GetDepartment();
			foreach(DataRow dataRow in dataTable.Rows)
			{
				dataGridViewComboBoxColumn.Items.Add(dataRow["department_name"].ToString());
			}
			dataGridView2.Columns.Insert(1, dataGridViewComboBoxColumn);
			doctorInfo = bll.GetDoctorInfo();
			foreach(DataRow row1 in doctorInfo.Rows)
			{
				dataGridView2.Rows.Add(row1["doctor_name"].ToString(), row1["department_name"].ToString(), row1["doctor_level"].ToString(), row1["doctor_info"].ToString(), row1["doctor_type"].ToString(), row1["doctor_sex"].ToString());
			}

		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 8 && e.RowIndex < dataGridView1.Rows.Count - 1)
			{
				if (new Bll().DeleteDoctorsschedule(int.Parse(doctorsScheduletable.Rows[e.RowIndex]["doctorsschedule_id"].ToString())))
				{
					Bll bll = new Bll();
					dataGridView1.Rows.Clear();
					doctorsScheduletable = bll.Loaddoctorsschedule();
					foreach (DataRow row in doctorsScheduletable.Rows)
					{
						dataGridView1.Rows.Add(row["星期"].ToString(), row["时段"].ToString(), row["系统总号数"].ToString(), row["网站总号数"].ToString(), row["网站已挂号数"].ToString(), row["医生姓名"].ToString(), row["总剩余号数"].ToString(),  row["系统可加号数"].ToString());
					}
				}
			}
		}


		private void button3_Click(object sender, EventArgs e)
		{
			for(int i = dataGridView1.Rows.Count - 2; i > doctorsScheduletable.Rows.Count-1; i--)
			{
				try
				{
			        DoctorsSchedule doctorsSchedule = new DoctorsSchedule
				   {
					doctorsSchedule_weekday = dataGridView1.Rows[i].Cells[0].Value.ToString(),
						doctorsSchedule_worktime = dataGridView1.Rows[i].Cells[1].Value.ToString(),
						doctorsSchedule_num = int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()),
						doctorsschedule_numweb = int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()),
						doctorsschedule_surplusnumweb = 0,
						doctorsSchedule_surplusnum = int.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString()),
						doctorsSchedule_addnum = int.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString()),
						doctorsSchedule_doctorid = new Bll().GetdoctorID(dataGridView1.Rows[i].Cells[5].Value.ToString())
					};
					
                   if(new Bll().AddDoctorsSchedul(doctorsSchedule))
					{
						continue;
					}
					else
					{
						MessageBox.Show("添加失败!","提示");
					}

				}
				catch
				{
					if (MessageBox.Show($"不存在医生{dataGridView1.Rows[i].Cells[5].Value.ToString()}") == DialogResult.OK)
					{
						continue;
					}

				}
			}
			Bll bll = new Bll();
			dataGridView1.Rows.Clear();
			doctorsScheduletable = bll.Loaddoctorsschedule();
			foreach (DataRow row in doctorsScheduletable.Rows)
			{
				dataGridView1.Rows.Add(row["星期"].ToString(), row["时段"].ToString(), row["系统总号数"].ToString(), row["网站总号数"].ToString(), row["网站已挂号数"].ToString(), row["医生姓名"].ToString(), row["总剩余号数"].ToString(), row["系统可加号数"].ToString());
			}
		}

		private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{

		}

		private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 6 && e.RowIndex < dataGridView2.Rows.Count - 1)
			{
				
				if (new Bll().DeleteDoctorInfo(int.Parse(doctorInfo.Rows[e.RowIndex]["doctor_id"].ToString())))
				{
					dataGridView2.Rows.Clear();
					doctorInfo = new Bll().GetDoctorInfo();
					foreach (DataRow row1 in doctorInfo.Rows)
					{
						dataGridView2.Rows.Add(row1["doctor_name"].ToString(), row1["department_name"].ToString(), row1["doctor_level"].ToString(), row1["doctor_info"].ToString(), row1["doctor_type"].ToString(), row1["doctor_sex"].ToString());
					}
				}
			}
		}

		private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{   string[] filed = new string[] {"doctor_name","department_name","doctor_type","doctor_info","doctor_level","doctor_sex"};
			if (e.RowIndex < doctorInfo.Rows.Count - 1&&e.ColumnIndex!=6&&e.ColumnIndex!=1)
			{
			if(new Bll().UpdateDoctorInfo(filed[e.ColumnIndex],dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),int.Parse(doctorInfo.Rows[e.RowIndex]["doctor_id"].ToString())))
			{
				;
				doctorInfo = new Bll().GetDoctorInfo();			
			}
			}
			if (e.RowIndex < doctorInfo.Rows.Count - 1 && e.ColumnIndex== 1)
			{
				if (new Bll().UpdateDoctorInfo("doctor_departmentid",new Bll().GetDepartmentId(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString()), int.Parse(doctorInfo.Rows[e.RowIndex]["doctor_id"].ToString())))
				{
					
					doctorInfo = new Bll().GetDoctorInfo();
					
				}
			}


		}

		private void button4_Click(object sender, EventArgs e)
		{
			for (int i = dataGridView2.Rows.Count - 2; i > doctorInfo.Rows.Count - 1; i--)
			{
				try
				{ Doctor doctor = new Doctor();

					doctor.doctor_departmentid = int.Parse(new Bll().GetDepartmentId(dataGridView2.Rows[i].Cells[1].Value.ToString()));
					doctor.doctor_enable = 1;
			       doctor.doctor_info = dataGridView2.Rows[i].Cells[3].Value.ToString();
					doctor.doctor_level = dataGridView2.Rows[i].Cells[4].Value.ToString();
					doctor.doctor_name = dataGridView2.Rows[i].Cells[0].Value.ToString();
					doctor.doctor_sex = dataGridView2.Rows[i].Cells[5].Value.ToString();
					doctor.doctor_type = dataGridView2.Rows[i].Cells[2].Value.ToString();
					doctor.doctor_userid = 34;
			   if(new Bll().AddDoctor(doctor))
			   {
				continue;
			    }

				}
				catch
				{
					throw;
				}
			 

			}
			dataGridView2.Rows.Clear();
			doctorInfo = new Bll().GetDoctorInfo();
			foreach (DataRow row1 in doctorInfo.Rows)
			{
				dataGridView2.Rows.Add(row1["doctor_name"].ToString(), row1["department_name"].ToString(), row1["doctor_level"].ToString(), row1["doctor_info"].ToString(), row1["doctor_type"].ToString(), row1["doctor_sex"].ToString());
			}

			
		}
	}
}
