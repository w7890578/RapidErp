<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectProductProperty.aspx.cs" Inherits="Rapid.ProduceManager.ProjectProductProperty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>项目产品属性</title>
     <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        *
        {
            margin: 0;
            padding: 0;
        }
        body
        {
            font: 14px Verdana, Arial, Helvetica, sans-serif;
        }
        ul
        {
            list-style: none;
        }
        #tab
        {
            margin: 15px auto;
            width: 100%;
        }
        #tab ul
        {
            overflow: hidden;
            zoom: 1;
        }
        #tab li
        {
            float: left;
            margin-right: 8px;
            width: 200px;
            height: 30px;
            line-height: 30px;
            border: 1px solid green;
            border-bottom: 0;
            cursor: pointer;
            text-align: center;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
        }
        #tab li.on
        {
            background: green;
            color: White;
            font-size: 16px;
            font-weight: bold;
        }
        #content
        {
            border-top: 4px solid green;
            background-color: #f3ffe3;
        }
        #content div
        {
        }
        #content div.show
        {
            display: block;
        }
    </style>

    <script type="text/javascript">
     function Delete(projectnumber) {
            if (confirm("确定删除？")) {
                $.ajax({
                    type: "Get",
                    url: "ProjectProductProperty.aspx",
                    data: { time: new Date(), ProjectNumber: projectnumber},
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
                    OpenDialog("AddProjectProductProperty.aspx.aspx?ProjectNumber=<%=projectnumber %>", "btnSearch", "300", "600");
                    }
                   });
  
       
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
        <!--背景top-->
        <tr>
            <td height="30">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15" height="30">
                            <img src="../Img/tab_03.gif" width="15" height="30" />
                        </td>
                        <td width="1101" background="../Img/tab_05.gif" style="padding: 5px;">
                            <img src="../Img/311.gif" width="16" height="16" />
                            <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;项目信息列表&nbsp;&nbsp;>项目产品属性</span>
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
                            <div>
                                <input type="hidden" id="saveInfo" runat="server" />
                                <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                                    <img src="../Img/loading.gif" alt="loading" />
                                </div>
                                <table class="pg_table">
                                    <tr>
                                        <td>
                                            <div>
                                                <input type="hidden" id="hdnumber" runat="server" />
                                                <div>
                                                    <div style="padding-right: 10px;">
                                                        <table style="margin-top: 10px;">
                                                            <tr>
                                                                <td class="pg_talbe_head">
                                                                </td>
                                                                <td class="pg_talbe_content">
                                                                </td>
                                                                <td class="pg_talbe_head">
                                                                </td>
                                                                <td class="pg_talbe_content">
                                                                    <input type="button" value="增加" id="btnAdd" class="button" />
                                                                </td>
                                                                <td class="pg_talbe_head">
                                                                    <asp:Button ID="btnSearch" runat="server" Text="查询" class="button" OnClick="btnSearch_Click" />
                                                                </td>
                                                                <td class="pg_talbe_content">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div id="tab">
                                                            <table class="tablesorter" cellpadding="1" cellspacing="1" id="printTalbe">
                                                                <thead>
                                                                    <tr>
                                                                        <td class="tdOperar_项目编号">
                                                                            项目编号
                                                                        </td>
                                                                        <td class="tdOperar_产品编号">
                                                                            产品编号
                                                                        </td>
                                                                        <td class="tdOperar”">
                                                                            操作
                                                                        </td>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Repeater runat="server" ID="rpList">
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td class="tdOperar_项目编号">
                                                                                    <%#Eval("项目编号")%>
                                                                                </td>
                                                                                <td class="tdOperar_产品编号">
                                                                                    <%#Eval("产品编号")%>
                                                                                </td>
                                                                                <td>
                                                                                    <a href="###" onclick="Delete('<%#Eval("项目编号")%>')">删除</a>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </tbody>
                                                                <tfoot>
                                                                    <tr>
                                                                        <td colspan="3" style="background-color: #F3FFEe3; padding-top: 10px; padding-left: 5px;
                                                                            padding-right: 5px;">
                                                                            <div id="pageing" class="pages clearfix">
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tfoot>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="13">
                            <div id="outsideDiv">
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
    </td> </tr>
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
