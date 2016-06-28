<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProductPlan.aspx.cs"
    Inherits="Rapid.ProduceManager.EditProductPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>开工单总表维护</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                开工单号：
            </td>
            <td>
                <asp:Label ID="lblPlanNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
          <tr>
               <td align="right">
                计划开始时间
            </td>
            <td>
                <asp:TextBox ID="txtPlanStartTime" runat="server" CssClass="input required" size="25"
                    onfocus="WdatePicker({skin:'green',dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
               <td align="right">
                计划结束时间
            </td>
            <td>
                <asp:TextBox ID="txtPlanEndTime" runat="server" CssClass="input required" size="25"
                    onfocus="WdatePicker({skin:'green',dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                实际开始时间：
            </td>
            <td>
                <asp:TextBox ID="txtFactStartTime" runat="server" CssClass="input" size="25"
                    onfocus="WdatePicker({skin:'green',dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                实际结束时间：
            </td>
            <td>
            <asp:TextBox ID="txtFactEndTime" runat="server" CssClass="input" size="25"
                    onfocus="WdatePicker({skin:'green',dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                 
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" Height="31px" Width="300px" MaxLength="200"
                    class="input requied"></asp:TextBox>
                <asp:Label ID="lblMemo" runat="server" Text="（限制输入200字）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="修改" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;
                <%--  <asp:Label ID="lbMsg" runat="server" Style="color: Red;" Text=""></asp:Label>--%>
                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
