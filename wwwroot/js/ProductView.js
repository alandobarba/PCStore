$(document).ready(function () {
    $('.btnAddToCart').on('click', function () {
        var idProduct = $(this).data('idproduct');
        var quantity = $('#quantity').val();

        var cartModel = {
            IdProduct: parseInt(idProduct),
            Quantity: parseInt(quantity)
        };

        $.ajax({
            type: "POST",
            url: "/Cart/AddToCart",
            data: JSON.stringify(cartModel),
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function () {
                alert("Ocurrió un error inesperado");
            }
        });
    });
});