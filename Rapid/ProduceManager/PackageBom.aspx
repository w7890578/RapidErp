<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackageBom.aspx.cs" Inherits="Rapid.ProduceManager.PackageBom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>包与产品对应关系表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        .printDiv
        {
            border-radius: 5px;
            border: 1px solid #B3D08F;
            margin-right: 10px;
            background-color: #F3FFE3;
            width: 1200px;
        }
    </style>

    <script type="text/javascript">


        function Delete(packagenumber, productnumber, version) {
            if (confirm("确认删除？")) {
                $.ajax({
                    type: "Get",
                    url: "PackageBom.aspx",
                    data: { time: new Date(), PackageNumber: packagenumber, ProductNumber: productnumber, Version: version },
                    success: function(result) {
                        if (result == "1") {
                            alert("删除成功！");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("删除失败！原因：" + result);
                            return;
                        }
                    }
                });
            }
        }
        $(document).ready(function() {

            //添加
            var pn = getQueryString("Id");
            $("#btnAdd").click(function() {

                OpenDialog("AddPackageBom.aspx?PackageNumber=" + pn, "btnSearch", "280", "600");
            });

            //进入页面加载数据
            //$("#btnSearch").click();
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 740px;
            font-size: 14px;
            text-align: center;
        }
        .border thead tr td
        {
            padding: 4px;
            background-color: white;
        }
        .border tbody tr td
        {
            padding: 4px;
            background-color: white;
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
            top: 25px;
            left: 300px;
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
    </style>
    <div class="printDiv" id="upDiv">
        <table class="pg_table">
            <tr>
                <td>
                    <div>
                        <input type="hidden" id="hdnumber" runat="server" />
                        <div id="divHeader" align="center" style="margin-top: 10px">
                            客户产成品编号：
                            <asp:TextBox runat="server" ID="txtCustomnerProductNumber"></asp:TextBox>
                            客户物料编号：
                            <asp:TextBox runat="server" ID="txtCustomerMaterialNumber"></asp:TextBox>
                           
                            <asp:Button runat="server" ID="btnSearch" Text="查询" OnClick="btnSearch_Click" class="button" /> <input type="button" id="btnAdd" runat="server" value="增加" class="button" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="13">
                    <div id="outsideDiv">
                        <table class="tablesorter" cellpadding="1" cellspacing="1" id="printTalbe">
                            <thead>
                                <tr>
                                    <td class="tdOperar_包编号">
                                        包编号(自己的号)
                                    </td>
                                    <td class="tdOperar_包编号">
                                        客户包编号(客户的号)
                                    </td>
                                    <td class="tdOperar_产成品编号">
                                        产成品编号
                                    </td>
                                    <td class="tdOperar_版本">
                                        版本
                                    </td>
                                    <td>
                                        客户产成品编号(图纸号)
                                    </td>
                                    <td>
                                        原材料编号
                                    </td>
                                    <td>
                                        客户物料编号
                                    </td>
                                    <td>
                                        单机用量
                                    </td>
                                    <td>
                                        单位
                                    </td>
                                    <td class="tdOperar" style="width: 100px">
                                        操作
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rpList">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="tdOperar_包编号">
                                                <%#Eval("PackageNumber")%>
                                            </td>
                                            <td class="tdOperar_包编号">
                                                <%#Eval("客户包号")%>
                                            </td>
                                            <td class="tdOperar_产成品编号">
                                                <%#Eval("ProductNumber")%>
                                            </td>
                                            <td class="tdOperar_版本">
                                                <%#Eval("Version")%>
                                            </td>
                                            <td class="tdOperar_包编号">
                                                <%#Eval("客户产成品号")%>
                                            </td>
                                            <td>
                                                <%#Eval("MaterialNumber")%>
                                            </td>
                                            <td>
                                                <%#Eval("CustomerMaterialNumber")%>
                                            </td>
                                            <td>
                                                <%#Eval("SingleDose").ToString().Replace(".00000","")%>
                                            </td>
                                            <td>
                                                <%#Eval("Unit") %>
                                            </td>
                                            <td>
                                                <a href="###" onclick="Delete('<%#Eval("PackageNumber") %>','<%#Eval("ProductNumber") %>','<%#Eval("Version") %>')">
                                                    删除</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
