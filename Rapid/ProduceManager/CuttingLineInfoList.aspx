<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CuttingLineInfoList.aspx.cs"
    Inherits="Rapid.ProduceManager.CuttingLineInfoList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>裁线信息表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
        $("#btnExit").click(function() {
            $("#choosePrintClounm").hide();
        });
            $("#btnPrint").click(function() {
                $("#choosePrintClounm").toggle();
            });

            $("#btnChoosePrintColum").click(function() {
                
                var chooseResult = "";
                var unChooseResult = "";
                var className = "";
                var arrChk = $("input[name='columList']:checkbox");
                $(arrChk).each(function() {
                    if ($(this).is(':checked')) {
                        chooseResult += $(this).val() + ",";
                    }
                    else {
                        unChooseResult += $(this).val() + ",";
                    }
                });
                var reg = /,$/gi; //替换最后一个 ','
                chooseResult = chooseResult.replace(reg, "");
                unChooseResult = unChooseResult.replace(reg, "");
                var unChoosedArray = unChooseResult.split(',');
                if (chooseResult == "") {
                    alert("请选择要打印的列!");
                    return;
                }
                if (!confirm("确定打印所选列？")) {
                    $("#choosePrintClounm").hide();
                    return;
                }
                $("#divHead").hide();
                $("#showNumber").show();

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
                newwin = window.open("", "newwin", "height=700,width=1050,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
                newwin.document.body.innerHTML = document.getElementById("form1").innerHTML;
                newwin.window.print();
                newwin.window.close();
                $(".border tr td").each(function() {
                    $(this).show();
                })
                $("#choosePrintClounm").hide();
                $("#divHead").show();
                $("#showNumber").hide();
            });
        })
    </script>

</head>
<body style="background-color: White;">
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
            left: 580px;
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
    <div>
        <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            <img src="../Img/loading.gif" alt="loading" />
        </div>
        <div>
            <div style="text-align: center; font-size: 18px; display: none;" id="showNumber">
                开工单<%=plannumber %></div>
            <div style="background-color: #F3FFE3; margin-top: 20px" id="divHead">
                <div style="vertical-align: middle">
                    <div style="float: left; width: 150;">
                        <asp:Label ID="Label1" runat="server" Text="产品编号：" Style="margin-left: 50px"></asp:Label>
                        <asp:TextBox runat="server" ID="txtProductNumber"></asp:TextBox>
                        <asp:Label ID="ii" runat="server" Text="原材料编号："></asp:Label>
                        <asp:TextBox runat="server" ID="txtMarerilNumber"></asp:TextBox>
                        <%-- 每页显示条数：
                                    <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                        onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                        maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="15" />
                                    &nbsp;&nbsp; --%>
                    </div>
                </div>
                <div>
                    <div style="float: left; width: 65px; margin-left: 20px;">
                        <input type="button" id="btnPrint" value=" 打 印 " class="button" />
                    </div>
                    <div style="float: left; width: 65px;">
                        <%--<input type="button" value="查询" id="btnSearch" class="button" />--%><asp:Button
                            ID="btnSearch" runat="server" Text="查询" class="button" OnClick="btnSearch_Click"
                            Style="margin-left: 50px" />
                    </div>
                </div>
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_产成品编号" checked="checked" />
                                产成品编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_客户产成品编号" checked="checked" />
                                客户产成品编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_版本" checked="checked" />
                                版本</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_原材料编号" checked="checked" />
                                原材料编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_客户物料编号" checked="checked" />
                                客户物料编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_长度" checked="checked" />
                                长度</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_数量" checked="checked" />
                                数量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_实际生产数量" checked="checked" />
                                实际生产数量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_需要裁线的数量" checked="checked" />
                                需要裁线的数量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_描述" checked="checked" />
                                描述</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_备注" checked="checked" />
                                备注</label>
                        </li>
                    </ul>
                    <div>
                        &nbsp;<br />
                        <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                            type="button" value=" 取 消 " id="btnExit" /></div>
                </div>
            </div>
            <br />
            <table class="pg_table" style="margin-top: 10px; background-color: White;">
                <tr>
                    <td colspan="14">
                        <table class="border" cellpadding="1" cellspacing="1" style="width: 1050px">
                            <thead>
                                <tr>
                                    <td class="tdOperar_产成品编号">
                                        产成品编号
                                    </td>
                                    <td class="tdOperar_客户产成品编号">
                                        客户产成品编号
                                    </td>
                                    <td class="tdOperar_版本">
                                        版本
                                    </td>
                                    <td class="tdOperar_原材料编号">
                                        原材料编号
                                    </td>
                                    <td class="tdOperar_客户物料编号">
                                        客户物料编号
                                    </td>
                                    <td class="tdOperar_长度">
                                        长度
                                    </td>
                                    <td class="tdOperar_数量">
                                        数量
                                    </td>
                                    <td class="tdOperar_实际生产数量">
                                        实际生产数量
                                    </td>
                                    <td class="tdOperar_需要裁线的数量">
                                        需要裁线的数量
                                    </td>
                                    <td class="tdOperar_描述">
                                        描述
                                    </td>
                                    <td class="tdOperar_备注">
                                        备注
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rpList">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="tdOperar_产成品编号">
                                                <%#Eval("产成品编号")%>
                                            </td>
                                            <td class="tdOperar_客户产成品编号">
                                                <%#Eval("客户产成品编号")%>
                                            </td>
                                            <td class="tdOperar_版本">
                                                <%#Eval("版本")%>
                                            </td>
                                            <td class="tdOperar_原材料编号">
                                                <%#Eval("原材料编号")%>
                                            </td>
                                            <td class="tdOperar_客户物料编号">
                                                <%#Eval("客户物料编号")%>
                                            </td>
                                            <td class="tdOperar_长度">
                                                <%#Eval("长度新")%>
                                            </td>
                                            <td class="tdOperar_数量">
                                                <%#Eval("数量")%>
                                            </td>
                                            <td class="tdOperar_实际生产数量">
                                                <%#Eval("实际生产数量")%>
                                            </td>
                                            <td class="tdOperar_需要裁线的数量">
                                                <%#Eval("需要裁线的数量")%>
                                            </td>
                                            <td class="tdOperar_描述">
                                                <%#Eval("原材料描述")%>
                                            </td>
                                            <td class="tdOperar_备注" style="width: 100px;">
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <%--       </div>--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
