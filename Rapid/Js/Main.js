//屏蔽js错误
window.onerror = function () { return true; }

//复选框选中改变本行颜色
function SetTrColor(obj) {
    if (obj.is(":checked")) {
        obj.parent().parent().find("td").each(function () {
            $(this).css("background-color", "#CCBBFF");
        });
    }
    else {
        obj.parent().parent().find("td").each(function () {
            $(this).css("background-color", "white");
        });
    }
}

//关闭当前窗口
function WindowClose() {
    window.close();
}

//遮罩提示窗口
function jsmsg(w, h, msgtitle, msgbox, url, msgcss) {
    $("#msgdialog").remove();
    var cssname = "";
    switch (msgcss) {
        case "Success":
            cssname = "icon-01";
            break;
        case "Error":
            cssname = "icon-02";
            break;
        default:
            cssname = "icon-03";
            break;
    }
    var str = "<div id='msgdialog' title='" + msgtitle + "'><p class='" + cssname + "'>" + msgbox + "</p></div>";
    $("body").append(str);
    $("#msgdialog").dialog({
        //title: null,
        //show: null,
        bgiframe: true,
        autoOpen: false,
        width: w,
        //height: h,
        resizable: false,
        closeOnEscape: true,
        buttons: { "确定": function () { $(this).dialog("close"); } },
        modal: true
    });
    $("#msgdialog").dialog("open");
    if (url == "back") {
        sysMain.history.back(-1);
    } else if (url != "") {
        sysMain.location.href = url;
    }
}

//可以自动关闭的提示
function jsprint(msgtitle, url, msgcss) {
    $("#msgprint").remove();
    var cssname = "";
    switch (msgcss) {
        case "Success":
            cssname = "pcent correct";
            break;
        case "Error":
            cssname = "pcent disable";
            break;
        default:
            cssname = "pcent warning";
            break;
    }
    var str = "<div id=\"msgprint\" class=\"" + cssname + "\">" + msgtitle + "</div>";
    $("body").append(str);
    $("#msgprint").show();
    if (url == "back") {
        sysMain.history.back(-1);
    } else if (url != "") {
        sysMain.location.href = url;
    }
    //3秒后清除提示
    setTimeout(function () {
        $("#msgprint").fadeOut(500);
        //如果动画结束则删除节点
        if (!$("#msgprint").is(":animated")) {
            $("#msgprint").remove();
        }
    }, 3000);
}

//#####################
//    常用功能js
//#####################
//字符串格式化
String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}

//格式化当前时间为yyyy-mm-dd形式
function getNowFormatDate() {
    var day = new Date();
    var Year = 0;
    var Month = 0;
    var Day = 0;
    var CurrentDate = "";
    //初始化时间
    //Year= day.getYear();//有火狐下2008年显示108的bug
    Year = day.getFullYear(); //ie火狐下都可以
    Month = day.getMonth() + 1;
    Day = day.getDate();
    //Hour = day.getHours();
    // Minute = day.getMinutes();
    // Second = day.getSeconds();
    CurrentDate += Year + "-";
    if (Month >= 10) {
        CurrentDate += Month + "-";
    }
    else {
        CurrentDate += "0" + Month + "-";
    }
    if (Day >= 10) {
        CurrentDate += Day;
    }
    else {
        CurrentDate += "0" + Day;
    }
    return CurrentDate;
}

//判断日期是否为标准格式 yyyy-MM-dd
function isDateString(sDate) {
    sDate = $.trim(sDate);
    var mp = /\d{4}-\d{2}-\d{2}/;
    var matchArray = sDate.match(mp);
    if (matchArray == null) return false;
    var iaMonthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    var iaDate = new Array(3);
    var year, month, day;
    iaDate = sDate.split("-");
    year = parseFloat(iaDate[0])
    month = parseFloat(iaDate[1])
    day = parseFloat(iaDate[2])
    if (year < 1900 || year > 2100) return false;
    if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
        iaMonthDays[1] = 29;
    if (month < 1 || month > 12) return false;
    if (day < 1 || day > iaMonthDays[month - 1]) return false;
    return true;
}

//空值检测
function CheckNull(str) {
    str = $.trim(str);
    if (str == "") {
        return true;
    }
    else if (str == "请选择") {
        return true;
    }
    else {
        return false;
    }
}

/// <summary>
/// 内容形式转换
/// </summary>
/// <param name="value">1,2,3</param>
/// <returns>'1','2','3'</returns>
function ConvertsContent(value) {
    //如果包含“,”
    if (value.indexOf(",") >= 0) {
        //正则替换所有",",注意：不用正则只替换第一找到的","
        value = "'" + value.replace(/,/g, "','") + "'";
    }
    else {
        value = "'" + value + "'";
    }
    return value;
}

// 打开普通窗口
function OpenNewWindow(url, height, width) {
    window.open(url + "", "newwin", "height=" + height + ",width=" + width + ",toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
}
//打开模态窗口
function OpenDialog(url, queryId, height, width) {
    window.showModalDialog("" + url, "", "dialogWidth:" + width + "px;dialogHeight:" + height + "px;scroll:no;status:no");
    //模态窗口关闭，再执行一遍查询
    $("#" + queryId).click();
}

////3秒后关闭页面【不能用】
//function PageClose() {
//   window.setTimeout(function() {
//        window.close();
//    }, 3000);
//}

//#####################
//  控件绑定通用js
//#####################

//绑定下拉控件
function BindSelect(contentType, controlId) {
    $.ajax({
        type: "Get",
        url: "../AjaxRequest/GetToolContent.aspx?time=" + new Date(),
        data: { contentType: contentType },
        success: function (result) {
            $("#" + controlId).html("");
            $("#" + controlId).html(result);
        }
    });
}
//页面自己绑定
function BindSelectForMe(contentType, urlName, controlId) {
    $.ajax({
        type: "Get",
        url: urlName + "?time=" + new Date(),
        data: { contentType: contentType },
        success: function (result) {
            $("#" + controlId).html("");
            $("#" + controlId).html(result);
        }
    });
}

//#####################
//  数据操作通用js
//#####################

//删除数据 url:页面路径，ids:主键集合，queryId:查询按钮Id
function DeleteData(url, ids, queryId) {
    $.ajax({
        type: "Get",
        url: url + "?time=" + new Date(),
        data: { ids: ids },
        success: function (result) {
            if (result == "1") {
                alert("删除成功！");
                $("#" + queryId).click();
            }
            else {
                alert("删除失败！原因：" + result);
                return;
            }
        }
    });
}

function DeleteDataProperty(url, id, ids, queryId) {
    $.ajax({
        type: "Get",
        url: url + "?time=" + new Date(),
        data: { id: id, ids: ids },
        success: function (result) {
            if (result == "1") {
                alert("删除成功！");
                $("#" + queryId).click();
            }
            else {
                alert("删除失败！原因：" + result);
                return;
            }
        }
    });
}

function DeleteProductProperty(url, id, version, ids, queryId) {
    $.ajax({
        type: "Get",
        url: url + "?time=" + new Date(),
        data: { id: id, version: version, ids: ids },
        success: function (result) {
            if (result == "1") {
                alert("删除成功！");
                $("#" + queryId).click();
            }
            else {
                alert("删除失败！原因：" + result);
                return;
            }
        }
    });
}
//重载
function DeleteProductCuttingLineInfo(url, id, version, materialnumber, ids, queryId) {
    $.ajax({
        type: "Get",
        url: url + "?time=" + new Date(),
        data: { ProductNumber: id, Version: version, MaterialNumber: materialnumber, ids: ids },
        success: function (result) {
            if (result == "1") {
                alert("删除成功！");
                $("#" + queryId).click();
            }
            else {
                alert("删除失败！原因：" + result);
                return;
            }
        }
    });
}
function DeleteProductWorkSnCoefficient(url, id, version, WorkSnNumber, ids, queryId) {
    $.ajax({
        type: "Get",
        url: url + "?time=" + new Date(),
        data: { ProductNumber: id, Version: version, WorkSnNumber: WorkSnNumber, ids: ids },
        success: function (result) {
            if (result == "1") {
                alert("删除成功！");
                $("#" + queryId).click();
            }
            else {
                alert("删除失败！原因：" + result);
                return;
            }
        }
    });
}

//#####################
//  页面打印js
//#####################
var hkey_root, hkey_path, hkey_key
hkey_root = "HKEY_CURRENT_USER" //注册表根目录
hkey_path = "\\Software\\Microsoft\\Internet Explorer\\PageSetup\\"
//设置网页打印的页眉页脚为空
function pagesetup_null() {
    try {
        var RegWsh = new ActiveXObject("WScript.Shell")
        hkey_key = "header"//页眉
        RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "")

        hkey_key = "footer"//页脚
        RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "")

        hkey_key = "margin_left"//左边距
        RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "0.7520")

        hkey_key = "margin_right"//右边距
        RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "0.7520")

        hkey_key = "margin_top"//上边距
        RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "0.0")

        hkey_key = "margin_bottom"//下边距
        RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "0.0")
    } catch (e) {
        alert("您的ie浏览器限制了打印操作：请打开你的ie浏览器internet选项—— 安全—— 自定义级别—— 把对没有标记为安全的activex控件进行初始化和脚本运行 设置为启用");
        return false;
    }
}

function doPrint(printDiv, btnPrint, btnBack, btnAdd) {
    try {
        //pagesetup_null();

        newwin = window.open("../Index/Print.aspx", "newwin", "height=900,width=750,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
        newwin.document.body.innerHTML = document.getElementById(printDiv).innerHTML;
        //alert(document.getElementById(printDiv).innerHTML+"");
        //        newwin.document.getElementById("table1").style.width = '750';
        //        newwin.document.getElementById("table1").style.height = '800';
        newwin.document.getElementById(btnPrint).style.display = 'none';
        //newwin.document.getElementsByClassName("back").style.display = 'none';
        //newwin.document.getElementsByClassName("add").style.display = 'none';
        if (btnAdd != '') {
            newwin.document.getElementById(btnAdd).style.display = 'none';
        }
        if (btnBack != '') {
            newwin.document.getElementById(btnBack).style.display = 'none';
        }

        newwin.window.print();
        newwin.window.close();
        //        pagesetup_default();
    }
    catch (e) { }
}

//==========================页面加载时JS函数开始===============================
//输入框显示提示效果，配合CSS运用
$(function () {
    $(".input,.login_input,.textarea").focus(function () {
        $(this).addClass("focus");
    }).blur(function () {
        $(this).removeClass("focus");
    });

    //输入框提示,获取拥有HintTitle,HintInfo属性的对象
    $("[HintTitle],[HintInfo]").focus(function (event) {
        $("*").stop(); //停止所有正在运行的动画
        $("#HintMsg").remove(); //先清除，防止重复出错
        var HintHtml = "<ul id=\"HintMsg\"><li class=\"HintTop\"></li><li class=\"HintInfo\"><b>" + $(this).attr("HintTitle") + "</b>" + $(this).attr("HintInfo") + "</li><li class=\"HintFooter\"></li></ul>"; //设置显示的内容
        var offset = $(this).offset(); //取得事件对象的位置
        $("body").append(HintHtml); //添加节点
        $("#HintMsg").fadeTo(0, 0.85); //对象的透明度
        var HintHeight = $("#HintMsg").height(); //取得容器高度
        $("#HintMsg").css({ "top": offset.top - HintHeight + "px", "left": offset.left + "px" }).fadeIn(500);
    }).blur(function (event) {
        $("#HintMsg").remove(); //删除UL
    });

    document.onkeydown = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {
            var btnSearch = $("#btnSearch");
            if (btnSearch != null && btnSearch != undefined) {
                btnSearch.click();
                return false;
            }
            //.click();
        }
    }
});

//------------------------------------------------------------------------------------------------------------------------
//打开模态窗口（有滚动条）
function OpenDialogWithscroll(url, queryId, height, width) {
    window.showModalDialog("" + url, "", "dialogWidth:" + width + "px;dialogHeight:" + height + "px;status:no");
    //模态窗口关闭，再执行一遍查询
    $("#" + queryId).click();
}
//获取url参数
function getQueryString(name) { var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i"); var r = window.location.search.substr(1).match(reg); if (r != null) return unescape(r[2]); return null; }
//审核
function Check(url, check, queryId) {
    $.ajax({
        type: "Get",
        url: url + "?time=" + new Date(),
        data: { check: check },
        success: function (result) {
            if (result == "1") {
                alert("审核成功！");
                $("#" + queryId).click();
            }
            else {
                alert("审核失败！原因：" + result);
                return;
            }
        }
    });
}

$(function () {
    //    var screenWidth = window.screen.width;
    //    screenWidth = screenWidth - 177 - 50;
    //    $("#outsideDiv").attr("style", "width: " + screenWidth + "px; overflow: auto");
    //全局修复table样式问题 IE10及以上
    $.each($(".border td"), function () {
        if ($(this).css("display") == "inline") {
            $(this).css("display", "");
        }
    });
    $.each($(".tablesorter td,th"), function () {
        var nowrap = $(this).attr("nowrap");
        if (nowrap == undefined) {
            $(this).attr("nowrap", "nowrap")
        }
    })
})