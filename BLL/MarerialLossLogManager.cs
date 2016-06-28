using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Model;
using System.Web.UI.WebControls;
using System.Data;

namespace BLL
{
   public  class MarerialLossLogManager
    {
       /// <summary>
       /// 绑定补领人
       /// </summary>
       /// <param name="drp"></param>
       public static void BindTakeMaterialPerson(DropDownList drp)
       {
           string error = string.Empty;
           string sql = string.Format(" select [USER_ID], [USER_NAME] FROM pm_user ");
          DataTable  dt = SqlHelper.GetTable(sql, ref error);
           if (dt.Rows.Count > 0)
           {
               drp.DataSource = dt;
               drp.DataValueField = "USER_ID";
               drp.DataTextField = "USER_NAME";
               drp.DataBind();
           } drp.Items.Insert(0, new ListItem("- - - 请 选 择 - - -", ""));

       }
       /// <summary>
       /// 通过补领人绑出班组
       /// </summary>
       /// <param name="drp"></param>
       /// <param name="TakeMaterialPerson"></param>
       public static void BindTeam(DropDownList drp, string TakeMaterialPerson)
       {
           string error = string.Empty;
           string sql = string.Format("  select Team FROM pm_user WHERE USER_ID='{0}'", TakeMaterialPerson);
           DataTable dt = SqlHelper.GetTable(sql, ref error);
           if (dt.Rows.Count > 0)
           {
               drp.DataSource = dt;
               drp.DataValueField = "Team";
               drp.DataTextField = "Team";
               drp.DataBind();
           } drp.Items.Insert(0, new ListItem("- - - 请 选 择 - - -", ""));

       }

    }
}
