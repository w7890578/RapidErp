<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupplyMaterialInfoReport.aspx.cs"
    Inherits="Rapid.PurchaseManager.SupplyMaterialInfoReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>供应商物料信息</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            var querySql = "";

            //获取查询条件
            function GetQueryCondition() {
                var condition = " where 1=1 ";
                return condition;
            }

            //导出Execl前将查询条件内容写入隐藏标签
            function ImpExecl() {
                querySql = "   select * from V_SupplyMaterialInfoReport";
                querySql = querySql + " " + GetQueryCondition();
                $("#saveInfo").val(querySql + "");
                return true;
            }
        })

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
                newwin = window.open("", "newwin", "height=900,width=1200,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
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
            top: 25px;
            left: 1100px;
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
        供应商物料信息</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="margin-top: 20px;">
            <div style="position: relative; float: left; margin-bottom: 10px; top: 0px; left: 0px; width: 1800px;" >
                &nbsp;&nbsp;
                <asp:Label ID="txtd" runat="server" CssClass="input required" Text="原材料编号："></asp:Label>
                <asp:TextBox ID="txtMaterialNumber" runat="server"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="供应商名称：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtSupplyName" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                 <asp:Label ID="Label1" runat="server" Text="描述：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtDescription" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                   <asp:Label ID="Label3" runat="server" Text="供应商物料编号：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtSupplierMaterialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                 <asp:Label ID="Label4" runat="server" Text="型号：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtXH" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                <input type="button" value="打印" id="btnPrint"  style="margin-left:10px"/>
                <input type="hidden" id="saveInfo" runat="server" />
                <asp:Button ID="btnExcel" runat="server" Text="导出Excel" Style="margin-left: 10px;"
                    OnClick="btnExcel_Click" />
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_原材料编号" checked="checked" />
                                原材料编号
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_描述" checked="checked" />
                                描述
                            </label>
                        </li>
                        
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_单价" checked="checked" />
                                单价
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_最小起订量" checked="checked" />
                                最小起订量
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_安全值" checked="checked" />
                                安全值
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_供应商物料编号" checked="checked" />
                                供应商物料编号
                            </label>
                        </li>
                         <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_型号" checked="checked" />
                                型号
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_供应商名称" checked="checked" />
                                供应商名称
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_联系电话" checked="checked" />
                                联系电话
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_QQ" checked="checked" />
                                QQ
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_Email" checked="checked" />
                                Email
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_联系人" checked="checked" />
                                联系人
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_手机" checked="checked" />
                                手机</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_付款账户" checked="checked" />
                                付款账户</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_付款银行" checked="checked" />
                                付款银行</label>
                        </li>
                    </ul>
                    <div>
                        &nbsp;<br />
                        <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                            type="button" value=" 取 消 " id="btnExit" /></div>
                </div>
            </div>
        </div>
        <br />
        <div style="width: 3000px;">
            <table class="border" cellpadding="1" cellspacing="1">
                <thead>
                    <tr>
                        <td class="tdOperar_原材料编号">
                            原材料编号
                        </td>
                        <td class="tdOperar_描述">
                            描述
                        </td>
                        
                        <td class="tdOperar_单价">
                            单价
                        </td>
                        <td class="tdOperar_最小起订量">
                            最小起订量
                        </td>
                        <td class="tdOperar_安全值">
                            安全值
                        </td>
                         <td class="tdOperar_型号">
                            型号
                        </td>
                        <td class="tdOperar_供应商物料编号">
                            供应商物料编号
                        </td>
                        <td class="tdOperar_供应商名称">
                            供应商名称
                        </td>
                        <td class="tdOperar_联系电话">
                            联系电话
                        </td>
                        <td class="tdOperar_QQ">
                            QQ
                        </td>
                        <td class="tdOperar_Email">
                            Email
                        </td>
                        <td class="tdOperar_联系人">
                            联系人
                        </td>
                        <td class="tdOperar_手机">
                            手机
                        </td>
                        <td class="tdOperar_付款账户">
                            付款账户
                        </td>
                        <td class="tdOperar_付款银行">
                            付款银行
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpList">
                        <ItemTemplate>
                            <tr>
                                <td class="tdOperar_原材料编号">
                                    <%#Eval("原材料编号")%>
                                </td>
                                <td class="tdOperar_描述">
                                    <%#Eval("描述")%>
                                </td>
                                 
                                <td class="tdOperar_ 单价">
                                    <%#Eval("单价")%>
                                </td>
                                <td class="tdOperar_ 最小起订量">
                                    <%#Eval("最小起订量")%>
                                </td>
                                <td class="tdOperar_ 安全值">
                                    <%#Eval("安全值")%>
                                </td>
                                <td class="tdOperar_ 型号">
                                    <%#Eval("型号")%>
                                </td>
                                <td class="tdOperar_供应商物料编号">
                                    <%#Eval("供应商物料编号")%>
                                </td>
                                <td class="tdOperar_ 供应商名称">
                                    <%#Eval("供应商名称")%>
                                </td>
                                <td class="tdOperar_联系电话">
                                    <%#Eval("联系电话")%>
                                </td>
                                <td class="tdOperar_QQ">
                                    <%#Eval("QQ")%>
                                </td>
                                <td class="tdOperar_Email">
                                    <%#Eval("Email")%>
                                </td>
                                <td class="tdOperar_联系人">
                                    <%#Eval("联系人")%>
                                </td>
                                <td class="tdOperar_手机">
                                    <%#Eval("手机")%>
                                </td>
                                <td class="tdOperar_付款账户">
                                    <%#Eval("付款账户")%>
                                </td>
                                <td class="tdOperar_付款银行">
                                    <%#Eval("付款银行")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
