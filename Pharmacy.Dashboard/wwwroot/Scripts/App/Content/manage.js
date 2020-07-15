/// <reference path="../../Libs/jquery-3.1.1.min.js" />
var ThumbnailImage = {};
var FullScreenImage = {};
var HeaderImage = {};
var ContentImage = {};
var ContentVideo = {};
var ContentAudio = {};
var ContentDocument = {};

$(document).ready(function () {
    //submit view
    $(document).on('click', '.btn-submit-content', function () {
        let $btn = $(this);
        let $frm = $(this).closest('form');
        if (!$frm.valid()) return;
        let model = customSerialize($frm, true);
        let tags = JSON.parse($('#temp_tags').val());
        model.ContentTags = tags.map(x => ({ TagId: parseInt(x.Value), ContentId: parseInt($('#ContentId').val()) }));
        model.AttachedFiles = [];
        if (typeof ThumbnailImage.AttachedFileId !== 'undefined')
            model.AttachedFiles.push(ThumbnailImage);
        if (typeof FullScreenImage.AttachedFileId !== 'undefined')
            model.AttachedFiles.push(FullScreenImage);
        if (typeof HeaderImage.AttachedFileId !== 'undefined')
            model.AttachedFiles.push(HeaderImage);
        if (typeof ContentImage.AttachedFileId !== 'undefined')
            model.AttachedFiles.push(ContentImage);
        if (typeof ContentVideo.AttachedFileId !== 'undefined')
            model.AttachedFiles.push(ContentVideo);
        if (typeof ContentAudio.AttachedFileId !== 'undefined')
            model.AttachedFiles.push(ContentAudio);
        if (typeof ContentDocument.AttachedFileId !== 'undefined')
            model.AttachedFiles.push(ContentDocument);
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

    $(document).on('change', '#type_selector', function () {
        console.log('changed');
        let $select = $(this);
        if ($select.val() === '') return;
        $('.up-type').addClass('d-none');
        $('.' + $select.val()).removeClass('d-none');
    });

});

