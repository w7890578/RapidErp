<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddHalfProduct.aspx.cs"
    Inherits="Rapid.StoreroomManager.AddHalfProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加半成品入库</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border tr td
        {
            padding: 2px;
            background-color: White;
        }
    </style>

    <script type="text/javascript">
        $(function() {

            $("#btnCK").click(function() {
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function() {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要出库的行！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定出库选中的数据?")) {
                    $.get("AddHalfProduct.aspx?sq="+new Date (), { time: new Date(), Data: ConvertsContent(checkResult) }, function(result) {
                        if (result == "1") {
                            alert("出库成功");
                            window.location.href = "HalfProductWarehouseLogList.aspx";
                        }
                        else {
                            alert(result);
                            return;
                        }
                    });
                }
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top: 5px;">
        <span>首页 >库存管理 ><a href="HalfProductWarehouseLogList.aspx">半成品出入库列表</a> >生成出库</span>
        <br />
        <br />
        <div style="text-align: center; font-size: 28px; font-weight: bold;">
            生成出库</div>
        <input type="button" value=" 出 库 " id="btnCK" style="margin-left: 10px; margin-bottom: 10px;" />
        <table cellpadding="1" cellspacing="1" class="border">
            <thead>
                <tr>
                    <td>
                        选择
                    </td>
                    <td>
                        开工单号
                    </td>
                    <td>
                        产成品编号
                    </td>
                    <td>
                        版本
                    </td>
                    <td>
                        客户产成品编号
                    </td>
                    <td>
                        缺料原材料编号
                    </td>
                    <td>
                        销售订单号
                    </td>
                    <td>
                        订单交期
                    </td>
                    <td>
                        数量
                    </td>
                    <td>
                        库存数量
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="checkbox" name='subBox' value="<%#Eval("Guid")%>" />
                            </td>
                            <td>
                                <%#Eval("DocumentNumber")%>
                            </td>
                            <td>
                                <%#Eval("ProductNumber")%>
                            </td>
                            <td>
                                <%#Eval("Version")%>
                            </td>
                             <td>
                                <%#Eval("CustomerProductNumber")%>
                            </td>
                            <td>
                                <%#Eval("MaterialNumber")%>
                            </td>
                            <td>
                                <%#Eval("SailOrderNumber")%>
                            </td>
                            <td>
                                <%#Eval("LeadTime")%>
                            </td>
                            <td>
                                <%#Eval("Qty")%>
                            </td>
                            <td>
                                <%#Eval("StockQty")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
