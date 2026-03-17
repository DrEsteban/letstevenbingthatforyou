(function ($) {
    $.fn.type = function (value, complete, index) {
        return this.each(function () {
            const element = $(this);
            const currentIndex = index === undefined ? 0 : index;
            const partialValue = value.substr(0, currentIndex + 1);

            element.val(partialValue);
            if (currentIndex < value.length) {
                setTimeout(function () {
                    element.type(value, complete, currentIndex + 1);
                }, Math.random() * 240);
                return;
            }

            if (complete) {
                setTimeout(function () {
                    complete();
                }, 240);
            }
        });
    };

    $.fn.doStep = function (type, action) {
        return this.each(function () {
            const element = $(this);
            element.show(type, function () {
                action();
            });
        });
    };

    function clickSearch(mouse) {
        $(".step4").doStep("drop", function () {
            const searchButton = $("#sb_form_go");
            mouse.animate(
                {
                    top: (searchButton.offset().top + 25) + "px",
                    left: (searchButton.offset().left + 25) + "px"
                },
                2000,
                "swing",
                function () {
                    searchButton.click();
                });
        });
    }

    window.startLmbtfy = function (query) {
        const searchQuery = $("#sb_form_q");
        const fakeMouse = $("#fake_mouse");

        searchQuery.val("");
        fakeMouse.stop(true, true).hide();
        $(".step1, .step2, .step3, .step4").hide();

        $("#lmbtfyResult").stop(true, true).show("bounce", "fast", function () {
            $(".step1").doStep("drop", function () {
                $(".step2").doStep("drop", function () {
                    fakeMouse.show("bounce", "fast");
                    fakeMouse.animate(
                        {
                            top: (searchQuery.offset().top + 15) + "px",
                            left: (searchQuery.offset().left + 10) + "px"
                        },
                        750,
                        "swing",
                        function () {
                            searchQuery.focus();
                            fakeMouse.animate({ top: "+=18px", left: "+=10px" }, "fast");

                            $(".step3").doStep("drop", function () {
                                searchQuery.type(query, function () {
                                    clickSearch(fakeMouse);
                                });
                            });
                        });
                });
            });
        });
    };
})(jQuery);
