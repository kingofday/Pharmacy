/// <reference path="../../../Libs/jquery-3.1.1.min.js" />
var coords = [];
var map = {};
var marker = {};
$(document).ready(function () {

    //====================================================================== Submit
    //======================================================================

    $(document).on('click', '.btn-submit-store', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        let model = customSerialize($('#frm-drug-store'));
        let frmObj = objectToFormData(model);
        console.log(attachments);
        frmObj.append('Logo', attachments.find(x => x.id = 'logo').file);
        ajaxBtn.inProgress($btn);
        $.ajax({
            type: 'POST',
            url: $frm.attr('action'),
            data: frmObj,
            contentType: false,
            processData: false,
            success: function (rep) {
                ajaxBtn.normal();
                if (!rep.IsSuccessful)
                    showNotif(notifyType.danger, rep.Message);
                else {
                    $('#modal').modal('hide');
                }
            },
            error: function (e) {
                ajaxBtn.normal();

            }
        });
    });
});


