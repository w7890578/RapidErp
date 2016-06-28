using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
  public  class PerformanceReviewLog
    {
      public string Year { get; set; }
      public string Month { get; set; }
      public string PerformanceReviewItem { get; set; }
      public int RowNumber { get; set; }
      public int FullScore { get; set; }
      public int Deduction { get; set; }
      public int Score { get; set; }
      public string Description { get; set; }
      public int StatMode { get; set; }
      public string Remark { get; set; }
      public string Name { get; set; }

    }
}
