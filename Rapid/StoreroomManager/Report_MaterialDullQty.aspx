<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_MaterialDullQty.aspx.cs" Inherits="Rapid.StoreroomManager.MaterialDullQty" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>库房员工考试月度报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .border {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }

            .border tr td {
                padding: 4px;
                background-color: White;
            }

        a {
            color: Blue;
        }

            a:hover {
                color: Red;
            }

        #choosePrintClounm {
            position: absolute;
            top: 24px;
            left: 540px;
            background-color: White;
            width: 170px;
            border: 1px solid green;
            padding: 10px;
            font-size: 14px;
            display: none;
        }

            #choosePrintClounm ul {
                margin-bottom: 10px;
            }

            #choosePrintClounm div {
                text-align: center;
                color: Green;
            }

            #choosePrintClounm ul li {
                list-style: none;
                float: left;
                width: 100%;
                cursor: pointer;
            }
    </style>
    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">

        <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px">
            呆滞物料表
        </div>
        <div>
            <input type="hidden" id="hdnumber" runat="server" />
            <div id="divHeader" style="padding: 10px;">

                <div style="position: relative; float: left; margin-bottom: 10px">
                    &nbsp;&nbsp; 
                    <asp:Label ID="Label2" runat="server" Text="原材料编号：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtMaterialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" Text="型号：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtMaterialName" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    <asp:Label ID="Label3" runat="server" Text="描述：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtDescription" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnExcel" runat="server" Text="导出Excel"
                        Style="margin-left: 10px;" OnClick="btnExcel_Click" />

                </div>

            </div>

            <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
                <thead>
                    <tr>
                        <td>原材料编号</td>
                        <td>型号</td>
                        <td>描述</td>
                        <td>6个月呆滞</td>
                        <td>12个月呆滞</td>
                        <td>一年以上呆滞</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpList">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("MaterialNumber") %></td>
                                <td><%#Eval("MaterialName") %></td>
                                <td><%#Eval("Description") %></td>
                                <td><%#Eval("SixMonth") %></td>
                                <td><%#Eval("TwelveMonth") %></td>
                                <td><%#Eval("AyearMonth") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

