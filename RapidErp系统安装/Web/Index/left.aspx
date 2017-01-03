<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="left.aspx.cs" Inherits="Rapid.Index.left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
        }
        .STYLE2
        {
            color: #43860c;
            font-size: 12px;
        }
        a:link
        {
            font-size: 12px;
            text-decoration: none;
            color: #43860c;
        }
        a:visited
        {
            font-size: 12px;
            text-decoration: none;
            color: #43860c;
        }
        a:hover
        {
            font-size: 12px;
            text-decoration: none;
            color: #FF0000;
        }
        .filetree span
        {
            font-size: 13px;
            font-weight: bold;
        }
    </style>
    <link rel="stylesheet" href="../Js/jquery.treeview/jquery.treeview.css" />
    <link rel="stylesheet" href="../Js/jquery.treeview/demo/screen.css" />
    <link href="Menu.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery.treeview/lib/jquery.js" type="text/javascript"></script>

    <script src="../Js/jquery.treeview/lib/jquery.cookie.js" type="text/javascript"></script>

    <script src="../Js/jquery.treeview/jquery.treeview.js" type="text/javascript"></script>
 
    <script type="text/javascript">
        $(function() {
            $("#menuList div img").hover(function() {
                $(this).attr("src", $(this).attr("over"));
            }, function() {
                var isclick = $(this).attr("isclick");
                if (isclick == "true") {
                    return;
                }
                $(this).attr("src", $(this).attr("out"));
            });
            $("#menuList div").click(function() {
                $(this).find("img").attr("isclick", "true").mouseover();
                $(this).siblings().find("img").attr("isclick", "false").mouseout();
                var menuId = $(this).find("img").attr("id");
                $.get("left.aspx?s="+new Date (), { menuId: menuId, time: new Date() }, function(result) {
                   
                    $("#browser").html(result);
                    $("#browser").treeview();
                });
            });

            var leng = $("#menuList div").length;
            if (leng > 0) {
                $("#menuList div:eq(0)").click();
            }

             function ReportTask() {
                 $.get("ReportTask.aspx", { "type": "MaterialDull" }, function (data) { })
             }
             
             window.setTimeout(ReportTask, 6000);
        })
    </script>
 
</head>
<body>
    <form id="form2" runat="server">
    <table width="177" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="table-layout: fixed">
                    <tr>
                        <td height="26" background="images/main_21.gif">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td height="80" style="background-image: url(images/main_23.gif); background-repeat: repeat-x;"
                            id="menuList">
                            <asp:Repeater runat="server" ID="rpMenu">
                                <ItemTemplate>
                                    <div style="text-align: center; float: left; margin: 10px 0px 6px 6px; cursor: pointer;">
                                        <img alt="<%#Eval("Menu_Name") %>" src="<%#Eval("Menu_Img") %>" name="Image1" width="40"
                                            height="40" border="0" id="<%#Eval("Menu_Id") %>" over="<%#Eval("Menu_Hover_Img") %>"
                                            out="<%#Eval("Menu_Img") %>" isclick="false" />
                                        <br />
                                        <a href="#" class="STYLE2" style="margin-top: 7px; display: block;">
                                            <%#Eval("Menu_Name") %></a>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <tr>
                        <td style="line-height: 4px; background: url(images/main_38.gif)">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul id="browser" class="filetree">
                              <%--  <li><span class="folder">Folder 1</span>
                                    <ul>
                                        <li><a href="../SystemManager/UserList.aspx" target="I1" class="folder">用户列表</a></li>
                                    </ul>
                                </li>--%>
                            </ul>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
