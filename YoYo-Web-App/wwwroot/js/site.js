"use strict";

var app = {

    startTest: () =>
    {
        $("#btnPlay").addClass("d-none");
        $("#btnPlayInfo").removeClass("d-none");

        $(".btn-sec a").removeClass("d-none");

        const url = "api/setting/StartTimer";
        const data = { ShuttleLevel: 1, ShuttleNumber: 1 };
        app.post(url, data, app.processTimerStatus);
    },

    processTimerStatus: (response) =>
    {
        console.log("Response ", response);
        if (!response)
        {
            // TODO: Test completed. Show "view results" button
            alert("Timer status not available");
            return;
        }

        $("#shuttleLevel").text(`Level ${response.shuttleLevel}`);
        $("#shuttleNumber").text(`Shuttle ${response.shuttleNumber}`);
        $("#speed").text(`${response.speed} km/h`);

        app.setNextShuttleTimer(response);
        app.setTotalTimeTimer(0, response.currentShuttleSecondsLeft);
        app.setTotalDistanceTimer(response);
    },

    setNextShuttleTimer: (response) =>
    {
        let secondsLeft = response.currentShuttleSecondsLeft, minutes, seconds;

        const nextShuttleTimer = setInterval(() =>
        {
            minutes = Math.floor(secondsLeft / 60);
            seconds = secondsLeft % 60;

            minutes = minutes < 10 ? `0${minutes}` : minutes;
            seconds = seconds < 10 ? `0${seconds}` : seconds;

            secondsLeft--;

            if (secondsLeft <= 0)
            {
                clearInterval(nextShuttleTimer);

                const url = "api/setting/GetTimerStatus";
                const data = {
                    ShuttleLevel: response.shuttleLevel + 1,
                    ShuttleNumber: response.shuttleNumber,
                    TotalDistance: response.totalDistance,
                    TotalTimeSeconds: response.currentShuttleSecondsLeft
                };
                console.dir("GetTimerStatus data ", data);
                app.post(url, data, app.processTimerStatus);
            } else
            {
                $("#currentShuttleSecondsLeft").text(`${minutes}:${seconds} s`);
            }
        }, 1000);
    },

    setTotalTimeTimer: (duration, timeLimit) =>
    {
        let timer = duration, minutes, seconds;
        const totalTimeTimer = setInterval(() =>
        {
            minutes = parseInt(timer / 60, 10);
            seconds = parseInt(timer % 60, 10);

            minutes = minutes < 10 ? `0${minutes}` : minutes;
            seconds = seconds < 10 ? `0${seconds}` : seconds;

            timer++;

            if (timeLimit - timer <= 1)
            {
                clearInterval(totalTimeTimer);

                minutes = Math.floor(timeLimit / 60);
                seconds = timeLimit % 60;

                $("#totalTime").text(`${minutes}:${seconds} m`);
            } else
            {
                $("#totalTime").text(`${minutes}:${seconds} m`);
            }

        }, 1000);
    },

    setTotalDistanceTimer: (response) =>
    {
        let distance = 0; // response.accumulatedShuttleDistance;
        const totalDistanceTimer = setInterval(() =>
        {
            distance = distance + response.distanceIncrementer;

            if (response.distanceLimit - distance <= 1)
            {
                clearInterval(totalDistanceTimer);
                $("#totalDistance").text(`${response.distanceLimit.toFixed(2)} m`);

            } else
            {
                $("#totalDistance").text(`${distance.toFixed(2)} m`);
            }
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