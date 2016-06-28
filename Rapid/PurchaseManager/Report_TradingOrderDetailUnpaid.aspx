<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_TradingOrderDetailUnpaid.aspx.cs" Inherits="Rapid.PurchaseManager.Report_TradingOrderDetailUnpaid" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>原材料贸易销售订单未交</title>
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
  
    <form id="form1" runat="server">
        <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px">
            贸易销售订单未交<%=Request["Id "] %>
        </div>
        <input type="hidden" id="Id" name="Id" />
        <div style="text-align: right; margin: 10px 50px 10px 0px;">
        </div>
        <div style="margin: 10px;">
            <table class="classtable">
                <thead>
                    <tr>
                        <%for (int i = 0; i < dtResult.Columns.Count; i++)
                            {
                                var item = dtResult.Columns[i];
                                if (item.ColumnName.ToLower().Equals("guid")) { continue; }
                        %>
                        <th><%=item.ColumnName %>  </th>
                        <%  } %>
                       
                    </tr>
                </thead>
                <tbody>
                    <%foreach (System.Data.DataRow item in dtResult.Rows)
                        {
                    %>
                    <tr>
                        <%  for (int i = 0; i < dtResult.Columns.Count; i++)
                            {
                                var column = dtResult.Columns[i];
                                if (column.ColumnName.ToLower().Equals("guid")) { continue; }
                        %>
                        <td><%=item[i] %></td>
                        <%  }%>
                        
                    </tr>
                    <%} %>
                </tbody>
            </table>
        </div> 
    </form>
     
</body>
</html>

