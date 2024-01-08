function AddToCart(Id, Name, UnitPrice, Quantity) {
   
    $.ajax({
        type: 'GET',
        contentType: 'application/json;charset=utf-8',
        url: "Cart/AddToCart/"+Id+"/"+UnitPrice+"/"+Quantity,
        success: function (d) {
            console.log(d);
            
            var data = d.length > 0 ? JSON.parse(d) : null;
            console.log(data);
            if (data != null&&data.CartItems.length>0) {
                var message = '<strong>' + Name + '</strong> added to <a href="cart">Cart</a> successfully';
                $('#toastCart >.toast-body').html(message);  
                $('#toastCart').toast('show');
                $('#CartCounter').text(data.CartItems.length);
                setTimeout(function () {
                    $('#toastCart').toast('hide'); }, 4000);
            }
        }
    });
}

$(document).ready(function () {
    $.ajax({
        type: 'GET',
        contentType: 'application/json;charset=utf-8',
        url: 'Cart/GetCartCount',
        dataType:"json",
        success: function (data) {
            $('#CartCounter').text(data);
            
        },
        error: function (result) { },
    });
});


function deleteItem(id) {
    
    if (id > 0) {
        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: '/Cart/DeleteItem/' + id,
            success: function (data) {
                if (data > 0) {
                    location.reload();
                }
            },
            error: function (result) {
                console.log(result);
            },
        });
    }
}
function updateQuantity(id, total, quantity) {
    console.log(id);
    console.log(total);
    console.log(quantity);
    if (id > 0 && quantity > 0) {
        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: '/Cart/UpdateQuantity/' + id + "/" + quantity,
            success: function (data) {
                if (data > 0) {
                    location.reload();
                }
            },
            error: function (result) {
            },
        });
    }
    else if (id > 0 && quantity < 0 && total > 1) {
        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: '/Cart/UpdateQuantity/' + id + "/" + quantity,
            success: function (data) {
                if (data > 0) {
                    location.reload();
                }
            },
            error: function (result) {
            },
        });
    }
}
