<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditExaminationLogList.aspx.cs" Inherits="Rapid.ProduceManager.EditExaminationLogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>员工考试成绩信息维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function JS() {
            var sum = 0;
            var Score = $("#txtScore").val();
            var Operation = $("#txtOperation").val();
            sum = parseInt(Score) + parseInt(Operation);
            $("#txtTotalScore").val(sum);

            return true;
        }
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
        <tr>
            <td align="right">
                年度：
            </td>
            <td>
               
                <asp:Label ID="lblYear" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                月份：
            </td>
            <td>
                
                <asp:Label ID="lblMonth" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                姓名：
            </td>
            <td>
                
                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
      
       <tr>
            <td align="right">
                笔试得分：
            </td>
            <td>
                <asp:TextBox ID="txtScore" runat="server" CssClass="input required number"
                    size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                实操得分：
            </td>
            <td>
                <asp:TextBox ID="txtOperation" runat="server" CssClass="input required number" size="25"></asp:TextBox>
            </td>
        </tr>
       
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Height="31px" Width="300px"
                    CssClass="input"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="修改" OnClick="btnSubmit_Click" CssClass="submit"
                    OnClientClick="return JS()" />
                &nbsp;&nbsp;&nbsp;&nbsp;<%--<asp:Button ID="Button1" runat="server" Text="返回" 
                        CssClass="submit" onclick="back_Click" />--%>
                <input type="button" value="返回" id="btnBack" class="submit" />&nbsp;&nbsp;
                <asp:Label ID="Label14" runat="server" ForeColor="Red" Text="（*为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
