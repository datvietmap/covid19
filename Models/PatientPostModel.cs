using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19App.Models
{
    public class PatientPostModel
    {
        public int? ParentId { get; set; }
        [Required(ErrorMessage ="Patient Group is not empty")]
        public LevelModel? Level { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        [Required(ErrorMessage = "Lng is not empty")]
        public double Lng { get; set; }
        [Required(ErrorMessage = "Lat is not empty")]
        public double Lat { get; set; }
    }
}
