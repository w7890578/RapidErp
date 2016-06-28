<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditMarerialInfoTable.aspx.cs"
    Inherits="Rapid.ProduceManager.AddOrEditMarerialInfoTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>原材料信息维护</title>
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
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr id="trMaterialNumber" runat="server">
            <td align="right">
                原材料编号：
            </td>
            <td>
                <asp:TextBox ID="txtMaterialNumber" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1">

                </asp:TextBox>&nbsp;<label style="color: Red;">禁止输入中文</label>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                型号：
            </td>
            <td>
                <asp:TextBox ID="txtMaterialName" runat="server" CssClass="input" size="25" MaxLength="50"
                    minlength="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                描述：
            </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                种类：
            </td>
            <td>
                <asp:TextBox ID="txtKind" runat="server"></asp:TextBox>
                <%--<asp:DropDownList ID="drpKind" runat="server">
                    <asp:ListItem>光缆</asp:ListItem>
                    <asp:ListItem>线材</asp:ListItem>
                    <asp:ListItem>连接器</asp:ListItem>
                    <asp:ListItem>其它</asp:ListItem>
                </asp:DropDownList>--%>
            </td>
        </tr>
        <tr>
            <td align="right">
                类别：
            </td>
            <td>
                <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
                <%--        <asp:DropDownList ID="drpType" runat="server">
                    <asp:ListItem>单芯电线</asp:ListItem>
                    <asp:ListItem>多芯电线</asp:ListItem>
                    <asp:ListItem>扁平线</asp:ListItem>
                    <asp:ListItem>同轴线</asp:ListItem>
                    <asp:ListItem>连接器</asp:ListItem>
                    <asp:ListItem>端子</asp:ListItem>
                    <asp:ListItem>光缆</asp:ListItem>
                    <asp:ListItem>网管</asp:ListItem>
                    <asp:ListItem>其它</asp:ListItem>
                    <asp:ListItem>护套、壳、管、盖</asp:ListItem>
                </asp:DropDownList>--%>
            </td>
        </tr>
        <tr>
            <td align="right">
                品牌：
            </td>
            <td>
                <asp:TextBox ID="txtBrand" runat="server" CssClass="input" size="25" MaxLength="50"
                    minlength="1"></asp:TextBox>
            </td>
        </tr>
        <%--  <tr style ="display :none ;">
            <td align="right">
                库存安全值：
            </td>
            <td>
                <asp:TextBox ID="txtStockSafeQty" runat="server" CssClass="input required number"
                    size="25" MaxLength="50" minlength="1" Text="0"></asp:TextBox>
            </td>
        </tr>--%>
        <%--<tr>
            <td align="right">
                6个月库存安全值：
            </td>
            <td>
                <asp:TextBox ID="txtSixStockSafeQty" runat="server" CssClass="input required number"
                    size="25" MaxLength="50" minlength="1" Text ="0"></asp:TextBox>
            </td>
        </tr>--%>
        <tr style="display: none">
            <td align="right">
                采购价格：
            </td>
            <td>
                <asp:TextBox ID="txtProcurementPrice" runat="server" size="25" MaxLength="50" minlength="1"
                    Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                原材料仓库：
            </td>
            <td>
                <%--  <asp:TextBox ID="txtMaterialPosition" runat="server" CssClass="input required" size="25"></asp:TextBox>--%>
                <asp:DropDownList ID="drpMaterialPosition" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                最小包装：
            </td>
            <td>
                <asp:TextBox ID="txtMinPacking" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                最小起订量：
            </td>
            <td>
                <asp:TextBox ID="txtMinOrderQty" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                废品仓库：
            </td>
            <td>
                <asp:DropDownList ID="drpScrapPosition" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="display: none">
            <td align="right">
                货位：
            </td>
            <td>
                <asp:TextBox ID="txtCargo" runat="server" CssClass="input" size="25" MaxLength="50"
                    minlength="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                货物类型：
            </td>
            <td>
                <asp:TextBox ID="txtCargoType" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                单位：
            </td>
            <td>
                <asp:TextBox ID="txtUnit" runat="server" CssClass="input" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                编号属性：
            </td>
            <td>
                <asp:TextBox ID="txtNumberProperties" runat="server" MaxLength="500" size="25" Height="31px"
                    Width="300px" CssClass="input"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" Text="(限制输入500字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" size="25" Height="31px"
                    Width="300px" CssClass="input"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="返回" id="btnBack" class="submit" />
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
