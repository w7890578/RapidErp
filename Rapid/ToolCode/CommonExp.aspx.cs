using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.ToolCode
{
    public partial class CommonExp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtExp = Session["ExpTable"] as DataTable;
            string talbeName = Session["ExpTableName"] as String;
            ExcelHelper.Instance.ExpExcel(dtExp, talbeName);
        }
    }
}