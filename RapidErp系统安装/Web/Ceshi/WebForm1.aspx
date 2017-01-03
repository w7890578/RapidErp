<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Rapid.Ceshi.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title> 
    <script src="../Js/jquery-1.3.2.min.js"></script>
</head>
<body>
    <form>
        <div>
        </div>
    </form>
</body>
</html>
<script>
    $(function () {
        $.ajax({
            url: "WebForm1.aspx",
            type: "POST",
            data: { "ajax": "en" },
            success: function (res) {

            },
            error: function (error) {
                alert(JSON.stringify(error));
            }
        });
    })
</script>
