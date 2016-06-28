using BLL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.StoreroomManager
{ 
    public partial class AjaxOrderCheck : Page
    {
        protected void Delete()
        {
            string str2;
            string requestString = ToolManager.GetRequestString("Id");
            try
            {
                OrderCheckBLL.Instance.Delete(new Guid(requestString));
                str2 = JsonConvert.SerializeObject(new { Status = true, Msg = string.Empty });
                base.Response.Write(str2);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception exception)
            {
                str2 = JsonConvert.SerializeObject(new { Status = false, Msg = exception.Message });
                base.Response.Write(str2);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void Get()
        {
            string requestString = ToolManager.GetRequestString("Id");
            string s = JsonConvert.SerializeObject(OrderCheckBLL.Instance.Get(new Guid(requestString)));
            base.Response.Write(s);
            base.Response.End();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string requestString = ToolManager.GetRequestString("Action");
            if (requestString != null)
            {
                if (!(requestString == "Save"))
                {
                    if (requestString == "Get")
                    {
                        this.Get();
                    }
                    else if (requestString == "Delete")
                    {
                        this.Delete();
                    }
                }
                else
                {
                    this.Save();
                }
            }
        }

        protected void Save()
        {
            string str2;
            string requestString = ToolManager.GetRequestString("Id");
            OrderCheck entity = new OrderCheck
            {
                MaterialNumber = ToolManager.GetRequestString("MaterialNumber"),
                TakeQty = ToolManager.GetRequestDouble("TakeQty"),
                TakeDateTime = ToolManager.GetRequestDateTime("TakeDateTime"),
                Status = (OrderCheckStatus)ToolManager.GetRequestInt("Status"),
                TakeUserName = ToolManager.GetRequestString("TakeUserName"),
                Remark = ToolManager.GetRequestString("Remark")
            };
            try
            {
                if (!string.IsNullOrEmpty(requestString))
                {
                    entity.Id = new Guid(requestString);
                    OrderCheckBLL.Instance.Update(entity);
                }
                else
                {
                    entity.Id = Guid.NewGuid();
                    entity.WarehouseNumber = ToolManager.GetRequestString("WarehouseNumber");
                    entity.OrderType = CheckOrderType.采购入库;
                    entity.CreateTime = DateTime.Now;
                    OrderCheckBLL.Instance.Create(entity);
                }
                str2 = JsonConvert.SerializeObject(new { Status = true, Msg = string.Empty });
                base.Response.Write(str2);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception exception)
            {
                str2 = JsonConvert.SerializeObject(new { Status = false, Msg = exception.Message });
                base.Response.Write(str2);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}