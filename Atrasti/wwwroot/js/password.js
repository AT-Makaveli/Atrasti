mandatoryInputCheckFunction = function (val, el) {
    if ($.trim(val) == '') {
        if (el.id == 'passwordold') {
            return txt.password_error_missing_old;
        } else if (el.id == 'Password') {
            return txt.password_error_missing_new;
        } else if (el.id == 'ConfirmPassword') {
            return txt.password_error_missing_repetition;
        } else {
            return MISSING_FIELD_MESSAGE;
        }
    } else {
        return null;
    }
};

$(document).ready(function () {

    $('#pwd_rule_info, .pwd-rules-info-switch, #pwd_rule_info_back').click(function () {
        $('#pwd-change-form .vertical-input-form, #pwd-set-reset-form .vertical-input-form, #pwdRules, #pe-pwd-rules, #pe-pwd-change').toggle();
        resizeIFrame();
    });

    $('#Password').keyup(clearPwErrors)
    bindPwdRegexRule("#Password", /[A-Z]+/, "#pwd_rule_composition_1");
    bindPwdRegexRule("#Password", /[a-z]+/, "#pwd_rule_composition_2");
    bindPwdRegexRule("#Password", /[0-9]+/, "#pwd_rule_composition_3");
    bindPwdRegexRule("#Password", /[!"#$%&'()*+,./;:=?_@>-]+/, "#pwd_rule_composition_4");

    bindPwdRegexRule("#Password", /^.{8,}$/, "#pwd_rule_composition_5");
});


function bindPwdRule(src, checkFunction, dest) {
    src = $(src);
    dest = $(dest);
    src.keyup(function () {
        if (checkFunction(src.val())) {
            dest.removeClass("nok").addClass("ok");
        } else {
            dest.removeClass("ok").addClass("nok");
        }
    });
}
function bindPwdRegexRule(src, regex, dest) {
    bindPwdRule(src, function (val) {
        return regex.test(val);
    }, dest);
}

function clearPwErrors() {
    detachError($('#Password'));
    detachError($('#ConfirmPassword'));
    resizeIFrame();
}