using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19App.Models
{
    public class PatientViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string PatientGroup { get; set; }
        public string Note { get; set; }
        public DateTime VerifyDate { get; set; }
    }
    public class PatientAdminViewModel
    {
        public int Id { get; set; }
        public string Parent { get; set; }
        public LevelModel Level { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Status{ get; set; }
        public string Note { get; set; }
        public DateTime VerifyDate { get; set; } = new DateTime(2000, 1, 1);
        public double Lng { get; set; }
        public double Lat { get; set; }
        public DateTime CreatedDt { get; set; } = DateTime.Now;
        public DateTime ModifiedDt { get; set; } = DateTime.Now;
    }
}
