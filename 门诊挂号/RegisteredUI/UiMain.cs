using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegisteredBLL;
using RegisteredMODEL;

namespace RegisteredUI
{
	public partial class UiMain : Form
	{
		public static string id;
		private static int lastnum;
		private static User User { get; set; }
		public UiMain(string useraccount,string password)
		{
			InitializeComponent();
			User = new User { user_account=useraccount,user_password=password,user_type="挂号员"};

		}
		public UiMain()
		{
			InitializeComponent();
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			time.Text = DateTime.Now.ToString();
			
		}
		private void Updatedoctorregisterstatu(DateTime dateTime,string workday)
		{
			Bll bll = new Bll();
			DataTable dataTable = bll.GetRegistrationStatus(dateTime,workday);
			dataGridView1.Rows.Clear();
			foreach (DataRow item in dataTable.Rows)
			{
				int num = int.Parse(item["doctorsschedule_surplusnum"].ToString()) - int.Parse(item["doctorsschedule_surplusnumweb"].ToString());
				dataGridView1.Rows.Add("选择", "查询", item["doctor_name"].ToString(),num.ToString() , item["department_name"].ToString(), item["doctor_level"].ToString());
			}
		}
		private void UiMain_Load(object sender, EventArgs e)
		{
			time.Text = DateTime.Now.ToString();
			result.Hide();
			dateTimePicker1.MinDate = DateTime.Now;
			dateTimePicker1.MaxDate = DateTime.Now.AddDays(7);
			label16.Text = DateTime.Now.ToShortDateString();
			comboBox1.SelectedIndex = comboBox5.SelectedIndex = comboBox4.SelectedIndex = 0;
			Updatedoctorregisterstatu(dateTimePicker1.Value,comboBox5.SelectedItem.ToString());
			uid.Text = User.user_account;
			role.Text = User.user_type;
			}
		private void ShowResultState()
		{
			result.Show();
			timer2.Enabled = true;
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			result.Hide();
			timer2.Enabled = false;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "")
			{
				try
				{Patient patient = new Bll().GetPatient($@"patient_id={textBox1.Text}");
				textBox2.Text = patient.patient_name;
				if (patient.patient_sex == "男")
				{
                  radioButton1.Checked = true;
				}
				else
				{
					radioButton2.Checked=true;
				}
					if (patient.patient_borndate.ToShortDateString() == "0001/1/1")
					{
						if (MessageBox.Show("诊疗卡账号不存在","提示") == DialogResult.OK) { return; }
					}
				string[] borndeatsp = patient.patient_borndate.ToShortDateString().Split('/');
				if (borndeatsp[1].Length < 2)
				{
					borndeatsp[1] = "0" + borndeatsp[1];
				}
				if (borndeatsp[2].Length < 2)
				{
					borndeatsp[2] = "0" + borndeatsp[2];
				}

				maskedTextBox1.Text =borndeatsp[0]+ borndeatsp[1]+ borndeatsp[2];
				id = textBox1.Text;
			   maskedTextBox2.Text = patient.patient_icd;
				ShowResultState();

				}
				catch
				{

				}
				
			}
			else
			{
				MessageBox.Show("请先输入诊疗卡号","提示");
			}
			
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (textBox2.Text != "" && maskedTextBox2.Text != "" && maskedTextBox1.Text != "" && textBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
			{
				Registerdatatransfer registerdatatransfer = new Registerdatatransfer() 
				{
					department_name = comboBox2.Text,
					doctor_name = comboBox3.Text,
					registrationrecord_time = dateTimePicker1.Value,
					registrationrecord_money = float.Parse(label24.Text.Substring(0, label24.Text.Length - 1)),
					patient_id = int.Parse(textBox1.Text),
					user_account = uid.Text,
					doctorsschedule_surplusnum = lastnum - 1,
					doctorsschedule_worktime = comboBox5.Text

				};
				int num = new Bll().Register(registerdatatransfer);
				if( num> 0)
				{
				  
			     if (MessageBox.Show($"挂号成功,您的排队号为{num}！\n是否需要打印？", "挂号结果", MessageBoxButtons.YesNo) == DialogResult.Yes) 
			      {

						new Bll().PrintingRegisterrecord(new string[] {new Bll().GetPatient($"patient_id='{registerdatatransfer.patient_id}'").patient_name ,registerdatatransfer.patient_id.ToString(), registerdatatransfer.doctor_name, registerdatatransfer.department_name, registerdatatransfer.registrationrecord_time.ToShortDateString()+"-上午", num.ToString(),registerdatatransfer.registrationrecord_money.ToString() });


			      }
			     else
			{
				ShowResultState();
			}
			    Updatedoctorregisterstatu(dateTimePicker1.Value, comboBox5.SelectedItem.ToString());
			   清屏ToolStripMenuItem_Click(sender, e);
		
			     }
				else
				{
					MessageBox.Show("诊疗卡余额不足！", "提示");
				}
			}		
		}
		
		
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
				if (e.ColumnIndex == 0&&e.RowIndex<dataGridView1.Rows.Count&&dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()=="选择")
				{
				if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() == "0")
				{
					MessageBox.Show("该医生号数用完,无法挂号", "提示");
					return;
				}
				    lastnum = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
					comboBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
					comboBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
				    comboBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
				   label22.Text = ConfigurationManager.AppSettings.Get("挂号费")+"元";
				label23.Text = ConfigurationManager.AppSettings.Get(comboBox4.Text)+"元";
				float money= float.Parse(ConfigurationManager.AppSettings.Get("挂号费")) + float.Parse(ConfigurationManager.AppSettings.Get(comboBox4.Text));
				label24.Text = money.ToString() + "元";
				}
				if(e.ColumnIndex== 1&&e.RowIndex < dataGridView1.Rows.Count&& dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() == "查询")
			    {
				if (int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString())> 0)
				{
					MessageBox.Show("该医生尚有余号，无法加号！","提示");
				}
				else 
				{

					if(new Bll().IsAddnum(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(),dateTimePicker1.Value,label20.Text))
					{
						dataGridView1.Rows[e.RowIndex].Cells[3].Value = 1;
						MessageBox.Show("加号成功", "提示");
						
					}
					else
					{
						MessageBox.Show("该医生不支持加号！", "提示");
					}
				}

			    }
			dataGridView1.ClearSelection();
		}

		private void 清屏ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox1.Text = textBox2.Text = maskedTextBox1.Text = richTextBox1.Text = comboBox2.Text = comboBox3.Text =maskedTextBox2.Text=label22.Text=label23.Text=label24.Text= "";
			radioButton1.Checked = radioButton2.Checked = false;
			if (comboBox1.SelectedIndex == 0)
			{
				dateTimePicker1.MinDate = DateTime.Now;
              dateTimePicker1.Value = DateTime.Now;
			}
			else
			{
				dateTimePicker1.MinDate = DateTime.Now.AddDays(1);
				dateTimePicker1.Value = DateTime.Now.AddDays(1);
			}
			
			comboBox4.SelectedIndex = comboBox5.SelectedIndex = 0;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (textBox2.Text != "" && maskedTextBox1.Text != "" && maskedTextBox2.Text != "")
			{
				Patient patient = new Patient()
				{
					patient_id =int.Parse(id),
					patient_name = textBox2.Text,
					patient_borndate = DateTime.ParseExact(maskedTextBox1.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture),
					patient_icd = maskedTextBox2.Text.Replace("-", ""),
					patient_gms = richTextBox1.Text
				};
				if (radioButton1.Checked)
				{
					patient.patient_sex = "男";
				}
				else
				{
					patient.patient_sex = "女";
				}

				if (new Bll().UpdatePatient(patient))
				{
					ShowResultState();
				}

			}
			else
			{
				MessageBox.Show("姓名或出生日期或身份证号不能为空！", "提示");
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (textBox2.Text != "" && maskedTextBox1.Text != "" && maskedTextBox2.Text != "")
			{
				Patient patient = new Patient()
				{
					patient_name = textBox2.Text,
					patient_borndate = DateTime.ParseExact(maskedTextBox1.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture),
					patient_icd = maskedTextBox2.Text.Replace("-", ""),
					patient_gms = richTextBox1.Text,
					patient_enable = 1
				};
				if (radioButton1.Checked)
				{
					patient.patient_sex = "男";
				}
				else
				{
					patient.patient_sex = "女";
				}
				
				if (new Bll().Addpatient(patient))
				{
					Patient patient1 = new Bll().GetPatient($@"patient_name='{patient.patient_name}' and patient_icd='{patient.patient_icd}'");
					MessageBox.Show("诊疗卡号为："+patient1.patient_id, "创建成功");

				}

			}
			else
			{
				MessageBox.Show("姓名或出生日期或身份证号不能为空！","提示");
			}
			
			

		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == 0)
			{
				dateTimePicker1.Enabled = false;
				label16.Text = DateTime.Now.ToShortDateString();
				dateTimePicker1.MinDate = DateTime.Now;
				dateTimePicker1.Value = DateTime.Now;
			}
			else
			{
				dateTimePicker1.Enabled = true;
				dateTimePicker1.MinDate = DateTime.Now.AddDays(1);
				dateTimePicker1.Value= DateTime.Now.AddDays(1);
				label16.Text = DateTime.Now.AddDays(1).ToShortDateString();
			}
		}

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
		{
			label16.Text = dateTimePicker1.Value.ToShortDateString();
			comboBox2.Text = comboBox3.Text = comboBox4.Text = "";
			if (comboBox5.SelectedItem != null)
			{
            Updatedoctorregisterstatu(dateTimePicker1.Value, comboBox5.SelectedItem.ToString());
			}
			
		}

		private void 退号ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new Qvxiaoguahao().Show();
			
		}

		private void 挂号收费汇总统计ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new Shoufeitongji().Show();
		}

		private void 挂号查询ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new Guahaochaxun().Show();
		}

		private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
		{
			label20.Text = comboBox5.Text;
			comboBox2.Text = comboBox3.Text = comboBox4.Text = "";
			if (comboBox5.SelectedItem != null)
			{
				Updatedoctorregisterstatu(dateTimePicker1.Value, comboBox5.SelectedItem.ToString());
			}

		}
		/// <summary>
		/// 点击系统管理菜单后弹出系统管理身份验证对话框。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void 系统管理ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new Guanliyuanyanzheng() {TopMost = true }.ShowDialog();
		}
	}
}
