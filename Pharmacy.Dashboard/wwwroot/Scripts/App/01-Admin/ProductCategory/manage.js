/// <reference path="../../../Libs/jquery-3.1.1.min.js" />

$(document).ready(function () {

    $(document).on('click', '.btn-submit', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        let model = customSerialize($frm);
        ajaxBtn.inProgress($btn);
        $.post($frm.attr('action'), model)
            .done(function (rep) {
                if (rep.IsSuccessful) {
                    $('#nested-view').replaceWith(rep.Result);
                    showNotif(notifyType.success, strings.success);
                }
                else {
                    showNotif(notifyType.danger, rep.Message);
                }
                ajaxBtn.normal();
            })
            .fail(function () {
                ajaxBtn.normal();

            });

    });
});


