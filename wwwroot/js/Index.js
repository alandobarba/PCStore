let page = 2;  // Página inicial de la paginación (ya cargamos la primera página en el servidor)
const pageSize = 10; // Número de productos por carga
let isLoading = false; // Para evitar múltiples solicitudes de carga al mismo tiempo

// Función para cargar más productos
function loadMoreProducts() {
    if (isLoading) return;  // Si ya estamos cargando, no hacer nada
    isLoading = true; // Marcamos que estamos cargando

    $.ajax({
        url: "/Home/LoadMoreProducts",  // Llamamos a la acción LoadMoreProducts
        type: "GET",
        data: {
            page: page,
            pageSize: pageSize
        },
        success: function (data) {
            let htmlContent = '';
            data.forEach(function (item) {
                htmlContent += `
                    <div class='col' data-product-id='${item.idProduct}'>
                        <div class='card h-100 shadow-sm border-light rounded'>
                            <div class='card-body d-flex flex-column'>
                                <div class='text-center'>
                                    <a href='/Home/ProductView?idProduct=${item.idProduct}'>
                                        ${item.picture != null ?
                        `<img src='data:image/jpeg;base64,${item.picture}' class='card-img-top' alt='${item.productName}' style='object-fit: cover; height: 200px;' />` :
                        "<img src='https://via.placeholder.com/300' class='card-img-top' alt='No image' style='object-fit: cover; height: 200px;' />"}
                                    </a>
                                </div>
                                <h5 class='card-title text-center mt-3'>${item.productName}</h5>
                                <p class='card-text text-muted text-center' style='font-size: 14px;'>${item.productDescription}</p>
                                <p class='card-text text-center fw-bold'>${item.sellingPrice}</p>
                            </div>
                        </div>
                    </div>
                `;
            });

            // Añadimos los nuevos productos al final de la lista actual
            $("#product-cards").append(htmlContent);

            // Incrementamos la página para la próxima carga
            page++;

            // Terminamos el proceso de carga
            isLoading = false;
        },
        error: function () {
            alert("Error al cargar los productos.");
            isLoading = false; // Si ocurre un error, volvemos a permitir que se cargue
        }
    });
}

// Detectamos el scroll para cargar más productos
$(window).on('scroll', function () {
    // Verificamos si el usuario ha llegado al final de la página
    if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
        // Si estamos al final de la página y no estamos cargando ya, cargamos más productos
        if (!isLoading) {
            loadMoreProducts();
        }
    }
});
