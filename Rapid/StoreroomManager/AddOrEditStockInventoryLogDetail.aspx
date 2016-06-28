<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditStockInventoryLogDetail.aspx.cs"
    Inherits="Rapid.StoreroomManager.AddOrEditStockInventoryLogDetail" UICulture="af" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>库存盘点明细编辑</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {

        $("#btnSubmit").click(function() {
  
            })
            //            function Quantity() {
            //                var pagerqty = $("#txtPaperQty").val();
            //                var inventoryqty = $("#txtInventoryQty").val();
            //                var profitandlossqty = 0;
            //                try {
            //                    profitandlossqty = pagerqty - inventoryqty;

            //                }
            //                catch (e) {
            //                    profitandlossqty = 0;
            //                }
            //                if (isNaN(profitandlossqty) || profitandlossqty < 0) {
            //                    profitandlossqty = 0;
            //                }
            //                $("#txtProfitAndLossQty").val(profitandlossqty);
            //                //                alert($("#txtProfitAndLossQty").val(profitandlossqty));
            //            }
            //            $("#txtPaperQty").blur(function() {
            //                Quantity();
            //            });
            //            $("#inventoryqty").blur(function() {
            //                Quantity();
            //            });
            //            $("#txtProfitAndLossQty").focus(function() {
            //                Quantity();
            //            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                盘点编号：
            </td>
            <td>
                <asp:Label ID="lbInventoryNumber" runat="server" Text="" size=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                物料编号：
            </td>
            <td>
                <asp:Label ID="lbMaterialNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:Label ID="lbVersion" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                账面数量：
            </td>
            <td>
                <asp:TextBox ID="txtPaperQty" runat="server" CssClass="input" ReadOnly="true" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                实盘数量：
            </td>
            <td>
                <asp:TextBox ID="txtInventoryQty" runat="server" CssClass="input" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr visible="false" style="display: none;">
            <td align="right">
                盈盘数量：
            </td>
            <td>
                <asp:TextBox ID="txtProfitAndLossQty" runat="server" ReadOnly="true" size="25" CssClass="input required "></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" Width="300px" MaxLength="200" Height="31px"
                    CssClass="input"></asp:TextBox>
                <asp:Label ID="lblMemo" runat="server" Text="（限制输入200字）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="修改" OnClick="btnSubmit_Click" class="submit" />
                <%--  <asp:Label ID="lbMsg" runat="server" Text="" ></asp:Label>--%>
                <asp:Label ID="Label4" runat="server" Text="（*号为必填项）" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
