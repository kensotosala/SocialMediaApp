document.addEventListener("DOMContentLoaded", function () {
    const friendsButton = document.getElementById("friends-btn");
    friendsButton.addEventListener("click", function () {
        window.location.href = '/Mostrar_Amigos/Index';
    });
});