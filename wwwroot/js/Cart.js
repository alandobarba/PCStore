$(document).ready(function () {

    const tax = 0.13;
    let debounceTimeout;

    function updateSubtotalAndTotal(input) {
        const price = parseFloat(input.data('price'));
        let quantity = parseInt(input.val());
        if (isNaN(quantity) || quantity < 1) quantity = 1;

        const subtotal = price * quantity;
        const totalWithTax = subtotal + subtotal * tax;

        input.closest('.card-body').find('.subtotal').text(
            `Subtotal: ${subtotal.toLocaleString('en-US', { style: 'currency', currency: 'USD' })}`
        );
    }

    $('.quantity').each(function () {
        updateSubtotalAndTotal($(this));
    });

    $('.quantity').on('input', function () {
        const input = $(this);
        const cartItem = input.closest('.card');

        clearTimeout(debounceTimeout);
        debounceTimeout = setTimeout(function () {
            updateSubtotalAndTotal(input);

            const quantity = parseInt(input.val());
            const idProduct = input.data('idproduct');

            $.ajax({
                url: '/Cart/UpdateQuantity',
                type: 'POST',
                data: { idProduct: idProduct, quantity: quantity },
                success: function (response) {
                    $("#message").removeClass("alert-danger").addClass("alert-success").text(response.message).fadeIn().delay(2000).fadeOut();
                },
                error: function () {
                    $("#message").removeClass("alert-success").addClass("alert-danger").text("Error updating the item.").fadeIn().delay(2000).fadeOut();
                }
            });
        }, 1000);
    });

    $('#btnBuy').on('click', function () {
        $.ajax({
            url: '/User/GetIdUser',
            type: 'GET',
            success: function (response) {
                window.location.href = `/Stripe/CheckOut?idUser=${response}`;
            },
            error: function () {
                alert('Error fetching user data.');
            }
        });
    });
});
