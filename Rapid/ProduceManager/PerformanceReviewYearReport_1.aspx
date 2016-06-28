<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerformanceReviewYearReport_1.aspx.cs" Inherits="Rapid.ProduceManager.PerformanceReviewYearReport_1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Ա����Ч��ȱ���</title>
     <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            //��ѯsql���


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
                    alert("��ѡ��Ҫ��ӡ����!");
                    return;
                }
                if (!confirm("ȷ����ӡ��ѡ�У�")) {
                    return;
                }
                //����border��ʽ��table�µ�td 
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

        //��ȡ��ѯ����
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            return condition;
        }

        //����Execlǰ����ѯ��������д�����ر�ǩ
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
        margin-top: 20px">Ա����Ч��ȱ���</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
          
            <div style="position: relative; float: left; margin-bottom:10px">
                &nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="��ݣ�"></asp:Label>
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
                <asp:Label ID="Label2" runat="server" Text="Ա��������" style="margin-left:20px;"></asp:Label>
                <asp:TextBox ID="txtName" runat="server" style="margin-right:20px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="��ѯ" onclick="btnSearch_Click" />
                <input type="button" value="��ӡ" id="btnPrint"  style="margin-left:10px"/>
                <input type="hidden" id="saveInfo" runat="server" />
               <asp:Button ID="btnExcel" runat="server" Text="����Excel" 
                    style="margin-left:10px;" onclick="btnExcel_Click" /></span>
                
                <div id="choosePrintClounm">
                    <div>
                        ��ѡ��Ҫ��ӡ����</div>
                    <ul>
                     <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_����" checked="checked" />
                                ����
                                </label>
                        </li>
                         <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_���" checked="checked" />
                                ���
                                </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_Ա������" checked="checked" />
                                Ա������
                                </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_��Ч" checked="checked" />
                                ��Ч
                                </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_����" checked="checked" />
                                ����
                               </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_�ܷ���" checked="checked" />
                                �ܷ���
                                </label>
                        </li>
      
                    </ul>
                    <div>
                        &nbsp;<br />
                        <input type="button" value=" ȷ �� " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                            type="button" value=" ȡ �� " id="btnExit" /></div>
                </div>
            </div>
           
        </div>
        
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                <td class="tdOperar_����">
                        ����
                    </td>
                    <td class="tdOperar_���">
                        ���
                    </td>
                    <td class="tdOperar_Ա������">
                        Ա������
                    </td>
                    <td class="tdOperar_��Ч">
                        ��Ч
                    </td>
                    <td class="tdOperar_����">
                        ����
                    </td>
                    <td class="tdOperar_�ܷ���">
                        �ܷ���
                    </td>
                  
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                         <td class="tdOperar_����">
                                <%#Eval("����")%>
                            </td>
                             <td class="tdOperar_���">
                                <%#Eval("���")%>
                            </td>
                            
                            <td class="tdOperar_Ա������">
                                <%#Eval("Ա������")%>
                            </td>
                            <td class="tdOperar_��Ч">
                                <%#Eval("��Ч")%>
                            </td>
                            <td class="tdOperar_����">
                                <%#Eval("����")%>
                            </td>
                            <td class="tdOperar_�ܷ�">
                                <%#Eval("�ܷ���")%>
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
