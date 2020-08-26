var notifyType = {
    success: "success",
    danger: "danger",
    info: "info",
    warning: "warning"
};

var fireGlobalPlugins = function () {
    $('.footable').footable({
        pageSize: 200,
        "breakpoints": {
            phone: 576,
            tablet: 1024
        }
    });

    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green'
    });

    $('.pdate').Zebra_DatePicker();

    $("select.select2:not(.with-ajax)").each(function () {
        if (!$(this).data('select2-fired')) {
            let nullable = $(this).find('option[value=""]').length > 0;
            $(this).data('select2-fired', true).select2({
                placeholder: strings.pleaseSelect,
                allowClear: nullable,
                "language": {
                    "noResults": function () {
                        return strings.thereIsNoResult;
                    }
                }
            });
        }

    });

    $('select.select2.with-ajax').each(function () {
        if (!$(this).data('select2-fired')) {
            let $elm = $(this).prop({ 'autocomnplete': 'off', 'autocorrect': 'off' });
            let minimumInputLength = typeof $elm.data('min-length') !== 'undefined' ? $elm.data('min-length') : 2;
            let $opt = $(this).find('option[value=""]');
            let nullable = $opt.length > 0;
            $elm.data('select2-fired', true).select2({
                placeholder: nullable ? $opt.text() : strings.pleaseSelect,
                searchInputPlaceholder: strings.searchHere,
                allowClear: nullable,
                language: {
                    noResults: function () {
                        return strings.thereIsNoResult;
                    },
                    searching: function () { return strings.searching; },
                    inputTooShort: function () { return ''; }
                },
                minimumInputLength: parseInt(minimumInputLength),
                ajax: {
                    url: $elm.attr('data-url'),
                    dataType: 'json',
                    data: function (params) {
                        var query = {
                            q: params.term
                        };
                        return query;
                    },
                    processResults: function (data) {
                        return {
                            results: data.map(x => ({ text: x.Text, id: x.Value }))
                        };
                    }
                }
            });
        }

    });


    //place holder for select2 input plugin
    //$(document).on("select2:open", function (event) {
    //    $('input.select2-search__field').attr('placeholder', strings.typeHere);
    //});
};

/*--------------------------------------
            notifications
---------------------------------------*/
$.fn.inlineNotify = function (type, message) {
    let $frm = $(this);
    let template = `<div class="row inner-notification">
                        <div class="col-12">
                            <div class="alert">
                                <p class="text"></p>
                            </div>
                        </div>
                    </div>`;
    let $elm = $frm.find('.inner-notification');
    if ($elm.length === 0)
        $elm = $(template).prependTo($frm);
    $elm.find('.alert').removeClass().addClass('alert alert-' + type);
    $elm.find('.text').text(message);
    $elm.slideDown(300);
    return this;
};

var showNotif = function (type, message) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "progressBar": true,
        "preventDuplicates": false,
        "positionClass": "toast-top-left",
        "onclick": null,
        "showDuration": "400",
        "hideDuration": "1000",
        "timeOut": "7000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    type = type === 'danger' ? 'error' : type;
    toastr[type](message);
};

var fireDropzone = function () {
    let getOptions = function ($elm) {
        let removable = true;

        if ($elm.data('removable') === 'false') removable = false;
        let opt = {
            renameFilename: function (fileName) {
                var ascii = /^[ -~\t\n\r]+$/;
                if (!ascii.test(fileName)) return "noneAscciFileName." + fileName.split('.').reverse()[0];
                else return fileName;
            },
            init: function () {
                this.on("addedfile", function (file) {
                    if (!file.type.match(/image.*/)) {
                        this.emit("thumbnail", file, getDefaultImageUrl(file.name));
                    }
                });
                let urls = $elm.data('file-urls');
                let temp = this;
                if (typeof urls !== 'undefined') {
                    urls.forEach(function (u) {
                        var mockFile = {
                            name: u.split('/').reverse()[0],
                            size: 10244,
                            accepted: true,
                            status: Dropzone.ADDED,
                            type: getFileType(u).type,
                            url: u
                        };
                        temp.files.push(mockFile);
                        temp.emit("addedfile", mockFile);
                        temp.emit("thumbnail", mockFile, u);
                        temp.emit('complete', mockFile);
                    });
                }
                let url = $elm.data('file-url');
                if (typeof url !== 'undefined') {
                    var mockFile = {
                        name: url.split('/').reverse()[0],
                        size: 10244,
                        accepted: true,
                        status: Dropzone.ADDED,
                        type: getFileType(url).type,
                        url: url
                    };
                    temp.files.push(mockFile);
                    temp.emit("addedfile", mockFile);
                    temp.emit("thumbnail", mockFile, url);
                    temp.emit('complete', mockFile);
                }
            },
            dictRemoveFile: typeof $elm.data('remove-message') !== 'undefined' ? $elm.data('remove-message') : strings.remove,
            addRemoveLinks: removable,
            url: $elm.data('url'),
            params: function () {
                let p = {};
                let params = $elm.data('params');
                if (typeof params !== 'undefined') {

                    for (var i = 0; i < params.length; i++) {
                        p[params[i].Key] = $('#' + params[i].Value).val();
                    }
                }
                return p;
            },
            maxFilesize: typeof $elm.data('max-file-size') !== 'undefined' ? $elm.data('max-file-size') : 500,
            acceptedFiles: $elm.data('accepted-files'),
            success: function (file, rep) {
                if (rep.IsSuccessful === true) {
                    file.url = rep.Result;
                    if (typeof $elm.data('success-func') !== 'undefined') {
                        eval($elm.data('success-func'))(rep.Result);
                    }
                    this.emit("complete", file);

                }
                else {
                    if (this.files.length === 1) {
                        this.removeAllFiles(true);
                    }
                    else {
                        var ref;
                        if (file.previewElement) {
                            if ((ref = file.previewElement) !== null) {
                                ref.parentNode.removeChild(file.previewElement);
                            }
                        }
                    }
                    showNotif(notifyType.danger, rep.Message);
                }

            },
            error: function (file, errorMessage, xhr) {
                console.log(errorMessage);
                showNotif(notifyType.danger, errorMessage);
                if (this.files.length === 1) {
                    this.removeAllFiles(true);
                }
                else {
                    var ref;
                    if (file.previewElement) {
                        if ((ref = file.previewElement) !== null) {
                            ref.parentNode.removeChild(file.previewElement);
                        }
                    }
                }
            }
        };
        //if (typeof $elm.data('accept') !== 'undefined')
        //    eval($elm.data('accept'))(file);
        if (removable)
            rep.removedfile = function (file) {
                let p = { url: file.url };
                let params = $elm.data('remove-params');
                if (typeof params !== 'undefined') {
                    for (var i = 0; i < params.length; i++) p[params[i].Key] = $('#' + params[i].Value).val();
                }
                let drop = this;
                $.post($elm.data('remove-url'), p)
                    .done(function (rep) {
                        var _ref;
                        let res = (_ref = file.previewElement) !== null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
                        if (!rep.IsSuccessful) {
                            drop.files.push(file);
                            drop.emit("addedfile", file);
                            drop.emit("thumbnail", file, file.url);
                            drop.emit('complete', file);
                            showNotif(notifyType.danger, rep.Message);
                        }
                        else if (typeof $elm.data('remove-func') !== 'undefined') {
                            eval($elm.data('remove-func'))(rep.Result);
                        }
                        //return res;
                    })
                    .fail(function (e) {
                        return void 0;
                    });
                console.log(this.files.length);
                //reset dropzone.js
                if (this.files.length === 0)
                    this.removeAllFiles(true);

                return this._updateMaxFilesReachedClass();
            };
        return rep;
    };

    $('.multi-uploader').each(function () {
        let $elm = $(this);
        let opts = getOptions($elm);
        opts.maxFiles = typeof $elm.data('max-files') !== 'undefined' ? $elm.data('max-files') : 10;
        opts.dictDefaultMessage = typeof $elm.data('default-message') !== 'undefined' ? $elm.data('default-message') : strings.dropYourFilesHere;

        $elm.dropzone(opts);
    });

    $('.single-uploader').each(function () {
        let $elm = $(this);
        let opts = getOptions($elm);
        opts.maxFiles = 1;
        opts.dictDefaultMessage = typeof $elm.data('default-message') !== 'undefined' ? $elm.data('default-message') : strings.dropYourFileHere;
        $elm.dropzone(opts);

    });
};
///<reference path="../Libs/jquery-3.1.1.min.js" />
//================================================== Map
//==================================================
var mapToken = 'pk.eyJ1Ijoia2luZ29mZGF5IiwiYSI6ImNrYWNweWQxaTFpbXcydnF3bDJiZ3QyODcifQ.RRNM5g4uAbca39ZWwp6p2A';
//================================================== Loader
//==================================================
var $threeDotLoader = '<span class="three-dot-loader"><span class="dot"></span><span class="dot"></span><span class="dot"></span></span>';
var $circularLoader = '<div class="spinner"><svg viewBox="25 25 50 50"><circle cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10"></circle></svg></div>';

function commaThousondSeperator(input) {
    let str = isNaN(input) ? input : input.toString();
    return str.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
};

$(document).ready(function () {

    var setActiveMenu = function () {
        let currentUrl = window.location.href.toString().toLowerCase();
        $('nav ul.nav > li:not(".nav-header")  a:not([href="#"])').each(function () {
            var linkUrl = $(this).attr('href');
            if (currentUrl.endsWith(linkUrl.toLowerCase().split('?')[0])) {
                $(this).closest('li').addClass('active').closest('.link-parent').addClass('active');
                let $parentUl = $(this).closest('ul.nav-second-level');
                if ($parentUl.length > 0) {
                    $parentUl.addClass('in');
                }
                return false;
            }
        });
    }();

    fireGlobalPlugins();

    //auto submit form with ajax
    $(document).on('click', '[data-ajax-submit="true"]', function () {
        let $btn = $(this);
        submitAjaxForm($btn, function (rep) { showNotif(notifyType.success, strings.success); });
    });

    //open close filter ibox
    $(document).on('click', '.filters .ibox-title', function (e) {
        var $ibox = $(this).closest('div.ibox');
        var button = $(this).find('.collapse-icon i');
        let nowClosed = button.hasClass('zmdi-chevron-down');
        var content = $ibox.children('.ibox-content');
        content.slideToggle(200);
        button.toggleClass('zmdi-chevron-up').toggleClass('zmdi-chevron-down');
        $ibox.toggleClass('').toggleClass('border-bottom');
        setTimeout(function () {
            $ibox.resize();
            $ibox.find('[id^=map-]').resize();
        }, 50);
        let $filters = $(this).closest('.filters');
        if ($ibox.hasClass('fixed-ibox'))
            return;
        if (nowClosed) {
            $ibox.animate({ 'margin-left': '-=125px' });
            $filters.animate({ 'padding-top': '+=40px' });
        }
        else {
            $ibox.animate({ 'margin-left': '+=125px' });
            $filters.animate({ 'padding-top': '-=40px' });
        }
    });

    //call appropriate action with json response via ajax
    $(document).on('click', '[data-ajax-action]', function () {
        let $elm = $(this);
        let type = 'post';
        if ($elm.data('type'))
            tyep = $elm.data('type');
        let $content = $elm.html();
        $elm.html($circularLoader);
        $.ajax({
            type: type,
            url: $elm.data('ajax-action'),
            contentType: 'json',
            success: function (rep) {
                $elm.html($content);
                if (rep.IsSuccessful) showNotif(notifyType.success, strings.success);
                else showNotif(notifyType.danger, rep.Message);
            },
            error: function () {
                $elm.html($content);
                showNotif(notifyType.danger, strings.error);
            }
        });
    });

    //load appropriate partial to load in target element
    $(document).on('click', '[data-ajax-load]', function () {
        let $target = $('.content-wrapper');
        if ($(this).data('target')) $target = $($(this).data('target'));
        $target.loadOverStart();
        $.get($(this).data('ajax-load'))
            .done(function (rep) {
                $target.html(rep).loadOverStop();
            })
            .fail(function (e) {
                $target.loadOverStop();
            });
    });

    //change default validation messages 
    jQuery.extend(jQuery.validator.messages, {
        required: validationMessages.required,
        remote: validationMessages.remote,
        email: validationMessages.email,
        url: validationMessages.url,
        date: validationMessages.date,
        dateISO: validationMessages.dateISO,
        number: validationMessages.number,
        digits: validationMessages.digits,
        creditcard: validationMessages.creditcard,
        equalTo: validationMessages.equalTo,
        accept: validationMessages.accept,
        maxlength: jQuery.validator.format(validationMessages.maxlength),
        minlength: jQuery.validator.format(validationMessages.minlength),
        rangelength: jQuery.validator.format(validationMessages.rangelength),
        range: jQuery.validator.format(validationMessages.range),
        max: jQuery.validator.format(validationMessages.max),
        min: jQuery.validator.format(validationMessages.min)
    });

    //
    $(document).on('click', '.inner-notification', function () {
        $(this).slideUp(300).find('.text').text('');
    });

    //Copy 'data-copy-value' attribute value to clipboard
    $(document).on('click', '.copy-value', function (e) {
        e.preventDefault();

        let $this = $(this);
        let value = $this.data('copy-value');
        let elm = document.createElement('textarea');

        elm.value = value;
        document.body.appendChild(elm);
        elm.select();
        document.execCommand('copy');
        document.body.removeChild(elm);
        $this.addClass('animated fadeIn').bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
            $this.removeClass('animated fadeIn');
        });

        //$this.hide('fast')
        //    .removeClass('zmdi-copy copy-value')
        //    .addClass('zmdi-check-all copied-value')
        //    .show('fast')
        //    .delay(3000)
        //    .hide('fast', function () {
        //        $this.removeClass('zmdi-check-all copied-value')
        //            .addClass('zmdi-copy copy-value')
        //            .show('fast');
        //    });

    });

    //auto submit filter on pressing enter
    $(document).on('keypress', '.filters input[type="text"]', function (e) {
        if (e.keyCode === 13) $(this).closest('form').find('button.search').trigger('click');
    });

    $(document).on('input', '.rial', function () {
        let v = $(this).val();
        v = v.substr(0, v.length - 1);
        let $target = $(this).data('target');
        if ($target) $($target).text(commaThousondSeperator(v) + 'ريال');
        else {
            let $lbl = $(this).prev();
            let txt = $lbl.clone().children().remove().end().text();
            $lbl.html(txt + ' <small>(' + commaThousondSeperator(v) + ' تومان' + ')</small>');
        }
    });

});

//--- loading function
var ajaxBtn = new (function () {
    var ins = function () { };
    var _$btn, _$icon, _$btnHtml, btnH;
    ins.prototype.inProgress = function ($btn, $loader) {
        _$btn = $btn;
        $loader = typeof $loader !== 'undefined' ? $loader : $circularLoader;
        _$btn.prop('disabled', true);
        if (_$btn.find('.icon').length > 0) {
            _$icon = $btn.find('.icon');
            _$btnHtml = _$icon.html();
            _$icon.html($loader);
        }
        else {
            btnH = _$btn.outerHeight();
            _$btnHtml = _$btn.html();
            _$btn.css({ "height": btnH + "px" }).html($loader);
        }

    };
    ins.prototype.normal = function () {
        _$btn.prop('disabled', false);
        if (_$btn.find('.icon').length > 0) {
            _$icon.html(_$btnHtml);
        }
        else {
            _$btn.html(_$btnHtml);
        }
    };
    return ins;
}());
/*--------------------------------------
            submit ajax form
---------------------------------------*/
var submitAjaxForm = function ($btn, successFunc, errorFunc, useToastr) {
    useToastr = typeof useToastr !== 'undefined' ? useToastr : true;
    if (!useToastr)
        $('.inner-notification').hide();
    let $frm = $btn.closest('form');
    if (!$frm.valid()) return;
    ajaxBtn.inProgress($btn);
    let model = customSerialize($frm, true);
    console.log(model);
    $.post($frm.attr('action'), model)
        .done(function (rep) {
            if (rep.IsSuccessful) {
                if (successFunc && typeof successFunc === 'function') successFunc(rep);
            }
            else {
                if (typeof errorFunc === 'function') errorFunc(rep);
                else {
                    if (useToastr) showNotif(notifyType.danger, rep.Message);
                    else $frm.inlineNotify(notifyType.danger, rep.Message);
                }
            }
            ajaxBtn.normal();
        })
        .fail(function (e) {
            ajaxBtn.normal();
            if (useToastr) showNotif(notifyType.danger, strings.error);
            else $frm.inlineNotify(notifyType.danger, strings.error);

        });
};
/*--------------------------------------
            for simple ajax call
---------------------------------------*/
var ajaxCall = function ($elm, data, successFunc, errorFunc, loader, method) {
    loader = typeof loader !== 'undefined' ? loader : $threeDotLoader;
    method = typeof method !== 'undefined' ? method.toLowerCase() : 'post';
    data = typeof data !== 'undefined' ? data : {};
    let isBtnAction = $elm.hasClass('btn-action');
    if (isBtnAction)
        ajaxBtn.inProgress($elm);
    else {
        var elmHtml = $elm.html();
        $elm.html(loader);
    }
    $elm.prop('disabled', true);
    if (method === 'post') {
        $.post($elm.data('url'), data)
            .done(function (rep) {
                if (isBtnAction) ajaxBtn.normal();
                else $elm.html(elmHtml);

                $elm.prop('disabled', false);

                if (rep.IsSuccessful) {
                    if (typeof successFunc === 'function') successFunc(rep);
                }
                else {
                    if (typeof errorFunc === 'function') errorFunc(rep.Message);
                    else showNotif(notifyType.danger, strings.error + ' (' + e.status + ')');
                }
            })
            .fail(function (e) {
                console.log(e);
                if (isBtnAction) ajaxBtn.normal();
                else $elm.html(elmHtml);

                $elm.prop('disabled', false);

                if (typeof errorFunc === 'function') errorFunc(strings.error);
                else showNotif(notifyType.danger, strings.error + '(' + e.status + ')');
            });
    }
    else {
        $.get($elm.data('url'), data)
            .done(function (rep) {
                if (isBtnAction) ajaxBtn.normal();
                else $elm.html(elmHtml);

                $elm.prop('disabled', false);
                if (rep.IsSuccessful) {
                    if (typeof successFunc === 'function') successFunc(rep);
                }
                else {
                    if (typeof errorFunc === 'function') errorFunc(rep.Message);
                }
            })
            .fail(function (e) {
                if (isBtnAction) ajaxBtn.normal();
                else $elm.html(elmHtml);
                $elm.prop('disabled', false);
                if (typeof errorFunc === 'function') errorFunc(strings.error);
                else showNotif(notifyType.danger, strings.error + '(' + e.status + ')');
            });
    }
};
/*-----------------------------------------------------
            customized swal confirmed modal
-------------------------------------------------------*/
var swalConfirm = function (confirmedFunc, denyFunc) {
    swal({
        title: '',
        text: strings.AreYouSure,
        confirmButtonColor: "#4285F4",
        showCancelButton: true,
        confirmButtonText: strings.yes,
        cancelButtonText: strings.no
    },
        function (isConfirm) {
            if (isConfirm) {
                confirmedFunc();
            }
            else if (typeof denyFunc !== 'undefined') {
                denyFunc();
            }
        });
};
/*--------------------------------------
           ajax global error log
---------------------------------------*/
$(document).ajaxError(function (event, jqxhr, settings, thrownError) {
    console.log(jqxhr.responseText);
    try {
        if (jqxhr.status === 403 || jqxhr.status === 408)
            window.location.href = strings.signInUrl;
        else if (jqxhr.status === 401)
            showNotif(notifyType.danger, strings.unAuthorizedErrorMessage);

    }
    catch (e) { console.log(e); }
});
/*--------------------------------------
           custom serialize
---------------------------------------*/
var customSerialize = function ($wrapper, checkNumbers) {
    let model = {};
    let checkNumberValue = function (v) {
        if (checkNumbers && !isNaN(v) && v !== '') return parseInt(v);
        else return v;
    };
    function valueSetter(obj, name, v) {
        let arr = name.split('.');
        if (arr.length > 1) {
            if (typeof obj[arr[0]] === 'undefined')
                obj[arr[0]] = {};
            valueSetter(obj[arr[0]], arr.splice(1, arr.length - 1).join('.'), v);
            return;
        }
        if (typeof obj[name] !== 'undefined') {
            if (Array.isArray(obj[name])) obj[name].push(v);
            else obj[name] = [obj[name], v];
        }
        else obj[name] = v;
    };
    $wrapper.find('input:not([type="checkbox"]):not([type="radio"]),select,textarea').each(function () {
        let name = $(this).attr('name');
        if (typeof name !== 'undefined') {
            let v = checkNumberValue($(this).val());
            valueSetter(model, name, v);
        }

    });

    $wrapper.find('input[type="checkbox"]:checked,input[type="radio"]:checked').each(function () {
        let name = $(this).attr('name');
        if (typeof name !== 'undefined') {
            let val = $(this).attr('value').toLowerCase();
            if (!val || val === 'true' || val === 'false') val = $(this).prop('checked');
            valueSetter(model, name, val);
        }
    });
    console.log(model);
    return model;
};

var postObjectList = function (url, model, success, error) {
    console.log('model5:');
    console.log(JSON.stringify(model));
    $.ajax({
        type: 'POST',
        url: url,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(model),
        success: function (rep) { if (success) success(rep); },
        error: function (e) { if (error) error(e); }
    });
};

const fileTypes = {
    Unknown: { id: 0, type: 'application/octet-stream' },
    Image: { id: 1, type: 'image/png' },
    Document: { id: 2, type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' },
    Archive: { id: 3, type: 'application/zip' },
    Audio: { id: 4, type: 'audio/mpeg' },
    Video: { id: 5, type: 'video/mp4' }
};

var getFileType = function (fileName) {
    let ext = fileName.toLowerCase().split('.').reverse()[0];
    switch (ext) {
        case "png":
        case "jpg":
        case "jpeg":
        case "gif":
        case "tiff":
            return fileTypes.Image;
        case "mp3":
        case "wav":
        case "flm":
        case "fsm":
        case "ogg":
        case "m4a":
        case "m4b":
        case "m4p":
        case "m4r":
            return fileTypes.Audio;
        case "mp4":
        case "mkv":
        case "avi":
        case "ts":
        case "m4v":
        case "flv":
            return fileTypes.Video;
        case "zip":
        case "rar":
        case "iso":
        case "tar":
        case "jar":
            return fileTypes.Archive;
        case "pdf":
        case "doc":
        case "docx":
        case "txt":
        case "xls":
        case "xlsx":
        case "josn":
        case "pptx":
            return fileTypes.Document;
        default:
            return fileTypes.Unknown;
    }
};

var getDefaultImageUrl = function (fileName) {
    let ext = fileName.toLowerCase().split('.').reverse()[0];
    switch (getFileType(ext).id) {
        case fileTypes.Image:
            return null;
        case fileTypes.Audio.id:
            return urlPrefix + '/Images/FileTypes/audio.png';
        case fileTypes.Video.id:
            return urlPrefix + '/Images/FileTypes/video.png';
        case fileTypes.Archive.id:
            return urlPrefix + '/Images/FileTypes/archive.png';
        case fileTypes.Document.id:
            return urlPrefix + '/Images/FileTypes/document.png';
        default:
            return urlPrefix + '/Images/FileTypes/unknown.png';
    }
};

var convertToOptionTags = function (items, isNullable) {
    let $optTags = '';
    if (isNullable) $optTags = '<option value="">' + strings.pleaseSelect + '</option>';
    $optTags = items.reduce(function (total, x) {
        return total + ('<option value="' + x.Value + '">' + x.Text + '</option>');
    }, $optTags);
    return $optTags;
};

var objectToFormData = function (obj, form, namespace) {

    var fd = form || new FormData();
    var formKey;

    for (var property in obj) {
        if (obj.hasOwnProperty(property)) {

            if (namespace) {
                formKey = namespace + '[' + property + ']';
            } else {
                formKey = property;
            }
            if (Array.isArray(obj[property])) {
                for (var i = 0; i < obj[property].length; i++) {
                    if (typeof obj[property][i] === 'object' && !(obj[property][i] instanceof File))
                        objectToFormData(obj[property][i], fd, property + '[' + i + ']');
                    else fd.append(formKey, obj[property][i]);
                }
            }
            else if (typeof obj[property] === 'object' && !(obj[property] instanceof File)) {

                objectToFormData(obj[property], fd, property);

            }

            else {

                // if it's a string or a File object
                fd.append(formKey, obj[property]);
            }

        }
    }

    return fd;

};
