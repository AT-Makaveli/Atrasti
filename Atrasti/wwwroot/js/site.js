// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    const btn = $('#back-to-top');

    $(window).scroll(function() {
        if ($(window).scrollTop() > 300) {
            btn.addClass('show');
        } else {
            btn.removeClass('show');
        }
    });

    btn.on('click', function(e) {
        e.preventDefault();
        $('html, body').animate({scrollTop:0}, '300');
    });
    
    const popularTags = $('#popular-tags');
    if (popularTags !== undefined) {
        popularTags.css('opacity', '0');
        popularTags.animate({
            opacity: '1', // animate slideUp
        }, 'slow', 'linear');
    }

    $('.navbar-toggler').click(function () {
        const navbarAriaExpanded = $('.navbar-toggler').attr('aria-expanded');


        const navigator = $('#navigator');
        const hasTransparent = navigator.hasClass('no-transition');
        if (hasTransparent === false)
            if (navbarAriaExpanded === 'true') {
                navigator.removeClass('bg-dark').addClass('bg-transparent');
            } else {
                navigator.removeClass('bg-transparent').addClass('bg-dark');
            }
    });
});