using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ExaminationLog
    {
        public string Id { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Name { get; set; }
        public decimal Score { get; set; }
        public decimal LeaderScore { get; set; }
        public decimal TotalScore { get; set; }
        public decimal Operation { get; set; }
        public decimal WorkAttendance { get; set; }
        public decimal WorkState { get; set; }
        public decimal Teamwork { get; set; }
        public decimal RejectsProduct { get; set; }
        public decimal Security { get; set; }
        public string Remark { get; set; }
    }
}
