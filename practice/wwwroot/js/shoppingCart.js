let cartProducts = new Map; // dictionary /w product and number of products

$('document').ready(function () {
    cartProducts = getCartData();
    loadEvents();
});

// возвращает куки с указанным name,
// или undefined, если ничего не найдено
function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

// получение словаря товаров и их количества в корзине из cookies
function getCartData() {
    let cartData = getCookie('cartData');

    if (cartData == null || cartData == undefined) {
        return new Map();
    }

    let pairs = cartData.split(',');
    let outData = new Map;

    for (let i = 0; i < pairs.length; i++) {
        let values = pairs[i].split('.');
        if (outData.has(values[0])) {
            let newValue = Number(outData.get(values[0])) + Number(values[1]);
            outData.set(values[0], newValue);
        }
        else {
            outData.set(values[0], Number(values[1]));
        }
    }

    return outData;
}

// события 
function loadEvents() {
    $('.product-count-button-plus').on('click', productIncrement);
    $('.product-count-button-minus').on('click', productDecrement);
    $('.product-count-button-delete').on('click', productDelete);
}

// updating cart items cost in header
function updateHeaderCost(id, sign, productNumber=1) {
    let header_cart_cost = document.getElementById('header_cart_cost');
    let product_price_elements = document.getElementById('product_model_' + id).getElementsByClassName('product-price');
    let productPrice = product_price_elements[0].innerText.slice(0, -5);
    let amount_cost = header_cart_cost.innerText.slice(0, -5);
    let totalCost = Number(amount_cost) + Number(productPrice) * Number(sign) * Number(productNumber);
    header_cart_cost.innerText = totalCost + ' руб.';
}

// adding product to cart
async function productIncrement() {
    let id = $(this).attr('data-id');

    await $.ajax({
        type: 'POST',
        url: '/ShoppingCart/AddToCart/' + id,
        data: id,
        success: function () {
            console.log('success')
        },
        error: function () {
            alert("Произошел сбой! (shoppingCart/productIncrement.js)");
        }
    })

    cartProducts.set(id, cartProducts.get(id) + 1); // item number + 1
    let pr_ = document.getElementById('pr_' + id);
    pr_.innerText = cartProducts.get(id);

    updateHeaderCost(id, 1);

    //updateCookies();
}

// decreasing product number in cart
async function productDecrement() {
    let id = $(this).attr('data-id');

    if (cartProducts.get(id) <= 1) {
        productDelete(this, true);
        return;
    }

    await $.ajax({
        type: 'POST',
        url: '/ShoppingCart/DecreaseNumberOfCartItem/' + id,
        data: id,
        success: function () {
            console.log('success')
        },
        error: function () {
            alert("Произошел сбой! (shoppingCart/productDecrement.js)");
        }
    });

    cartProducts.set(id, cartProducts.get(id) - 1)
    let pr_ = document.getElementById('pr_' + id);
    pr_.innerText = cartProducts.get(id);

    updateHeaderCost(id, -1);

    updateCookies();
}

// delete product entity from cart
async function productDelete(element, otherFunc = false) {
    let id = "";
    if (otherFunc) {
        id = $(element).attr('data-id');
    }
    else {
        id = $(this).attr('data-id');
    }

    await $.ajax({
        type: 'POST',
        url: '/ShoppingCart/RemoveFromCart/' + id,
        data: id,
        success: function () {
            console.log('success')
        },
        error: function () {
            alert("Произошел сбой! (shoppingCart/productDelete.js)");
        }
    });

    updateHeaderCost(id, -1, cartProducts.get(id));

    // delete element in html
    let product_model_ = document.getElementById('product_model_' + id);
    product_model_.outerHTML = '';
    //

    cartProducts.delete(id);

    updateCookies();
}

// update cart cookies
function updateCookies() {
    let cartData = "";
    cartProducts.forEach((value, key, map) => {
        cartData += key + '.' + value + ',';
    });
    cartData = cartData.slice(0, -1);

    let cartCost = "";
    let header_cart_cost = document.getElementById('header_cart_cost');
    cartCost = header_cart_cost.innerText.slice(0, -5);

    if (cartCost.trim() != '' && cartCost != '0')
        setCookie('cartCost', cartCost);
    else
        deleteCookie('cartCost');

    if (cartData.trim() != '')
        setCookie('cartData', cartData);
    else
        deleteCookie('cartData');
}

// update cookie string
function setCookie(name, value) {
    let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

    // max age = 7 days
    updatedCookie += '; path=/; max-age=604800'

    document.cookie = updatedCookie;
}

// delete cookie string 
function deleteCookie(name) {
    let deletedCookie = encodeURIComponent(name) + "=";
    deletedCookie += '; path=/; max-age=-1';

    document.cookie = deletedCookie;
}
