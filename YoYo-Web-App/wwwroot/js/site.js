﻿"use strict";

var shuttleLevel = 1, speedLevel, shuttleNumber = 1, progressBar, timeStarterSecond = 0, distanceStarter;

var app = {

    startTest: () =>
    {
        $("#btnPlay").addClass("d-none");
        $("#btnPlayInfo").removeClass("d-none");

        $(".btn-sec a").removeClass("d-none");

        progressBar = new ProgressBar.Line(container, {
            strokeWidth: 2,
            easing: "easeInOut",
            duration: 1400,
            color: "#ED6A5A",
            trailColor: "#eee",
            trailWidth: 1,
            svgStyle: null
        });

        const url = "api/setting/StartTimer";
        const data = { ShuttleLevel: 1, ShuttleNumber: shuttleNumber };
        app.post(url, data, app.processTimerStatus);
    },

    processTimerStatus: (response) =>
    {
        if (!response.data)
        {
            // TODO: Test completed. Show "view results" button
            alert("Timer status not available");
            return;
        }

        shuttleLevel = response.data.shuttleLevel;
        speedLevel = response.data.speedLevel;
        shuttleNumber = response.data.shuttleNumber;

        $("#shuttleLevel").text(`Level ${shuttleLevel}`);
        $("#shuttleNumber").text(`Shuttle ${shuttleNumber}`);
        $("#speed").text(`${response.data.speed} km/h`);

        app.setBarProgress(response.data.progressStep);

        app.setNextShuttleTimer(response.data);
        app.setTotalTimeTimer(response.data);
        app.setTotalDistanceTimer(response.data);
    },

    setBarProgress: (step) =>
    {
        // Ref - https://kimmobrunfeldt.github.io/progressbar.js/
        // Progress should be decimal format as 0.15 represents 15%

        progressBar.animate(step); // Number from 0.0 to 1.0
    },

    setNextShuttleTimer: (testStatusVm) =>
    {
        let secondsLeft = testStatusVm.currentShuttleSecondsLeft, minutes, seconds;

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
                    ShuttleLevel: testStatusVm.shuttleLevel + 1,
                    ShuttleNumber: testStatusVm.shuttleNumber,
                    TimeStarterSecond: timeStarterSecond,
                    DistanceStarter: distanceStarter
                };

                app.post(url, data, app.processTimerStatus);
            } else
            {
                $("#currentShuttleSecondsLeft").text(`${minutes}:${seconds} s`);
            }
        }, 1000);
    },
    
    setTotalTimeTimer: (testStatusVm) =>
    {
        let minutes, seconds;
        timeStarterSecond = testStatusVm.timeStarterSecond;
        const timeLimitSecond = testStatusVm.timeLimitSecond;

        const totalTimeTimer = setInterval(() =>
        {
            minutes = parseInt(timeStarterSecond / 60, 10);
            seconds = parseInt(timeStarterSecond % 60, 10);

            minutes = minutes < 10 ? `0${minutes}` : minutes;
            seconds = seconds < 10 ? `0${seconds}` : seconds;

            timeStarterSecond++;

            if (timeLimitSecond - timeStarterSecond > 0)
            {
                $("#totalTime").text(`${minutes}:${seconds} s`);

            } else
            {
                clearInterval(totalTimeTimer);

                minutes = Math.floor(timeLimitSecond / 60);
                seconds = timeLimitSecond % 60;

                $("#totalTime").text(`${minutes}:${seconds} s`);
            }
        }, 1000);
    },

    setTotalDistanceTimer: (testStatusVm) =>
    {
        distanceStarter = testStatusVm.distanceStarter;
        const distanceLimit = testStatusVm.distanceLimit;

        const totalDistanceTimer = setInterval(() =>
        {
            distanceStarter = distanceStarter + testStatusVm.distanceIncrementer;

            if (distanceLimit - distanceStarter > 0)
            {
                $("#totalDistance").text(`${distanceStarter.toFixed(2)} m`);

            } else
            {
                clearInterval(totalDistanceTimer);
                $("#totalDistance").text(`${distanceLimit.toFixed(2)} m`);
            }
        }, 1000);
    },

    warnAthlete: (athleteId) =>
    {
        const target = $(event.target);

        const url = "api/setting/WarnAthlete";
        const data = { AthleteId: athleteId };

        app.post(url, data, function (result)
        {
            if (result.data)
            {
                target.text("Warned");
                target.addClass("warned");
                target.attr("disabled", true);
            } else
            {
                const msg = result.errorMessage ? result.errorMessage : `Failed to warn athlete`;
                alert(msg);
            }
        });
    },

    stopAthlete: (athleteId) =>
    {
        const parent = $(event.target.parentElement);

        const url = "api/setting/StopAthlete";
        const data = {
            AthleteId: athleteId,
            SpeedLevel: speedLevel,
            ShuttleLevel: shuttleLevel,
            ShuttleNumber: shuttleNumber
        };

        app.post(url, data, function (result)
        {
            if (result.data.isStopped)
            {
                parent.addClass("d-none");
                $(parent).siblings("select").removeClass("d-none");
                $(parent).siblings("select").val(result.data.score);
            } else
            {
                const msg = result.errorMessage ? result.errorMessage : `Failed to stop athlete`;
                alert(msg);
            }
        });
    },

    updateAthleteTestScore: (ddlTestScore, athleteId) =>
    {
        const url = "api/setting/UpdateAthleteTestScore";
        const data = {
            AthleteId: athleteId,
            TestScore: ddlTestScore.value
        };

        app.post(url, data, function (result)
        {
            if (!result.data)
            {
                const msg = result.errorMessage ? result.errorMessage : `Failed to update athlete's test score.`;
                alert(msg);
            }
        });
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