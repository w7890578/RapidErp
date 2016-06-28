<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HalfProductWarehouseLogDetailList.aspx.cs"
    Inherits="Rapid.StoreroomManager.HalfProductWarehouseLogDetailList1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>出入库明细</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

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
    </style>

    <script type="text/javascript">
        function Edit(key) {
            OpenDialog("EditQty.aspx?Guid=" + key, "btnSearch", "300", "550");
        }
        $(function() {
            $("#btnBack").click(function() {
                window.location.href = "HalfProductWarehouseLogList.aspx";
            });
        })
    </script>

    <script type="text/javascript">

        $(function() {

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
            top: 120px;
            left: 150px;
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
    <div style="font-size: 28px; font-weight: bold; text-align: center; margin-top: 30px;">
        <%=titleName %></div>
    <div style="margin-bottom: 10px;" id="divHeader">
        <br />
        <br />
        开工单号：<asp:TextBox ID="txtPlanNumber" runat="server"></asp:TextBox>&nbsp;&nbsp; 客户产成品编号：<asp:TextBox
            ID="txtCustomerProductNumber" runat="server"></asp:TextBox>&nbsp;&nbsp; 缺料原材料编号：<asp:TextBox
                ID="txtQLNumber" runat="server"></asp:TextBox>&nbsp;&nbsp;
        <asp:Button runat="server" Text="查询" ID="btnSearch" OnClick="btnSearch_Click" Style="margin-left: 10px;" />
        <span style="display: <%=showEdit%>;">
            <asp:Button runat="server" Text="审核" ID="btnCheck" OnClick="btnCheck_Click" Style="margin-left: 10px;" />
        </span>
        <input type="button" value="返回" id="btnBack" style="margin-right: 10px; margin-left: 10px;" />
        <input type="button" value="打印" id="btnPrint" style="margin-left: 10px" />
        <asp:Label runat="server" ID="LbMsg" ForeColor="Red"></asp:Label>
        <div id="choosePrintClounm">
            <div>
                请选择要打印的列</div>
            <ul>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_开工单号" checked="checked" />
                        开工单号
                    </label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_产成品编号" checked="checked" />
                        产成品编号
                    </label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_版本" checked="checked" />
                        版本
                    </label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_缺料原材料编号" checked="checked" />
                        缺料原材料编号
                    </label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_销售订单号" checked="checked" />
                        销售订单号
                    </label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_订单交期" checked="checked" />
                        订单交期
                    </label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_数量" checked="checked" />
                        数量
                    </label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_库存数量" checked="checked" />
                        库存数量
                    </label>
                </li>
            </ul>
            <div>
                &nbsp;<br />
                <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                    type="button" value=" 取 消 " id="btnExit" /></div>
        </div>
    </div>
    <div>
        <table cellpadding="1" cellspacing="1" class="border">
            <thead>
                <tr>
                    <td class="tdOperar_开工单号">
                        开工单号
                    </td>
                    <td class="tdOperar_产成品编号">
                        产成品编号
                    </td>
                    <td class="tdOperar_客户产成品编号">
                        客户产成品编号
                    </td>
                    <td class="tdOperar_版本">
                        版本
                    </td>
                    <td class="tdOperar_缺料原材料编号">
                        缺料原材料编号
                    </td>
                    <td class="tdOperar_销售订单号">
                        销售订单号
                    </td>
                    <td class="tdOperar_订单交期">
                        订单交期
                    </td>
                    <td class="tdOperar_数量">
                        数量
                    </td>
                    <td class="tdOperar_库存数量">
                        库存数量
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
                            <td class="tdOperar_开工单号">
                                <%#Eval("DocumentNumber")%>
                            </td>
                            <td class="tdOperar_产成品编号">
                                <%#Eval("ProductNumber")%>
                            </td>
                            <td class="tdOperar_客户产成品编号">
                                <%#Eval("CustomerProductNumber")%>
                            </td>
                            <td class="tdOperar_版本">
                                <%#Eval("Version")%>
                            </td>
                            <td class="tdOperar_缺料原材料编号">
                                <%#Eval("MaterialNumber")%>
                            </td>
                            <td class="tdOperar_销售订单号">
                                <%#Eval("SailOrderNumber")%>
                            </td>
                            <td class="tdOperar_订单交期">
                                <%#Eval("LeadTime")%>
                            </td>
                            <td class="tdOperar_数量">
                                <%#Eval("Qty")%>
                            </td>
                            <td class="tdOperar_库存数量">
                                <%#Eval("StockQty")%>
                            </td>
                            <td class="tdOperar">
                                <span style="display: <%=showEdit%>;"><a href="###" onclick="Edit('<%#Eval("Guid")%>')">
                                    编辑</a> </span>
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
