// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function copyPath() {
    const copyText = document.getElementById("copyPath");

    navigator.clipboard.writeText(copyText.innerText);
    alert("Copied the text: " + copyText.innerText);
}


$("document").ready(function () {
    setTimeout(function () {
        $(".alert").hide('medium');
    }, 5000);
});