<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NonconformingProductReport.aspx.cs" Inherits="Rapid.ProduceManager.NonconformingProductReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>不合格品报表</title>
     <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>  
</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border tr td
        {
            padding: 4px;
            background-color: White;
        }
        a
        {
            color: Blue;
        }
        a:hover
        {
            color: Red;
        }
        #choosePrintClounm
        {
            position: absolute;
            top: 20px;
            left: 50px;
            background-color: White;
            width: 170px;
            border: 1px solid green;
            padding: 10px;
            font-size: 14px;
            display: none;
        }
        #choosePrintClounm ul
        {
            margin-bottom: 10px;
        }
        #choosePrintClounm div
        {
            text-align: center;
            color: Green;
        }
        #choosePrintClounm ul li
        {
            list-style: none;
            float: left;
            width: 100%;
            cursor: pointer;
        }
        .style1
        {
            height: 26px;
        }
    </style>
    <div style="width: 100%; text-align: center; font: 96px; font-size:xx-large; font-weight: bold;
        margin-top: 20px">不合格品报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
          
            <div style="position: relative; float: left; margin-bottom:10px">
                &nbsp;&nbsp;
                <asp:Label ID="txtd" runat="server"  Text="年份："></asp:Label>
                <asp:DropDownList ID="drpYear" runat="server" CssClass="required " 
                    AutoPostBack="True" onselectedindexchanged="drpYear_SelectedIndexChanged" >
                    <asp:ListItem Value="2014">2014</asp:ListItem>
                    <asp:ListItem Value="2015">2015</asp:ListItem>
                    <asp:ListItem Value="2016">2016</asp:ListItem>
                    <asp:ListItem Value="2017">2017</asp:ListItem>
                    <asp:ListItem Value="2018">2018</asp:ListItem>
                    <asp:ListItem Value="2019">2019</asp:ListItem>
                    <asp:ListItem Value="2020">2020</asp:ListItem>
                    <asp:ListItem ></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label1" runat="server"  Text="月份：" style="margin-left:10px"></asp:Label>
                <asp:DropDownList ID="drpMonth" runat="server" CssClass="required " 
                    AutoPostBack="True" onselectedindexchanged="drpMonth_SelectedIndexChanged" >
                <asp:ListItem Text="- - - - - 请 选 择 - - - - -" Value=""></asp:ListItem>
                <asp:ListItem Value="01">1月</asp:ListItem>
                <asp:ListItem Value="02">2月</asp:ListItem>
                <asp:ListItem Value="03">3月</asp:ListItem>
                <asp:ListItem Value="04">4月</asp:ListItem>
                <asp:ListItem Value="05">5月</asp:ListItem>
                <asp:ListItem Value="06">6月</asp:ListItem>
                <asp:ListItem Value="07">7月</asp:ListItem>
                <asp:ListItem Value="08">8月</asp:ListItem>
                <asp:ListItem Value="09">9月</asp:ListItem>
                <asp:ListItem Value="10">10月</asp:ListItem>
                <asp:ListItem Value="11">11月</asp:ListItem>
                <asp:ListItem Value="12">12月</asp:ListItem>
                </asp:DropDownList>
            </div>
           
        </div>
        
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                <td class="tdOperar_班组">
                        班组
                    </td>
                    <td class="tdOperar_数量">
                        数量
                    </td>
                     <td class="tdOperar_不合格率">
                        不合格率
                    </td>
                    
                    <td class="tdOperar_ 合格率">
                        合格率
                    </td>
                 </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                         <td class="tdOperar_班组">
                                <%#Eval("班组")%>
                            </td>
                            <td class="tdOperar_数量">
                                <%#Eval("数量")%>
                            </td>
                             <td class="tdOperar_不合格率">
                                <%#Eval("不合格率")%>
                            </td>
                            <td class="tdOperar_合格率">
                                <%#Eval("合格率")%>
                            </td> 
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
