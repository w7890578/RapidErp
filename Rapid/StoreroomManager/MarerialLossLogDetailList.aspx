<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarerialLossLogDetailList.aspx.cs"
    Inherits="Rapid.StoreroomManager.MarerialLossLogDetailList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>原材料损耗列表</title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        var WarehouseNumber = getQueryString("WarehouseNumber");
        function edit(ProductNumber, Version, MaterialNumber) {
            OpenDialog("AddOrEditMarerialLossLogDetail.aspx?WarehouseNumber=" + WarehouseNumber + "&ProductNumber=" + ProductNumber + "&Version=" + Version + "&MaterialNumber=" + MaterialNumber, "btnSearch", "500", "500");
        }
        function Delete(ProductNumber, Version, MaterialNumber) {
            if (confirm("确定删除？")) {
                var WarehouseNumber = $("#hdnumber").val();
                $.ajax({
                    type: "Get",
                    url: "MarerialLossLogDetailList.aspx",
                    data: { time: new Date(), WarehouseNumber: WarehouseNumber, ProductNumber: ProductNumber, Version: Version, MaterialNumber: MaterialNumber },
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

                OpenDialog("../StoreroomManager/AddOrEditMarerialLossLogDetail.aspx?WarehouseNumber=" + WarehouseNumber, "btnSearch", "500", "500");
            });

            $("#btnPrint").click(function() {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function() {
                $("#choosePrintClounm").hide();
            });
            $("#btnChoosePrintColum").click(function() {
                var chooseResult = "";
                var unChooseResult = "";
                var arrChk = $("input[name='columList']:checkbox");
                $(arrChk).each(function() {
                    if ($(this).is(':checked')) {
                        chooseResult += $(this).val() + ",";
                    }
                    else {
                        unChooseResult += $(this).val() + ",";
                    }
                });
                var reg = /,$/gi;
                chooseResult = chooseResult.replace(reg, "");
                unChooseResult = unChooseResult.replace(reg, "");
                var unChoosedArray = unChooseResult.split(',');

                if (chooseResult == "") {
                    alert("请选择要打印的列!");
                    return;
                }
                if (!confirm("确定打印所选列？")) {
                    return;
                }
                //遍历border样式的table下的td 
                $(".border tr td").each(function() {
                    className = $(this).attr("class");
                    if (className == "tdOperar") {
                        $(this).hide();
                    }
                    for (var j = 0; j < unChoosedArray.length; j++) {
                        if (className == unChoosedArray[j] + "") {
                            $(this).hide();
                        }
                    }
                });
                newwin = window.open("", "newwin", "height=900,width=750,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
                newwin.document.body.innerHTML = document.getElementById("form1").innerHTML;
                newwin.document.getElementById("divHeader").style.display = 'none';
                newwin.document.getElementById("choosePrintClounm").style.display = 'none';
                newwin.window.print();
                newwin.window.close();
                $("#choosePrintClounm").hide();
                $(".border tr td").each(function() {
                    $(this).show();
                })
            });

            $("#btnBack").click(function() {
                window.location.href = "MarerialWarehouseLogList.aspx";
            });
        })
        
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
            padding: 4px;
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
    </style>
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        原材料损耗出库</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="float: left;">
                <span style="display: <%=show%>;">
                    <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="查询" CssClass="button" />
                    
                    <asp:Button id="btnCheck" runat ="server" text="审核" 
                    onclick="btnCheck_Click"/></span>
              <span id="spAdd" runat="server"> <input type="button" value="增加" id="btnAdd" class="button" style="display: <%=show%>;" /></span> </div>
            &nbsp;
            <input type="button" value="返回" id="btnBack" class="button" />
            &nbsp;&nbsp;<asp:Label id="lbMsg" runat ="server" ForeColor ="Red" ></asp:Label>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td class="tdOperar_WarehouseNumber">
                        出入库编号
                    </td>
                    <td class="tdOperar_ProductNumber">
                        产成品编号
                    </td>
                    <td class="tdOperar_Version">
                        版本
                    </td>
                    <td class="tdOperar_MaterialNumber">
                        原材料编号
                    </td>
                    <td class="tdOperar_MaterialName">
                        原材料名称
                    </td>
                    <td class="tdOperar_Description">
                        原材料描述
                    </td>
                    <td class="tdOperar_Date">
                        日期
                    </td>
                    <td class="tdOperar_TakeMaterialPerson">
                        补领人
                    </td>
                    <td class="tdOperar_Team">
                        班组
                    </td>
                    <td class="tdOperar_Qty">
                        数量
                    </td>
                    <td>库存数量</td>
                    <td class="tdOperar_MaterialPosition">
                        原材料仓位
                    </td>
                    <td class="tdOperar_LossReason">
                        损耗原因
                    </td>
                    <td class="tdOperar_Remark">
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
                            <td class="tdOperar_WarehouseNumber">
                                <%#Eval("WarehouseNumber")%>
                            </td>
                            <td class="tdOperar_ProductNumber">
                                <%#Eval("ProductNumber")%>
                            </td>
                            <td class="tdOperar_Version">
                                <%#Eval("Version")%>
                            </td>
                            <td class="tdOperar_MaterialNumber">
                                <%#Eval("MaterialNumber")%>
                            </td>
                            <td class="tdOperar_MaterialName">
                                <%#Eval("MaterialName")%>
                            </td>
                            <td class="tdOperar_Description">
                                <%#Eval("Description")%>
                            </td>
                            <td class="tdOperar_Date">
                                <%#Eval("Date")%>
                            </td>
                            <td class="tdOperar_TakeMaterialPerson">
                                <%#Eval("TakeMaterialPerson")%>
                            </td>
                            <td class="tdOperar_Team">
                                <%#Eval("Team")%>
                            </td>
                            <td class="tdOperar_Qty">
                                <%#Eval("Qty")%>
                            </td>
                            <td><%#Eval("StockQty") %></td>
                            <td class="tdOperar_MaterialPosition">
                                <%#Eval("MaterialPosition")%>
                            </td>
                            <td class="tdOperar_LossReason">
                                <%#Eval("LossReason")%>
                            </td>
                            <td class="tdOperar_Remark">
                                <%#Eval("Remark")%>
                            </td>
                            <td class="tdOperar">
                            <span style="display:<%=hasEdit%>;">
                                <span style='display: <%=show%>;'>
                                <a href="###" onclick="edit('<%#Eval("ProductNumber")%>','<%#Eval("Version")%>','<%#Eval("MaterialNumber")%>')">
                                    编辑</a> </span></span>
                                    <span style="display:<%=hasDelete%>;">
                                  <span style='display: <%=show%>;'>
                                 <a href="###" onclick="Delete('<%#Eval("ProductNumber")%>','<%#Eval("Version")%>','<%#Eval("MaterialNumber")%>')">
                                        删除</a></span></span>
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
