<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Edit.Master" AutoEventWireup="true"
    CodeBehind="AddOrEditCertificateOrdersList.aspx.cs" Inherits="Rapid.PurchaseManager.AddOrEditCertificateOrdersList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>采购单信息维护</title>
    <link href="../Js/AutoComplete/AutoComplete.css" rel="stylesheet" />
    <script src="../Js/AutoComplete/AutoComplete.js" type="text/javascript"></script>
    <script type="text/javascript">
        var autoComplete;
        $(function () {
            $("#aspnetForm").keydown(function (event) {
                if (event.keyCode == 13) {
                    event.returnValue = false;
                    return false;
                }
            })
            $("#ctl00_bText").html("您当前的位置：采购单信息");
            $("#aspnetForm").validate({
                //出错时添加的标签
                errorElement: "span",
                success: function (label) {
                    //正确时的样式
                    label.text(" ").addClass("success");
                }
            });
            var SupplierIds = "<%=SupplierIds%>";
            var arrays = SupplierIds.split(",");
            autoComplete = new AutoComplete('ctl00_ContentPlaceHolder1_SupplierName', 'auto', arrays);
        })

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <tr style="display: none">
        <td align="right">采购订单号：
        </td>
        <td>
            <asp:TextBox ID="txtOrderNumber" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">采购合同号：
        </td>
        <td>
            <asp:TextBox ID="txtHTNumber" runat="server"></asp:TextBox><br />
            采购合同号：用户输入的合同号。
            采购订单编号：系统自动生成的编号。
        </td>
    </tr>
    <tr style="display: none;">
        <td align="right">物料类型：
        </td>
        <td>
            <asp:DropDownList runat="server" ID="drpOrderType">
                <asp:ListItem>原材料</asp:ListItem>
                <asp:ListItem>产成品</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right">付款方式：
        </td>
        <td>
            <asp:DropDownList runat="server" ID="drpPaymentMode" CssClass="required">
            </asp:DropDownList>
            <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">采购订单日期：
        </td>
        <td>
            <asp:TextBox ID="txtOrdersDate" runat="server" CssClass="input required" size="25"
                onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
            <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">供应商：
        </td>
        <td>
            <%-- <asp:DropDownList runat="server" ID="drpSupplierId" CssClass="required">
            </asp:DropDownList>--%>
            <div style="position: relative;">
                <input type="text" style="height: 15px; width: 250px; font-size: 12px;" placeholder="请输入供应商名称" id="SupplierName" name="SupplierName" onkeyup="autoComplete.start(event)" runat="server"  class="required"/>
                <div class="auto_hidden" id="auto" >
                    <!--自动完成 DIV-->
                </div>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </div>

        </td>
    </tr>
    <tr>
        <td align="right">业务员：
        </td>
        <td>
            <asp:DropDownList runat="server" ID="drpContactId" CssClass="required">
            </asp:DropDownList>
            <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">备注：
        </td>
        <td>
            <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" size="25" Height="31px"
                Width="300px" class="input"></asp:TextBox>
            <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right"></td>
        <td>
            <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" OnClick="btnSubmit_Click" />
            <span style="color: Red;">&nbsp;&nbsp;（*为必填项）</span>
        </td>
    </tr>
</asp:Content>
