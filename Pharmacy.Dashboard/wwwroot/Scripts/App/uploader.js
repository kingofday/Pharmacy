///<reference path="../Libs/jquery-3.1.1.min.js" />

var attachments = [];
//====================================================================== single uploader 
//======================================================================
$('#modal').on('click', '.btn-remove', function (e) {
    e.stopPropagation();
    let $btn = $(this);
    let $box = $btn.closest('.single-uploader');

    let removeUrl = $btn.data('url');
    function removeAsset(id) {
        let idx = attachments.findIndex(function (x) {
            return x.id === id;
        });
        attachments.splice(idx, 1);
    }
    if (removeUrl) {
        ajaxBtn.inProgress($btn);
        $.post(removeUrl)
            .done(function (data) {
                ajaxBtn.normal();
                if (data.IsSuccessful) {
                    removeAsset($box.data('id'));
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
        removeAsset($box.data('id'));
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
        attachments.push({ id: $box.data('id'), type: $box.data('type'), file: file });
    };
    reader.readAsDataURL(file);
});