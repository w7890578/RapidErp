<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderCheckPage.aspx.cs" Inherits="Rapid.StoreroomManager.OrderCheckPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单抽检</title>
    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <link href="../Js/AutoComplete/AutoComplete.css" rel="stylesheet" />
    <script src="../Js/AutoComplete/AutoComplete.js" type="text/javascript"></script>
    <!--日期插件-->
    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery.validate.min.js"></script>
    <script src="../Js/messages_cn.js" type="text/javascript"></script>
    <script src="../Js/Main.js" type="text/javascript"></script>
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

        #Remark {
            width: 141px;
        }

        #MaterialNumber {
            width: 163px;
        }

        #TakeQty {
            width: 161px;
        }

        #TakeDateTime {
            width: 160px;
        }

        .classtable {
            border: 1px solid #C8CCD1;
            border-width: 1px 0 0 1px;
            width: 100%;
            font-size: 12px;
        }

            .classtable th, .classtable td {
                padding: 5px 10px;
                border: 1px solid #C8CCD1;
                border-width: 0 1px 1px 0;
                color: #555;
                line-height: 18px;
                text-align: center;
                vertical-align: top;
            }

            .classtable th {
                background: #E5EFFB;
                white-space: nowrap;
            }
    </style>
</head>
<body>
    <div id="floatBoxBg" style="display: none; width: 100%; height: 100%; opacity: 0.5; background-color: rgb(0, 0, 0); top: 0px; left: 0px; position: fixed; z-index: 1002; background-position: initial initial; background-repeat: initial initial;">
    </div>
    <script type="text/javascript">
        var autoComplete;
        var materialAutoComplete;
        $(function () {
            var UserNames = "<%=BLL.PM_UserManager.Instance.GetUserNames()%>";
            var arrays = UserNames.split(",");
            autoComplete = new AutoComplete('TakeUserName', 'auto', arrays);

            var MaterilNames = "<%=BLL.MarerialInfoTableManager.GetMarerialNames()%>";
            var materialArrays = MaterilNames.split(",");
            materialAutoComplete = new AutoComplete('MaterialNumber', 'MaterialAuto', materialArrays);
        })
    </script>
    <form id="form1" runat="server">
          <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px">
            采购入库检验表   
        </div>
        <input type="hidden" id="Id" name="Id" />
        <div style="text-align: right; margin: 10px 50px 10px 0px;"><a href="###" onclick="AddRow()">添加</a></div>
        <div style="margin: 10px;">
            <table class="classtable">
                <thead>
                    <tr>
                        <th>原材料编号</th>
                        <th>抽检数量</th>
                        <th>抽检时间</th>
                        <th>是否合格</th>
                        <th>检验员</th>
                        <th>备注</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <%foreach (var item in orderCheckList)
                      { %>
                    <tr>
                        <td><%=item.MaterialNumber %></td>
                        <td><%=item.TakeQty %></td>
                        <td><%=item.TakeDateTime.ToLongDateString() %></td>
                        <td><%=(int)item.Status==1?"不合格":"合格" %></td>
                        <td><%=item.TakeUserName %></td>
                        <td><%=item.Remark %></td>
                        <td><a href="###" onclick="EditRow('<%=item.Id %>')">编辑</a>&nbsp;&nbsp;<a href="###" onclick="Delete('<%=item.Id %>')">删除</a></td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
        <div id="divDialog" style="display: none; background-color: white; border: 2px solid rgb(138, 159, 175); position: absolute; left: 39%; width: 406px; height: 313px; top: 102px; z-index: 10000;">
            <div style="width: 100%; height: 30px; background-color: #0DB1E8; color: white; font-size: 17px; font-weight: 500; text-align: center; line-height: 30px;">抽检信息维护</div>
            <div style="padding: 20px;">
                <table>
                    <tr>
                        <td style="text-align: right;">原材料编号：</td>
                        <td style="position: relative;">
                            <input type="text" style="height: 15px; font-size: 12px;" placeholder="请输入原材料编号" id="MaterialNumber" name="MaterialNumber" onkeyup="materialAutoComplete.start(event)" autocomplete="off" required="required" />
                            <div class="auto_hidden" id="MaterialAuto">
                                <!--自动完成 DIV-->
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">抽检数量：</td>
                        <td>
                            <input type="text" id="TakeQty" name="TakeQty" required="required" class="number" /></td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">抽检时间：</td>
                        <td>
                            <input type="text" id="TakeDateTime" name="TakeDateTime" onfocus="WdatePicker({skin:'green'})" required="required" /></td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">是否合格：</td>
                        <td>
                            <select id="Status" name="Status">
                                <option selected="selected" value="2">合格</option>
                                <option value="1">不合格</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">检验员：</td>
                        <td style="position: relative;">
                            <input type="text" style="height: 15px; font-size: 12px;" placeholder="请输入检验员姓名" id="TakeUserName" name="TakeUserName" onkeyup="autoComplete.start(event)" autocomplete="off" required="required" />
                            <div class="auto_hidden" id="auto">
                                <!--自动完成 DIV-->
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">备注：</td>
                        <td>
                            <textarea id="Remark" name="Remark" rows="5" style="width: 270px;"></textarea></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <input  type="button" value=" 保 存 " id="btnSave" style="width: 100px; height: 32px; cursor: pointer;" />&nbsp;&nbsp;<input type="button" value=" 取 消 " id="btnCancel" onclick="javascript: $('#divDialog,#floatBoxBg').hide();" style="width: 100px; height: 32px; cursor: pointer;" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>

    <script type="text/javascript">

        function AddRow() {
            $("#Id").val("");
            $("#MaterialNumber").val("").removeAttr("disabled");
            $("#TakeQty").val("");
            $("#TakeDateTime").val("");
            $("#Status").val("");
            $("#TakeUserName").val("");
            $("#Remark").val("");
            $("#divDialog").show();
            $("#floatBoxBg").show();
        }

        function Delete(Id) {
            if (confirm("确定删除该条记录？")) {
                $.post("AjaxOrderCheck.aspx", {
                    Id: Id,
                    Action: "Delete"
                }, function (data) {
                    if (data.Status) {
                        alert("删除成功！");
                        location.reload();
                    }
                    else {
                        alert(data.Msg);
                    }
                }, "json");
            }
        }

        function EditRow(Id) {
            $.post("AjaxOrderCheck.aspx", {
                Id: Id,
                Action: "Get"
            }, function (data) {
                $("#Id").val(data.Id);
                $("#MaterialNumber").val(data.MaterialNumber).attr("disabled", "disabled");
                $("#TakeQty").val(data.TakeQty);
                $("#TakeDateTime").val(data.TakeDateTimeStr);
                $("#Status").val(data.Status);
                $("#TakeUserName").val(data.TakeUserName);
                $("#Remark").val(data.Remark);
                $("#divDialog").show();
                $("#floatBoxBg").show();
            }, "json");
        }

        $(function () {
            $("#btnSave").click(function () {
                if ($("#form1").valid()) {
                    $.post("AjaxOrderCheck.aspx", {
                        Id: $.trim($("#Id").val()),
                        WarehouseNumber: "<%=Request["WarehouseNumber"]%>",
                        MaterialNumber: $.trim($("#MaterialNumber").val()),
                        TakeQty: $.trim($("#TakeQty").val()),
                        TakeDateTime: $.trim($("#TakeDateTime").val()),
                        Status: $.trim($("#Status").val()),
                        TakeUserName: $.trim($("#TakeUserName").val()),
                        Remark: $.trim($("#Remark").val()),
                        Action: "Save"
                    }, function (data) {
                        if (data.Status) { 
                            location.reload();
                        }
                        else {
                            alert(data.Msg);
                        }
                    }, "json");
                }
            });
            //表单验证JS
            $("#form1").validate({
                //出错时添加的标签
                errorElement: "span",
                success: function (label) {
                    //正确时的样式
                    label.text(" ").addClass("success");
                }
            });
        })
    </script>
</body>
</html>
