<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditProduct.aspx.cs"
    Inherits="Rapid.ProduceManager.AddOrEditProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>产成品信息维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            //表单验证JS
            $("#form1").validate({
                //出错时添加的标签
                errorElement: "span",
                success: function(label) {
                    //正确时的样式
                    label.text(" ").addClass("success");
                }
            });
            $("#btnBack").click(function() {
                window.close(this);
            });
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写 &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr id="trProductNumber" runat="server">
            <td align="right">
                产成品编号：
            </td>
            <td>
                <asp:TextBox ID="txtProductNumber" runat="server" CssClass="required" size="25"></asp:TextBox>
                <asp:Label ID="lblProductNumber" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:TextBox ID="txtVersion" runat="server" CssClass="required" size="25" Text="WU"></asp:TextBox>
                <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>&nbsp;产品没有版本的话，请填写成"WU"。
            </td>
        </tr>
        <tr>
            <td align="right">
                型号：
            </td>
            <td>
                <asp:TextBox ID="txtProductName" runat="server"  size="25"></asp:TextBox>
            </td>
        </tr>
         <%--<tr>
            <td align="right">
                种类：
            </td>
            <td>
                <asp:TextBox ID="txtKind" runat="server"></asp:TextBox>
               
            </td>
        </tr>--%>
        <tr>
            <td align="right">
                类别：
            </td>
            <td>
                <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
       
            </td>
        </tr>

        <tr id="trRatedManhour" runat="server">
            <td align="right">
                额定工时：
            </td>
            <td>
                <asp:TextBox ID="txtRatedManhour" runat="server" CssClass="number digits" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                报价工时：
            </td>
            <td>
                <asp:TextBox ID="txtQuoteManhour" runat="server" CssClass="number" size="25" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr id='trCostPrice' runat="server">
            <td align="right">
                成本价：
            </td>
            <td>
                <asp:Label ID="lbCostPrice" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                销售报价：
            </td>
            <td>
                <asp:TextBox ID="txtSalesQuotation" runat="server" CssClass="number" size="25" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                半成仓库：
            </td>
            <td>
                <%--<asp:TextBox ID="txtHalfProductPosition" runat="server" CssClass="input required"
                    size="25"></asp:TextBox>--%>
                <asp:DropDownList ID="drpHalfProductPosition" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                产成仓库：
            </td>
            <td>
                <%-- <asp:TextBox ID="txtFinishedGoodsPosition" runat="server" CssClass="input required"
                    size="25"></asp:TextBox>--%>
                <asp:DropDownList ID="drpFinishedGoodsPosition" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                是否为旧版本：
            </td>
            <td>
                <%-- <asp:TextBox ID="txtFinishedGoodsPosition" runat="server" CssClass="input required"
                    size="25"></asp:TextBox>--%>
                <asp:DropDownList ID="drpIsOldVersion" runat="server">
                    <asp:ListItem Text="否" Value="否"></asp:ListItem>
                    <asp:ListItem Text="是" Value="是"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;选择是旧版本该条数据将以橘黄色显示。
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                货位：
            </td>
            <td>
                <%-- <asp:TextBox ID="txtFinishedGoodsPosition" runat="server" CssClass="input required"
                    size="25"></asp:TextBox>--%>
                <asp:TextBox ID="txtCargo" runat="server"></asp:TextBox>&nbsp;选填：便于库房人员填写。
            </td>
        </tr>
         <tr  >
            <td align="right">
                单位：
            </td>
            <td>
                <asp:TextBox ID="txtUnit" runat="server"  size="25" ></asp:TextBox>
            </td>
        </tr>
        <tr  >
            <td align="right">
                编号属性：
            </td>
            <td>
            <asp:TextBox ID="txtNumberProperties" runat="server" MaxLength="500" size="25" Height="31px"
                    Width="300px" ></asp:TextBox>
                <asp:Label ID="Label4" runat="server" Text="(限制输入500字)"></asp:Label>
              
            </td>
        </tr>
        <tr>
            <td align="right">
                描述：
            </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="required" size="25" MaxLength="200"
                    Height="31px" Width="300px"></asp:TextBox>
                <asp:Label ID="Label12" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Height="31px" Width="300px"
                   ></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;&nbsp;<%--<asp:Button ID="Button1" runat="server" Text="返回" 
                        CssClass="submit" onclick="back_Click" />--%>
                <input type="button" value="返回" id="btnBack" class="submit" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label11" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
