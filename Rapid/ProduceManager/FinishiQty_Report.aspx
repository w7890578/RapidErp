<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinishiQty_Report.aspx.cs" Inherits="Rapid.ProduceManager.FinishiQty_Report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server">
        <div>
            <div style="text-align: center; font-size: 20px; font-weight: bold; margin-top: 20px;">
                月度完成数量报表 
                
            </div>
            <div style="text-align: center; margin-top:10px;font-size:15px;  "><span style="color:red;">PS:由于是新做页面，该数据自2015年8月4日起开始统计，在此之前的数据无法在此页面统计出。</span></div>
            <div style="text-align: center; margin: 20px;">
                年度：
        <asp:DropDownList runat="server" ID="drpYear" OnSelectedIndexChanged="drpYear_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="2014" Text="2014年"></asp:ListItem>
            <asp:ListItem Value="2015" Text="2015年"></asp:ListItem>
            <asp:ListItem Value="2016" Text="2016年"></asp:ListItem>
            <asp:ListItem Value="2017" Text="2017年"></asp:ListItem>
            <asp:ListItem Value="2018" Text="2018年"></asp:ListItem>
            <asp:ListItem Value="2019" Text="2019年"></asp:ListItem>
            <asp:ListItem Value="2020" Text="2020年"></asp:ListItem>
            <asp:ListItem Value="2021" Text="2021年"></asp:ListItem>
            <asp:ListItem Value="2022" Text="2022年"></asp:ListItem>
        </asp:DropDownList>&nbsp;&nbsp;
        月份：
        <asp:DropDownList ID="drpMonth" runat="server" OnSelectedIndexChanged="drpMonth_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="" Text="- 请选择 -"></asp:ListItem>
            <asp:ListItem Value="01" Text="1月"></asp:ListItem>
            <asp:ListItem Value="02" Text="2月"></asp:ListItem>
            <asp:ListItem Value="03" Text="3月"></asp:ListItem>
            <asp:ListItem Value="04" Text="4月"></asp:ListItem>
            <asp:ListItem Value="05" Text="5月"></asp:ListItem>
            <asp:ListItem Value="06" Text="6月"></asp:ListItem>
            <asp:ListItem Value="07" Text="7月"></asp:ListItem>
            <asp:ListItem Value="08" Text="8月"></asp:ListItem>
            <asp:ListItem Value="09" Text="9月"></asp:ListItem>
            <asp:ListItem Value="10" Text="10月"></asp:ListItem>
            <asp:ListItem Value="11" Text="11月"></asp:ListItem>
            <asp:ListItem Value="12" Text="12月"></asp:ListItem>
        </asp:DropDownList>&nbsp;&nbsp;
        <asp:Button runat="server" ID="btnSearch" Text=" 统 计 " OnClick="btnSearch_Click" />
            </div>
            <div style="text-align: center; margin: 20px;">
                <table class="border" cellpadding="1" cellspacing="1" id="printTalbe" style="width: 500px;margin:0 auto;">
                    <thead>
                        <tr>
                            <td>年度</td>
                            <td>月份</td>
                            <td>数量</td>
                        </tr>
                    </thead>
                    <tbody>
                        <% int count = 0; %>
                        <%foreach (System.Data.DataRow dr in dt.Rows)
                          { %>
                        <tr>
                            <td><%=dr["Year"] %></td>
                            <td><%=dr["Month"] %></td>
                            <td><%=dr["FinishQty"] %></td>
                        </tr>
                        <%
                              count += BLL.ToolManager.ConvertInt(dr["FinishQty"]);
                          } %>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>合计</td>
                            <td></td>
                            <td><%=count %></td>
                        </tr>
                    </tfoot>
                </table>

            </div>
        </div>
    </form>
</body>
</html>
