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
                    MetrixNews.InitializeArticleSliders();
                    MetrixNews.InitializeStatusSliders();
                    MetrixNews.CheckIsFirstVisit();
                },
                error: function (xhr) {
                    alert('Error: ' + xhr.statusText);
                }
            });
        });
    },
    InitializeArticleSliders: function () {
        var mainContent = $('.mainContent');

        mainContent.find('.slider').slick({
            /*infinite: true,
            slidesToShow: 5,
            slidesToScroll: 5*/
            centerMode: true,
            centerPadding: '40%',
            slidesToShow: 1
        });

        mainContent.find('.sliderV2').slick({
            slidesToShow: 5,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1400,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 1180,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 900,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1
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

        MetrixNews.ResizeSliders();
    },
    InitializeStatusSliders: function () {
        var mainContent = $('.mainContent');
        var gradientSpectrum = mainContent.find('.gradientSpectrum');

        gradientSpectrum.slider({
            value: 50,
            max: 100
        });

        mainContent.find('.gradientSpectrum.ui-slider, .gradientSpectrum ui-slider-handler').off();
        var label = $('<div class="handleLabel"><span>Slightly Conservative</span></div>');
        gradientSpectrum.find('.ui-slider-handle').append(label);
    },
    CheckIsFirstVisit: function () {
        var popup = $('.modalPopupContainer');
        //popup.show();
        $('.closeModal').off('click').on('click', function () {
            popup.hide();
        });
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

$(document).ready(function () {
    MetrixNews.InitializeActions();
});

$(window).resize(function () {
    MetrixNews.ResizeSliders();
});