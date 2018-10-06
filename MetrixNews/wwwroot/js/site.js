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
            infinite: true,
            slidesToShow: 5,
            slidesToScroll: 5
        });
    },
    InitializeStatusSliders: function () {

    }
};

$(document).ready(function () {
    MetrixNews.InitializeActions();
});