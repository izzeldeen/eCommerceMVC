function errorCallback(error) {
    console.log(JSON.stringify(error));
}
function completeCallBack() {
    console.log('Payment success');
}
function cancelCallBack() {
    console.log('Payment Canceled');

}


function transferPrice(price) {
    document.cookie = "price=" + price;
    $(window).scrollTop(0);

   // $("#scrollToDiv").animate({ scrollTop: 0 }, "fast");
    //var myDiv = document.getElementById('scrollToDiv');
    //myDiv.scrollTop = 0;
}
function transferName() {
    var fullName = document.getElementById("userFullName").value;
    document.cookie = "fullname=" + fullName;
}

function createSession() {

    $(window).scrollTop(0);

    var price = document.cookie.replace(/(?:(?:^|.*;\s*)price\s*\=\s*([^;]*).*$)|^.*$/, "$1");

    var settings = {
        "url": "https://mepspay.gateway.mastercard.com/api/rest/version/55/merchant/9800048600/session",
        "method": "POST",
        "timeout": 0,
        "headers": {
        "Authorization": "Basic bWVyY2hhbnQuOTgwMDA0ODYwMDpBYV82NjY5MDk2",
        "Content-Type": "application/json"
        },
        "data": JSON.stringify({ "apiOperation": "CREATE_CHECKOUT_SESSION", "interaction": { "operation": "PURCHASE" }, "order": { "amount": price,"currency":"JOD","id":"3d414dba-bf45-49af-8625-cf5320f56929"}}),
    };

    $.ajax(settings).done(function (response) {
        console.log(response);
    });
}

Checkout.configure({
    merchant: 'TEST9800048600',
    order: {
        amount: function () {
            //Dynamic calculation of amount
            return document.cookie.replace(/(?:(?:^|.*;\s*)price\s*\=\s*([^;]*).*$)|^.*$/, "$1");
        },
        currency: 'JOD',
        description: 'Cart CheckOut',
        id: Date.now()

    },
    interaction: {
        operation: 'PURCHASE', // set this field to 'PURCHASE' for Hosted Checkout to perform a Pay Operation.
        merchant: {
            name: 'Jomlah Jo'
        }
    }
});



