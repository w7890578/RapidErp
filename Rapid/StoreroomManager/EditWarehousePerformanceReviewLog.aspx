<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditWarehousePerformanceReviewLog.aspx.cs" Inherits="Rapid.StoreroomManager.EditWarehousePerformanceReviewLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>员工绩效信息维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        #tbMarerial
        {
            width: 100%;
            text-align: center;
        }
        #mText
        {
            cursor: pointer;
        }
        .bgGray
        {
            background-color: #EBEBEB;
        }
    </style>

    <script type="text/javascript">
        //数量计算
        function CalculateQuantity() {
            var FullScore = $("#txtFullScore").val();
            var score = $("#txtScore").val();
            var Deduction = 0;
            FullScore = parseInt(FullScore);
            score = parseInt(score);
            try {
                Deduction = FullScore - score;
            }
            catch (e) {
                Deduction = 0;
            }
            if (isNaN(Deduction) || Deduction < 0) {
                Deduction = 0;
            }

            $("#txtDeduction").val(Deduction);
        }
        $(function() {

            $("#txtFullScore").blur(function() {
                CalculateQuantity();
            });
            $("#txtScore").blur(function() {
                CalculateQuantity();
            });

            $("#txtDeduction").focus(function() {
                CalculateQuantity();
            });
            $("#form1").keypress(function(e) {
                if (e.which == 13) {
                    return false;
                }
            });
        });   
    </script>

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
    <input type="hidden" id="hdOderNumber" runat="server" />
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
                <asp:Label runat="server" ID="lbYear"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                月份：
            </td>
            <td>
                <asp:Label runat="server" ID="lbMonth"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                姓名：
            </td>
            <td>
                <asp:Label runat="server" ID="lbName"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                考核项目：
            </td>
            <td>
                <asp:Label runat="server" ID="lbProject"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                序号：
            </td>
            <td>
                <asp:Label ID="lblRowNumber" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                满分：
            </td>
            <td>
                <asp:Label ID="lblFullScore" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                得分：
            </td>
            <td>
                <asp:TextBox ID="txtScore" runat="server" CssClass="input required number" size="25"></asp:TextBox>
                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label>
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
                <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" OnClick="btnSubmit_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="返回" id="btnBack" class="submit" />&nbsp;
                <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

