<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarerialKindList.aspx.cs"
    Inherits="Rapid.ProduceManager.MarerialKindList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>原材料种类</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 200px;
        }
    </style>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function edit(id) {
            OpenDialog("../ProduceManager/AddOrEditMarerialKind.aspx?Kind=" + encodeURI(id), "btnSearch", "180", "350");
        }
         $(function() {
            $("#btnAdd").click(function() {
                OpenDialog("../ProduceManager/AddOrEditMarerialKind.aspx", "btnSearch", "180", "300");
            });
        
        function Delete(id) {
            if (confirm("确定删除？")) {
                $.ajax({
                    type: "Get",
                    url: "MarerialKindList.aspx",
                    data: { Time: new Date(), Id: id },
                    success: function(result) {
                        if (result == "1") {
                            alert("删除成功");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("删除失败！原因：" + result);
                            return;
                        }
                    }

                })
            }
        }
      

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
    </style>
    <div style="height: 30">
    </div>
    <table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
        <!--背景top-->
        <tr>
            <td height="30">
                <div>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="15" height="30">
                                <img src="../Img/tab_03.gif" width="15" height="30" />
                            </td>
                            <td width="1101" background="../Img/tab_05.gif" style="padding: 5px;">
                                <img src="../Img/311.gif" width="16" height="16" />
                                <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;原材料种类列表</span>
                            </td>
                            <td width="281" background="../Img/tab_05.gif">
                                <table border="0" align="right" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="60">
                                        </td>
                                        <td width="52">
                                        </td>
                                        <td width="60">
                                        </td>
                                        <td width="60">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="14">
                                <img src="../Img/tab_07.gif" width="14" height="30" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <!--主内容-->
        <tr>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="9" background="../Img/tab_12.gif">
                            &nbsp;
                        </td>
                        <td bgcolor="#f3ffe3" style="padding-top: 5px;">
                            <span id="spAdd" runat="server">
                                <input type="button" value="增加" id="btnAdd" class="button" /></span>
                            <div>
                                <input type="hidden" id="saveInfo" runat="server" />
                                <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                                    <img src="../Img/loading.gif" alt="loading" /><asp:Button runat="server" ID="btnSearch"
                                        OnClick="btnSearch_Click" Text="查询" CssClass="button" />
                                </div>
                                <table class="pg_table" style="width: 300px">
                                    <tr>
                                        <td colspan="2">
                                            <div id="outsideDiv">
                                                <table class="tablesorter" cellpadding="1" cellspacing="1" id="printTalbe" style="width: 400px">
                                                    <thead>
                                                        <tr>
                                                            <td class="tdOperar_原材料种类">
                                                                原材料种类
                                                            </td>
                                                            <td class="tdOperar">
                                                                操作
                                                            </td>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rpList">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="ttdOperar_原材料种类">
                                                                        <%#Eval("原材料种类")%>
                                                                    </td>
                                                                    <td class="tdOperar">
                                                                        <a href="###" onclick="edit('<%#Eval("原材料种类") %>')">编辑</a>
                                                                        <%-- <a href="###" onclick="Delete('<%#Eval("id") %>')">
                                                                            删除</a>--%>
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
                        </td>
                        <td width="9" background="../Img/tab_16.gif">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <!--背景down-->
        <tr>
            <td height="29">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15" height="29">
                            <img src="../Img/tab_20.gif" width="15" height="29" />
                        </td>
                        <td background="../Img/tab_21.gif">
                        </td>
                        <td width="14">
                            <img src="../Img/tab_22.gif" width="14" height="29" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
