/// <reference path="../../../Libs/jquery-3.1.1.min.js" />
$(document).ready(function () {
    var model = { a: 'aa' };


    console.log(model);
    $('.btn-check').click(function () {
        if (!$('#Username').val()) return;
        let $btn = $(this);
        let $frm = $btn.closest('form');
        let btnText = $btn.text();
        let $a = $('<a>check</a>').attr({
            id: 'a_chk',
            href: 'https://www.instagram.com/' + $('#Username').val(),
            target: '_blank',
            class: 'd-none'
        });
        $a.appendTo('body');
        $('#a_chk')[0].click();
        $a.remove();
        console.log('clieced');
    });

    $('.btn-submit').click(function () {
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        let btnText = $btn.text();
        $btn.html($circularLoader);
        let model = customSerialize($frm);
        $.ajax({
            type: "POST",
            url: $frm.attr('action'),
            contentType: 'application/json;charset=utf-8',
            headers: {
                "X-CSRF-TOKEN": $('input[name="__RequestVerificationToken"]').val()
            },
            data: JSON.stringify(model),
            success: function (rep) {
                $btn.html(btnText);
                if (rep.IsSuccessful)
                    window.location.href = rep.Result;
                else {
                    showAlert(rep.Message);
                }
            },
            error: function (e) {
                $btn.html(btnText);
                showAlert(strings.error);
            }
        });
    });

});

var showAlert = function (msg) {
    let $alert = $('#error-message');
    $alert.find('.text').text(msg);
    $alert.slideDown();
    setTimeout(function () {
        $alert.slideUp();
    }, 4000);
};