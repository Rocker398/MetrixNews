// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var MetrixNews = {
    InitializeActions: function() {
        var mainContent = $('.mainContent');

        mainContent.off('click', '.gridTile').on('click', '.gridTile', function () {
            $.ajax({
                url: "/Home/Articles",
                type: 'GET',
                data: {
                    category: 1
                },
                success: function (result) {
                    $('.mainContent').html(result);
                    MetrixNews.InitializeArticleActions();
                    MetrixNews.CheckIsFirstVisit();
                },
                error: function (xhr) {
                    PopupMsg.ShowError("Something went wrong, please refresh and try again.");
                }
            });
        });

        mainContent.off('click', '.signUpButton').on('click', '.signUpButton', function () {
            var email = $('.signUpEmailInput').val();
            var password = $('.signUpPasswordInput').val();

            if (email.length == 0 || password.length == 0) {
                PopupMsg.ShowError("Please enter an email and password");
                return;
            }
            
            $.ajax({
                url: "/SignUp/Subscribe",
                type: 'POST',
                dataType: 'json',
                data: {
                    email: email,
                    password: password
                },
                success: function (result) {
                    if (result.success) {
                        PopupMsg.ShowMsg("Your information has been submitted, thank you for showing us your interest!");
                    }
                    else {
                        PopupMsg.ShowError("Something went wrong, please refresh and try again.");
                    }
                },
                error: function (xhr) {
                    PopupMsg.ShowError("Something went wrong, please refresh and try again.");
                }
            });
        });
    },
    InitializeArticleActions: function () {
        MetrixNews.InitializeArticleSliders();
        MetrixNews.InitializeStatusSliders();

        var mainContent = $('.mainContent');

        mainContent.off('click', '.categoryTitle').on('click', '.categoryTitle', function () {
            var topic = $(this).closest('.articleCategory').data('category');
            $.ajax({
                url: "/Home/Topics",
                type: 'GET',
                data: {
                    topic: topic
                },
                success: function (result) {
                    $('.mainContent').html(result);
                    MetrixNews.InitializeTopicActions();
                },
                error: function (xhr) {
                    PopupMsg.ShowError("Something went wrong, please refresh and try again.");
                }
            });
        });
    },
    InitializeTopicActions: function () {
        MetrixNews.InitializeArticleSliders();
        MetrixNews.InitializeStatusSliders();
    },
    InitializeArticleSliders: function () {
        var mainContent = $('.mainContent');

        mainContent.find('.slider').slick({
            infinite: false,
            /*slidesToShow: 5,
            slidesToScroll: 5*/
            centerMode: true,
            centerPadding: '40%',
            slidesToShow: 1
        });

        mainContent.find('.slider').each(function () {
            var numItemsInSlider = $(this).data('num-items');
            var middle = Math.round(numItemsInSlider / 2);

            if ($(this).hasClass('slick-initialized')) {
                $(this).slick('slickGoTo', middle, false);

                //$(this).find('.slick-slide[data-slick-index="' + middle + '"] .articleCard').mouseover();
            }
        });

        mainContent.find('.sliderV2').slick({
            slidesToShow: 5,
            slidesToScroll: 5,
            infinite: false,
            responsive: [
                {
                    breakpoint: 1400,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 4
                    }
                },
                {
                    breakpoint: 1180,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3
                    }
                },
                {
                    breakpoint: 900,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2
                    }
                },
                {
                    breakpoint: 670,
                    settings: {
                        centerMode: true,
                        slidesToShow: 1,
                        centerPadding: '108px'
                    }
                }
            ]
        });

        mainContent.find('.sliderV2').each(function () {
            var numItemsInSlider = $(this).data('num-items');
            var middle = Math.round(numItemsInSlider / 2);

            if ($(this).hasClass('slick-initialized')) {
                $(this).slick('slickGoTo', middle, false);
            }
        });
        
        mainContent.off('mouseover', '.articleCategory .articleCard').on('mouseover', '.articleCategory .articleCard', function () {
            var card = $(this);
            var biasness = card.data('spectrum-bias');
            var articleCategory = card.closest('.articleCategory');
            var gradientSpectrum = articleCategory.children('.gradientSpectrum');
            var handle = gradientSpectrum.find('.handleLabel');
            var change = true;
            var sliderVal = 0;

            if (biasness != null && biasness.length > 0) {
                switch (biasness.toLowerCase()) {
                    case "hyper-partisan liberal":
                        {
                            if (handle.hasClass('opt0')) {
                                change = false;
                            }

                            sliderVal = 0;
                            break;
                        }

                    case "skews liberal":
                        {
                            if (handle.hasClass('opt1')) {
                                change = false;
                            }

                            sliderVal = 13;
                            break;
                        }

                    case "skews conservative":
                        {
                            if (handle.hasClass('opt3')) {
                                change = false;
                            }

                            sliderVal = 35;
                            break;
                        }

                    case "hyper-partisan conservative":
                        {
                            if (handle.hasClass('opt4')) {
                                change = false;
                            }

                            sliderVal = 50
                            break;
                        }

                    case "balanced":
                    default:
                        {
                            if (handle.hasClass('opt2')) {
                                change = false;
                            }

                            sliderVal = 25;
                            break;
                        }
                }

                if (change) {
                    gradientSpectrum.slider("value", sliderVal);
                }
            }
        });

        MetrixNews.ResizeSliders();
    },
    InitializeStatusSliders: function () {
        var mainContent = $('.mainContent');
        var gradientSpectrum = mainContent.find('.gradientSpectrum');

        gradientSpectrum.slider({
            value: 25,
            max: 50,
            step: 0.1,
            animate: "slow",
            slide: function (event, ui) {
                MetrixNews.SetSliderLabel($(this), event, ui, true);
            },
            change: function (event, ui) {
                MetrixNews.SetSliderLabel($(this), event, ui, false);
            }
        });

        //mainContent.find('.gradientSpectrum.ui-slider, .gradientSpectrum ui-slider-handler').off();
        var label = $('<div class="handleLabel opt2" data-prev-opt="opt2"><span>Balanced</span></div>');
        gradientSpectrum.find('.ui-slider-handle').append(label);
    },
    SetSliderLabel: function (element, event, ui, goToSlide) {
        var sliderHandle = $(ui.handle).find('.handleLabel');
        var newValue = ui.value;
        var newClass = "";
        var newText = "";

        if (newValue >= 0 && newValue < 10) {
            newClass = "opt0";
            newText = "Hyper-Partisan Liberal";
        }
        else if (newValue >= 10 && newValue < 20) {
            newClass = "opt1";
            newText = "Skews Liberal";
        }
        else if (newValue >= 30 && newValue < 40) {
            newClass = "opt3";
            newText = "Skews Conservative";
        }
        else if (newValue >= 40 && newValue <= 50) {
            newClass = "opt4";
            newText = "Hyper-Partisan Conservative";
        }
        else {
            newClass = "opt2";
            newText = "Balanced";
        }

        // If we haven't changed categories, don't waste time going to the same slide
        var prevOpt = sliderHandle.data('prev-opt');
        if (newClass == prevOpt) {
            return;
        }

        sliderHandle.data('prev-opt', newClass);

        sliderHandle.removeClass().addClass('handleLabel').addClass(newClass);
        sliderHandle.children('span').text(newText);

        if (goToSlide) {
            var slider = element.closest('.articleCategory').find('.slider');
            var matchingArticle = slider.find('.articleCard[data-spectrum-bias="' + newText.toLowerCase() + '"]').first();

            if (matchingArticle != null && matchingArticle.length > 0) {
                var slideNum = matchingArticle.closest('.slick-slide').data('slick-index');
                slider.slick('slickGoTo', slideNum);
            }
            /*else {
                var slideNum = slider.find('.slick-slide:nth-last-of-type(2)').data('slick-index');
                slider.slick('slickGoTo', slideNum);
            }*/
        }
    },
    CheckIsFirstVisit: function () {
        var popup = $('.modalPopupContainer');

        // Check if cookie was already set, i.e. they have already visited the site
        var existingCookie = $.cookie('metrix.news');

        // If first time visitor, show the popup and set the cookie
        if (existingCookie == null || existingCookie.length <= 0) {
            popup.show();
            $('.closeModal').off('click').on('click', function () {
                popup.hide();
            });

            // Set the cookie for the next visit
            $.cookie('metrix.news', '1', { expires: 30 });
        }
    },
    ResizeSliders: function () {
        var mainContent = $('.mainContent');
        var screenWidth = mainContent.width();

        var cardSize = 240;
        var cardMargin = 7 * 2;
        var categoryMargin = 50 * 2;

        var newPadding = (screenWidth - (cardSize + cardMargin + categoryMargin)) / 2;

        if (newPadding < 0) {
            newPadding = 0;
        }

        var newPaddingStr = newPadding + 'px';

        mainContent.find('.slider').slick('slickSetOption', 'centerPadding', newPaddingStr, true);
    }
};

var PopupMsg = {
    ShowMsg: function (message) {
        var popup = $('.messagePopup');
        popup.html(message);
        popup.addClass('success');

        PopupMsg.ShowPopup(popup);
    },
    ShowError: function (message) {
        var popup = $('.messagePopup');
        popup.html(message);
        popup.addClass('error');

        PopupMsg.ShowPopup(popup);
    },
    ShowPopup: function (popup) {
        popup.slideDown('slow', function () {
            setTimeout(function () {
                popup.slideUp('slow', function () {
                    popup.removeClass('success').removeClass('error');
                });
            }, 5000)
        });
    }
};

$(document).ready(function () {
    MetrixNews.InitializeActions();
});

$(window).resize(function () {
    MetrixNews.ResizeSliders();
});