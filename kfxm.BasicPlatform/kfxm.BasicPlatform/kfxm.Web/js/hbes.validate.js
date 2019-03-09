$(function () {

    $(".form-control").blur(function () {
        $(this).valiInput();
    });

    // $("#no-null").valiInput();
});
function valiType(vtype, el) {
    var resultObj = new Object();
    resultObj.isPass = false;
    resultObj.msg = "";
    if (vtype == null || vtype == undefined || $.trim(vtype) == "") {
        resultObj.isPass = true;
        return resultObj;
    }
    var valiArray = vtype.split(";");
    if (valiArray != null && valiArray != undefined && valiArray.length > 0) {
        for (var v = 0 ; v < valiArray.length; v++) {
            var vali = $.trim(valiArray[v]);
            if (vali != "") {
                var valiMap = vali.split(":");

                var valiKey = $.trim(valiMap[0]);
                var valiValue = $.trim(valiMap[1]);

                if (valiKey != null && valiKey != undefined && valiKey != "" && valiValue != null && valiValue != undefined && valiValue != "") {
                    switch (valiKey) {
                        case "required":
                            if (valiValue == "true") {
                                if (el.val().trim() == "") {
                                    resultObj.isPass = false;
                                    resultObj.msg = "不能为空";
                                    return resultObj;
                                }
                            }
                            break;
                        case "maxLength":
                            if (el.val() != "" && el.val().length > valiValue) {
                                resultObj.isPass = false;
                                resultObj.msg = "长度不能超过" + valiValue;
                                return resultObj;
                            }
                            break;
                        case "minLength":
                            if (el.val() != "" && el.val().length < valiValue) {
                                resultObj.isPass = false;
                                resultObj.msg = "长度不能少于" + valiValue;
                                return resultObj;
                            }
                            break;
                        case "rangeLength":
                            var startRange = valiValue.split(",")[0];
                            var endRange = valiValue.split(",")[1];
                            if (el.val() != "" && (el.val().length < startRange || el.val().length > endRange)) {
                                resultObj.isPass = false;
                                resultObj.msg = "字符长度必须在" + startRange + "到" + endRange + "之间";
                                return resultObj;
                            }
                            break;
                        case "max":
                            if (el.val() != "" && parseFloat(el.val()) > valiValue) {
                                resultObj.isPass = false;
                                resultObj.msg = "最大值为" + valiValue;
                                return resultObj;
                            }
                            break;
                        case "min":
                            if (el.val() != "" && parseFloat(el.val()) < valiValue) {
                                resultObj.isPass = false;
                                resultObj.msg = "最小值为" + valiValue;
                                return resultObj;
                            }
                            break;
                        case "range":
                            var startRange = valiValue.split(",")[0];
                            var endRange = valiValue.split(",")[1];
                            if (el.val() != "" && parseFloat(el.val()) < startRange || parseFloat(el.val()) > endRange) {
                                resultObj.isPass = false;
                                resultObj.msg = "值的范围在" + startRange + "到" + endRange + "之间";
                                return resultObj;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (valiKey != null && valiKey != undefined && valiKey != "") {
                    
                    switch (valiKey) {
                        case "int":
                            var test = /^-?\d+$/;
                            if (el.val() != "" && !test.test(el.val())) {
                                resultObj.isPass = false;
                                resultObj.msg = "请输入整数";
                                return resultObj;
                            }
                            break;
                        case "email":
                            var test = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
                            if (el.val() != "" && !test.test(el.val())) {
                                resultObj.isPass = false;
                                resultObj.msg = "请输入邮件格式";
                                return resultObj;
                            }
                            break;
                        case "url":
                            var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
                         + "?(([0-9a-z_!~*'().&amp;=+$%-]+: )?[0-9a-z_!~*'().&amp;=+$%-]+@)?" //ftp的user@
                         + "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184
                         + "|" // 允许IP和DOMAIN（域名）
                         + "([0-9a-z_!~*'()-]+\.)*" // 域名- www.
                         + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名
                         + "[a-z]{2,6})" // first level domain- .com or .museum
                         + "(:[0-9]{1,4})?" // 端口- :80
                         + "((/?)|" // a slash isn't required if there is no file name
                         + "(/[0-9a-z_!~*'().;?:@&amp;=+$,%#-]+)+/?)$";
                            var test = new RegExp(strRegex);
                            if (el.val() != "" && !test.test(el.val())) {
                                resultObj.isPass = false;
                                resultObj.msg = "请输入url格式";
                                return resultObj;
                            }
                            break;
                        case "float":
                            var test = /^(-?\d+)(\.\d+)?$/;
                            if (el.val() != "" && !test.test(el.val())) {
                                resultObj.isPass = false;
                                resultObj.msg = "请输入数字";
                                return resultObj;
                            }
                            break;
                    }
                }


            }
        }
        resultObj.isPass = true;
        return resultObj;
    }
    else {
        resultObj.isPass = true;
        return resultObj;
    }
}

(function ($) {
    $.fn.extend({
        valiInput: function () {
            var vtype = $(this).attr("vtype");
            var myVali = $(this).attr("onvalidation");

            var obj;
            if (myVali != null && myVali != undefined) {

                var func = eval(myVali);
                obj = new func($(this));
            }
            else {
                obj = valiType(vtype, $(this));
            }
            if (!obj.isPass) {
                $(this).css("border", "1px solid red");
                $(this).attr("title", obj.msg);
            }
            else {
                $(this).css("border", "");
                $(this).attr("title", "");
            }
            return obj.isPass;
        },
        valiForm: function () {
            var form_control_array = $(this).find(".form-control");
            if (form_control_array != null && form_control_array != undefined && form_control_array.length > 0) {
                var isPass = true;
                for (var i = 0 ; i < form_control_array.length; i++) {
                    var control = form_control_array[i];

                    if (!$(control).valiInput()) {
                        isPass = false;
                    };
                }
            }
            return isPass;
        }
    })
})(jQuery);