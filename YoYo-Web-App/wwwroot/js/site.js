// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

var app = {

    startTest: () =>
    {
        //console.log(event.target);

        $("#btnPlay").addClass("d-none");
        $("#btnPlayInfo").removeClass("d-none");

        $(".btn-sec a").removeClass("d-none");

        const url = "api/setting/StartTimer";
        const data = { NextLevel: 1, ShuttleNumber: 1 };
        app.post(url, data, app.processTimerStatus);
    },

    processTimerStatus: (response) =>
    {
        console.log(response);
        if (!response)
        {
            alert("Timer status not available");
            return;
        }

        $("#shuttleLevel").text(`Level ${response.currentShuttleLevel}`);
        $("#shuttleNumber").text(`Shuttle ${response.shuttleNumber}`);
        $("#speed").text(`${response.speed} km/h`);

        $("#currentShuttleSecondsLeft").text(`${response.currentShuttleSecondsLeft}s`);
        //$("#totalTime").text();
        $("#totalDistance").text(`${response.totalDistance} m`);

        var secondsLeft = response.currentShuttleSecondsLeft;
        var nextShuttleTimer = setInterval(() =>
        {
            secondsLeft--;
            
            if (secondsLeft <= 0)
            {
                clearInterval(nextShuttleTimer);
                const url = "api/setting/GetTimerStatus";
                const data = {
                    NextLevel: response.currentShuttleLevel + 1,
                    ShuttleNumber: response.shuttleNumber
                };
                app.post(url, data, app.processTimerStatus);
            } else
            {
                $("#currentShuttleSecondsLeft").text(`${secondsLeft}s`);
            }
        }, 1000);


        // Timer play status
        //const url = "api/setting/GetTimerStatus";
        //const data = {
        //    NextLevel: response.currentShuttleLevel + 1,
        //    ShuttleNumber: response.shuttleNumber
        //};

        //setTimeout(() =>
        //{
        //    app.post(url, data, app.processTimerStatus);
        //}, 1000);
    },

    request: (url, data, successCallback) =>
    {
        $.ajax({
            type: "GET",
            data: data,
            url: url,
            success: function (response)
            {
                successCallback(response);
            },
            error: function (xhr)
            {
                const errorMessage = `${xhr.status}:${xhr.statusText}`;
                console.error(errorMessage);
                console.log(xhr.responseText);
            }
        });
    },

    post(url, data, successCallback)
    {
        const json = JSON.stringify(data);
        $.ajax({
            type: "POST",
            data: json,
            contentType: "application/json; charset=utf-8",
            url: url,
            success: function (response)
            {
                successCallback(response);
            },
            error: function (xhr)
            {
                const errorMessage = `${xhr.status}:${xhr.statusText}`;
                console.error(errorMessage);
                console.log(xhr.responseText);
            }
        });
    }
};