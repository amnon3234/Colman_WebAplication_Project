
//  Search Users
function search() {
    var price = document.getElementById("MaxPrice");
    var gender = document.getElementById("gender");
    var categoryId = document.getElementById("category");
    window.location = "/Products/Search?categoryid=" + categoryId.value + "&price=" + price.value + "&gender=" + gender.value;
}

// Search Order History
function searchOrderHistory() {
    var categoryId = document.getElementById("category");
    var userId = document.getElementById("userid");
    var monthNumber = document.getElementById("monthNumber");
    window.location = "/Administrator/Search?userID=" + userId.value + "&categoryID=" + categoryId.value + "&monthNumber=" + monthNumber.value;
}

// Cart select box
function addCartItem(path) {
    var product = document.getElementById("product");
    window.location = path + product.value;
}

// Change location
function onButtonClick(path) {
    window.location = path;
}

const token = "NO";

// Post using facebook api
function postAStatus() {
    var status = document.getElementById('postTxt').value;

    FB.api(
        '/NO/feed',
        'POST',
        {
            "message": status,
            "access_token": token
        },
        function (response) {
            console.log(response);
        }
    );
}

function boostAProduct(status , photoURL) {
    FB.api(
        '/NO/photos',
        'POST',
        {
            "message": status,
            "url": photoURL,
            "access_token": token
        },
        function (response) {
            console.log(response);
        }
    );
}
