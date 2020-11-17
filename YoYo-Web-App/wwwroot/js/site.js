"use strict";

var shuttleLevel = 1, speedLevel, shuttleNumber = 1, bar;

var app = {

    startTest: () =>
    {
        $("#btnPlay").addClass("d-none");
        $("#btnPlayInfo").removeClass("d-none");

        $(".btn-sec a").removeClass("d-none");

        bar = new ProgressBar.Line(container, {
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
        console.log("Response ", response);
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
                    TotalDistance: testStatusVm.accumulatedDistance,
                    TotalTimeSeconds: testStatusVm.currentShuttleSecondsLeft
                };

                app.post(url, data, app.processTimerStatus);
            } else
            {
                $("#currentShuttleSecondsLeft").text(`${minutes}:${seconds} s`);
            }
        }, 1000);
    },

    setBarProgress: (step) =>
    {
        // Ref - https://kimmobrunfeldt.github.io/progressbar.js/
        // Progress should be decimal format as 0.15 represents 15%

        bar.animate(step); // Number from 0.0 to 1.0
    },

    setTotalTimeTimer: (testStatusVm) =>
    {
        let timer = testStatusVm.totalTimeSeconds, minutes, seconds;
        const timeLimit = testStatusVm.currentShuttleSecondsLeft;

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

                $("#totalTime").text(`${minutes}:${seconds} s`);
            } else
            {
                $("#totalTime").text(`${minutes}:${seconds} s`);
            }

        }, 1000);
    },

    setTotalDistanceTimer: (testStatusVm) =>
    {
        let distance = testStatusVm.totalDistance;
        const distanceLimit = testStatusVm.accumulatedDistance;

        const totalDistanceTimer = setInterval(() =>
        {
            distance = distance + testStatusVm.distanceIncrementer;

            if (distanceLimit - distance <= 1)
            {
                clearInterval(totalDistanceTimer);
                $("#totalDistance").text(`${distanceLimit.toFixed(2)} m`);

            } else
            {
                $("#totalDistance").text(`${distance.toFixed(2)} m`);
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