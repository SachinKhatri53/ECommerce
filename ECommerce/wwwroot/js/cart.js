
/*Increase or decrease product quantity*/
function updateQuantity(productId, action, availableStock) {
    var productQuantity = $("#product-quantity-" + productId);
    var value = parseInt(productQuantity.text());

    $.ajax({
        type: "POST",
        url: "/Cart/UpdateQuantity",
        data: { productId: productId, action: action, quantity: value, availableStock: availableStock },
        success: function (result) {
            location.reload()
            console.log("Quantity updated successfully");
            console.log(result.cartItems)
        },
        error: function (error) {
            console.error("Error updating quantity: " + error.responseText);
        }
    });
}




// Remove item from the cart
function removeItem(productId) {
    $.ajax({
        type: "POST",
        url: "/Cart/DeleteFromCart",
        data: { productId: productId },
        success: function (result) {
            console.log("Item removed successfully");
            location.reload();
        },
        error: function (error) {
            console.error("Error removing item: " + error.responseText);
        }
    });
}

/*Clear the Cart*/
function clearCart() {
    $.ajax({
        type: "POST",
        url: "/Cart/ClearCart",
        success: function (result) {
            location.reload();
            console.log("Cart cleared successfully");
        },
        error: function (error) {
            console.error("Error clearing cart: " + error.responseText);
        }
    });
}