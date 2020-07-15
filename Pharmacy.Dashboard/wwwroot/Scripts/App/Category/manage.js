/// <reference path="../../Libs/jquery-3.1.1.min.js" />
var mainImage = {};
$(document).ready(function () {
    //submit view
    $(document).on('click', '.btn-submit-category', function () {
        let $btn = $(this);
        let $frm = $(this).closest('form');
        let model = customSerialize($frm, true);
        model.AttachedFiles = [];
        if (typeof mainImage.AttachedFileId !== 'undefined')
            model.AttachedFiles.push(mainImage);
        let tags = JSON.parse($('#temp_tags').val());
        model.CategoryTags = tags.map(x => ({ TagId: parseInt(x.Value), CategoryId: parseInt($('#CategoryId').val()) }));
        ajaxBtn.inProgress($btn);
        console.log(model);
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

