using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorWorkstationMODEL
{
	public class Registrationrecord
	{

		public DateTime registrationrecord_time { get; set; }
		public int registrationrecord_patientid { get; set; }
		public int registrationrecord_registrarsid { get; set; }

		public int registrationrecord_departmentid { get; set; }
		public int registrationrecord_doctorid { get; set; }
		public int registrationrecord_num { get; set; }
		public sbyte registrationrecord_enable { get; set; }
		public float registrationrecord_money { get; set; }
	}
}
