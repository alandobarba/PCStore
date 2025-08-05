$(document).ready(function () {
    const tax = 0.13;
    $(".cartItem").each(function () {
        var quantityText = $(this).find(".quantity").text();
        var quantity = parseFloat(quantityText.replace(/[^\d.]/g, ''));
        var price = parseFloat($(this).data("price"));
        var totalPrice = (quantity * price) * tax + (quantity * price);
        $(this).find(".totalPrice").text("Total: $" + totalPrice.toFixed(2));
    });
});