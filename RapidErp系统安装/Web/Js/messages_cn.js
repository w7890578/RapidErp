/*
* Translated default messages for the jQuery validation plugin.
* Locale: CN
*/
jQuery.extend(jQuery.validator.messages, {
    required: "必选",
    remote: "请修正该字段",
    email: "电子邮件格式有误",
    url: "请输入合法的网址",
    date: "请输入合法的日期",
    dateISO: "请输入合法的日期 (ISO).",
    number: "请输入合法的数字",
    digits: "只能输入整数",
    creditcard: "请输入合法的信用卡号",
    equalTo: "请再次输入相同的值",
    accept: "请输入拥有合法后缀名的字符串",
    maxlength: jQuery.validator.format("长度最多 {0} 个字符"),
    minlength: jQuery.validator.format("长度最少 {0} 个字符"),
    rangelength: jQuery.validator.format("长度要在 {0} 和 {1} 之间"),
    range: jQuery.validator.format("请输入一个介于 {0} 和 {1} 之间的值"),
    max: jQuery.validator.format("请输入一个最大为 {0} 的值"),
    min: jQuery.validator.format("请输入一个最小为 {0} 的值")
});



// ----------------------------------------------------------------------
// <summary>
// 限制只能输入数字
// </summary>
// ----------------------------------------------------------------------
$.fn.onlyNum = function() {
    $(this).keypress(function(event) {
        var eventObj = event || e;
        var keyCode = eventObj.keyCode || eventObj.which;
        if ((keyCode >= 48 && keyCode <= 57))
            return true;
        else
            return false;
    }).focus(function() {
        //禁用输入法
        this.style.imeMode = 'disabled';
    }).bind("paste", function() {
        //获取剪切板的内容
        var clipboard = window.clipboardData.getData("Text");
        if (/^\d+$/.test(clipboard))
            return true;
        else
            return false;
    });
};
// ----------------------------------------------------------------------
// <summary>
// 限制只能输入数字和小数点 
// </summary>
// ---------------------------------------------------------------------- 
$.fn.number = function() {
    $(this).css("ime-mode", "disabled");
    this.bind("keypress", function(e) {
        var code = (e.keyCode ? e.keyCode : e.which); //兼容火狐 IE 
        if (!$.browser.msie && (e.keyCode == 0x8)) { //火狐下不能使用退格键 
            return;
        }
        if (this.value.indexOf(".") == -1) {
            return (code >= 48 && code <= 57) || (code == 46);
        } else {
            return code >= 48 && code <= 57
        }
    });
    this.bind("paste", function() {
        return false;
    });
    this.bind("keyup", function() {
        if (this.value.slice(0, 1) == ".") {
            this.value = "";
        }
    });
    this.bind("blur", function() {
        if (this.value.slice(-1) == ".") {
            this.value = this.value.slice(0, this.value.length - 1);
        }
    });
};



$(function() {
    //表单验证JS
    $("#form1").validate({
        //出错时添加的标签
        errorElement: "span",
        success: function(label) {
            //正确时的样式
            label.text(" ").addClass("success");
        }
    });
    $(".digits").onlyNum();
    $(".number").number();
});



