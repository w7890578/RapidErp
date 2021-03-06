
(function () {
    $.fn.extend({
        checks_select: function (btnId, options, firstText) {
            jq_checks_select = null;
            //var firstText = "===== 请选择原材料种类 =====";
            //if ($(this).val() != firstText)
            //{
            //    $(this).val(firstText);
            //}

            $(this).next().empty(); //先清空 
            $(this).unbind("click");
            $(this).click(function (e) {
                jq_check = $(this);
                //jq_check.attr("class", ""); 
                if (jq_checks_select == null) {
                    jq_checks_select = jq_check.next();
                    jq_checks_select.addClass("checks_div_select");
                    //jq_checks_select = $("<div class='checks_div_select'></div>").insertAfter(jq_check); 
                    $.each(options, function (i, n) {
                        check_div = $("<div><input type='checkbox' value='" + n.value + "' text='" + n.text + "'>" + n.text + "</div>").appendTo(jq_checks_select);
                        check_box = check_div.children();
                        check_box.click(function (e) {
                            //jq_check.attr("value",$(this).attr("value") ); 
                            var tempValue = "";
                            var tempText = "";
                            $(this).parents(".checks_div").find("input:checked").each(function (i) {
                                if (i == 0) {
                                    tempText = $(this).attr("text");
                                    tempValue = $(this).val();
                                } else {
                                    tempText += "," + $(this).attr("text");
                                    tempValue += "," + $(this).val();
                                }
                            });
                            //alert(temp); 
                            jq_check.val(tempText);
                            jq_check.attr("values", tempValue);
                            e.stopPropagation();
                        });
                    });
                    jq_checks_select.show();
                    var txtKindValues = $(this).val();
                    if (txtKindValues != "") {
                        $(".checks_div_select input[type=checkbox]").each(function (i, item) {
                            var itemValue = $(this).val();
                            var temps = txtKindValues.split(",");
                            $.each(temps, function (h, arryvalue) {
                                if (itemValue == arryvalue) {
                                    $(item).attr("checked", true);
                                }
                            });

                        });
                    } 
                } else {
                    jq_checks_select.toggle();

                }
                e.stopPropagation();
            });
            $(document).click(function () {
                flag = $("#" + btnId);
                if (flag.val() == "") {
                    //flag.val(firstText);
                }
                jq_checks_select.hide();
            });
        }
    })

})(jQuery);