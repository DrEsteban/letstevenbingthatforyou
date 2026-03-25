$(function ($) {
    $.fn.type = function (value, complete, index) {
        return this.each(function () {
            if (index === undefined) {
                index = 0;
            }
            var self = this;
            var val = value.substr(0, index + 1);
            $(self).val(val);
            if (index < value.length - 1) {
                setTimeout(function () { $(self).type(value, complete, index + 1); }, Math.random() * 240);
            }
            else {
                if (complete) {
                    setTimeout(function () { complete(); }, 240);
                }
            }
        });
    };

    $.fn.doStep = function (type, action) {
        return this.each(function () {
            $(this).show(type, function () {
                action();
            });
        });
    };
});

$(function () {
    function getRenderedElementCenter(element) {
        var rect = element.get(0).getBoundingClientRect();

        return {
            top: window.scrollY + rect.top + (rect.height / 2),
            left: window.scrollX + rect.left + (rect.width / 2)
        };
    }

    function clickSearch(mouse) {
        $(".step4").doStep("drop", function () {
            var searchButton = $("#sb_form_go");
            var searchButtonCenter = getRenderedElementCenter(searchButton);
            mouse.animate({
                top: searchButtonCenter.top + "px",
                left: searchButtonCenter.left + "px"
            }, 2000, 'swing', function () {
                searchButton.click();
            });
        });
    }

    var searchQuery = $("#sb_form_q");
    var fakeMouse = $("#fake_mouse");
    $("#lmbtfyResult").show("bounce", "fast", function () {
        $(".step1").doStep("drop", function () {
            $(".step2").doStep("drop", function () {
                fakeMouse.show("bounce", "fast");
                var rect = searchQuery.get(0).getBoundingClientRect();
                var targetTop = window.scrollY + rect.top + (rect.height / 2);
                var targetLeft = window.scrollX + rect.left + (rect.width / 10);

                fakeMouse.animate({
                    top: targetTop + "px",
                    left: targetLeft + "px"
                }, 750, 'swing', function () {
                    searchQuery.focus();
                    fakeMouse.animate({ top: "+=18px", left: "+=10px" }, "fast");

                    $(".step3").doStep("drop", function () {
                        searchQuery.type(query, function () { clickSearch(fakeMouse); });
                    });
                });
            });
        });
    });
});
