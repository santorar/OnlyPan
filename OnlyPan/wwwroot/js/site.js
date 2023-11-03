// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// JavaScript para ocultar la notificación al recargar la página
if (performance.navigation.type === 1) {
    var notificationDiv = document.getElementById("notificationDiv");
    if (notificationDiv) {
        notificationDiv.style.display = "none";
    }
}