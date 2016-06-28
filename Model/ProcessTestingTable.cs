using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ProcessTestingTable
    {
        public string Id { get; set; }
        public string ProductionOrderNumber { get; set; }
        public string ProductNumber { get; set; }
        public string Version { get; set; }
        public string CustomerProductNumber { get; set; }
        public string ImportTime { get; set; }
        public string ImportPerson { get; set; }
        public string Remark { get; set; }
        public string ImgUrl{get;set;}
        public string FileName { get; set; }
    }
}
