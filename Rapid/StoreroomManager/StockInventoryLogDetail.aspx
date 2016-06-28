<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockInventoryLogDetail.aspx.cs"
    Inherits="Rapid.StoreroomManager.StockInventoryLogDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>库存盘点明细</title>

    <%--    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>--%>
    
    <script src="../Js/jquery2.1.4.min.js"></script>
    <script src="../Js/Main.js" type="text/javascript"></script>
    <link href="../Js/Multiple_Select/js/jquery_mutili.css" rel="stylesheet" />
    <script type="text/javascript" src="../Js/Multiple_Select/js/jquery_mutili.js"></script>

    <script type="text/javascript">
        function Edit(InventoryNumber, MaterialNumber, Version, PaperQty) {
            Version = encodeURI(Version);
            //            var temp = encodeURI(Version); //编码
            //            alert(temp);
            //            temp = decodeURI(temp);//解码
            //            alert(temp);
            //            return;
            OpenDialog("../StoreroomManager/AddOrEditStockInventoryLogDetail.aspx?InventoryNumber=" + InventoryNumber + "&MaterialNumber=" + MaterialNumber + "&Version=" + Version + "&PaperQty=" + PaperQty, "btnSearch", "320", "600");
        }
        $(function () {
            var options = JSON.parse('<%=Newtonsoft.Json.JsonConvert.SerializeObject(kinds)%>');
            $("#txtKind").checks_select("txtKind", options, "===== 请选择原材料种类 =====");
            //var txtKindValues = $("#txtKind").val();
            //if (txtKindValues != "") {
            //    $(".checks_div_select input[type=checkbox]").each(function (i, item) {
            //        if (txtKindValues.indexOf($(this).val()) != -1) {
            //            $(this).attr("checked", true);
            //        }
            //    });
            //}

            $("#btnPrint").click(function () {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function () {
                $("#choosePrintClounm").hide();
            });
            $("#btnChoosePrintColum").click(function () {
                var chooseResult = "";
                var unChooseResult = "";
                var arrChk = $("input[name='columList']:checkbox");
                $(arrChk).each(function () {
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
                $(".border tr td").each(function () {
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
                $(".border tr td").each(function () {
                    $(this).show();
                })
            });

            $("#btnBack").click(function () {
                window.location.href = "StockInventoryLogList.aspx";
            });
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <style type="text/css">
            .border {
                background-color: Black;
                width: 100%;
                font-size: 14px;
                text-align: center;
            }

                .border tr td {
                    padding: 4px;
                    background-color: White;
                }

            a {
                color: Blue;
            }

                a:hover {
                    color: Red;
                }

            #choosePrintClounm {
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

                #choosePrintClounm ul {
                    margin-bottom: 10px;
                }

                #choosePrintClounm div {
                    text-align: center;
                    color: Green;
                }

                #choosePrintClounm ul li {
                    list-style: none;
                    float: left;
                    width: 100%;
                    cursor: pointer;
                }
        </style>
        <% bool ismaterial = !(warehouseName.Equals("产成品库") || warehouseName.Equals("半成品库")); %>
        <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px">
            <%=warehouseName%>盘点明细<%=number%>
        </div>
        <%
            bool hasCheckPermission = Rapid.ToolCode.Tool.GetUserMenuFunc("L0405", "Check");

            bool hasEdit = Rapid.ToolCode.Tool.GetUserMenuFunc("L0405", "Edit");
        %>
        <div>
            <div id="divHeader" style="padding: 10px;">
                <div style="margin: 10px; width: 100%;">
                    <%if (showMareial.Equals("inline"))
                        { %>

                    <div class="checks_div">
                        <asp:TextBox ID="txtKind" runat="server" CssClass="checks_div_input" ReadOnly="true"></asp:TextBox>
                        <div></div>
                    </div>
                    <br />
                    编号属性：<asp:TextBox ID="txtNumberProperties" runat="server"></asp:TextBox>&nbsp;&nbsp;
                      原材料编号：<asp:TextBox ID="txtMareialNumber" runat="server"></asp:TextBox>&nbsp;&nbsp;
                型号：<asp:TextBox runat="server" ID="txtMatrialName"></asp:TextBox>&nbsp;&nbsp;
                    <%} %>
                </div>
                <div style="margin: 10px; width: 100%;">
                    <%if (showVersion.Equals("inline"))
                        { %>
                 编号属性：<asp:TextBox ID="txtNumberPropertiesa"
                     runat="server"></asp:TextBox>
                    &nbsp;&nbsp; 产成品编号：<asp:TextBox ID="txtProductNumber" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;
                <%} %>

                 

                  账面数量:<asp:DropDownList runat="server" ID="drpZhangMianNumber" OnSelectedIndexChanged="drpZhangMianNumber_SelectedIndexChanged" AutoPostBack="true">
                      <asp:ListItem Text="==请选择==" Value=""></asp:ListItem>

                      <asp:ListItem Text="大于0" Value=">0"></asp:ListItem>
                      <asp:ListItem Text="等于0" Value="=0"></asp:ListItem>
                      <asp:ListItem Text="小于0" Value="<0"></asp:ListItem>

                  </asp:DropDownList>
                    &nbsp;&nbsp;盈亏查询:<asp:DropDownList runat="server" ID="dpIsYK">
                        <asp:ListItem Text="---请选择---" Value=""></asp:ListItem>
                        <asp:ListItem Text="盘盈" Value="1"></asp:ListItem>
                        <asp:ListItem Text="盘亏" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="盈亏数量为0" Value="0"></asp:ListItem>

                    </asp:DropDownList>
                
                </div>


                <div style="width: 100%; margin-top: 10px; margin-left: 400px;">
                    <input type="button" value="返回" id="btnBack" class="button" />&nbsp;<asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click" />&nbsp;
          
                    <%if (hasCheckPermission)
                        { %>
                    <asp:Button ID="btnCheck" runat="server" Text="审核" OnClick="btnCheck_Click" OnClientClick="return Check();" />
                    <%} %> 

                    &nbsp;
                    <span style="position: relative; margin-top: 40px; margin-bottom: 10px;">&nbsp;&nbsp; <span id="spPrint" runat="server">
                        <input type="button" value="打印" id="btnPrint" class="button" /></span>
                        <div id="choosePrintClounm">
                            <div>
                                请选择要打印的列
                            </div>
                            <ul>
                                <li style="display: none;">
                                    <label>
                                        <input type="checkbox" name="columList" value="tdOperar_InventoryNumber" />
                                        盘点编号</label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" name="columList" value="tdOperar_MaterialNumber" checked="checked" />
                                        物料编号</label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" name="columList" value="tdOperar_Version" checked="checked" />
                                        版本</label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" name="columList" value="tdOperar_PaperQty" checked="checked" />
                                        账面数量</label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" name="columList" value="tdOperar_InventoryQty" checked="checked" />
                                        实盘数量</label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" name="columList" value="tdOperar_NumberProperties" checked="checked" />
                                        编号属性</label>
                                </li>

                                <%if (ismaterial)
                                    { %>
                                <li>
                                    <label>
                                        <input type="checkbox" name="columList" value="tdOperar_MaterialName" />
                                        型号</label>
                                </li>
                                <%} %>

                                <li>
                                    <label>
                                        <input type="checkbox" name="columList" value="tdOperar_ProfitAndLossQty" checked="checked" />
                                        盈亏数量</label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" name="columList" value="tdOperar_Remark" checked="checked" />
                                        备注
                                    </label>
                                </li>
                            </ul>
                            <div>
                                &nbsp;<br />
                                <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                                    type="button" value=" 取 消 " id="btnExit" />
                            </div>
                        </div>
                    </span>
                        &nbsp;&nbsp;   <asp:Button ID="Button1" runat="server" Text="导出Excel" 
                            CssClass="button" OnClick="Button1_Click1" />
                    <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>



            <table class="border" cellpadding="1" cellspacing="1">
                <thead>
                    <tr>
                        <td class="tdOperar_InventoryNumber" style="display: none;">盘点编号
                        </td>
                        <td class="tdOperar_MaterialNumber">
                            <%=name  %>
                        </td>
                        <%if (showVersion == "inline")
                            { %>
                        <td class="tdOperar_Version">版本
                        </td>
                        <%} %>

                        <td class="tdOperar_PaperQty">账面数量
                        </td>
                        <td class="tdOperar_InventoryQty">实盘数量
                        </td>
                        <td class="tdOperar_ProfitAndLossQty">盈亏数量
                        </td>
                        <%if (showMareial == "inline")
                            { %>
                        <td>种类
                        </td>
                        <%} %>

                        <td><span style="color: red;">
                            <asp:LinkButton runat="server" ID="lbbHuoWei" OnClick="lbbHuoWei_Click"></asp:LinkButton></span>
                            <%-- <asp:Label Text="货位" runat="server" ID="lbHuoWei"></asp:Label>--%>
                        </td>
                        <td class="tdOperar_NumberProperties">编号属性
                        </td>
                        <%if (ismaterial)
                            { %>
                        <td class="tdOperar_MaterialName">型号
                        </td>
                        <%} %>

                        <%if (showMareial == "inline")
                            { %>
                        <td>货物类型
                        </td>
                        <%} %>

                        <td class="tdOperar_Remark">备注
                        </td>
                        <td class="tdOperar">操作
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <%
                        double sumPaperQty = 0;
                        double sumInventoryQty = 0;
                        double sumProfitAndLossQty = 0;
                    %>

                    <%
                        if (ResultTable != null)
                        {
                            foreach (System.Data.DataRow dr in ResultTable.Rows)
                            {%>
                    <tr>
                        <td class="tdOperar_WarehouseNumber" style="display: none;">
                            <%=dr["InventoryNumber"] %> 
                        </td>
                        <td class="tdOperar_WarehouseNumber">
                            <%=dr["MaterialNumber"]%>
                        </td>
                        <%if (showVersion == "inline")
                            { %>
                        <td class="tdOperar_DocumentNumber">
                            <%=dr["Version"]%>
                        </td>
                        <%} %>

                        <td class="tdOperar_Description">
                            <%=dr["PaperQty"]%>
                        </td>
                        <td class="tdOperar_Qty">
                            <%=dr["InventoryQty"]%>
                        </td>
                        <td class="tdOperar_Position">
                            <%=dr["ProfitAndLossQty"]%>
                        </td>
                        <%if (showMareial == "inline")
                            { %>
                        <td>
                            <%=dr["Kind"]%>
                        </td>
                        <%} %>


                        <td>
                            <%=dr["Cargo"]%>
                        </td>
                        <td class="tdOperar_NumberProperties">
                            <%=dr["NumberProperties"]%>
                        </td>
                        <%if (ismaterial)
                            { %>
                        <td class="tdOperar_MaterialName"><%=dr["MaterialName"]%>
                        </td>
                        <%} %>


                        <%if (showMareial == "inline")
                            { %>
                        <td>
                            <%=dr["CargoType"]%>
                        </td>
                        <%} %>

                        <td class="tdOperar_Remark">
                            <%=dr["Remark"]%>
                        </td>
                        <td class="tdOperar">
                            <%if (hasEdit && !IsCheck)
                                { %>
                            <a href="###" onclick="Edit('<%=dr["InventoryNumber"]%>','<%=dr["MaterialNumber"]%>','<%=dr["Version"]%>','<%=dr["PaperQty"]%>')">编辑</a>
                            <%} %>
                          
                        </td>
                    </tr>
                    <%
                        sumPaperQty = sumPaperQty + (dr["PaperQty"] == null ? 0 : Convert.ToDouble(dr["PaperQty"]));
                        sumInventoryQty = sumInventoryQty + (dr["InventoryQty"] == null ? 0 : Convert.ToDouble(dr["InventoryQty"]));
                        sumProfitAndLossQty = sumProfitAndLossQty + (dr["ProfitAndLossQty"] == null ? 0 : Convert.ToDouble(dr["ProfitAndLossQty"]));
                    %>
                    <% }
                        } %>

                    <tr>
                        <td class="tdOperar_WarehouseNumber" style="display: none;"></td>
                        <td class="tdOperar_WarehouseNumber">合计
                        </td>
                        <%if (showVersion == "inline")
                            { %>
                        <td class="tdOperar_DocumentNumber"></td>
                        <%} %>

                        <td class="tdOperar_Description">
                            <%=sumPaperQty%>
                        </td>
                        <td class="tdOperar_Qty">
                            <%=sumInventoryQty%>
                        </td>
                        <td class="tdOperar_Position">
                            <%=sumProfitAndLossQty%>
                        </td>
                        <%if (showMareial == "inline")
                            { %>
                        <td></td>
                        <%} %>


                        <td></td>
                        <td class="tdOperar_NumberProperties"></td>
                        <%if (showMareial == "inline")
                            { %>
                        <td></td>
                        <%} %>

                        <td class="tdOperar_Remark"></td>
                        <td class="tdOperar"></td>
                        <td class="tdOperar"></td>
                    </tr>

                </tbody>
            </table>
        </div>
    </form>
    <script type="text/javascript">
        function Check() {
            if (confirm("确定审核？")) {
                return true;
            }
            return false;
        }
    </script>
</body>
</html>
