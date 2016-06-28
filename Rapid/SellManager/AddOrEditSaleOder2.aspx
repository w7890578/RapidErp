<%@ Page Title="" Language="C#" MasterPageFile="~/Master/easyui.Master" AutoEventWireup="true" CodeBehind="AddOrEditSaleOder2.aspx.cs" Inherits="Rapid.SellManager.AddOrEditSaleOder2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Js/AutoComplete/AutoComplete.css" rel="stylesheet" />
    <script src="../Js/AutoComplete/AutoComplete.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%
        string CustomerNames = string.Empty;
        foreach (System.Data.DataRow dr in dtClient.Rows)
        {
            CustomerNames += dr["CustomerName"].ToString() + ",";
        }
        CustomerNames = CustomerNames.TrimEnd(',');
    %>
    <script type="text/javascript">
        var autoComplete;
        $(function () {
            var CustomerNames = "<%=CustomerNames%>";
            var arrays = CustomerNames.split(",");
            autoComplete = new AutoComplete('CustomerName', 'auto', arrays);
        })

    </script>
    <form id="fmDetail">
        <input type="hidden" name="Id" value="<%=model.OdersNumber %>" />
        <table>
            <tr>
                <td style="width: 150px; text-align: right;">生产类型:</td>
                <td style="width: 150px;">
                    <select id="product_type" name="ProductType">

                        <%if (model.ProductType == "加工")
                          { %>
                        <option selected="selected">加工</option>
                        <option>贸易</option>
                        <%}
                          else
                          {%>
                        <option>加工</option>
                        <option selected="selected">贸易</option>
                        <%} %>
                    </select>
                </td>
                <td style="width: 150px; text-align: right;">订单类型:</td>
                <td style="width: 150px;">
                    <select id="oder_type" name="OdersType">
                    </select>
                </td>
                <td style="width: 150px; text-align: right;">客户采购订单号:</td>
                <td style="width: 150px;">
                    <input type="text" class="easyui-validatebox" required="required" name="CustomerOrderNumber" value="<%=model.CustomerOrderNumber %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 150px; text-align: right;">客户订单号:</td>
                <td style="width: 150px;">
                    <input type="text" class="easyui-validatebox" required="required" name="KhddH" value="<%=model.KhddH %>" />

                </td>
                <td style="width: 150px; text-align: right;">订单日期:</td>
                <td style="width: 150px;">
                    <input type="text" class="easyui-datebox" required="required" name="OrdersDate" data-option="editable:false;" value="<%=model.OrdersDate %>" />

                </td>
                <td style="width: 150px; text-align: right;">客户:</td>
                <td style="width: 150px;">
                    <%-- <select name="CustomerId" class="easyui-combobox" id="CustomerId">
                        <%foreach (System.Data.DataRow dr in dtClient.Rows)
                          { %>
                        <option
                            <%if (dr["CustomerId"].ToString() == model.CustomerId)
                              { %>
                            selected="selected"
                            <%} %>
                            value="<%=dr["CustomerId"] %>"><%=dr["CustomerName"] %></option>
                        <%} %>
                    </select>--%>
                    <div style="position: relative;">
                        <input type="text" style="height: 15px; width: 250px; font-size: 12px;" placeholder="请输入客户名称" id="CustomerName" name="CustomerName" onkeyup="autoComplete.start(event)" autocomplete="off" required="required" value="<%=model.CustomerName %>" />
                        <div class="auto_hidden" id="auto">
                            <!--自动完成 DIV-->
                        </div>
                    </div>

                </td>
            </tr>
            <tr>

                <td style="width: 150px; text-align: right;">业务员:</td>
                <td style="width: 150px;">
                    <select name="ContactId" class="easyui-combobox">
                        <%foreach (System.Data.DataRow dr in dtContact.Rows)
                          { %>
                        <option
                            <%if (dr["user_id"].ToString() == model.ContactId)
                              { %>
                            selected="selected"
                            <%} %>
                            value="<%=dr["user_id"] %>"><%=dr["USER_NAME"] %></option>
                        <%} %>
                    </select>

                </td>
                <%if (!string.IsNullOrEmpty(model.OdersNumber))
                  { %>
                <td style="width: 150px; text-align: right;">收款方式:</td>
                <td style="width: 150px;">
                    <select name="makeCollectionsMode">
                        <%foreach (System.Data.DataRow dr in dtMakeCollectionsMode.Rows)
                          { %>
                        <option
                            <%if (dr[0].ToString() == model.MakeCollectionsMode)
                              { %>
                            selected="selected"
                            <%} %>
                            value="<%=dr[0] %>"><%=dr[1] %></option>
                        <%} %>
                    </select>
                </td>
                <%} %>
            </tr>
            <tr>
                <td style="width: 150px; text-align: right;">备注:</td>
                <td colspan="5">
                    <input name="Remark" value="" type="hidden" id="Remark" />
                    <textarea rows="5" cols="20" style="width: 100%;" id="Remark_t"><%=model.Remark %></textarea>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <label style="color: red; margin-left: 30px;">ps：客户支持模糊筛选。收款方式在选择客户后自动匹配，编辑时可修改。</label></td>
                <td colspan="1">
                    <a href="javascript:void(0)" class="easyui-linkbutton" id="btnAddOK" iconcls="icon-ok" onclick="SaveEntity()">确定</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-cancel" onclick="javascript:window.close();">关闭</a>
                </td>
            </tr>

        </table>

    </form>

    <script type="text/ecmascript">

        function SaveEntity() {
            $('#fmDetail').form('submit', {
                url: "ajax/addoreditSaleOder.aspx",
                onSubmit: function (param) {                //提交时触发
                    var flag = $(this).form('validate');    //是否通过验证 
                    jQuery("#Remark").val(jQuery("#Remark_t").val());
                    var orderType = jQuery("#oder_type").val();
                    if (orderType == "" || orderType == null) {
                        alert("请选择订单类型");
                        return false;

                    };
                    return flag;
                },
                success: function (res) {
                    if (res == "ok") {
                        $.messager.alert("操作提示", "保存成功！", "info");
                        window.close();
                    }
                    else {
                        $.messager.alert("操作提示", res, "error");
                    }
                }
            })
        }
        jQuery(function () {
            jQuery("#product_type").change(function () {
                var product_type = jQuery(this).val();
                if (product_type == "加工") {
                    jQuery("#oder_type").empty()
                    .append("<option>正常订单</option>")
                    .append("<option>加急订单</option>")
                    .append("<option>维修订单</option>")
                    .append("<option>临时订单</option>")
                    .append("<option>样品订单</option>")
                    .append("<option>包装生产订单</option>");
                }
                else {
                    jQuery("#oder_type").empty()
                   .append("<option>正常订单</option>")
                   .append("<option>加急订单</option>")
                   .append("<option>维修订单</option>")
                   .append("<option>样品订单</option>");
                }
            });
            jQuery("#product_type").change();
            $(".datebox :text").attr("readonly", "readonly");
            var OdersType = "<%=model.OdersType%>";
            $("#oder_type").val(OdersType);
        })

    </script>

</asp:Content>
