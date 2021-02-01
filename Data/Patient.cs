using Covid19App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19App.Data
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public LevelModel? Level { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Status? Status { get; set; }
        public DateTime? VerifyDate  { get; set; }
        public string Note { get; set; }
        public double? Lng { get; set; }
        public double? Lat { get; set; }
        public DateTime? CreatedDt { get; set; } = DateTime.Now;
        public DateTime? ModifiedDt { get; set; } = DateTime.Now;
    }
}
