<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialSafetyStockCalculationReport.aspx.cs" Inherits="Rapid.StoreroomManager.MaterialSafetyStockCalculationReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>原材料安全库存计算报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

  <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        .border
        {
            background-color: Black;
            width:100%;
            font-size: 14px;
            text-align: center;
        }
        .border tr td
        {
            padding: 1px;
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
    </style>
    <div style=" text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px;margin-bottom:20px;">
        原材料安全库存计算报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="margin-bottom:10px;">
                &nbsp;&nbsp; 
                推算日期：
                <asp:TextBox ID="txtDate" runat="server" Style="margin-right: 10px;" onfocus="WdatePicker({skin:'green'})" ></asp:TextBox>
                原材料编号：
                <asp:TextBox ID="txtMaterialNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                客户物料编号：
                <asp:TextBox ID="txtCustomerMateialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                型号：
                <asp:TextBox ID="txtxinghao" runat="server"></asp:TextBox>

                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 10px;"/>
                
                <asp:Button ID="btnEmp" runat="server" Text="导出Excel" onclick="btnEmp_Click" />
            </div>
    </div>
    <table class="border" cellpadding="1" cellspacing="1"  >
        <thead>
            <tr>
               <td >
                    原材料编号
                </td>
                <td>型号</td>
              <td >
                    客户物料编号
                </td>
                <td >
                    现有安全库存数
                </td>
               <td >
                    1个月出库数
                </td>
               <td >
                    3个月出库数
                </td>
               <td >
                    6个月出库数
                </td>
                
            </tr>
        </thead>
        <tbody>
            <asp:Repeater runat="server" ID="rpList">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("原材料编号")%>
                        </td>
                        <td><%#Eval("名称") %></td>
                        <td>
                            <%#Eval("客户物料编号")%>
                        </td>
                        <td>
                            <%#Eval("现有安全库存数")%>
                        </td>
                        <td>
                            <%#Eval("一个月出库数")%>
                        </td>
                        <td>
                            <%#Eval("三个月出库数")%>
                        </td>
                        <td>
                            <%#Eval("六个月出库数")%>
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