// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", function() {
    const navLogoutCtn = document.querySelector("#nav-logout-ctn");
    const logoutBtn = document.querySelector("#logout-btn");

    if (!!navLogoutCtn) {
        navLogoutCtn.addEventListener("click", function() {
            logoutBtn.click();
        });
    }
})
