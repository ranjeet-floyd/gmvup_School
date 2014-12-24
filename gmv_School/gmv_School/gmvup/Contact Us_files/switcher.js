// cookie functions http://www.quirksmode.org/js/cookies.html
// 
function createCookie(name,value,days)
{
	if (days)
	{
		var date = new Date();
		date.setTime(date.getTime()+(days*24*60*60*1000));
		var expires = "; expires="+date.toGMTString();
	}
	else var expires = "";
	document.cookie = name+"="+value+expires+"; path=/";
}

function readCookie(name)
{
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for(var i=0;i < ca.length;i++)
	{
		var c = ca[i];
		while (c.charAt(0)==' ') c = c.substring(1,c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	}
	return null;
}
function eraseCookie(name)
{
	createCookie(name,"",-1);
}


// Shaper University Style Switcher

jQuery(function($){

	$('.change-preset .settings-icon').on('click',function(){		

		if($('.change-preset').hasClass("active")){
			$('.change-preset').animate({"left":"-180px"},function(){
				$('.change-preset').toggleClass("active");
			});						
		}else{
			$('.change-preset').animate({"left":"0px"},function(){
				$('.change-preset').toggleClass("active");
			});			
		} 
	});

	$('#preset-styles > a').on('click', function(){
		var preset = $(this).data('link');
		$('link#preset-css').attr('href', preset);

		eraseCookie('preset_css');
		createCookie('preset_css', preset, 2);

		return false;
	});


	if( readCookie('preset_css') != null || readCookie('preset_css')=='' ){
		$('link#preset-css').attr('href', readCookie('preset_css'));
	}


	$('#zeereset').on('click', function(){

		eraseCookie('preset_css');
		window.location.reload();
		return false;

	});	

});

