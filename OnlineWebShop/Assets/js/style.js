//---------- Login / Signup ----------//
$('.link_modal_login').click(function(){
  $(this).toggleClass("openClick")
  if($(this).hasClass("openClick")){
    $(".modal_outside_login").fadeIn();
    $(".border_modal_login").animate({"opacity":"0", "width": "0"},100).animate({"opacity": "1", "width": "60%"},500);
    $(".modal_contain_login").animate({"opacity":"0", "height": "0"},100).animate({"opacity": "1", "height": "100%"},500);
    $('.modal_outside_signup').fadeOut();
  }
  else{
    setTimeout(function(){
      $(".modal_outside_login").fadeOut();
    },300);
    $(".border_modal_login").animate({"opacity": "1", "width": "60%"},100).animate({"opacity":"0", "width": "0"},500);
    $(".modal_contain_login").animate({"opacity": "1", "height": "100%"},100).animate({"opacity":"0", "height": "0"},500);
    $('.link_modal_login').removeClass("openClick");
  }
});


$("body").click(function(e){
  if($(e.target).attr('id') == "modal"){
    setTimeout(function(){
      $(".modal_outside_login").fadeOut();
    },300);
    $(".border_modal_login").animate({"opacity": "1", "width": "60%"},100).animate({"opacity":"0", "width": "0"},850);
    $(".modal_contain_login").animate({"opacity": "1", "height": "100%"},100).animate({"opacity":"0", "height": "0"},850);
    $('.link_modal_login').removeClass("openClick");
  }
});

$('.link_modal_signup').click(function(){
  $(this).toggleClass("openClick")
  if($(this).hasClass("openClick")){
    $(".modal_outside_signup").fadeIn();
    $(".border_modal_signup").animate({"opacity":"0", "width": "0"},100).animate({"opacity": "1", "width": "60%"},500);
    $(".modal_contain_signup").animate({"opacity":"0", "height": "0"},100).animate({"opacity": "1", "height": "100%"},500);
    $('.modal_outside_login').fadeOut();
  }
  else{
    setTimeout(function(){
      $(".modal_outside_signup").fadeOut();
    },300);
    $(".border_modal_signup").animate({"opacity": "1", "width": "60%"},100).animate({"opacity":"0", "width": "0"},500);
    $(".modal_contain_signup").animate({"opacity": "1", "height": "100%"},100).animate({"opacity":"0", "height": "0"},500);
    $('.link_modal_signup').removeClass("openClick");
  }
});


$("body").click(function(e){
  if($(e.target).attr('id') == "modal"){
    setTimeout(function(){
      $(".modal_outside_signup").fadeOut();
    },300);
    $(".border_modal_signup").animate({"opacity": "1", "width": "60%"},100).animate({"opacity":"0", "width": "0"},850);
    $(".modal_contain_signup").animate({"opacity": "1", "height": "100%"},100).animate({"opacity":"0", "height": "0"},850);
    $('.link_modal_signup').removeClass("openClick");
  }
});

//---------- End Login / Signup ----------//


//---------- Set h4 Height ----------//
equalheight = function(container){
  var currentTallest = 0,
      currentRowStart = 0,
      rowDivs = new Array(),
      topPosition = 0;
  $(container).each(function(){
    $($(this)).height('auto');
    topPosition = $(this).position().top;
    if(currentRowStart != topPosition){
      for(currentDiv = 0; currentDiv < rowDivs.length; currentDiv ++){
        rowDivs[currentDiv].height(currentTallest);
      }
      rowDivs.length = 0;
      currentRowStart = topPosition;
      currentTallest = $(this).height();
      rowDivs.push($(this));
    }
    else {
      rowDivs.push($(this));
      currentTallest = (currentTallest < $(this).height()) ? ($(this).height()) : (currentTallest);
    }
    for(currentDiv = 0; currentDiv < rowDivs.length; currentDiv ++){
      rowDivs[currentDiv].height(currentTallest);
    }
  });
}

function set_h4_height(){
  var w_recent = document.documentElement.clientWidth;
  if(w_recent > 736){
    equalheight('.h4-title');
  }
  else {
    $('.h4-title').css('height','auto');
  }
}
//----------End Set h4 Height  ----------//

function reset_form(){
    var form = document.getElementById("form-login-modal");
    form.reset();
}


$(document).ready(function(){
    set_h4_height();
    reset_form();
});

$(window).resize(function(){
  set_h4_height();
});
