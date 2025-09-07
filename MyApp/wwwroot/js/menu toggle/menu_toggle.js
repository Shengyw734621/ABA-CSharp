// 響應式menu_toggle開啟關閉
$(document).ready(function() {
    $('.menu_toggle').on('click', function() {
        $(this).css({'width': '0', 'height': '0'});
        $('.popup_menu').css('visibility', 'visible');
        $('.hamburger_icon1, .hamburger_icon2, .hamburger_icon3').css('display', 'none');
    });

    $('.popup_close_btn').on('click', function() {
        $('.menu_toggle').css({'width': '30px', 'height': '30px'});
        $('.popup_menu').css('visibility', 'hidden');
        $('.hamburger_icon1, .hamburger_icon2, .hamburger_icon3').css('display', 'block');
    });
});

// 響應式menu_toggle切換頁面
$(document).ready(function () {
    // 顧客資訊menu
    $('.popup_item_link_L1.item2').on('click', function () {
        setTimeout(function(){
            $('.popup_lastpage_btn').css('display', 'inline-block');
            $('.popup_item_link_L1.item1').text('客戶資料');
            $('.popup_item_link_L1.item2').text('供應商資料');
            $('.popup_item_L1').slice(2, 5).remove();
            registerClickHandlers(); // 重新註冊點擊事件
        },500)
    });

    // 服務紀錄menu
    $('.popup_item_link_L1.item3').on('click', function () {
        setTimeout(function(){
            $('.popup_lastpage_btn').css('display', 'inline-block');
            $('.popup_item_link_L1.item1').text('產品出貨紀錄');
            $('.popup_item_link_L1.item2').text('產品維修紀錄');
            $('.popup_item_L1').slice(2, 5).remove();
            registerClickHandlers(); 
        },500) //延遲0.5秒
    });

    // 技術資訊menu
    $('.popup_item_link_L1.item4').on('click', function () {
        setTimeout(function(){
            $('.popup_lastpage_btn').css('display', 'inline-block');
            $('.popup_item_link_L1.item1').text('專案進度');
            $('.popup_item_link_L1.item2').text('Universal Robots');
            $('.popup_item_link_L1.item3').text('UR Plus');
            $('.popup_item_L1').slice(3, 5).remove();
            registerClickHandlers(); 
        },500)
    });

    // 回復成.popup_menu的狀態
    $('.popup_lastpage_btn').on('click', function () {
        var popupLastpageBtn = $(this);
        setTimeout(function() {
            popupLastpageBtn.css('display', 'none');
            $('.popup_item_link_L1.item1').text('最新消息');
            $('.popup_item_link_L1.item2').text('顧客資訊');
            if($('.popup_item_link_L1.item3').length === 0){
                $('.popup_menu').append('<li class="popup_item_L1"><a href="#!" class="popup_item_link_L1 item3">服務紀錄</a></li>');
            }
            else{
                $('.popup_item_link_L1.item3').text('服務紀錄');
            }
            $('.popup_menu').append('<li class="popup_item_L1"><a href="#!" class="popup_item_link_L1 item4">技術資訊</a></li>');
            $('.popup_menu').append('<li class="popup_item_L1"><a href="#!" class="popup_item_link_L1 item5">關於技術部</a></li>');
            registerClickHandlers();
        }, 500);
    });

    // 註冊點擊事件
    // 遞迴函數(將舊的點擊事件處理函數移除 在程式內調用可以回復到初始未點擊狀態)
    function registerClickHandlers() {
        // 移除先前註冊的點擊事件監聽器
        $('.popup_item_link_L1.item2').off('click');
        $('.popup_item_link_L1.item3').off('click');
        $('.popup_item_link_L1.item4').off('click');

        // 重新註冊點擊事件
        $('.popup_item_link_L1.item2').on('click', function () {
            setTimeout(function() {
                $('.popup_lastpage_btn').css('display', 'inline-block');
                $('.popup_item_link_L1.item1').text('客戶資料');
                $('.popup_item_link_L1.item2').text('供應商資料');
                $('.popup_item_L1').slice(2, 5).remove();
                registerClickHandlers(); // 重新註冊點擊事件
            }, 500); 
        });

        $('.popup_item_link_L1.item3').on('click', function () {
            setTimeout(function() {
                $('.popup_lastpage_btn').css('display', 'inline-block');
                $('.popup_item_link_L1.item1').text('產品出貨紀錄');
                $('.popup_item_link_L1.item2').text('產品維修紀錄');
                $('.popup_item_L1').slice(2, 5).remove();
                registerClickHandlers(); 
            }, 500); 
        });

        $('.popup_item_link_L1.item4').on('click', function () {
            setTimeout(function(){
                $('.popup_lastpage_btn').css('display', 'inline-block');
                $('.popup_item_link_L1.item1').text('專案進度');
                $('.popup_item_link_L1.item2').text('Universal Robots');
                $('.popup_item_link_L1.item3').text('UR Plus');
                $('.popup_item_L1').slice(3, 5).remove();
                registerClickHandlers(); 
            },500)
        });
    }
});
