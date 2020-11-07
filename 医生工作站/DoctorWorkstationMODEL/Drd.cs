using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorWorkstationMODEL
{
public 	class Drd
	{
		//public int drd_id { get; set; }
		public DateTime drd_time { get; set; }

		public int drd_patientid { get; set; }
		public int drd_doctorid { get; set; }
		public string drd_checkresult { get; set; }
		public string drd_mainRemarks { get; set; }
		public string drd_pastHistory { get; set; }
		public string drd_nowHistory { get; set; }
		public string drd_diagnosticResults { get; set; }
		public string drd_prescription { get; set; }

		public string drd_yizhu { get; set; }
	}
}
