using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorWorkstationMODEL
{
	public class Patient
	{
		public string patient_name { get; set; }

		public string patient_sex { get; set; }

		public DateTime patient_borndate { get; set; }
		public float patient_money { get; set; }
		public string patient_icd { get; set; }

		public string patient_address { get; set; }
		public int patient_id { get; set; }

		public sbyte patient_enable { get; set; }

		public string patient_phone { get; set; }
		public int patient_userid { get; set; }
		public string patient_gms { get; set; }

	}
}
