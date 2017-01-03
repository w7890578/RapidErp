
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
//文本框只能输入数字(不包括小数)，并屏蔽输入法和粘贴 
// </summary>
// ---------------------------------------------------------------------- 
$.fn.integer = function() {
    $(this).css("ime-mode", "disabled");
    this.bind("keypress", function(e) {
        var code = (e.keyCode ? e.keyCode : e.which); //兼容火狐 IE 
        if (!$.browser.msie && (e.keyCode == 0x8)) { //火狐下不能使用退格键 
            return;
        }
        return code >= 48 && code <= 57;
    });
    this.bind("paste", function() {
        return false;
    });
    this.bind("keyup", function() {
        if (/(^0+)/.test(this.value)) {
            this.value = this.value.replace(/^0*/, '');
        }
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
    $(".digits").onlyNum();
    $(".number").number();
    $(".msgtable tr ").each(function() {
        $(this).find("td:eq(1)").append("<label style='width:100px;height:30px;color:red;'>* &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</la&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;bel>");
    });
    $(".msgtable tr:last td:last label").remove();
    //不是必选的，td加上noRequired样式
    $(".noRequired label").remove();

    $(".required").blur(function() {
        var result = $(this).val();
        result = $.trim(result);
        if (result == "" || result == null) {
            $(this).next("label").html("<img src='../Css/Verification/error.gif' />必选");
        }
        else {
            $(this).next("label").html("<img src='../Css/Verification/success.gif' />");
        }
    });
    $(".digits").blur(function() {
        var result = $(this).val();
        result = $.trim(result);
        if (isNaN(result) || result == "" || result == null) {
            $(this).next("label").html("<img src='../Css/Verification/error.gif' />只能输入数字");
        }
        else {
            $(this).next("label").html("<img src='../Css/Verification/success.gif' />");
        }
    })

    $("#btnSubmit").click(function() {
        var result = "";
        var i = 0;
        $(".required").each(function() {
            result = $(this).val();
            if (result == "" || result == null) {
                $(this).next("label").html("<img src='../Css/Verification/error.gif' />必选");

                i++;
            }
        })
        $(".digits").each(function() {
            result = $(this).val();
            result = $.trim(result);
            if (isNaN(result) || result == "" || result == null) {
                $(this).next("label").html("<img src='../Css/Verification/error.gif' />只能输入数字");
                i++;
            }
        });
        if (i > 0) {
            return false;
        }
        else {
            return true;
        }
    });
})