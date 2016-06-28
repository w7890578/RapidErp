<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TableList.Master" AutoEventWireup="true" CodeBehind="PackagingProcessInfoList.aspx.cs" Inherits="Rapid.StoreroomManager.PackagingProcessInfoList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    <script type="text/javascript">

        function Delete(productType, workSnNumber) {
            $.get("PackagingProcessInfoList.aspx", { time: new Date(), ProductType: encodeURI(productType), WorkSnNumber: workSnNumber }, function(result) {
                if (result == "1") {
                    alert("删除成功");
                    $("#ctl00_ContentPlaceHolder1_btnSearch").click();
                }
                else {
                    alert("删除失败！原因：" + result);
                }
            });
        }
        $(function() {
            $("#navHead").html("&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;库房管理&nbsp;&nbsp;>&nbsp;&nbsp;包装工序信息");
            $("#btnAdd").click(function() {
            OpenDialog("../StoreroomManager/AddOrEditPackagingProcessInfo.aspx", "ctl00_ContentPlaceHolder1_btnSearch", "350", "600");
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table class="pg_table">
        <tr>
            <td colspan="8">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td colspan="5" style="text-align: left">
                <%--<div style="vertical-align: middle">
                    <div style="float: left; width: 150;">
                        每页显示条数：
                        <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                            onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                            type="text" style="width: 60px;" id="txtPageSize" value="10" maxlength="3" />
                        &nbsp;&nbsp;</div>
                </div>--%>
                <div>
                    <div style="float: left; width: 65px;display :none ;">
                        <asp:Button ID="btnSearch" runat="server" Text="查询" class="button" OnClick="btnSearch_Click" />
                    </div>
                    <div style="float: left; width: 65px;" id="divAdd" runat="server">
                        <input type="button" value="增加" id="btnAdd" class="button" />
                    </div>
                </div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <div>
                    <table class="tablesorter" cellpadding="1" cellspacing="1">
                        <thead>
                            <tr>
                                <td>
                                    成品类别
                                </td>
                                <td>工序编号</td>
                                <td>
                                    工序名称
                                </td>
                                <td>
                                    序号
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
                                            <%#Eval("ProductType")%>
                                        </td>
                                        <td><%#Eval("WorkSnNumber")%></td>
                                        <td>
                                            <%#Eval("WorkSnName")%>
                                        </td>
                                        <td>
                                            <%#Eval("Sn")%>
                                        </td>
                                        <td>
                                            <a href="###" onclick="Delete('<%#Eval("ProductType")%>','<%#Eval("WorkSnNumber")%>')">
                                                删除</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="5" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
                                    padding-right: 10px;">
                                    <div id="pageing" class="pages clearfix">
                                    </div>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
