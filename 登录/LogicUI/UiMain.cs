using System;
using System.Windows.Forms;
using LogicBLL;
using LogicMODEL;

namespace LogicUI
{
	public partial class UiMain : Form
	{
		
		public UiMain()
		{
			InitializeComponent();
		
		}
		/// <summary>
		/// 登录按钮点击事件。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "" && textBox2.Text != "")//验证登录前先判断账号和密码是否为空。
			{
				string password = new Bll().Md5Hash(textBox2.Text);//将用户输入的密码用MD5算法加密成32位密文。
				User user = new User {  //创建一个用户实体模型类对象，并为其赋值。
					user_account = textBox1.Text,
					user_password = password
				};
				if (new Bll().TryLogin(user))//判断账号和密码是否正确
				{
					string t = new Bll().GetUserType(user);//获取该账号对应的权限，根据权限加载不同的模块。
					switch (t)
					{
						case "医生":new DoctorWorkstationUI.UiMain(user.user_account,user.user_password).Show();this.WindowState = FormWindowState.Minimized;break;
						case "系统管理员":new UserManageUI.UiMain().Show();this.WindowState = FormWindowState.Minimized;break;
						case "挂号员":new RegisteredUI.UiMain(user.user_account,user.user_password).Show(); this.WindowState = FormWindowState.Minimized; break;
						case "收费员": new ChargesUI.UIMain(user.user_account,user.user_password).Show(); this.WindowState = FormWindowState.Minimized; break;
						case "药房人员":new PharmacyUI.UiMain(user.user_account).Show(); this.WindowState = FormWindowState.Minimized; break;

					}
				}
				else
				{
					MessageBox.Show("账号或密码错误！","提示");
				}
			}
			else
			{
				MessageBox.Show("请先输入账号和密码！","提示");
			}
			
		}
	}
}
