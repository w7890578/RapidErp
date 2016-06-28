<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackageAndProductRelationList.aspx.cs"
    Inherits="Rapid.ProduceManager.PackageAndProductRelationList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>包与产品对应关系表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        var PackageNumber = getQueryString("PackageNumber");
        var ProductNumber = $("#hdProductNumber").val();
        var Version = $("#hdVersion").val();

        function edit(PackageNumber, ProductNumber, Version) {
            OpenDialog("AddOrEditPackageAndProductRelation.aspx?PackageNumber=" + PackageNumber + "&ProductNumber=" + ProductNumber + "&Version=" + Version, "btnSearch", "400", "550");
        }
        function Delete(PackageNumber, ProductNumber, Version) {
            if (confirm("确定删除？")) {
                $.ajax({
                    type: "Get",
                    url: "PackageAndProductRelationList.aspx",
                    data: { PackageNumber: PackageNumber, ProductNumber: ProductNumber, Version: Version },
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
        $(function() {
            $("#btnAdd").click(function() {
                OpenDialog("../ProduceManager/AddOrEditPackageAndProductRelation.aspx?PackageNumber=" + PackageNumber, "btnSearch", "400", "550");
            });
            $("#btnBack").click(function() {
                window.location.href = "PackageInfoList.aspx";
            });

        }
       );
       
    </script>

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
            padding: 2px;
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
        .choose
        {
            background-color: Green;
        }
    </style>
     <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        包信息明细列表</div>
    <div>
        <input type="hidden" id="hdProductNumber" runat="server" />
        <input type="hidden" id="hdVersion" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            &nbsp;&nbsp;
            <div style="float: left;">
                <asp:Button runat="server" ID="btnSearch" Text="查询" OnClick="btnSearch_Click" CssClass="button" />
                &nbsp;&nbsp;
              <span id="spAdd" runat="server">  <input type="button" value="增加" id="btnAdd" class="button" /></span>
            </div>
            <input type="button" value="返回" id="btnBack" class="button" />
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td>
                        包编码
                    </td>
                    <td>
                        产成品编号
                    </td>
                    <td>
                        版本
                    </td>
                    <td>
                        操作
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("PackageNumber")%>
                            </td>
                            <td>
                                <%#Eval("ProductNumber")%>
                            </td>
                            <td>
                                <%#Eval("Version")%>
                            </td>
                            <td>
                            <span style="display:<%=hasEdit%>;">
                            <%--    <a href="###" onclick="edit('<%#Eval("PackageNumber") %>','<%#Eval("ProductNumber")%>','<%#Eval("Version")%>')">
                                    编辑</a>--%> </span>
                                    <span style="display:<%=hasDelete%>;">
                                    <a href="###" onclick="Delete('<%#Eval("PackageNumber") %>','<%#Eval("ProductNumber")%>','<%#Eval("Version")%>')">
                                        删除</a></span>
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
