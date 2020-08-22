/// <reference path="../../../Libs/jquery-3.1.1.min.js" />
var postsPageNumber = 1;
var posts = [];
var selectedPosts = [];
var assets = [];
var props = [];
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
    //====================================================================== props
    //======================================================================

    $(document).on('click', '#btn-prop', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        if (!$('#prop_name').val() || !$('#prop_value').val()) {
            showNotif(notifyType.danger, validationMessages.validationFailed);;
            return;
        }
        let prop = {
            name: $('#prop_name').val(),
            value: $('#prop_value').val()
        };
        props.push(prop);
        let $row = `<tr><td>${prop.name}</td><td>${prop.value}</td><td><a class="new-prop prop-delete"><i class="zmdi zmdi-close"> </i></a></td></tr>`;
        $('#props').append($row);
        $('#prop_name').val('');
        $('#prop_value').val('');
    });

    $('#modal').on('click', '.prop-delete', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let removeUrl = $btn.data('url');
        if (removeUrl) {
            ajaxBtn.inProgress($btn);
            $.post(removeUrl)
                .done(function (rep) {
                    ajaxBtn.normal();
                    if (!rep.IsSuccessful) showNotif(notifyType.danger, rep.Message);
                    else $btn.closest('tr').remove();

                })
                .fail(function (e) {
                    ajaxBtn.normal();
                });
        }
        else {
            
            let idx = $('.prop-delete.new-prop').index($btn);
            console.log($('.prop-delete.new-prop').length);
            console.log(idx);
            if (idx > -1) {
                props.splice(idx, 1);
                $btn.closest('tr').remove();
            }
        }
        console.log(props);
    });
    //====================================================================== Submit
    //======================================================================

    $(document).on('click', '.btn-submit-drug', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) {
            showNotif(notifyType.danger, validationMessages.validationFailed);
            return;
        }
        let model = customSerialize($('#frm-drug'));
        let tags = JSON.parse($('#tags_wrapper').val());
        model.TagIds = [];
        for (let i = 0; i < tags.length; i++)
            model.TagIds.push(tags[i].Value);
        model.Properties = props;
        console.log(model);
        let frmData = objectToFormData(model);
        //return;
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