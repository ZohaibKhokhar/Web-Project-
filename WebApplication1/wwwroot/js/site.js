// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//ready function makes sure that the page is completely loaded on client side and then  
$(document).ready(function () {

    $("#search").click(function () {   //when you click on the button this funtion executes

        var data = $('#searchText').val();
        $.get('/Home/Search', { x: data}, function (result) {
            $('#myplaceholder').html(result);
        });
    });
});