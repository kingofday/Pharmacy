/// <reference path="../../../Libs/jquery-3.1.1.min.js" />
var postsPageNumber = 1;
var posts = [];
var selectedPosts = [];
var assets = [];
$(document).ready(function () {

    //====================================================================== Assets
    //======================================================================
    $('#modal').on('click', '.btn-remove', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $box = $btn.closest('.single-uploader');

        let removeUrl = $btn.data('url');
        console.log(removeUrl);
        function removeAsset(id) {
            let idx = assets.findIndex(function (x) {
                return x.id === id;
            });
            assets.splice(idx, 1);
        }
        if (removeUrl) {
            ajaxBtn.inProgress($btn);
            $.post(removeUrl)
                .done(function (data) {
                    console.log(data);
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
            assets.push({ id: $box.data('id'), file: file });
        };
        reader.readAsDataURL(file);
    });
    //====================================================================== Submit
    //======================================================================

    $(document).on('click', '.btn-submit-product', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $frm = $btn.closest('form');
        let model = customSerialize($('#frm-product'));
        let tags = JSON.parse($('#tags_wrapper').val());
        model.TagIds = [];
        for (let i = 0; i < tags.length; i++)
            model.TagIds.push(tags[i].Value);
        let frmData = objectToFormData(model);
        console.log('fired');
        //return;
        //for (var k in model) {
        //    frmData.append(k, model[k]);
        //}
        for (var i = 0; i < assets.length; i++)  frmData.append('Files', assets[i].file);

        ajaxBtn.inProgress($btn);
        $.ajax({
            type: 'POST',
            url: $frm.attr('action'),
            data: frmData,
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
function getPosts($btn) {
    ajaxBtn.inProgress($btn);
    console.log($btn.data('url'));
    $.get($btn.data('url'), { username: $('#select-store option:selected').data('un'), pageNumber: postsPageNumber })
        .done(function (rep) {
            ajaxBtn.normal();
            if (rep.IsSuccessful) {
                posts = rep.Result.Posts;
                $('#posts-wrapper').html(rep.Result.Partial);
            }
            else {
                showNotif(notifyType.danger, rep.Message);
            }
        })
        .fail(function (e) {
            ajaxBtn.normal();
        });
}

