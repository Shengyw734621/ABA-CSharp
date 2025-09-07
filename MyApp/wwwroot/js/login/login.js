// login 互動
$(document).ready(function() {

    $('.login_link').off('click').on('click', function() {
        $('.session_mask').fadeIn(300);
        $('.register_area').css('display','none')
        $('.session_area').css('display', 'block'); 
        $('.session_area').css('background-color', '#00a4e3');
        $('.session_mask').css('display', 'flex');
        $('.search_form').css('display', 'none');
        $('.login_form').css('display', 'inline-block');
        $('.login_textbox-1').css('margin-bottom','10px');
        $('.account_error').css('display','none');
    });

    $('.close_btn').on('click', function() {
        $('.session_mask').fadeOut(300, function() {
            $(this).css('display', 'none');
        });
    }); 
});