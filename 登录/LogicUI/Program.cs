﻿using System;
using System.Windows.Forms;
using LogicMODEL;
namespace LogicUI
{
  public static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
	    public	static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new UiMain());
		}
	}
}
