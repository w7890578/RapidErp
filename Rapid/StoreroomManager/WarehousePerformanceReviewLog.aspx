<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehousePerformanceReviewLog.aspx.cs"
    Inherits="Rapid.StoreroomManager.WarehousePerformanceReviewLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>员工仓库绩效上报</title>
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

        function edit(year, month, PerformanceReviewItem, name) {

            OpenDialog("../StoreroomManager/EditWarehousePerformanceReviewLog.aspx?Year=" + year + "&Month=" + month + "&PerformanceReviewItem=" + encodeURI(PerformanceReviewItem) + "&Name=" +encodeURI(name), "btnSearch", "400", "600");
        } 
        function Delete() {
            if (confirm("确定删除当前年度月份的数据？")) {
                return true;
            }
            return false;
        }
        $(function() {
            $("#btnAdd").click(function() {
                OpenDialog("../StoreroomManager/AddWarehousePerformanceReviewLog.aspx", "btnSearch", "400", "500");

            });
         
        })
    </script>

    <style type="text/css">
        .printDiv
        {
            border-radius: 5px;
            border: 1px solid #B3D08F;
            margin-top: 5px;
            margin-right: 10px;
            background-color: #F3FFE3;
            width: 1410px;
        }
    </style>
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
                            <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;仓库管理&nbsp;&nbsp;>&nbsp;&nbsp;员工仓库绩效上报</span>
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
                                <table class="pg_table" style="wdith: 1500px">
                                    <tr>
                                        <td>
                                            <div>
                                                <input type="hidden" id="hdnumber" runat="server" />
                                                <div id="divHeader">
                                                    <div style="position: relative; float: left; margin-bottom: 10px; top: 0px; left: 0px;">
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="Label1" runat="server" Text="年度："></asp:Label>
                                                        <asp:DropDownList ID="drpYear" runat="server" Style="margin-right: 10px">
                                                            <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
                                                            <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
                                                            <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
                                                            <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
                                                            <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
                                                            <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
                                                            <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label2" runat="server" Text="月份："></asp:Label>
                                                        <asp:DropDownList ID="drpMonth" runat="server" Style="margin-right: 10px">
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
                                                        <asp:Label ID="Label3" runat="server" Text="考核项目："></asp:Label>
                                                        <asp:TextBox ID="txtPerformanceReviewItem" runat="server" Style="margin-right: 10px"></asp:TextBox>
                                                        <asp:Label ID="Label4" runat="server" Text="姓名："></asp:Label>
                                                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" class="button"
                                                            Style="margin-right: 10px" />
                                                        <input type="hidden" id="Hidden1" runat="server" />
                                                        <input type="button" value="上报" id="btnAdd" class="button" />
                                                        <asp:Button ID="Button1" runat="server" Text="删除" OnClientClick="return Delete()"
                                                            class="button" Style="margin-right: 10px" OnClick="Button1_Click" />
                                                        <asp:Label runat="server" ID="lbMsg" ForeColor ="Red" ></asp:Label>
                                                    </div>
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
                                                            <td class="tdOperar_年度">
                                                                年度
                                                            </td>
                                                            <td class="tdOperar_月份">
                                                                月份
                                                            </td>
                                                            <td class="tdOperar_姓名">
                                                                姓名
                                                            </td>
                                                            <td class="tdOperar_考核项目">
                                                                考核项目
                                                            </td>
                                                            <td class="tdOperar_序号">
                                                                序号
                                                            </td>
                                                            <td class="tdOperar_满分">
                                                                满分
                                                            </td>
                                                            <td class="tdOperar_扣分">
                                                                扣分
                                                            </td>
                                                            <td class="tdOperar_得分">
                                                                得分
                                                            </td>
                                                            <td class="tdOperar_描述" style="width: 100px">
                                                                描述
                                                            </td>
                                                            <td class="tdOperar_统计方式">
                                                                统计方式
                                                            </td>
                                                            <td class="tdOperar_备注">
                                                                备注
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
                                                                    <td class="tdOperar_年度">
                                                                        <%#Eval("年度")%>
                                                                    </td>
                                                                    <td class="tdOperar_月份">
                                                                        <%#Eval("月份")%>
                                                                    </td>
                                                                    <td class="tdOperar_姓名">
                                                                        <%#Eval("姓名")%>
                                                                    </td>
                                                                    <td class="tdOperar_考核项目">
                                                                        <%#Eval("考核项目")%>
                                                                    </td>
                                                                    <td class="tdOperar_序号">
                                                                        <%#Eval("年度").ToString().Equals("合计") ? "" : Eval("序号")%>
                                                                    </td>
                                                                    <td class="tdOperar_满分">
                                                                        <%#Eval("满分")%>
                                                                    </td>
                                                                    <td class="tdOperar_扣分">
                                                                        <%#Eval("扣分")%>
                                                                    </td>
                                                                    <td class="tdOperar_得分">
                                                                        <%#Eval("得分")%>
                                                                    </td>
                                                                    <td class="tdOperar_描述" style="width: 200px">
                                                                        <%#Eval("描述")%>
                                                                    </td>
                                                                    <td class="tdOperar_统计方式">
                                                                        <%#Eval("统计方式")%>
                                                                    </td>
                                                                    <td class="tdOperar_备注">
                                                                        <%#Eval("备注")%>
                                                                    </td>
                                                                    <td>
                                                                        <a href="###" onclick="edit('<%#Eval("年度") %>','<%#Eval("月份")%>', '<%#Eval("考核项目")%>','<%#Eval("姓名")%>')">
                                                                              <%#Eval("年度").ToString().Equals("合计")?"":"编辑"%></a>
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
