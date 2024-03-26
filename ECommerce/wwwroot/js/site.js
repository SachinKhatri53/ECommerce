function hello() {
    var sidebar = document.getElementById("sidebar")
    var main = document.getElementById("main")
    var sidebarWidth = window.getComputedStyle(sidebar).width
    if (sidebarWidth === '300px') {
        sidebar.style.width = '0'
        sidebar.style.padding = '0'
        main.style.marginLeft = '0'
    }
    else {
        sidebar.style.width = '300px'
        sidebar.style.padding = '20px'
        main.style.marginLeft = '300px'
    }
}

function checkScreenWidth() {
    var sidebar = document.getElementById("sidebar");
    var main = document.getElementById("main");
    var screenWidth = window.innerWidth;

    if (screenWidth <= 600) { // Adjust this value based on when you want the sidebar to close
        sidebar.style.width = '0';
        sidebar.style.padding = '0';
        main.style.marginLeft = '0';
    } else {
        sidebar.style.width = '300px';
        sidebar.style.padding = '20px';
        main.style.marginLeft = '300px';
    }
}




$(document).ready(() => {
    // Call the function on page load and on window resize
    window.onload = checkScreenWidth;
    window.onresize = checkScreenWidth;

    // Increase or decrease quantity
    var plusButton = $("#increase-quantity")
    var minusButton = $("#decrease-quantity")
    var inputField = $("#input-quantity")
    var quantityValidation = $("#max-quantity-validation")
    var maxQuantity = parseInt(inputField.attr("max"));
    inputField.on("input", function () {
        var value = parseInt($(this).val(), 10)
        var maxQuantity = parseInt($(this).attr("max"));
        if (isNaN(value) || value < 1) {
            $(this).val(1)
        }
        if (value >= maxQuantity) {
            $(this).val(maxQuantity)
            quantityValidation.removeClass("d-none");
            plusButton.prop('disabled', true);
        } else {
            quantityValidation.addClass("d-none");
            plusButton.prop('disabled', false);
        }
    })
    minusButton.click(() => {
        var value = parseInt(inputField.val(), 10)
        if (value > 1) {
            inputField.val(--value);
        }
        if (value < maxQuantity) {
            quantityValidation.addClass("d-none");
            plusButton.prop('disabled', false);
        }
    })
    plusButton.click(() => {
        var value = parseInt(inputField.val(), 10)
        /*        var maxQuantity = parseInt(inputField.attr("max"));*/
        if (value >= maxQuantity - 1) {
            $(inputField).val(maxQuantity)
            quantityValidation.removeClass("d-none");
            plusButton.prop('disabled', true);
        }
        inputField.val(++value)
    })


    // Attach click event to cart buttons
    $(".cart-button").on("click", function () {
        /*var inputField = $("#input-quantity")*/
        var productId = $(".cart-button").data("product-id")
        var productName = $(".cart-button").data("product-name")
        var price = $(".cart-button").data("price")
        var stock = $(".cart-button").data("product-stock")
        console.log(stock)
        var newItemData = {
            productId: productId,
            productName: productName,
            price: price,
            quantity: inputField.val(),
            availableStock: stock
        };
        console.log(newItemData)
        $.ajax({
            url: "/Cart/AddItemToCart",
            method: "POST",
            dataType: 'json',
            data: newItemData,
            success: function (response) {
                $('.badge-number').text(response.cartItems.length);
            },
            error: function (error) {
                console.error("Error adding item to cart:", error);
            }
        });
    });
})

// Edit Profile Form Validation
function updateProfileValidation(event) {
    event.preventDefault();

    if ($("input[name='FullName']").val() === '') {
        $(".fullname-validation").removeClass('d-none')
        $("#profile-edit").addClass("active")
        return
    }
    else if ($("input[name='Email']").val() === '') {
        $(".email-validation").removeClass('d-none')
        $("#profile-edit").addClass("active")
        return
    }
    else if ($("input[name='Birthdate']").val() === '') {
        $(".birthdate-validation").removeClass('d-none')
        $("#profile-edit").addClass("active")
        return
    }
    $("#profile-edit").submit()
}

// Checkout Form Validation
function checkFormValidation() {
    var amount = Number($("#grand-total").text())
    var fullName, state, country, city, postal, street, phone, email
    fullName = $("#fullName").val()
    state = $("#state").val()
    country = $("#country").val()
    city = $("#city").val()
    postal = $("#postal").val()
    street = $("#street").val()
    phone = $("#phone").val()
    email = $("#email").val()

    var errorMessages = {};

    // Check if any of the fields is empty
    if (fullName.trim() === '') {
        errorMessages.fullName = "Full Name is required.";
    }
    if (state.trim() === '') {
        errorMessages.state = "State is required.";
    }
    if (country.trim() === '') {
        errorMessages.country = "Country is required.";
    }
    if (city.trim() === '') {
        errorMessages.city = "City is required.";
    }
    if (postal.trim() === '') {
        errorMessages.postal = "Postal Code is required.";
    }
    if (street.trim() === '') {
        errorMessages.street = "Street is required.";
    }
    if (phone.trim() === '') {
        errorMessages.phone = "Contact is required.";
    }
    if (email.trim() === '') {
        errorMessages.email = "Email is required.";
    }

    // Check if there are any error messages
    if (Object.keys(errorMessages).length > 0) {
        // Display error messages on the page
        displayErrorMessages(errorMessages);
    } else {
        // All fields are non-empty, proceed with the AJAX post
        var shippingAddress = {
            ShippingName: fullName,
            Email: email,
            Phone: phone,
            Street: street,
            City: city,
            State: state,
            ZipCode: postal,
            Country: country,
            Amount: amount
        }
        $.ajax({
            type: "POST",
            url: "/Checkout/CashOnDeliveryPayment", // Replace with your actual URL
            data: {
                shippingAddress: shippingAddress
            },
            success: function (result) {
                // Handle success
                console.log("AJAX post successful");
                window.location.href = result.redirectUrl;
            },
            error: function (error) {
                // Handle error
                console.error("Error in AJAX post: " + error.responseText);
            }
        });
    }
}
// Function to display error messages on the page
function displayErrorMessages(errorMessages) {
    // Clear existing error messages
    $(".error-message").remove();

    // Display new error messages next to corresponding fields
    $.each(errorMessages, function (fieldName, message) {
        $("#" + fieldName).after("<div class='error-message text-danger'>" + message + "</div>");
    });
}