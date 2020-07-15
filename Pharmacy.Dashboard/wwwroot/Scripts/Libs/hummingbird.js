/*! 
 * Copyright (c) 2018 SHB
 * 
 * Multiple Ajax Auto Complete
 * 
 * Author: kingofday.b@gmail.com
 * 
 * Version: 2.0.0
 *
 */
(function ($) {
    $.fn.hummingbird = function (args) {
        //--default
        args = typeof args !== 'undefined' ? args : {};
        args.url = typeof args.url !== 'undefined' ? args.url : this.data('url');
        args.class = typeof args.class !== 'undefined' ? args.class : 'form-control';
        args.placeholder = typeof args.placeholder !== 'undefined' ? args.placeholder : '';
        args.removeClass = typeof args.removeClass !== 'undefined' ? args.removeClass : 'zmdi zmdi-close';
        args.wordMinLength = typeof args.wordMinLength !== 'undefined' ? args.wordMinLength : 1;
        //--
        var $targetInput = this;
        var items = $targetInput.val() === '' ? [] : JSON.parse($targetInput.val());
        var $parent = $targetInput.parent();
        var $wrapper = $parent.find('#' + $targetInput.attr('id') + '-wrapper');
        if ($wrapper.length === 0) {
            $wrapper = $('<div></div>', { id: $targetInput.attr('id') + '-wrapper', class: 'hummingbird-wrapper' });
            var $input = $('<input />',
                {
                    type: 'text',
                    id: $targetInput.attr('id') + '-input',
                    placeholder: this.attr('placeholder'),
                    class: args.class + ' hummingbird-input',
                    autocomplete: "off",
                    autocorrect: "off"
                });
            $parent.append($wrapper.append($input));
            var $result = $('<div></div>', { id: $targetInput.attr('id') + '-result', class: 'hummingbird-result' });
            $wrapper.append($result);
        }
        var opt = '<div class="hummingbird-opt"><a href="#">{0}</a></div>';
        //
        var setRemoveEvent = function (item) {
            $wrapper.find('.remove').off('click').on('click', function () {
                items.splice(items.findIndex(x => x.Value === item.Value), 1);
                $targetInput.val(JSON.stringify(items));
                $(this).closest('.hummingbird-tag').remove();
            });
        };



        //add tag and update target input value
        var tag = '<div class="hummingbird-tag"><span>{0}</span><i class="remove ' + args.removeClass + '"><i></div>';
        var select = function (item, calback) {
            if (!items.find(x => x.Value === item.Value)) {
                items.push(item);
                $(tag.replace('{0}', item.Text)).data('item', item).insertBefore($wrapper.find('.hummingbird-input'));
            }
            $targetInput.val(JSON.stringify(items));
            $input.val('');
            if (typeof calback === 'function') calback(item);
            setRemoveEvent(item);
        };
        //init
        
        items.forEach((item) => {
            $(tag.replace('{0}', item.Text)).data('item', item).insertBefore($wrapper.find('.hummingbird-input'));
            setRemoveEvent(item);
        });
       
        //search after type
        $input.off('input').on('input', function () {
            var txt = $input.val();
            $result.empty();
            if (txt.length > parseInt(args.wordMinLength)) {
                if ($wrapper.find('.three-dot-loader').length === 0) $wrapper.append($threeDotLoader);
                $.get(args.url, { q: $input.val() })
                    .done(function (searchRep) {
                        $wrapper.find('.three-dot-loader').remove();
                        if (typeof args.ajaxSuccess !== 'undefined') args.ajaxSuccess($result);
                        else {
                            var val = $targetInput.val();
                            var items = [];
                            if (val) items = JSON.parse(val);
                            if (searchRep.length === 0) return;
                            for (var i = 0; i < searchRep.length; i++) {
                                if (!items.find(x => x.Value === searchRep[i].Value)) {
                                    $result.append($(opt.replace("{0}", searchRep[i].Text)).data('item', searchRep[i]));
                                }
                            }
                            $result.show();
                        }
                        //--- events
                        var idx = -1;
                        $input.off('keydown').on('keydown', function (e) {
                            var $opts = $result.find('.hummingbird-opt');
                            if ($opts.length !== 0) {
                                //if (idx === -1) idx = 0;
                                if (e.keyCode === 13) {//enter
                                    e.preventDefault();
                                    if (idx > -1)
                                        select($opts.eq(idx).data('item'), function () {
                                            $opts.eq(idx).parent().empty().hide();
                                        });
                                }
                                else if (e.keyCode === 38) {//up
                                    if (idx > 0) idx--;
                                    else idx = 0;
                                    $opts.removeClass('active').eq(idx).addClass('active');
                                }
                                else if (e.keyCode === 40) {//down
                                    if (idx < $opts.length - 1) idx++;
                                    else idx = 0;
                                    $opts.removeClass('active').eq(idx).addClass('active');
                                }
                            }
                            //remove with back space
                            if (e.keyCode === 8 && $input.val() === '') {
                                var $tag = $wrapper.find('.hummingbird-tag').last();
                                var item = $tag.data('item');
                                var items = JSON.parse($targetInput.val());
                                items.splice(items.findIndex(x => x.Value === item.Value), 1);
                                $targetInput.val(JSON.stringify(items));
                                $tag.remove();
                            }
                        });
                        $result.find('.hummingbird-opt').off('click').on('click', function () {
                            let $opt = $(this);
                            select($opt.data('item'), function () {
                                $opt.parent().empty().hide();
                            });
                            
                        });
                        //---
                    })
                    .fail(function () {
                        $wrapper.find('.three-dot-loader').remove();
                        if (typeof args.ajaxError !== 'undefined') args.ajaxError();
                        else {
                            showNotif(notifyType.danger, strings.error);
                        }
                    });
            }
        });
        //
        //focus on input for typing
        $wrapper.on('click', function () {
            $(this).find('.hummingbird-input').focus();
        });
        return this;
    };
}(jQuery));
$(document).ready(function () {
    $(document).on('click', function () {
        if ($(this).closest('.hummingbird-result').length === 0) $('.hummingbird-result').empty().hide();
    });
});