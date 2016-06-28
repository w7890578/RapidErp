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
    public partial class Ajax_ChangeNumberInStorage : System.Web.UI.Page
    {
        protected void Delete()
        {
            string str2;
            string requestString = ToolManager.GetRequestString("Id");

            try
            {
                // OrderCheckBLL.Instance.Delete(new Guid(requestString));
                MaterialWarehouseLogDetailManager.Instance.Delete(requestString);
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
            var model = MaterialWarehouseLogDetailManager.Instance.Get(requestString);
            string s = JsonConvert.SerializeObject(model);
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
                    else if (requestString == "Autior")
                    {
                        string str2 = string.Empty;
                        try
                        {
                            string tempResult = MarerialWarehouseLogListManager.AuditorMarerialWarehouseLogForWarehouseNumber(ToolCode.Tool.GetUser().UserNumber, ToolManager.GetRequestString("WarehouseNumber"));
                            if (!tempResult.Equals("1"))
                            {
                                str2 = JsonConvert.SerializeObject(new { Status = false, Msg = tempResult });
                            }
                            else
                            {
                                str2 = JsonConvert.SerializeObject(new { Status = true, Msg = string.Empty });

                            }
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
                else
                {
                    this.Save();
                }
            }
        }

        protected void Save()
        {
            string str2;
            MaterialWarehouseLogDetail entity = new MaterialWarehouseLogDetail
            {
                Guid = ToolManager.GetRequestString("Id"),
                MaterialNumber = ToolManager.GetRequestString("MaterialNumber"),
                WarehouseNumber = ToolManager.GetRequestString("WarehouseNumber"),
                DocumentNumber = ToolManager.GetRequestString("DocumentNumber"),
                Qty = ToolManager.GetRequestDouble("Qty"),
                Remark = ToolManager.GetRequestString("Remark"),
                SupplierName = ToolManager.GetRequestString("SupplierName"),
            };
            try
            {
                if (!string.IsNullOrEmpty(entity.Guid))
                {
                    MaterialWarehouseLogDetailManager.Instance.Update(entity);
                }
                else
                {
                    entity.Guid = Guid.NewGuid().ToString();
                    MaterialWarehouseLogDetailManager.Instance.Create(entity);
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