"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sportHub").build();

//Disable buttons until connection is established
document.getElementById("firstButtonId").disabled = true;

connection.on("ReceiveResult", function (data) {
    ("#fpp").html(data.firstPlayerPoints),
    ("#spp").html(data.secondPlayerPoints)
});

connection.start().then(function () {
    document.getElementById("firstButtonId").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("firstButtonId").addEventListener("click", function (event) {
    var id = 141;
    connection.invoke("UpdateResult", id).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});