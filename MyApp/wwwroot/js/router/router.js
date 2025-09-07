let initialContent = ''; // 初始首頁內容
const NewsPage = '/templates/news/news.html'; // 最新消息的頁面
const CustomerPage = '/templates/customer/customer.html'
const SupplierPage = '/templates/customer/supplier.html'
const shipmentPage = '/templates/service/shipment.html'
const maintenancePage = '/templates/service/maintenance.html'
const projectprogressPage = '/templates/technical/project.html'
const urPage = '/templates/technical/UR.html'
const urplusPage = '/templates/technical/UR Plus.html'
const aboutPage = '/templates/about/about.html'



// 先記錄一開始的內容
window.onload = function() {
    const article = document.querySelector('article');
    if (article) {
        initialContent = article.outerHTML; // 把整個<article>記下來

        // 初始化時：看網址是否有 ?page=
        const urlParams = new URLSearchParams(window.location.search);
        const pageParam = urlParams.get('page');
        if (pageParam) {
            navigateToPage(pageParam, false); // 第一次不要 pushState
        }
    }

    // 綁定 home 按鈕
    const homeLink = document.querySelector('.logo_txt');
    const homeLink2 = document.querySelector('.navbar_title')
    if (homeLink) {
        homeLink.addEventListener('click', function(event) {
            event.preventDefault();
            navigateToPage('home');
        });
    }
    if(homeLink2){
        homeLink2.addEventListener('click', function(event) {
            event.preventDefault();
            navigateToPage('home');
        });
    }

    // 綁定 news 按鈕
    const newsLink = document.querySelector('.news');
    if (newsLink) {
        newsLink.addEventListener('click', function(event) {
            event.preventDefault();
            navigateToPage(NewsPage);
        });
    }

    // 綁定 customer 按鈕
    const customerLink = document.querySelector('.customer_information');
    if (customerLink) {
        customerLink.addEventListener('click', function(event) {
            event.preventDefault();
            navigateToPage(CustomerPage);
        });
    }

    // 綁定 supplier 按鈕
    const supplierLink = document.querySelector('.supplier_information');
    if(supplierLink){
        supplierLink.addEventListener('click', function(event){
            event.preventDefault();
            navigateToPage(SupplierPage);
        })
    }

    // 綁定 shipment 按鈕
    const shipmentLink = document.querySelector('.shipment_information');
    if(shipmentLink){
        shipmentLink.addEventListener('click', function(event){
            event.preventDefault();
            navigateToPage(shipmentPage);
        })
    }

    // 綁定 maintenance 按鈕
    const maintenanceLink = document.querySelector('.maintenance_information');
    if(maintenanceLink){
        maintenanceLink.addEventListener('click', function(event){
            event.preventDefault();
            navigateToPage(maintenancePage);
        })
    }

    // 綁定 project 按鈕
    const projectLink = document.querySelector('.project_progress');
    if(projectLink){
        projectLink.addEventListener('click', function(event){
            event.preventDefault();
            navigateToPage(projectprogressPage);
        })
    }

    // 綁定 UR 按鈕
    const urLink = document.querySelector('.UR');
    if(urLink){
        urLink.addEventListener('click', function(event){
            event.preventDefault();
            navigateToPage(urPage);
        })
    }

    // 綁定 UR plus 按鈕
    const urplusLink = document.querySelector('.UR_plus')
    if(urplusLink){
        urplusLink.addEventListener('click', function(event){
            event.preventDefault();
            navigateToPage(urplusPage);
        })
    }

    // 綁定 about 按鈕
    const aboutLink = document.querySelector('.about')
    if(aboutLink){
        aboutLink.addEventListener('click', function(event){
            event.preventDefault();
            navigateToPage(aboutPage);
        })
    }
};

// 專門用來切換頁面
function navigateToPage(page, push = true) {
    clearContent();

    if (page === 'home') {
        restoreInitialContent();
        if (push) history.pushState({}, '', '/'); // ✅ 正確推回首頁
    } else {
        createContainer();
        loadPage(page, push);
    }
}

// 載入頁面（支援要不要加歷史記錄）
function loadPage(page, pushState = true) {
    fetch(page)
        .then(response => {
            if (!response.ok) throw new Error('載入失敗');
            return response.text();
        })
        .then(html => {
            const content = document.getElementById('content');
            if (content) {
                content.innerHTML = html;
            }
            if (pushState) {
                history.pushState({ page }, '', `?page=${page}`);
            }
        })
        .catch(error => {
            console.error('載入失敗:', error);
        });
}

// 處理popstate（上一頁、下一頁）
window.addEventListener('popstate', (event) => {
    clearContent();
    if (event.state && event.state.page) {
        createContainer();
        loadPage(event.state.page, false);
    } else {
        restoreInitialContent();
    }
});

// 清除目前的顯示內容
function clearContent() {
    const article = document.querySelector('article');
    const container = document.getElementById('content');

    if (article) article.remove();
    if (container) container.remove();
}

// 建立一個新的 container
function createContainer() {
    const wrapper = document.querySelector('.wrapper');
    const footer = document.querySelector('footer');

    const container = document.createElement('section');
    container.id = 'content';

    if (wrapper && footer) {
        wrapper.insertBefore(container, footer);
    }
}

// 還原初始首頁內容
function restoreInitialContent() {
    const wrapper = document.querySelector('.wrapper');
    const footer = document.querySelector('footer');

    if (initialContent && wrapper && footer) {
        const temp = document.createElement('div');
        temp.innerHTML = initialContent;
        const article = temp.firstElementChild;
        wrapper.insertBefore(article, footer);
    }
}