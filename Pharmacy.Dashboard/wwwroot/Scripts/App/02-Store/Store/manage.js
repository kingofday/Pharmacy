/// <reference path="../../../Libs/jquery-3.1.1.min.js" />
var logo = {};
var coords = [];
var map = {};
var marker = {};
$(document).ready(function () {
    //====================================================================== logo
    //======================================================================
    $('#modal').on('click', '.btn-remove', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $box = $btn.closest('.single-uploader');

        if ($btn.data('remove-from-server')) {
            ajaxBtn.inProgress($btn);
            $.post($btn.data('url'))
                .done(function (data) {
                    console.log(data);
                    ajaxBtn.normal();
                    if (data.IsSuccessful) {
                        logo = {};
                        $btn.data('remove-from-server', false);
                        $box.removeClass('uploaded');
                    }
                    else showNotif(notifyType.danger, data.Message);
                })
                .fail(function (e) {
                    ajaxBtn.normal();
                });
        }
        else {
            $box.removeClass('uploaded');
            logo = {};
        }
    });
    $('#modal').on('click', '.single-uploader > .uploader', function (e) {
        e.stopPropagation();
        if ($(this).parent().hasClass('uploaded')) return;
        $(this).closest('.single-uploader').find('.input-file').trigger('click');
    });
    $('#modal').on('change', '.input-file', function (event) {
        event.stopPropagation();
        var $i = $(this);
        var file = this.files[0];
        var reader = new FileReader();
        let $box = $i.closest('.single-uploader');
        reader.onload = function (e) {
            var fileType = getFileType(file.name);
            var url = '';
            if (fileType === fileTypes.Image) url = e.target.result;
            else url = getDefaultImageUrl(file.name);

            $box.addClass('uploaded').find('img').attr('src', url);
            $i.val('');
            logo = file;
        };
        reader.readAsDataURL(file);
    });
    //====================================================================== Submit
    //======================================================================

    $(document).on('click', '.btn-submit-store', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        let model = customSerialize($('#frm-product'));
        let frmObj = objectToFormData(model);
        frmObj.append('Logo', logo);
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


