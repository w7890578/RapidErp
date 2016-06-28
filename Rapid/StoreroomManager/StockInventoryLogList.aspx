<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TableList.Master" AutoEventWireup="true"
    CodeBehind="StockInventoryLogList.aspx.cs" Inherits="Rapid.StoreroomManager.StockInventoryLogList" %>

<%--����  3��13��--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function TransferForQuoteInfo(number) {
            number = $.trim(number);
            window.location.href = "StockInventoryLogDetail.aspx?InventoryNumber=" + number;
        }
        $(function () {
            $("#navHead").html("&nbsp;&nbsp;��ҳ&nbsp;&nbsp;>&nbsp;&nbsp;������&nbsp;&nbsp;>&nbsp;&nbsp;����̵��б�");
        })
    </script>

    <script type="text/javascript">
        //�����ֶ�
        var sortname = "�̵���";
        //�������
        var sortdirection = "desc";
        //��ǰҳ
        var pageindex = 1;
        //��ҳ��
        var pageCount = 0;
        //������
        var totalRecords = 0;
        //һҳ��ʾ����
        var pageSize = 0;
        //��ѯsql���
        var querySql = "";

        //��ȡ��ѯ����
        function GetQueryCondition() {
            var condition = " where 1=1 ";

            var InventoryNumber = $("#InventoryNumber").val();
            //var InventoryNumber = $("#InventoryNumber").find("option:selected").text();

            var warehouseName = $("#WarehouseName").val();
            var WarehouseName = $("#WarehouseName").find("option:selected").text();

            var inventoryType = $("#InventoryType").val();
            var InventoryType = $("#InventoryType").find("option:selected").text();


            if (InventoryNumber != "" ) {
                condition += " and �̵��� like '%" + InventoryNumber + "%' ";
            }
            if (warehouseName != "" && warehouseName != null) {
                condition += " and �ֿ�����='" + WarehouseName + "' ";
            }
            if (inventoryType != "" && inventoryType != null) {
                condition += " and �̵�����='" + InventoryType + "' ";
            }


            return condition;
        }

        //����Execlǰ����ѯ��������д�����ر�ǩ
        function ImpExecl() {
            querySql = "   select  * from [V_StockInventoryLog]  ";
            querySql = querySql + " " + GetQueryCondition();
            $("#ctl00_ContentPlaceHolder1_saveInfo").val(querySql + "");
            //$("#progressBar").show();
            return true;
        }

        //��ȡ����
        function GetData(pageIndex, sortName, sortDirection) {
            //��ȡһҳ��ʾ����
            pageSize = $("#txtPageSize").val();
            if (pageSize == "" || isNaN(pageSize)) {
                alert("����ȷ����ÿҳ��ʾ����");
                return;
            }
            querySql = "   select * from V_StockInventoryLog  ";
            querySql = querySql + " " + GetQueryCondition();
            
            $.ajax({
                type: "Get",
                url: "AjaxGetStockInventoryLog.aspx",
                data: { time: new Date(), pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql },
                beforeSend: function () { $("#progressBar").show(); },
                success: function (result) {
                    //�������
                    $(".tablesorter tbody").html("");
                    //��������ݾ�׷��
                    if (result != "") {
                        var tempArray = result.split("^");
                        //��ҳ��
                        pageCount = tempArray[0];
                        //׷��html
                        $(".tablesorter tbody").append(tempArray[1]);

                        $(".tablesorter tbody tr").click(function () {
                            $(this).find("input[type='checkbox']").each(function () {
                                this.checked = !this.checked; //������ѡ
                            });
                        });
                        $(".tablesorter tbody tr").hover(function () {
                            $(this).find("td").css("background-color", "yellow");
                        }, function () {
                            $(this).find("td").css("background-color", "white");
                        });
                        //                        $(".tablesorter tbody tr:odd").hover(function() {
                        //                            $(this).find("td").css("background-color", "yellow");
                        //                        }, function() {
                        //                            $(this).find("td").css("background-color", "#EAFCD5");
                        //                        });
                        $("#pageing").html(tempArray[2]);
                        //������
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //���û������
                            var tempStr = " <tr> <td colspan='11' align='center'>  �������� </td> </tr>";
                            $(".tablesorter tbody").append(tempStr);
                            //��ҳ���
                            $("#pageing").html("");
                        }
                    }
                    //loading����
                    $("#progressBar").hide();
                    $(".tablesorter thead tr td input[type='checkbox']").attr("checked", false);

                }
            });
        }
        //��ҳ���
        function aClick(index) {
            if (index == "��һҳ") {
                pageindex = 1;
            }
            else if (index == "��һҳ") {
                if (pageindex != 1) {
                    pageindex = parseInt(pageindex) - 1;
                }
            }
            else if (index == "��һҳ") {
                if (pageindex != pageCount) {
                    pageindex = parseInt(pageindex) + 1;
                }
            }
            else if (index == "���һҳ") {
                pageindex = pageCount;
            }
            else {
                pageindex = index;
            }
            pageSize = $("#txtPageSize").val();
            if (pageSize == "" || isNaN(pageSize)) {
                alert("����ȷ����ÿҳ��ʾ����");
                return;
            }
            //�����ǰ����ҳ������ҳ��
            var tempPageCount = parseInt(totalRecords) % parseInt(pageSize);
            if (tempPageCount > 0) {
                tempPageCount = (parseInt(totalRecords) / parseInt(pageSize)) + 1;
            }
            else {
                tempPageCount = (parseInt(totalRecords) / parseInt(pageSize));
            }
            if (pageindex > tempPageCount) {
                pageindex = 1;
            }
            GetData(pageindex, sortname, sortdirection);
        }

        $(document).ready(function () {
            //��ѯ
            $("#btnSearch").click(function () {
                GetData(1, sortname, sortdirection);
            });

            //ɾ��
            $("#btnDelete").click(function () {
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("��ѡ��Ҫɾ�����У�");
                    return;
                }
                //ȥ�����һ������
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //���ǻ�ȡ��ֵ
                if (confirm("ȷ��ɾ��ѡ�������?")) {
                    // alert(ConvertsContent(checkResult));
                    //ͨ��ɾ��
                    DeleteData("../StoreroomManager/AjaxGetStockInventoryLog.aspx", ConvertsContent(checkResult), "btnSearch");
                }
            });

            //�������¼�����ʽ
            function tablesorter(className) {
                var obj = $("." + className + " thead tr th");
                obj.find("img").hide();
                //�����¼�
                obj.click(function () {
                    obj.find("img").hide();

                    sortname = $(this).attr("sortname");
                    if (sortdirection == "asc") {
                        $(this).find("img").attr("src", "../Img/asc.gif").show();
                        sortdirection = "desc";
                    }
                    else {
                        $(this).find("img").attr("src", "../Img/desc.gif").show();
                        sortdirection = "asc";
                    }
                    var index = $(".current").html();
                    if (index == null) {
                        index = 1;
                    }
                    GetData(1, sortname, sortdirection);
                });
            }

            //ȫѡ/��ѡ
            $(".tablesorter thead tr td input").click(function () {
                $("input[name='subBox']").each(function () {
                    this.checked = !this.checked; //������ѡ
                });
            });

            $("#btnAdd").click(function () {
                OpenDialog("../StoreroomManager/AddOrEditStockInventoryLog.aspx", "btnSearch", "180", "600");
            });

            //��
            tablesorter("tablesorter");
            //����ҳ���������
            $("#btnSearch").click();

            //���̵���
            //            BindSelect("StockInventoryLogInventoryNumber", "InventoryNumber");
            //�󶨲ֿ�����
            BindSelect("StockInventoryLogWarehouseName", "WarehouseName");
            //���̵�����
            BindSelect("StockInventoryLogInventoryType", "InventoryType");
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="saveInfo" runat="server" />
     
    <table class="pg_table">
        <tr>
            <td class="pg_talbe_head">�ֿ����ƣ�
            </td>
            <td class="pg_talbe_content">
                <select id="WarehouseName">
                    <option value="">- - - - - �� ѡ �� - - - - -</option>
                </select>
            </td>
            <td class="pg_talbe_head">�̵����ͣ�
            </td>
            <td class="pg_talbe_content">
                <select id="InventoryType">
                    <option value="">- - - - - �� ѡ �� - - - - -</option>
                </select>
            </td>
            <td class="pg_talbe_head">�̵��ţ�
            </td>
            <td class="pg_talbe_content">
                <input type="text" id="InventoryNumber" />
            </td>
            <td class="pg_talbe_head"></td>
            <td class="pg_talbe_content"></td>
        </tr>
        <tr>
            <td colspan="8">&nbsp;
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td colspan="5" style="text-align: left">
                <div style="vertical-align: middle">
                    <div style="float: left; width: 150;">
                        ÿҳ��ʾ������
                        <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                            onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                            type="text" style="width: 60px;" id="txtPageSize" value="100" maxlength="3" />
                        &nbsp;&nbsp;
                    </div>
                </div>
                <div>
                    <div style="float: left; width: 65px;">
                        <input type="button" value="��ѯ" id="btnSearch" class="button" />
                    </div>
                    <div style="float: left; width: 65px;" id="divAdd" runat="server">
                        <input type="button" value="�̵�" id="btnAdd" class="button" />
                    </div>
                    <div style="float: left; width: 65px;" id="divDelete" runat="server">
                        <input type="button" value="ɾ��" id="btnDelete" class="button" />
                    </div>
                    <div style="float: left; height: 24px; display: <%=Rapid.ToolCode.Tool.GetUserMenuFunc("L0405", "Exp")%>;" id="divExp"  >
                        <asp:Button ID="Button1" runat="server" Text="����Excel" OnClientClick="return ImpExecl()"
                            CssClass="button" OnClick="Button1_Click1" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <div>
                    <table class="tablesorter" cellpadding="1" cellspacing="1" width="1220px;">
                        <thead>
                            <tr>
                                <td nowrap>
                                    <label style="width: 100%; display: block; cursor: pointer;">
                                        <input type="checkbox" />ȫѡ/��ѡ</label>
                                </td>
                                <th nowrap sortname='���' style="display: none;">���<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                    id="Img10" /></span>
                                </th>
                                <th nowrap sortname='�ֿ�����'>�ֿ�����<span><img src="../Img/bg.gif" id="Img1" /></span>
                                </th>
                                <th nowrap sortname='������'>������<span><img src="../Img/bg.gif" id="Img2" /></span>
                                </th>
                                <th nowrap sortname='�̵�����'>�̵�����<span><img src="../Img/bg.gif" id="Img5" /></span>
                                </th>
                                <th nowrap sortname='�̵�ʱ��'>�̵�ʱ��<span><img src="../Img/bg.gif" id="Img8" /></span>
                                </th>
                                <td nowrap>�����
                                </td>
                                <th nowrap sortname='���ʱ��'>���ʱ��<span><img src="../Img/bg.gif" id="Img3" /></span>
                                </th>
                                <td nowrap>��ע
                                </td>
                                <th nowrap sortname='�̵���'>�̵���<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                </th>
                                <td nowrap>�༭
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="10" align="center">��������
                                </td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="10" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px; padding-right: 10px;">
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
