<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerformanceReviewYearReport_1.aspx.cs" Inherits="Rapid.ProduceManager.PerformanceReviewYearReport_1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>员工绩效年度报表</title>
     <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

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
            top: 24px;
            left: 540px;
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
    <div style="width: 100%; text-align: center; font: 96px; font-size:xx-large; font-weight: bold;
        margin-top: 20px">员工绩效年度报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
          
            <div style="position: relative; float: left; margin-bottom:10px">
                &nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="年份："></asp:Label>
                <asp:DropDownList ID="drpYear" runat="server" CssClass="required " 
                    AutoPostBack="True" OnSelectedIndexChanged="drpYear_SelectedIndexChanged"  > 
                    <asp:ListItem Value="2014">2014</asp:ListItem>
                    <asp:ListItem Value="2015">2015</asp:ListItem>
                    <asp:ListItem Value="2016">2016</asp:ListItem>
                    <asp:ListItem Value="2017">2017</asp:ListItem>
                    <asp:ListItem Value="2018">2018</asp:ListItem>
                    <asp:ListItem Value="2019">2019</asp:ListItem>
                    <asp:ListItem Value="2020">2020</asp:ListItem>
                    <asp:ListItem ></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label2" runat="server" Text="员工姓名：" style="margin-left:20px;"></asp:Label>
                <asp:TextBox ID="txtName" runat="server" style="margin-right:20px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" />
                <input type="button" value="打印" id="btnPrint"  style="margin-left:10px"/>
                <input type="hidden" id="saveInfo" runat="server" />
               <asp:Button ID="btnExcel" runat="server" Text="导出Excel" 
                    style="margin-left:10px;" onclick="btnExcel_Click" /></span>
                
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
                </div>
            </div>
           
        </div>
        
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                <td class="tdOperar_名次">
                        名次
                    </td>
                    <td class="tdOperar_年份">
                        年份
                    </td>
                    <td class="tdOperar_员工姓名">
                        员工姓名
                    </td>
                    <td class="tdOperar_绩效">
                        绩效
                    </td>
                    <td class="tdOperar_考试">
                        考试
                    </td>
                    <td class="tdOperar_总分数">
                        总分数
                    </td>
                  
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                         <td class="tdOperar_名次">
                                <%#Eval("名次")%>
                            </td>
                             <td class="tdOperar_年份">
                                <%#Eval("年份")%>
                            </td>
                            
                            <td class="tdOperar_员工姓名">
                                <%#Eval("员工姓名")%>
                            </td>
                            <td class="tdOperar_绩效">
                                <%#Eval("绩效")%>
                            </td>
                            <td class="tdOperar_考试">
                                <%#Eval("考试")%>
                            </td>
                            <td class="tdOperar_总分">
                                <%#Eval("总分数")%>
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
