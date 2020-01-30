"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/sportHub").build();
//Disable buttons until connection is established
//document.getElementById("firstButtonId").disabled = true;
var arr = ["0", "15", "30", "40", "Ad"];

connection.on("ReceiveResult", function (result) {
    var fpp = "fpp" + "-" + result.id;
    $("#fpp" + "-" + result.id).html(arr[result.firstPlayerPoints]),
    $("#fpg" + "-" + result.id).html(result.firstPlayerGames),
    $("#fps" + "-" + result.id).html(result.firstPlayerSets),
    $("#fptbp" + "-" + result.id).html(result.firstPlayerTieBreakPoints),
    $("#spp" + "-" + result.id).html(arr[result.secondPlayerPoints]),
    $("#spg" + "-" + result.id).html(result.secondPlayerGames),
    $("#sps" + "-" + result.id).html(result.secondPlayerSets),
    $("#sptbp" + "-" + result.id).html(result.secondPlayerTieBreakPoints)
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});
