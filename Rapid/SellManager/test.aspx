<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Rapid.SellManager.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Js/jquery-easyui-1.4/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jquery-easyui-1.4/themes/icon.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-easyui-1.4/jquery.min.js" type="text/javascript"></script>

    <script src="../Js/jquery-easyui-1.4/jquery.easyui.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        // data-options="valueField:'CustomerId',textField:'CustomerName',url:'AjaxGetCustomerJson.aspx'"
        $(function() {
            $.ajax({
                type: "Post",
                url: "AjaxGetCustomerJson.aspx",
                data: {},
                success: function(res) {
                    var obj = eval("(" + res + ")");
                    $('#cc').combobox({
                        data: obj,
                        valueField: "CustomerId",
                        textField: "CustomerName"
                    });
                }
            });

            $("#slThem").change(function() {
                $("head link:first").attr("href", "../Js/jquery-easyui-1.4/themes/" + $(this).val() + "/easyui.css");
            });
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList runat="server" ID="cc" class="easyui-combobox" Style="width: 200px;"
            OnSelectedIndexChanged="cc_SelectedIndexChanged">
        </asp:DropDownList>
        <select id="slThem">
            <option value='black'>black</option>
            <option value='bootstrap'>bootstrap</option>
            <option value='default'>default</option>
            <option value='gray'>gray</option>
        </select>
        <%--<input id="cc" class="easyui-combobox" name="dept" style="width:200px;" />--%>
    </div>
    </form>
</body>
</html>
