<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T_LessMaterialBreakdownDetail.aspx.cs"
    Inherits="Rapid.StoreroomManager.T_LessMaterialBreakdownDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>欠料明细</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <%--
    <script type="text/javascript">
        $(function() {
            //查询sql语句


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
        });
        var querySql = "";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            return condition;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = "   select * from V_PerformanceReviewYearReport_1";
            querySql = querySql + " " + GetQueryCondition();
            $("#saveInfo").val(querySql + "");
            return true;
        }
        </script>--%>

    <script type="text/javascript">
        function ChangeQL(guid) {


            var qty = prompt("填写欠料数量", "");
            if (qty == null || qty == "") {
                return false;
            }

            $.get("T_LessMaterialBreakdownDetail.aspx?sq=" + new Date(), { isEdit: "true", guid: guid, time: new Date(), qty: qty }, function (result) {
                if (result == "1") {
                    alert("修改成功！");
                    $("#btnSearch").click();
                }
                else { alert("修改失败！原因：" + result); }

            });
        }

        function Give(materilaNumber, qty, guid) {
            if (!confirm("确认还料？")) {

                return false;
            }

            else {
                var num = prompt("请输入还料的数量！", "0"); //将输入的内容赋给变量 name ，

                //                //这里需要注意的是，prompt有两个参数，前面是提示的话，后面是当对话框出来后，在对话框里的默认值

                if (num)//如果返回的有内容
                {

                    $.get("T_LessMaterialBreakdownDetail.aspx?sq=" + new Date(), { materilaNumber: materilaNumber, qty: qty, key: guid, Num: num, time: new Date() }, function (result) {
                        if (result == "1") {
                            alert("还料成功！");
                            $("#btnSearch").click();
                        }
                        else { alert("还料失败！原因：" + result); }

                    });

                }
            }

        }

        $(function () {
            $(".border tr").click(function () {

                $(this).find("td").css("background-color", "yellow");
                $(this).siblings().find("td").css("background-color", "white");
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
                cursor: pointer;
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
                top: 23px;
                left: 220px;
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
        <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px">
            欠料明细
        </div>
        <div>
            <input type="hidden" id="hdnumber" runat="server" />
            <div id="divHeader" style="padding: 10px;">
                <div style="position: relative; float: left; margin-bottom: 10px">
                    &nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="原材料编号："></asp:Label>
                    <asp:TextBox ID="txtMaterialNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                    <asp:Label ID="Label2" runat="server" Text="客户物料编号："></asp:Label>
                    <asp:TextBox ID="txtCustomerMaterialNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                    <asp:Label ID="Label3" runat="server" Text="开工单号："></asp:Label>
                    <asp:TextBox ID="txtKGNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox><label style="color: red;">(*支持模糊查询)</label>

                    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                    <%--<input type="button" value="打印" id="btnPrint"  style="margin-left:10px"/>
                <input type="hidden" id="saveInfo" runat="server" />
               <span style="display:none;">  <asp:Button ID="btnExcel" runat="server" Text="导出Excel" 
                    style="margin-left:10px;" onclick="btnExcel_Click" OnClientClick="return ImpExecl()"/></span>
                
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                     <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_名次" checked="checked" />
                                名次
                                </label>
                        </li>
                         <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_年份" checked="checked" />
                                年份
                                </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_员工姓名" checked="checked" />
                                员工姓名
                                </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_绩效" checked="checked" />
                                绩效
                                </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_考试" checked="checked" />
                                考试
                               </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_总分数" checked="checked" />
                                总分数
                                </label>
                        </li>
      
                    </ul>
                    <div>
                        &nbsp;<br />
                        <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                            type="button" value=" 取 消 " id="btnExit" /></div>
                </div>--%>
                </div>
            </div>
            <table class="border" cellpadding="1" cellspacing="1">
                <thead>
                    <tr>
                        <%-- <td style="width: 130px;">
                        <label style="width: 100%; display: block; cursor: pointer;">
                            <input type="checkbox" />全选/反选</label>
                    </td>--%>
                        <td class="tdOperar_单据编号">单据编号
                        </td>
                        <td class="tdOperar_原材料编号">原材料编号
                        </td>
                        <td class="tdOperar_客户物料编号">客户物料编号
                        </td>
                        <%--    <td class="tdOperar_库存数量">
                        瞬时库存数量
                    </td>--%>
                        <td class="tdOperar_欠料数量">欠料数量
                        </td>
                        <td class="tdOperar_创建时间">创建时间
                        </td>
                        <td class="tdOperar">操作
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpList">
                        <ItemTemplate>
                            <tr>
                                <td class="tdOperar_单据编号">
                                    <%#Eval("单据编号")%>
                                </td>
                                <td class="tdOperar_原材料编号">
                                    <%#Eval("原材料编号")%>
                                </td>
                                <td class="tdOperar_客户物料编号">
                                    <%#Eval("客户物料编号")%>
                                </td>
                                <%--  <td class="tdOperar_库存数量">
                                <%#Eval("库存数量")%>
                            </td>--%>
                                <td class="tdOperar_欠料数量">
                                    <%#Eval("欠料数量")%>
                                </td>
                                <td class="tdOperar_创建时间">
                                    <%#Eval("创建时间")%>
                                </td>
                                <td class="tdOperar">
                                    <a href="###" onclick="Give('<%#Eval("原材料编号")%>','<%#Eval("欠料数量")%>','<%#Eval("guid") %>')">
                                        <%#Eval("欠料数量").ToString ().Equals ("0")?"":"还料"%>
                                    </a><a href="###" onclick="ChangeQL('<%#Eval("guid") %>')" style="display: <%=userId=="sysAdmin"?"block":"none"%>;">编辑欠料数量</a>
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
