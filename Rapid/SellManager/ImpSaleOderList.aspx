<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpSaleOderList.aspx.cs"
    Inherits="Rapid.SellManager.ImpSaleOderList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>销售订单维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        function wite() {
            $("#lbMsg").html("正在导入！请稍等......");
            return true;
        }

        $(function() {
            //表单验证JS
            $("#form2").validate({
                //出错时添加的标签
                errorElement: "span",
                success: function(label) {
                    //正确时的样式
                    label.text(" ").addClass("success");
                }
            });
        })
    </script>

</head>
<body>
    <form id="form2" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr style="display: none;">
            <td align="right">
                生产类型：
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpProductType" AutoPostBack="True" OnSelectedIndexChanged="drpProductType_SelectedIndexChanged">
                    <asp:ListItem Text="加工" Value="加工"></asp:ListItem>
                    <asp:ListItem Text="贸易" Value="贸易"></asp:ListItem>
                </asp:DropDownList>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                订单类型：
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpOdersType" CssClass="required" AutoPostBack="true">
                </asp:DropDownList>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户采购订单号：
            </td>
            <td>
                <asp:TextBox ID="txtCustomerOrderNumber" runat="server" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户订单号：
            </td>
            <td>
                <asp:TextBox ID="txtKhddH" runat="server" size="25"></asp:TextBox>
                <asp:Label ID="Label1" runat="server">&nbsp;此选项仅仅提供加工包装生产订单录入，其它类型订单不用录入。</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                订单日期：
            </td>
            <td>
                <asp:TextBox ID="txtOrdersDate" runat="server" CssClass="input required" size="25"
                    onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                是否减库存：
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpIsMinusStock" CssClass="required">
                    <asp:ListItem Text="是" Value="是"></asp:ListItem>
                    <asp:ListItem Text="否" Value="否"></asp:ListItem>
                </asp:DropDownList>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户：
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpClient" CssClass="required" AutoPostBack="True"
                    OnSelectedIndexChanged="drpClient_SelectedIndexChanged" Width="160px">
                </asp:DropDownList>
                <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                收款方式：
            </td>
            <td>
                <asp:DropDownList ID="drpMakeCollectionsMode" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                业务员：
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpContact" CssClass="required">
                    <asp:ListItem Text="管理员" Value="d"></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" size="25" Height="31px"
                    Width="300px"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                Excel文件：
            </td>
            <td>
                <div>
                    <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpload"
                        runat="server" Text=" 导 入 " OnClick="btnUpload_Click" OnClientClick="return wite()" />&nbsp;&nbsp;&nbsp;
                    <a href="SaleOderList.aspx">返回列表</a><br />
                </div>
            </td>
        </tr>
         <tr>
            <td align="right">
                Excel导入模板：
            </td>
            <td>
                <div>
                   <a target="_blank" href="../Text/销售订单通用导入模板.xls">销售订单通用导入模板（含加工贸易）.xls</a>
                </div>
            </td>
        </tr>
       
        <tr>
            <td colspan="2">
                <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
