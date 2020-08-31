/// <reference path="../../../Libs/jquery-3.1.1.min.js" />

var items = [];
$(document).ready(function () {
    $(document).on('click', '#btn-add-item', function (e) {
        var $template = (name, count, price) => `<tr class="new-item" data-price="${price}">
                            <td>${name}</td>
                            <td>${count}</td>
                            <td><i class="delete-item zmdi zmdi-delete"></td>
                        </tr>`;
        let $drugId = $('#modal #DrugId');
        var data = $drugId.select2('data');

        if (data[0].id) {
            let sum = 0;
            $('#items tr').each(function () {
                if ($(this).data('price')) sum += $(this).data('price');
            });
            console.log(sum);
            $($template(data[0].text, data[0].id, data[0].Price)).insertBefore($('tr.total-sum'));
            items.push(data[0].id);
            $drugId.val(null).trigger('change');
        }
    });
    $(document).on('click', '.delete-item', function (e) {
        let $tr = $(this).closest('tr');
        let url = $tr.data('url');
        if (url) {
            ajaxActionLink.inProgress($tr);
            $.post(url)
                .done(function (rep) {
                    ajaxActionLink.normal();
                    if (rep.Issuccessful) $tr.remove();
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
        if (!$frm.valid()) return;
        let model = customSerialize($('#frm-drug-store'));
        model.Items = items.map(id => ({
            DrugId: id
        }));
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
});

function imageZoom(imgID, resultID) {
    var img, lens, result, cx, cy;
    img = document.getElementById(imgID);
    result = document.getElementById(resultID);
    /*create lens:*/
    lens = document.createElement("DIV");
    lens.setAttribute("class", "img-zoom-lens");
    /*insert lens:*/
    img.parentElement.insertBefore(lens, img);
    /*calculate the ratio between result DIV and lens:*/
    cx = result.offsetWidth / lens.offsetWidth;
    cy = result.offsetHeight / lens.offsetHeight;
    /*set background properties for the result DIV:*/
    result.style.backgroundImage = "url('" + img.src + "')";
    result.style.backgroundSize = (img.width * cx) + "px " + (img.height * cy) + "px";
    /*execute a function when someone moves the cursor over the image, or the lens:*/
    lens.addEventListener("mousemove", moveLens);
    img.addEventListener("mousemove", moveLens);
    /*and also for touch screens:*/
    lens.addEventListener("touchmove", moveLens);
    img.addEventListener("touchmove", moveLens);
    function moveLens(e) {
        var pos, x, y;
        /*prevent any other actions that may occur when moving over the image:*/
        e.preventDefault();
        /*get the cursor's x and y positions:*/
        pos = getCursorPos(e);
        /*calculate the position of the lens:*/
        x = pos.x - (lens.offsetWidth / 2);
        y = pos.y - (lens.offsetHeight / 2);
        /*prevent the lens from being positioned outside the image:*/
        if (x > img.width - lens.offsetWidth) { x = img.width - lens.offsetWidth; }
        if (x < 0) { x = 0; }
        if (y > img.height - lens.offsetHeight) { y = img.height - lens.offsetHeight; }
        if (y < 0) { y = 0; }
        /*set the position of the lens:*/
        lens.style.left = x + "px";
        lens.style.top = y + "px";
        /*display what the lens "sees":*/
        result.style.backgroundPosition = "-" + (x * cx) + "px -" + (y * cy) + "px";
    }
    function getCursorPos(e) {
        var a, x = 0, y = 0;
        e = e || window.event;
        /*get the x and y positions of the image:*/
        a = img.getBoundingClientRect();
        /*calculate the cursor's x and y coordinates, relative to the image:*/
        x = e.pageX - a.left;
        y = e.pageY - a.top;
        /*consider any page scrolling:*/
        x = x - window.pageXOffset;
        y = y - window.pageYOffset;
        return { x: x, y: y };
    }
}


