"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/sportHub").build();
//Disable buttons until connection is established
//document.getElementById("firstButtonId").disabled = true;
var arr = ["0", "15", "30", "40", "Ad"];

connection.on("ReceiveResult", function (result) {   
        $("#fpp").html(arr[result.firstPlayerPoints]),
        $("#spp").html(arr[result.secondPlayerPoints])
});

connection.start().then(function () {
    //document.getElementById("firstButtonId").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("firstButtonId").addEventListener("click", function (event) {
//    var id = 141;
//    connection.invoke("UpdateResult", id).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});