<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarerialWarehouseLogListForCG.aspx.cs"
    Inherits="Rapid.PurchaseManager.MarerialWarehouseLogListForCG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>原材料采购出入库</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        //详细跳转
        function Transfer(warehousenumber, Type) {
            window.location.href = "../StoreroomManager/ToolMaterialWarehouseLogDetail.aspx?WarehouseNumber=" + warehousenumber + "&Type=" + encodeURI(Type) + "&IsCG=true&time=" + new Date();
        }
        //确认
        function Determine(warehousenumber) {
            if (!confirm("确定确认？")) {
                return false;
            }
            $.get("MarerialWarehouseLogListForCG.aspx?sq=" + new Date(), { IsDetermine: "true", Warehousenumber: warehousenumber }, function (result) {
                if (result == "1") {
                    alert("确认成功");
                    $("#btnSearch").click();
                    return;
                }
                else {
                    alert(reusl);
                }
            })
        }
        //删除
        function Delete(warehousenumber) {
            if (!confirm("确定删除？")) {
                return false;
            }
            $.get("MarerialWarehouseLogListForCG.aspx?sq=" + new Date(), { IsDelete: "true", Warehousenumber: warehousenumber }, function (result) {
                if (result == "1") {
                    alert("删除成功");
                    $("#btnSearch").click();
                    return;
                }
                else {
                    alert(reusl);
                }
            })
        }
        $(function () {
            $("#btnAdd").click(function () {
                OpenDialog("../StoreroomManager/AddOrEditMarerialWarehouseLog.aspx?IsCG=true", "btnSearch", "240", "600");
            });
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <!--背景top-->
                <tr>
                    <td height="30">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="15" height="30">
                                    <img src="../Img/tab_03.gif" width="15" height="30" />
                                </td>
                                <td width="1101" background="../Img/tab_05.gif">
                                    <img src="../Img/311.gif" width="16" height="16" />
                                    <span class="STYLE4" id="navHead">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;采购管理&nbsp;&nbsp;>&nbsp;&nbsp;采购出入库</span>
                                </td>
                                <td width="281" background="../Img/tab_05.gif">
                                    <table border="0" align="right" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="60"></td>
                                            <td width="52"></td>
                                            <td width="60"></td>
                                            <td width="60"></td>
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
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="9" background="../Img/tab_12.gif">&nbsp;
                            </td>
                            <td bgcolor="#f3ffe3" style="padding-top: 5px;">
                                <div>
                                    <input type="hidden" id="saveInfo" runat="server" />
                                    <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                                        <img src="../Img/loading.gif" alt="loading" />
                                    </div>
                                    <table class="pg_table">
                                        <thead>
                                            <tr>
                                                <td colspan="8">&nbsp;&nbsp; 制单时间：<asp:TextBox ID="txtDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                                                    &nbsp;&nbsp; 制单人:<asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
                                                    &nbsp;&nbsp; 供应商名称:<asp:TextBox ID="txtSuppName" runat="server"></asp:TextBox>
                                                    &nbsp;&nbsp; 原材料编号：<asp:TextBox ID="txtMateriNumber" runat="server"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="8" style="text-align: left">
                                                    <div style="vertical-align: middle">
                                                        <div style="width: 150; display: none;">
                                                            每页显示条数：
                                                        <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                            onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                            maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="30" />
                                                            &nbsp;&nbsp;
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div style="width: 400px;" id="div1">
                                                            &nbsp;&nbsp; 供应商物料编号：<asp:TextBox ID="txtSuppileNumber" runat="server"></asp:TextBox>
                                                            <input type="button" value="增加" id="btnAdd" class="button" />
                                                            <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" class="button" />
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td colspan="8">
                                                    <div>
                                                        <table class="tablesorter" cellpadding="1" cellspacing="1">
                                                            <thead>
                                                                <tr>
                                                                    <td>出入库编号
                                                                    </td>
                                                                    <td>变动方向
                                                                    </td>
                                                                    <td>出入库类型
                                                                    </td>
                                                                    <td>制单人
                                                                    </td>
                                                                    <td>制单时间
                                                                    </td>
                                                                    <td>是否已确认
                                                                    </td>
                                                                    <td>备注</td>
                                                                    <td>操作
                                                                    </td>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Repeater ID="rpList" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <%#Eval("出入库编号")%>
                                                                            </td>
                                                                            <td>
                                                                                <%#Eval("变动方向")%>
                                                                            </td>
                                                                            <td>
                                                                                <%#Eval("出入库类型")%>
                                                                            </td>
                                                                            <td>
                                                                                <%#Eval("制单人")%>
                                                                            </td>
                                                                            <td>
                                                                                <%#Eval("制单时间")%>
                                                                            </td>
                                                                            <td>
                                                                                <%#Eval("是否已确认")%>
                                                                            </td>
                                                                            <td><%#Eval("备注") %></td>
                                                                            <td>
                                                                                <a href="###" onclick="Transfer('<%#Eval("出入库编号")%>','<%#Eval("出入库类型")%>')">详细</a>
                                                                                <span style="display: <%#Eval("是否已确认").ToString ().Equals ("是")?"none":"inline"%>;">&nbsp;&nbsp; <a href="###" onclick="Delete('<%#Eval("出入库编号")%>')">删除</a> &nbsp;&nbsp;
                                                                                <a href="###" onclick="Determine('<%#Eval("出入库编号")%>')">确认</a></span>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </tbody>
                                                            <tfoot>
                                                                <tr>
                                                                    <td align="center" colspan="13" style="padding: 2px; background-color: white;">
                                                                        <asp:LinkButton ID="lbtnFirstPage" runat="server" OnClick="lbtnFirstPage_Click">页首</asp:LinkButton>
                                                                        <asp:LinkButton ID="lbtnpritPage" runat="server" OnClick="lbtnpritPage_Click">上一页</asp:LinkButton>
                                                                        <asp:LinkButton ID="lbtnNextPage" runat="server" OnClick="lbtnNextPage_Click">下一页</asp:LinkButton>
                                                                        <asp:LinkButton ID="lbtnDownPage" runat="server" OnClick="lbtnDownPage_Click">页尾</asp:LinkButton>&nbsp;&nbsp;
                    第<asp:Label ID="labPage" runat="server" Text="Label"></asp:Label>页/共<asp:Label ID="LabCountPage"
                        runat="server" Text="Label"></asp:Label>页
                                                                    </td>
                                                                </tr>
                                                            </tfoot>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                            <td width="9" background="../Img/tab_16.gif">&nbsp;
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
                                <td background="../Img/tab_21.gif"></td>
                                <td width="14">
                                    <img src="../Img/tab_22.gif" width="14" height="29" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
