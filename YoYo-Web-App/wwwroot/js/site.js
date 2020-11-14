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
        app.request(url, null, app.processTimerStatus);
    },

    processTimerStatus: (response) =>
    {
        const url = "api/setting/GetTimerStatus";
        console.log(response);

        // Timer play status
        $("#speedLevel").text(`Level ${response.speedLevel}`);
        $("#shuttleNumber").text(`Shuttle ${response.shuttleNumber}`);
        $("#speed").text(`${response.speed} km/h`);

        $("#currentShuttleSecondsLeft").text(`${response.currentShuttleSecondsLeft}s`);
        //$("#totalTime").text();
        $("#totalDistance").text(`${response.totalDistance} m`);

        setTimeout(() =>
        {
            app.request(url, null, app.processTimerStatus);
        }, 1000);
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
    }
};