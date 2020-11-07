using System;
using System.Data;
using System.Windows.Forms;
using DoctorWorkstationBLL;
using DoctorWorkstationMODEL;

namespace DoctorWorkstationUI
{
	public partial class UiMain : Form
	{
		public static int RemoveTabpageIndex = 0;
		
		public static DataTable listtable;
		public static User user;
		public UiMain(string useraccount,string password)
		{
			InitializeComponent();
			user = new User { user_account = useraccount, user_type = "医生", user_password = password };
		}
		public UiMain()
		{
			InitializeComponent();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			TabPage tabPage = new TabPage()
			{
				Text = "西药处方"

			};
			tabPage.Controls.Add(new 西药处方(){ Dock=DockStyle.Fill,Index=tabControl1.TabPages.Count});
			tabControl1.TabPages.Add(tabPage);
			tabControl1.SelectedTab = tabControl1.TabPages[tabControl1.TabPages.Count - 1];
			

		}

		private void button2_Click(object sender, EventArgs e)
		{
			TabPage tabPage = new TabPage()
			{
				Text = "输液处方"
			};
			tabPage.Controls.Add(new 输液处方() { Dock = DockStyle.Fill, Index = tabControl1.TabPages.Count });
			tabControl1.TabPages.Add(tabPage);
			tabControl1.SelectedTab = tabControl1.TabPages[tabControl1.TabPages.Count - 1];

		}

		private void button3_Click(object sender, EventArgs e)
		{
			TabPage tabPage = new TabPage()
			{
				Text = "中药处方"
			};
			tabPage.Controls.Add(new 中药处方() { Dock = DockStyle.Fill, Index = tabControl1.TabPages.Count });
			tabControl1.TabPages.Add(tabPage);
			tabControl1.SelectedTab = tabControl1.TabPages[tabControl1.TabPages.Count - 1];

		}

		private void UiMain_Load(object sender, EventArgs e)
		{
			time.Text = DateTime.Now.ToString();
			uid.Text = user.user_account;
			Bll bll = new Bll();
			listtable = bll.GetlistDate(int.Parse(uid.Text));
			dataGridView1.Rows.Clear();
			foreach (DataRow row in listtable.Rows)
			{

				dataGridView1.Rows.Add(row["registrationrecord_patientid"], row["registrationrecord_num"]);
			}

		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			time.Text = DateTime.Now.ToString();
			
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			if (RemoveTabpageIndex != 0)
			{   
			    for(int i = tabControl1.TabPages.Count-1; i > RemoveTabpageIndex; i--)
				{
					switch(tabControl1.TabPages[i].Text)
				     {
						case "中药处方": { 中药处方 x = tabControl1.TabPages[i].Controls[0] as 中药处方; x.SetIndex(i - 1); } break;
						case "西药处方": { 西药处方 x = tabControl1.TabPages[i].Controls[0] as 西药处方;x.SetIndex(i-1); } break;
						case "输液处方": { 输液处方 x = tabControl1.TabPages[i].Controls[0] as 输液处方; x.SetIndex(i - 1); } break;
					}
				}
				tabControl1.Controls.Remove(tabControl1.TabPages[RemoveTabpageIndex]);
				tabControl1.SelectTab(RemoveTabpageIndex - 1);
				RemoveTabpageIndex = 0;
			}

		}

		private void 工作量统计ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new Gongzuoliangtongji().Show();
		}

		private void 影像学检查ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process p = new System.Diagnostics.Process();
			p.StartInfo.UseShellExecute = true;
			p.StartInfo.FileName =Environment.CurrentDirectory+ "\\检查申请单\\B超申请单模板.doc";
			p.Start();
		}

		private void 化验检查申请单ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process p = new System.Diagnostics.Process();
			p.StartInfo.UseShellExecute = true;
			p.StartInfo.FileName = Environment.CurrentDirectory + "\\检查申请单\\化验申请单模板.doc";
			p.Start();
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 2)
			{
				Clear();
				Bll bll = new Bll();
				Patient patient = bll.GetPatient(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()));
				name.Text = patient.patient_name;
				sex.Text = patient.patient_sex;
				age.Text = (DateTime.Now.Year - patient.patient_borndate.Year).ToString();
				

			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			try
			{
				Bll bll = new Bll();
				Drd drd = new Drd()
				{
					drd_doctorid = new Bll().GetdoctorID(uid.Text),
					drd_checkresult = comboBox6.Text,
					drd_mainRemarks = comboBox1.Text,
					drd_nowHistory = comboBox2.Text,
					drd_pastHistory = comboBox3.Text,
					drd_diagnosticResults = comboBox4.Text,
					drd_yizhu = comboBox5.Text,
					drd_time = DateTime.Now,
					drd_patientid = int.Parse(listtable.Rows[0]["registrationrecord_patientid"].ToString())
				};
				int ss = new Bll().Addchargerecord(int.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString()));
				float summoney = 0;
				foreach (TabPage tabPage in tabControl1.TabPages)
				{

					if (tabPage.Text == "西药处方")
					{
						foreach (Control c in tabPage.Controls)
						{
							if (c is 西药处方)
							{
								drd.drd_prescription += "\n西药处方：\n";
								西药处方 cf = (西药处方)c;
								foreach (Control control in cf.Controls)
								{

									if (control is SplitContainer)
									{
										SplitContainer splitContainer = (SplitContainer)control;
										foreach (Control control2 in splitContainer.Panel2.Controls)
										{
											if (control2 is DataGridView)
											{

												DataGridView dataGridView = (DataGridView)control2;
												for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
												{

													drd.drd_prescription += $"药品：{dataGridView.Rows[i].Cells[0].Value} 用法：{dataGridView.Rows[i].Cells[1].Value} 频率：{dataGridView.Rows[i].Cells[2].Value} 单次：{dataGridView.Rows[i].Cells[3].Value}\n";
													int num = int.Parse(dataGridView.Rows[i].Cells[4].Value.ToString());
													float money = num * new Bll().Getsellingprice(dataGridView.Rows[i].Cells[0].Value.ToString());
													summoney += money;
													ncdm nc = new ncdm { ncdm_chargedstate = 0, ncdm_medicinename = dataGridView.Rows[i].Cells[0].Value.ToString(), ncdm_num = num, ncdm_money = money, ncdm_pstate = 0, ncdm_patientid = int.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString()), ncdm_chargerecordid = ss };
													new Bll().Addncdm(nc);
												}
											}
										}
									}

								}
							}
						}

					}
					if (tabPage.Text == "中药处方")
					{
						foreach (Control c in tabPage.Controls)
						{
							if (c is 中药处方)
							{
								drd.drd_prescription += "\n中药处方：\n";
								中药处方 cf = (中药处方)c;
								foreach (Control control in cf.Controls)
								{
									if (control is SplitContainer)
									{
										SplitContainer splitContainer = (SplitContainer)control;
										foreach (Control control2 in splitContainer.Panel2.Controls)
										{
											if (control2 is SplitContainer)
											{
												SplitContainer splitContainer1 = (SplitContainer)control2;
												foreach (Control control3 in splitContainer1.Panel1.Controls)
												{
													if (control3 is DataGridView)
													{

														DataGridView dataGridView = (DataGridView)control3;
														for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
														{

															drd.drd_prescription += $"药品：{dataGridView.Rows[i].Cells[0].Value} 剂量：{dataGridView.Rows[i].Cells[1].Value} 特殊处理：{dataGridView.Rows[i].Cells[2].Value}\n";
															int num = int.Parse(dataGridView.Rows[i].Cells[4].Value.ToString());
															float money = num * new Bll().Getsellingprice(dataGridView.Rows[i].Cells[0].Value.ToString());
															summoney += money;
															ncdm nc = new ncdm { ncdm_chargedstate = 0, ncdm_medicinename = dataGridView.Rows[i].Cells[0].Value.ToString(), ncdm_num = num, ncdm_money = money, ncdm_pstate = 0, ncdm_patientid = int.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString()), ncdm_chargerecordid = ss };
															new Bll().Addncdm(nc);
														}
													}
												}
												for (int j = 0; j < splitContainer1.Panel2.Controls.Count; j++)
												{
													if (splitContainer1.Panel2.Controls[j] is ComboBox)
													{
														ComboBox comboBox = (ComboBox)splitContainer1.Panel2.Controls[j];
														if (j == 0)
														{
															drd.drd_prescription += $"服用方式：{comboBox.SelectedItem}";
														}
														if (j == 2)
														{
															drd.drd_prescription += $"频率：{comboBox.SelectedItem}\n";
														}

													}
												}
											}
										}
									}
								}
							}
						}
					}
					if (tabPage.Text == "输液处方")
					{
						foreach (Control c in tabPage.Controls)
						{
							if (c is 输液处方)
							{
								drd.drd_prescription += "\n输液处方：\n";
								输液处方 cf = (输液处方)c;
								foreach (Control control in cf.Controls)
								{
									if (control is SplitContainer)
									{
										SplitContainer splitContainer = (SplitContainer)control;
										foreach (Control control1 in splitContainer.Panel2.Controls)
										{
											if (control1 is DataGridView)
											{

												DataGridView dataGridView = (DataGridView)control1;
												for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
												{

													drd.drd_prescription += $"药品：{dataGridView.Rows[i].Cells[0].Value} 输液方式：{dataGridView.Rows[i].Cells[1].Value} 频率：{dataGridView.Rows[i].Cells[2].Value} 剂量：{dataGridView.Rows[i].Cells[3].Value}\n";
													int num = int.Parse(dataGridView.Rows[i].Cells[4].Value.ToString());
													float money = num * new Bll().Getsellingprice(dataGridView.Rows[i].Cells[0].Value.ToString());
													summoney += money;
													ncdm nc = new ncdm { ncdm_chargedstate = 0, ncdm_medicinename = dataGridView.Rows[i].Cells[0].Value.ToString(), ncdm_num = num, ncdm_money = money, ncdm_pstate = 0, ncdm_patientid = int.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString()), ncdm_chargerecordid = ss };
													new Bll().Addncdm(nc);
												}
											}

										}
									}

								}
							}
						}
					}
				}
				new Bll().Updatechargerecordmoney(summoney, ss);
				if (bll.UpdateRegistrationrecord(int.Parse(listtable.Rows[0]["registrationrecord_id"].ToString())) && bll.AddDrd(drd))
				{
					MessageBox.Show("病历录入成功", "提示");
					listtable = bll.GetlistDate(int.Parse(uid.Text));
					dataGridView1.Rows.Clear();
					foreach (DataRow row in listtable.Rows)
					{

						dataGridView1.Rows.Add(row["registrationrecord_patientid"], row["registrationrecord_num"]);
					}

				}
			}
			catch { throw; }
			
			
			
		}
		private void Clear()
		{

			TabPage tabPage = tabControl1.TabPages[0];
			tabControl1.TabPages.Clear();
			tabControl1.TabPages.Add(tabPage);
			RemoveTabpageIndex = 0;
			comboBox1.Text = comboBox2.Text = comboBox3.Text = comboBox4.Text = comboBox5.Text = comboBox6.Text = name.Text = sex.Text = age.Text = "";
		}
		private void timer3_Tick(object sender, EventArgs e)
		{
			Bll bll = new Bll();
			listtable = bll.GetlistDate(int.Parse(uid.Text));
			dataGridView1.Rows.Clear();
			foreach (DataRow row in listtable.Rows)
			{
				
				dataGridView1.Rows.Add(row["registrationrecord_patientid"], row["registrationrecord_num"]);
			}
			
		}

		private void 病历ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new Bingli().ShowDialog();
		}
	}
}
