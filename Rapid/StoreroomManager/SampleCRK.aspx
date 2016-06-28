<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TableList.Master" AutoEventWireup="true"
    CodeBehind="SampleCRK.aspx.cs" Inherits="Rapid.StoreroomManager.SampleCRK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function() {

            $("#navHead").html("&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;库房管理&nbsp;&nbsp;>&nbsp;&nbsp;样品出入库");
            $("#btnAdd").click(function() {
                OpenDialog("../StoreroomManager/AddSampleCRK.aspx", "ctl00_ContentPlaceHolder1_btnSearch", "200", "400");
            });
            $("#btnDelete").click(function() {
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function() {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要删除的行！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定删除选中的数据?")) {
                    //通用删除
                    DeleteData("../StoreroomManager/SampleCRK.aspx", ConvertsContent(checkResult), "ctl00_ContentPlaceHolder1_btnSearch");
                }
            });
            //全选/反选
            $(".tablesorter thead tr td input").click(function() {
                $("input[name='subBox']").each(function() {
                    this.checked = !this.checked; //整个反选
                });
            });
        })
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    变动方向：<asp:DropDownList ID="drpType" runat="server">
        <asp:ListItem Text=" - - - 请选择 - - - " Value=""></asp:ListItem>
        <asp:ListItem Text="入库" Value="入库"></asp:ListItem>
        <asp:ListItem Text="出库" Value="出库"></asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="btnSearch" runat="server" Text="查询" class="button" OnClick="btnSearch_Click" />
    <input type="button" value="增加" id="btnAdd" class="button" />
    <input type="button" value="删除" id="btnDelete" class="button" />
    <table class="tablesorter" cellpadding="1" cellspacing="1">
        <thead>
            <tr>
                <td>
                    <label style="width: 100%; display: block; cursor: pointer;">
                        <input type="checkbox" />全选/反选</label>
                </td>
                <td>
                    变动方向
                </td>
                <td>
                    出入库类型
                </td>
                <td>
                    制单人
                </td>
                <td>
                    制单时间
                </td>
                <td>
                    审核人
                </td>
                <td>
                    审核时间
                </td>
                <td>
                    出入库编号
                </td>
                <td>
                    操作
                </td>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rpList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <span style="display: <%#string.IsNullOrEmpty ( Eval("CheckTime").ToString ())?"inline":"none"%>;">
                                <input type="checkbox" name='<%#string.IsNullOrEmpty ( Eval("CheckTime").ToString ())?"subBox":"none"%>'
                                    value='<%#Eval("WarehouseNumber")%>' /></span>
                        </td>
                        <td>
                            <%#Eval("ChangeDirection")%>
                        </td>
                        <td>
                            <%#Eval("type") %>
                        </td>
                        <td>
                            <%#Eval("Creator")%>
                        </td>
                        <td>
                            <%#Eval("CreateTime")%>
                        </td>
                        <td>
                            <%#Eval("Auditor")%>
                        </td>
                        <td>
                            <%#Eval("CheckTime")%>
                        </td>
                        <td>
                            <%#Eval("WarehouseNumber")%>
                        </td>
                        <td>
                            <a href="SampleCRKDetail.aspx?WarehouseNumber=<%#Eval("WarehouseNumber")%>&type= <%# Server .UrlEncode ( Eval("type").ToString ())%>">
                                详细</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</asp:Content>
