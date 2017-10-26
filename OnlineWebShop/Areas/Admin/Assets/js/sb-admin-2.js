$('.link_modal_login').click(function () {
    $(this).toggleClass("openClick")
    if ($(this).hasClass("openClick")) {
        $(".modal_outside_login").fadeIn();
        $(".border_modal_login").animate({ "opacity": "0", "width": "0" }, 100).animate({ "opacity": "1", "width": "60%" }, 500);
        $(".modal_contain_login").animate({ "opacity": "0", "height": "0" }, 100).animate({ "opacity": "1", "height": "100%" }, 500);
        $('.modal_outside_signup').fadeOut();
    }
    else {
        setTimeout(function () {
            $(".modal_outside_login").fadeOut();
        }, 300);
        $(".border_modal_login").animate({ "opacity": "1", "width": "60%" }, 100).animate({ "opacity": "0", "width": "0" }, 500);
        $(".modal_contain_login").animate({ "opacity": "1", "height": "100%" }, 100).animate({ "opacity": "0", "height": "0" }, 500);
        $('.link_modal_login').removeClass("openClick");
    }
});
$(function () {
    $('#side-menu').metisMenu();
});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function() {
    $(window).bind("load resize", function() {
        var topOffset = 50;
        var width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
        }

        var height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            $("#page-wrapper").css("min-height", (height) + "px");
        }
    });

    var url = window.location;
    // var element = $('ul.nav a').filter(function() {
    //     return this.href == url;
    // }).addClass('active').parent().parent().addClass('in').parent();
    var element = $('ul.nav a').filter(function() {
        return this.href == url;
    }).addClass('active').parent();

    while (true) {
        if (element.is('li')) {
            element = element.parent().addClass('in').parent();
        } else {
            break;
        }
    }
});
