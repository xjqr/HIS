using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DoctorWorkstationMODEL;
using System.Configuration;
using IDAL;
using DAL;
using System.Data;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing;

namespace DoctorWorkstationBLL

{
	public class Bll
	{
        public static bool IsneedUpdatelist = false;
        private  string worktime;
        private string[] Texts;
        public bool UpdateRegistrationrecord(int id)
        {
            Idal idal = new Dal();
            return  idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.registrationrecord set registrationrecord_enable='0' where registrationrecord_id='{id}';SET FOREIGN_KEY_CHECKS=1;");
        }
        public  DataTable GetlistDate(int id)
        {
			try
			{
        Idal idal = new Dal();
            int num = int.Parse(idal.GetDataTable($"select * from his.v_doctor_user where user_account='{id}'").Rows[0]["doctor_id"].ToString());
            
            if (DateTime.Now.Hour < 12&&DateTime.Now.Hour>5)
            {
                worktime = "上午";
            }
            if((DateTime.Now.Hour >12||DateTime.Now.Hour== 12)&& DateTime.Now.Hour < 18)
            {
                worktime = "下午";
            }
            if (DateTime.Now.Hour >18||DateTime.Now.Hour==18)
            {
                worktime = "晚上";
            }
            return idal.GetDataTable($@"select registrationrecord_patientid,registrationrecord_num,registrationrecord_id from his.registrationrecord where  registrationrecord_enable='1' and registrationrecord_doctorid='{num}' and registrationrecord_workday='{worktime}' order by registrationrecord_num ASC");
			}
			catch
			{
                return new DataTable();
			}
    

        }
        public int GetdoctorID(string account)
        {
            Idal idal = new Dal();
            return int.Parse(idal.GetDataTable($"select doctor_id from his.v_doctor_user where user_account='{account}'").Rows[0]["doctor_id"].ToString());
        }
        public Patient GetPatient(int patient_id)
        {
            Idal idal = new Dal();
            return idal.SelectModel<Patient>($"patient_id='{patient_id}'");
        }
        public bool AddDrd(Drd drd)
        {
            Idal idal = new Dal();
            return idal.AddModelToDb(drd);
        }
        /// <summary>
        /// 启动缓存通知监听线程。
        /// </summary>
        public static void NoticelistenStart()
        {
            Task.Run(() => {

                while (true)
                {
                    string HostIp = GetIpAddress();
                    IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(HostIp), int.Parse(ConfigurationManager.AppSettings.Get("NoticeListenPort")));
                    Socket socketserver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socketserver.Bind(iPEndPoint);
                    socketserver.Listen(20);
                    Socket socketclient = socketserver.Accept();
                    Byte[] messagebuffer = new Byte[1024 * 1024 * 20];
                    socketclient.Receive(messagebuffer);
                    var message = Encoding.Default.GetString(messagebuffer);
                    socketserver.Close();
                    socketclient.Close();
                    if (message == "新增一个病人") { IsneedUpdatelist = true; }
                   
                }


            });

        }
        private static string GetIpAddress()
		{
			string AddressIP = string.Empty;
			foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
			{
				if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
				{
					AddressIP = _IPAddress.ToString();
				}
			}
			return AddressIP;
		}

        public DataTable GetMedicine(string type)
        {
            Idal idal = new Dal();
            return idal.GetDataTable($@"select * from his.medicine where medicine_type='{type}'");
        }
        public void Printing(string[] texts)
        {
            this.Texts = texts;
            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custum", 200, 250);
            printDocument.DocumentName = "";
            printDocument.PrintPage += new PrintPageEventHandler(this.PrintDocument_PrintPage);
            PrintDialog printDialog = new PrintDialog();
            printDialog.ShowDialog();
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();

        }
        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string time = System.DateTime.Now.ToString();
            e.Graphics.DrawString("XXX医院挂号单", new Font(new FontFamily("黑体"), 10), Brushes.Black, 50, 20);
            e.Graphics.DrawString("打印时间：" + time, new Font(new FontFamily("黑体"), 4), Brushes.Black, 60, 40);
            e.Graphics.DrawString("姓名：" + Texts[0], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 80);
            e.Graphics.DrawString("诊疗卡号：" + Texts[1], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 100);
            e.Graphics.DrawString("就诊医师：" + Texts[2], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 120);
            e.Graphics.DrawString("科室：" + Texts[3], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 140);
            e.Graphics.DrawString("挂号时间：" + Texts[4], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 160);
            e.Graphics.DrawString("排队号：" + Texts[5], new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 180);
            e.Graphics.DrawString("收费总额：" + Texts[6] + "元", new Font(new FontFamily("黑体"), 5), Brushes.Black, 65, 200);
        }
        public DataTable SelectBingli(int patient_id)
        {
            Idal idal = new Dal();
            return idal.GetDataTable($@" select * from his.v_drd_patient_doctor_department");
        }
        public DataTable JobStatistics(DateTime startime,DateTime endtime)
        {
            Idal idal = new Dal();
            return idal.GetDataTable($@"select drd_time,patient_name  from his.v_drd_patient_doctor_department where drd_time between '{startime:yyyy-MM-dd HH:mm:ss}' and '{endtime:yyyy-MM-dd HH:mm:ss}'");
        }
        public bool Addncdm(ncdm nc)
        {
            Idal idal = new Dal();
            return idal.AddModelToDb(nc);
        }
        public int Addchargerecord(int patientid)
        {
            Idal idal = new Dal();
            chargerecord chargerecord = new chargerecord { chargerecord_enable=1,chargerecord_patientid=patientid};
            idal.AddModelToDb(chargerecord);
            DataTable dataTable = idal.GetDataTable("select * from his.chargerecord");
            return int.Parse(dataTable.Rows[dataTable.Rows.Count - 1]["chargerecord_id"].ToString());

        }
           public bool Updatechargerecordmoney(float m,int id)
        {
            Idal idal = new Dal();
            return idal.DbModify($@"SET FOREIGN_KEY_CHECKS=0;update his.chargerecord set chargerecord_money='{m}' where chargerecord_id='{id}';SET FOREIGN_KEY_CHECKS=1;");
        }
        public float Getsellingprice(string medicinename)
        {
            string[] sp = medicinename.Split('(');
            Idal idal = new Dal();
           return  float.Parse( idal.GetDataTable($@"select * from his.medicine where Medicine_name='{sp[0]}'").Rows[0]["Medicine_sellingPrice"].ToString());
        }
    }
}
