using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class PerformanceReviewStandard
    {
        public string StandardName { get; set; }
        public string PerformanceReviewItem { get; set; }
        public int RowNumber { get; set; }
        public int FullScore { get; set; }
        public string Description { get; set; }
        public int StatMode { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }

    }
}
