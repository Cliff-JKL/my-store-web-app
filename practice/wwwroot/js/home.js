$('document').ready(function () {
    loadEvents();
});

function loadEvents() {
    $('.add-to-cart-button').on('click', sendProductId);
}

async function sendProductId() {
    let id = $(this).attr('product-id');

    await $.ajax({
        type: 'POST',
        url: '/ShoppingCart/AddToCart/' + id,
        data: id,
        success: function () {
        },
        error: function () {
            alert("Произошел сбой! (home/sendProductId.js)");
        }
    });

    let header_cart_cost = document.getElementById('header_cart_cost');
    let totalCost = getCookie('cartCost');
    header_cart_cost.innerText = totalCost + ' руб.';
}

// возвращает cookie с указанным name,
// или undefined, если ничего не найдено
function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}