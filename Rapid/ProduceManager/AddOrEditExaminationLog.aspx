<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditExaminationLog.aspx.cs"
    Inherits="Rapid.ProduceManager.AddOrEditExaminationLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                <asp:DropDownList ID="drpYear" runat="server" CssClass=" required number">
                    <asp:ListItem>2014</asp:ListItem>
                    <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>
                    <asp:ListItem>2017</asp:ListItem>
                    <asp:ListItem>2018</asp:ListItem>
                    <asp:ListItem>2019</asp:ListItem>
                    <asp:ListItem>2020</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblYear" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                月份：
            </td>
            <td>
                <asp:DropDownList ID="drpMonth" runat="server">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblMonth" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                姓名：
            </td>
            <td>
                <%-- <asp:TextBox ID="txtName" runat="server" CssClass="input required" size="25" HintTitle="姓名"
                    HintInfo="请输入姓名"></asp:TextBox>--%>
                <asp:DropDownList ID="drpName" runat="server">
                </asp:DropDownList>
                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                卷面成绩：
            </td>
            <td>
                <asp:TextBox ID="txtScore" runat="server" CssClass="input required number" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                组长评分：
            </td>
            <td>
                <asp:TextBox ID="txtLeaderScore" runat="server" CssClass="input required number"
                    size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                实操：
            </td>
            <td>
                <asp:TextBox ID="txtOperation" runat="server" CssClass="input required number" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                考勤：
            </td>
            <td>
                <asp:TextBox ID="txtWorkAttendance" runat="server" CssClass="input required number"
                    size="25"></asp:TextBox>
                <asp:Label ID="lbWorkAttendance" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                工作状态：
            </td>
            <td>
                <asp:TextBox ID="txtWorkState" runat="server" CssClass="input required number" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                团队合作：
            </td>
            <td>
                <asp:TextBox ID="txtTeamwork" runat="server" CssClass="input required number" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                不良品：
            </td>
            <td>
                <asp:TextBox ID="txtRejectsProduct" runat="server" CssClass="input required number"
                    size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                安全：
            </td>
            <td>
                <asp:TextBox ID="txtSecurity" runat="server" CssClass="input required number" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none">
            <td align="right">
                总分：
            </td>
            <td>
                <asp:TextBox ID="txtTotalScore" runat="server" CssClass="input number" size="25"
                    ReadOnly="true"></asp:TextBox>
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
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit"
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
