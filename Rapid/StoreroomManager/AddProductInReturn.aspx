<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProductInReturn.aspx.cs" Inherits="Rapid.StoreroomManager.AddProductInReturn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

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
        })
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
          <tr>
            <td align="right">
                出入库编号：
            </td>
            <td>
                <asp:Label runat="server" ID="lbWarehouseNumber"  ></asp:Label>
            </td>
        </tr>
         <tr>
            <td align="right">
                销售订单号：
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtOrderNumber"  CssClass="input required"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" OnClick="btnSubmit_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <label style="color: Red;">
                    (*号为必填项)</label>
            </td>
        </tr>
         </table>
    </div>
    </form>
</body>
</html>
