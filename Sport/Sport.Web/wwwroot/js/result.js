﻿"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/sportHub")
    .build();

var arr = ["0", "15", "30", "40", "Ad"];
console.log("YESSS");


connection.on("NewResult", function (result) {    

        $("#fpp" + "-" + result.id).html(arr[result.firstPlayerPoints]),
        $("#fpg" + "-" + result.id).html(result.firstPlayerGames),
        $("#fps" + "-" + result.id).html(result.firstPlayerSets),
        $("#fptbp" + "-" + result.id).html(result.firstPlayerTieBreakPoints),
        $("#spp" + "-" + result.id).html(arr[result.secondPlayerPoints]),
        $("#spg" + "-" + result.id).html(result.secondPlayerGames),
        $("#sps" + "-" + result.id).html(result.secondPlayerSets),
        $("#sptbp" + "-" + result.id).html(result.secondPlayerTieBreakPoints),
        console.log("finaly")
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});


