document.addEventListener("DOMContentLoaded", function () {
    const findButton = document.getElementById("find-btn");
    findButton.addEventListener("click", function (event) {
        event.preventDefault(); // Evita que el formulario envíe la solicitud
        window.location.href = '/Buscar_Usuarios/Index'; // Redirige a la página deseada
    });
});