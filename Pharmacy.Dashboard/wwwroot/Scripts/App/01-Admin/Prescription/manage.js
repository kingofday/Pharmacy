/// <reference path="../../../Libs/jquery-3.1.1.min.js" />

var items = [];
$(document).ready(function () {
    $(document).on('click', '#btn-add-item', function (e) {
        let $btn = $(this);
        let $frm = $btn.closest('form');

        if (!$frm.valid()) return;
        let count = $('#modal #Count').val();
        if (isNaN(count)) {
            showNotif(notifyType.danger, validationMessages.number);
            return;
        }
        if (parseInt(count) < 1) {
            showNotif(notifyType.danger, 'حداقل تعداد یک می باشد');
            return;
        }
        var $template = (name, count, price, discount) => `<tr class="new-item" data-price="${price}" data-discount="${discount}" data-count="${count}">
                            <td>${name}</td>
                            <td>${count}</td>
                            <td>${commaThousondSeperator(discount)}</td>
                            <td>${commaThousondSeperator(price)}</td>
                            <td><i class="delete-item zmdi zmdi-delete default-i"></td>
                        </tr>`;
        let $drugId = $('#modal select[name="DrugId"]');
        var data = $drugId.select2('data');

        if (data[0].id) {
            let sum = 0;
            let price = 0, discount = 0;
            $('#items tr').each(function () {
                price = $(this).data('price');
                discount = $(this).data('discount')
                if (price) sum += ((price - discount) * count);
            });
            $($template(data[0].text, count, data[0].Price, data[0].DiscountPrice)).insertBefore($('tr.tr-total'));
            items.push({ DrugId: data[0].id, Count: count });
            $drugId.val(null).trigger('change');
            $('#total-price').text(commaThousondSeperator(sum + (data[0].Price * count)) + strings.moneyCurrency)
        }
    });
    $(document).on('click', '.delete-item', function (e) {
        let $tr = $(this).closest('tr');
        let url = $(this).data('url');
        if (url) {
            ajaxActionLink.inProgress($tr);
            $.post(url)
                .done(function (rep) {
                    ajaxActionLink.normal();
                    if (rep.IsSuccessful) {
                        $tr.remove();
                        $('#items').replaceWith(rep.Result);
                    }
                    else showNotif(notifyType.danger, rep.Message);
                })
                .fail(function (e) {
                    ajaxActionLink.normal();
                    showNotif(notifyType.danger, strings.error);
                });
        }
        else {
            let idx = $('.new-item').index($tr);
            items.splice(idx, 1);
            $tr.remove();
        }
    });
    //====================================================================== Submit
    //======================================================================
    $(document).on('click', '.btn-submit', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $frm = $btn.closest('form');
        let model = customSerialize($frm);
        model.Items = items;
        ajaxBtn.inProgress($btn);
        $.ajax({
            type: 'POST',
            url: $frm.attr('action'),
            data: model,
            success: function (rep) {
                ajaxBtn.normal();
                if (!rep.IsSuccessful) showNotif(notifyType.danger, rep.Message);
                else {
                    $('#modal').modal('hide');
                }
            },
            error: function (e) {
                ajaxBtn.normal();

            }
        });
    });

    $(document).on('click', '#btn-send-link', function (e) {
        let $btn = $(this);
        ajaxBtn.inProgress($btn);
        $.post($btn.data('url'))
            .done(function (rep) {
                ajaxBtn.normal();
                if (rep.IsSuccessful) showNotif(notifyType.success, rep.Message);
                else showNotif(notifyType.danger, rep.Message);
            })
            .fail(function (e) { ajaxBtn.normal(); });

    });
});


