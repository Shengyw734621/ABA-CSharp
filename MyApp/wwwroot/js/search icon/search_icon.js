// search icon 互動
$(document).ready(function() {
    $('.search_link').on('click', function() {
        $('.session_area').css('background-color', '#fff');
        $('.session_mask').fadeIn(300);
        $('.session_mask').css('display', 'flex');
        $('.search_form').css('display', 'inline-block');
        $('.login_form').css('display', 'none');
    });

    $('.close_btn').on('click', function() {
        $('.session_mask').fadeOut(300, function() {
            $(this).css('display', 'none');
        });
    }); 
});