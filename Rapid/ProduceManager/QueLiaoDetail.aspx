<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueLiaoDetail.aspx.cs"
    Inherits="Rapid.ProduceManager.QueLiaoDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border thead tr td
        {
            padding: 4px;
            background-color: white;
        }
        .border tbody tr td
        {
            padding: 4px;
            background-color: white;
        }
        a
        {
            color: Blue;
        }
        a:hover
        {
            color: Red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div style ="text-align :center ;font-size:22px;">缺料明细</div>
        <div style ="margin :10px;">
            &nbsp;注意：正常情况下，物料单机用量和总物料单机用量值应该一致。当BOM内出现重复物料的
            情况下物料单机用量*数量=总物料单机用量&nbsp;&nbsp;<a href="WorkOrder.aspx">返回列表</a></div>
        <table cellspacing="1" cellpadding="1" class="border">
            <tr>
                <td style="display: <%=show%>;">
                    客户包号
                </td>
                <td style="display: <%=show%>;">
                    包号
                </td>
                <td>
                    客户产成品编号
                </td>
                <td>
                    产成品编号
                </td>
                <td>
                    版本
                </td>
                <td style="display: <%=show%>;">
                    产品单机
                </td>
                <td>
                    原材料编号
                </td>
                <td>
                    客户物料号
                </td>
                <td>
                    物料单机用量
                </td>
                <td>
                    总物料单机用量
                </td>
                <td>
                    单位
                </td>
                <td>
                    实际生产数量
                </td>
                <td>
                    总实际生产数量
                </td>
                <td>
                    库存数量
                </td>
                <td>
                    缺料数量
                </td>
            </tr>
            <asp:Repeater runat="server" ID="rpList">
                <ItemTemplate>
                    <tr>
                        <td style="display: <%=show%>;">
                            <%#Eval("客户包号")%>
                        </td>
                        <td style="display: <%=show%>;">
                            <%#Eval("包号")%>
                        </td>
                        <td>
                            <%#Eval("客户产成品编号")%>
                        </td>
                        <td>
                            <%#Eval("产成品编号")%>
                        </td>
                        <td>
                            <%#Eval("版本")%>
                        </td>
                        <td style="display: <%=show%>;">
                            <%#Eval("产品单机")%>
                        </td>
                        <td>
                            <%#Eval("原材料编号")%>
                        </td>
                        <td>
                            <%#Eval("客户物料号")%>
                        </td>
                        <td>
                            <%#Eval("物料单机用量")%>
                        </td>
                        <td>
                            <%#Eval("总物料单机用量")%>
                        </td>
                        <td>
                            <%#Eval("单位")%>
                        </td>
                        <td>
                            <%#Eval("实际生产数量")%>
                        </td>
                        <td>
                            <%#Eval("总实际生产数量")%>
                        </td>
                        <td>
                            <%#Eval("库存数量")%>
                        </td>
                        <td>
                            <span style='color: <%#Eval("缺料数量").ToString ().Contains ("-")?"red":"black"%>;'>
                                <%#Eval("缺料数量")%></span>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
