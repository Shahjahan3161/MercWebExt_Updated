$(document).ready(function () {

	/* ------ Owl Gallery Carousel script ------ */

    $("#freshproduce-roll").owlCarousel({
		loop:true,
		margin:0,
		autoplay:true,
		autoplayTimeout:3000,
		autoplayHoverPause:false,
		nav:false,
		dots:false,
		responsive:{
			0:{
				items:1
			},
			720:{
				items:2
			},
			1140:{
				items:4
			}
		}
	});


	/* ------ Owl Testimonials Carousel script ------ */

    $('#distribution-roll').owlCarousel({
		lazyLoad:true,
		loop:true,
		margin:30,
		dots:true,
		responsive:{
			0:{
				items:1
			},
			720:{
				items:2
			},
			1140:{
				items:3
			}
		}
	});


	/* ------ Smooth Scrolling ------ */

	$('a[href*="#"]:not([href="#"])').on('click', function() {
		if (location.pathname.replace(/^\//,'') == this.pathname.replace(/^\//,'') && location.hostname == this.hostname) {
			var target = $(this.hash);
			target = target.length ? target : $('[name=' + this.hash.slice(1) +']');
			if (target.length) {
				$('html, body').animate({
					scrollTop: target.offset().top
				}, 1000);
				return false;
			}
		}
	});


	/* ------ Remove button focus script ------ */

	$(".button").on('click', function(event) {
		// Removes focus of the button.
		$(this).blur();
	});

	/* ------ Counter script ------ */

	$('.timer').counterUp({
		delay: 20,
		time: 2500
	});

	/* ------ Navbar Fixed Top ------ */

	var didScroll;
	var lastScrollTop = 0;
	var delta = 5;
	var navbarHeight = $('header').outerHeight();

	$(window).on('scroll', function(event){
		didScroll = true;
	});

	setInterval(function() {
		if (didScroll) {
			hasScrolled();
			didScroll = false;
		}
	}, 250);

	function hasScrolled() {
		var st = $(this).scrollTop();

		if(Math.abs(lastScrollTop - st) <= delta)
			return;

		if (st > lastScrollTop && st > navbarHeight){
			// Scroll Down
			$('header').removeClass('nav-down').addClass('nav-up');
		} else {
			// Scroll Up
			if(st + $(window).height() < $(document).height()) {
				$('header').removeClass('nav-up').addClass('nav-down');
			}
		}

		lastScrollTop = st;
	}


	/* ------ Parallax background ------ */

	$window = $(window);
	if( $window.width() > 800){
		$('section[data-type="background"]').each(function(){
			var $bgobj = $(this);
			$(window).on('scroll', function() {
				var yPos = -( ($window.scrollTop() - $bgobj.offset().top) / $bgobj.data('speed'));
				var coords = '50% '+ yPos + 'px';
				$bgobj.css({ backgroundPosition: coords });
			});
		});    
	}
});