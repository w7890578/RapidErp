<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TableList.Master" AutoEventWireup="true"
    CodeBehind="T_ExaminationLog_KFList.aspx.cs" Inherits="Rapid.StoreroomManager.T_ExaminationLog_KFList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function Delete() {
            if (confirm("确定删除当前年度月份的数据？")) {
                return true;
            }
            return false;
        }
        $(function() {
            $("#navHead").html("&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;库房管理&nbsp;&nbsp;>&nbsp;&nbsp;库房考试成绩上报");
            $("#btnImp").click(function() {
                OpenDialog("ImpT_ExaminationLog_KFList.aspx", "ctl00_ContentPlaceHolder1_btnSearch", "520", "500");

            });
        })
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;    年度：<asp:DropDownList runat="server" ID="drpYear">
        <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
        <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
        <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
        <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
        <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
        <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
    </asp:DropDownList>
    月份：<asp:DropDownList runat="server" ID="drpMonth">
        <asp:ListItem Value="1" Text="1"></asp:ListItem>
        <asp:ListItem Value="2" Text="2"></asp:ListItem>
        <asp:ListItem Value="3" Text="3"></asp:ListItem>
        <asp:ListItem Value="4" Text="4"></asp:ListItem>
        <asp:ListItem Value="5" Text="5"></asp:ListItem>
        <asp:ListItem Value="6" Text="6"></asp:ListItem>
        <asp:ListItem Value="7" Text="7"></asp:ListItem>
        <asp:ListItem Value="8" Text="8"></asp:ListItem>
        <asp:ListItem Value="9" Text="9"></asp:ListItem>
        <asp:ListItem Value="10" Text="10"></asp:ListItem>
        <asp:ListItem Value="11" Text="11"></asp:ListItem>
        <asp:ListItem Value="12" Text="12"></asp:ListItem>
    </asp:DropDownList>
    姓名：<asp:TextBox runat="server" ID="txtName"></asp:TextBox>
    <asp:Button Text="查询" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
    <asp:Button Text="删除" ID="btnDelete" runat="server" OnClientClick="return Delete()"
        OnClick="btnDelete_Click" />
    <input type="button" value="导入" id="btnImp" />
    <asp:Label runat="server" ID="lbMsg" ForeColor="Red"></asp:Label>
    <table class="tablesorter" cellpadding="1" cellspacing="1">
        <thead>
            <tr>
                <td>
                    年度
                </td>
                <td>
                    月份
                </td>
                <td>
                    姓名
                </td>
                <td>
                    笔试得分
                </td>
                <td>
                    实操得分
                </td>
                <td>
                    总分
                </td>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater runat="server" ID="rpList">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("Year")%>
                        </td>
                        <td>
                            <%#Eval("Month")%>
                        </td>
                        <td>
                            <%#Eval("Name")%>
                        </td>
                        <td>
                            <%#Eval("BSScore")%>
                        </td>
                        <td>
                            <%#Eval("SCScore")%>
                        </td>
                        <td>
                            <%#Eval("SumScore")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</asp:Content>
