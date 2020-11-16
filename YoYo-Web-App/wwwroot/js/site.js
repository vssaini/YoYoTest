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
            // TODO: Test completed. Show "view results" button
            alert("Timer status not available");
            return;
        }

        $("#shuttleLevel").text(`Level ${response.currentShuttleLevel}`);
        $("#shuttleNumber").text(`Shuttle ${response.shuttleNumber}`);
        $("#speed").text(`${response.speed} km/h`);

        $("#currentShuttleSecondsLeft").text(`${response.currentShuttleSecondsLeft}s`);
        $("#totalDistance").text(`${response.totalDistance} m`);

        app.startNextShuttleTimer(response.currentShuttleSecondsLeft, response.currentShuttleLevel, response.shuttleNumber);
        app.startTotalTimeTimer(0);
    },

    startNextShuttleTimer: (secondsLeft, currentShuttleLevel, shuttleNumber) =>
    {
        const nextShuttleTimer = setInterval(() =>
        {
            secondsLeft--;

            if (secondsLeft <= 0)
            {
                clearInterval(nextShuttleTimer);

                const url = "api/setting/GetTimerStatus";
                const data = {
                    NextLevel: currentShuttleLevel + 1,
                    ShuttleNumber: shuttleNumber
                };
                app.post(url, data, app.processTimerStatus);
            } else
            {
                $("#currentShuttleSecondsLeft").text(`${secondsLeft}s`);
            }
        }, 1000);
    },

    startTotalTimeTimer: (duration) =>
    {
        let timer = duration, minutes, seconds;
        setInterval(() =>
        {
            minutes = parseInt(timer / 60, 10);
            seconds = parseInt(timer % 60, 10);

            minutes = minutes < 10 ? `0${minutes}` : minutes;
            seconds = seconds < 10 ? `0${seconds}` : seconds;

            $("#totalTime").text(minutes + ":" + seconds);

            timer++;

            //if (timer++ < 0)
            //{
            //    timer = duration;
            //}
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