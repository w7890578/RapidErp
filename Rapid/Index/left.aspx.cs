using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using System.Data;

namespace Rapid.Index
{
    public partial class left : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("menuId"))
                {
                    string menuId = ToolManager.GetQueryString("menuId");
                    string userId = ToolCode.Tool.GetUser().UserNumber;
                    string sql = string.Format(@"  select * from pm_menu_erp where (parent_id='{0}' or menu_id='{0}') and IsVisible=1 and  Menu_Id in (
select   MENU_ID  from PM_USER_MENU_PERMISSION  where APP_ID ='Rapid-Erp' and USER_ID='{1}'
union
select  MENU_ID  from PM_ROLE_MENU_PERMISSION  where APP_ID ='Rapid-Erp'
and ROLE_ID =(select RoleId  from PM_USER  where USER_ID='{1}'))  order by sn asc", menuId, userId);
                    DataTable dt = SqlHelper.GetTable(sql);
                    DataRow drFirst = dt.Rows[0];
                    string firstLi = string.Format("<li><span class='folderMenu {0}'>{1}</span>", drFirst["Menu_Iconcls"], drFirst["Menu_Name"]);
                    dt.Rows.RemoveAt(0);
                    string tempUl = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tempUl += string.Format("  <li><a href='{0}' target='I1' class='folderMenu {1}'>&nbsp;{2}</a></li>", dr["Menu_Action"], dr["Menu_Iconcls"], dr["Menu_Name"]);
                    }
                    string result = string.Format(@" {0}
                                    <ul>
                                       {1}
                                    </ul>
                                </li>", firstLi, tempUl);
                    Response.Write(result);
                    Response.End();
                    return;
                }
                BindMenu();
            }
        }
        private void BindMenu()
        {
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string sql = string.Format(@"select * from Pm_Menu_Erp where  Parent_Id ='ROOT_MENU' and Menu_Id in (
select   MENU_ID  from PM_USER_MENU_PERMISSION  where APP_ID ='Rapid-Erp' and USER_ID='{0}'
union
select  MENU_ID  from PM_ROLE_MENU_PERMISSION  where APP_ID ='Rapid-Erp' and ROLE_ID =(select RoleId  from PM_USER  where USER_ID='{0}'))", userId);
            rpMenu.DataSource = SqlHelper.GetTable(sql);
            rpMenu.DataBind();
        }
    }
}
