<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddExaminationLogListForUser.aspx.cs" Inherits="Rapid.ProduceManager.AddExaminationLogListForUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>单个员工绩效上报</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        #tbMarerial {
            width: 100%;
            text-align: center;
        }

        #mText {
            cursor: pointer;
        }

        .bgGray {
            background-color: #EBEBEB;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            //表单验证JS
            $("#form1").validate({
                //出错时添加的标签
                errorElement: "span",
                success: function (label) {
                    //正确时的样式
                    label.text(" ").addClass("success");
                }
            });
            $("#btnBack").click(function () {
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
                <th colspan="2" align="left">基本信息填写 &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
                </th>
            </tr>
            <tr>
                <td align="right">年度：
                </td>
                <td>
                    <asp:DropDownList ID="drpYear" runat="server">
                        <asp:ListItem>2013</asp:ListItem>
                        <asp:ListItem>2014</asp:ListItem>
                        <asp:ListItem>2015</asp:ListItem>
                        <asp:ListItem>2016</asp:ListItem>
                        <asp:ListItem>2017</asp:ListItem>
                        <asp:ListItem>2018</asp:ListItem>
                        <asp:ListItem>2019</asp:ListItem>
                        <asp:ListItem>2020</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">月份：
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
                </td>
            </tr>
            <tr>
                <td align="right">姓名：</td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="input required"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">笔试得分：</td>
                <td>
                    <asp:TextBox ID="txtBS" runat="server" CssClass="input required digits"></asp:TextBox></td>
            </tr>
             <tr>
                <td align="right">实操得分：</td>
                <td>
                    <asp:TextBox ID="txtSC" runat="server" CssClass="input required digits"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="上报" CssClass="submit" OnClick="btnSubmit_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="返回" id="btnBack" class="submit" />&nbsp;
                <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
