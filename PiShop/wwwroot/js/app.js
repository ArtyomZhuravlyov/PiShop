$(function () {
    let header = $("#header");
    let intro = $("#intro");
    let introH = intro.innerHeight();
    let scrollPos = $(window).scrollTop();
    let nav = $("#nav");
    let navToggle = $("#navToggle");
    
    checkScroll(scrollPos, introH);
    $(window).on("scroll resize", function () {
        introH = intro.innerHeight();
        scrollPos = $(this).scrollTop();
        checkScroll(scrollPos, introH);
    });

    function checkScroll(scrollPos, introH) {
        if (scrollPos > introH) {
            header.addClass("fixed");
            if (innerWidth < 420) {
                $(".contactsNew").addClass("displayNone");
            }
            
        }
        else {
            header.removeClass("fixed");
            if (innerWidth < 420) {
                $(".contactsNew").removeClass("displayNone");
            }
           
        }
    }
    $("[data-scroll]").on("click", function (event) {
        event.preventDefault();
        let elementid = $(this).data('scroll');
        let elementOffset = $(elementid).offset().top;
        nav.removeClass("show");
        $("html, body").animate({
            scrollTop: elementOffset - 59
        }, 700);
    });
    
    navToggle.on("click", function (event) {
        event.preventDefault();
        nav.toggleClass("show");
    });
    
  

 


  
    
});

