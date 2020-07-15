/// <reference path="../../Libs/jquery-3.1.1.min.js" />
var mainAttch = {};
$(document).ready(function () {
    //submit view
    $(document).on('click', '.btn-submit-answer', function () {
        let $btn = $(this);
        let $frm = $(this).closest('form');
        let model = customSerialize($frm, true);
        model.AttachedFiles = [];
        if (typeof mainAttch.AttachedFileId !== 'undefined')
            model.AttachedFiles.push(mainAttch);
        console.log(model);
        ajaxBtn.inProgress($btn);
        postObjectList(
            $frm.attr('action'),
            model,
            function (rep) {
                ajaxBtn.normal();
                if (rep.IsSuccessful)
                    $('#modal').modal('hide');
                else $frm.inlineNotify(notifyType.danger, rep.Message);
            },
            function (e) {
                ajaxBtn.normal();
                showNotif(notifyType.danger, strings.error);
            }
        );
    });

});

