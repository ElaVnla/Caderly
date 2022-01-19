(function ($) {

    "use strict";

    // Given data for events in JSON format
    var event_data = {
        "events": [
            {
                "booktype": 1,
                "booktitle": " Repeated Test Event ",
                "bookvisitors": 120,
                "bookyear": 1970,
                "bookmonth": 1,
                "bookday": 20,
                "booktime": "1030",
                "bookstatus": "OnGoing",
                "bookduration" : "15mins"
            },
        ]
    };


    // Setup the calendar with the current date
    $(document).ready(function () {
        var date = new Date();
        var today = date.getDate();
        //alert(event_data);
        loadData2(date);
        // Set click handlers for DOM elements
        $(".right-button").click({ date: date }, next_year);
        $(".left-button").click({ date: date }, prev_year);
        $(".month").click({ date: date }, month_click);
        $("#add-button").click({ date: date }, new_event);
        // Set current month as active
        $(".months-row").children().eq(date.getMonth()).addClass("active-month");
        init_calendar(date);
        var events = check_events(today, date.getMonth() + 1, date.getFullYear());
        show_events(events, months[date.getMonth()], today, date.getFullYear());
    });

    function loadData2(date) {
        //alert("Hello!");
        $.getJSON('https://localhost:44362/api/Booking', function (data) {
            // JSON result in `data` variable
            var event_datax = JSON.parse(JSON.stringify(data));
            //alert(event_datax);
            var jsonData = JSON.parse(event_datax);
            //alert(jsonData);
            //alert(jsonData.length);
            for (var i = 0; i < jsonData.length; i++) {
                var eventx = jsonData[i];
                var year = eventx.bookyear;
                var month = eventx.bookmonth;
                var booktype = eventx.booktype;
                var name = eventx.booktitle;
                var day = eventx.bookday;
                var stime = eventx.booktime;
                var count = eventx.bookvisitors;
                var sdate = ' ' + year + '-' + month + '-' + day;
                var date = new Date(sdate);
                if (booktype == 1) {
                    new_event_json(booktype, name, count, date, day, stime);
                    var toDay = new Date();
                    day = toDay.getDay();
                    date.setDate(day);
                    init_calendar(toDay);
                }
            }
            //alert("Ok");
        });
    }

    // Initialize the calendar by appending the HTML dates
    function init_calendar(date) {
        $(".tbody").empty();
        $(".events-container").empty();
        var calendar_days = $(".tbody");
        var month = date.getMonth();
        var year = date.getFullYear();
        var day_count = days_in_month(month, year);
        var row = $("<tr class='table-row'></tr>");
        var today = date.getDate();
        // Set date to 1 to find the first day of the month
        date.setDate(1);
        var first_day = date.getDay();
        // 35+firstDay is the number of date elements to be added to the dates table
        // 35 is from (7 days in a week) * (up to 5 rows of dates in a month)
        for (var i = 0; i < 35 + first_day; i++) {
            // Since some of the elements will be blank, 
            // need to calculate actual date from index
            var day = i - first_day + 1;
            // If it is a sunday, make a new row
            if (i % 7 === 0) {
                calendar_days.append(row);
                row = $("<tr class='table-row'></tr>");
            }
            // if current index isn't a day in this month, make it blank
            if (i < first_day || day > day_count) {
                var curr_date = $("<td class='table-date nil'>" + "</td>");
                row.append(curr_date);
            }
            else {
                var curr_date = $("<td class='table-date'>" + day + "</td>");
                var events = check_events(day, month + 1, year);
                if (today === day && $(".active-date").length === 0) {
                    curr_date.addClass("active-date");
                    show_events(events, months[month], day,year);
                }
                // If this date has any events, style it with .event-date
                if (events.length !== 0) {
                    curr_date.addClass("event-date");
                }
                // Set onClick handler for clicking a date
                curr_date.click({ events: events, month: months[month], day: day,year: year }, date_click);
                row.append(curr_date);
            }
        }
        // Append the last row and set the current year
        calendar_days.append(row);
        $(".year").text(year);
    }

    // Get the number of days in a given month/year
    function days_in_month(month, year) {
        var monthStart = new Date(year, month, 1);
        var monthEnd = new Date(year, month + 1, 1);
        return (monthEnd - monthStart) / (1000 * 60 * 60 * 24);
    }

    // Event handler for when a date is clicked
    function date_click(event) {
        $(".events-container").show(250);
        $("#dialog").hide(250);
        $(".active-date").removeClass("active-date");
        $(this).addClass("active-date");
        show_events(event.data.events, event.data.month, event.data.day, event.data.year);
    };

    // Event handler for when a month is clicked
    function month_click(event) {
        $(".events-container").show(250);
        $("#dialog").hide(250);
        var date = event.data.date;
        $(".active-month").removeClass("active-month");
        $(this).addClass("active-month");
        var new_month = $(".month").index(this);
        date.setMonth(new_month);
        init_calendar(date);
    }

    // Event handler for when the year right-button is clicked
    function next_year(event) {
        $("#dialog").hide(250);
        var date = event.data.date;
        var new_year = date.getFullYear() + 1;
        $("year").html(new_year);
        date.setFullYear(new_year);
        init_calendar(date);
    }

    // Event handler for when the year left-button is clicked
    function prev_year(event) {
        $("#dialog").hide(250);
        var date = event.data.date;
        var new_year = date.getFullYear() - 1;
        $("year").html(new_year);
        date.setFullYear(new_year);
        init_calendar(date);
    }

    // Event handler for clicking the new event button
    function new_event(event) {
        // if a date isn't selected then do nothing
        if ($(".active-date").length === 0)
            return;
        // remove red error input on click
        $("input").click(function () {
            $(this).removeClass("error-input");
        })
        // empty inputs and hide events
        $("#dialog input[type=text]").val('');
        $("#dialog input[type=number]").val('');
        $(".events-container").hide(250);
        $("#dialog").show(250);
        // Event handler for cancel button
        $("#cancel-button").click(function () {
            $("#name").removeClass("error-input");
            $("#count").removeClass("error-input");
            $("#dialog").hide(250);
            $(".events-container").show(250);
        });
        // Event handler for ok button
        $("#ok-button").unbind().click({ date: event.data.date }, function () {
            var date = event.data.date;
            var name = $("#name").val().trim();
            var count = parseInt($("#count").val().trim());
            var day = parseInt($(".active-date").html());
            var stime = "1000";
            var booktype = 1;
            // Basic form validation
            if (name.length === 0) {
                $("#name").addClass("error-input");
            }
            else if (isNaN(count)) {
                $("#count").addClass("error-input");
            }
            else {
                $("#dialog").hide(250);
                console.log("new event");
                new_event_json(booktype, name, count, date, day, stime);
                date.setDate(day);
                init_calendar(date);
            }
        });
    }

    function myFunc() {
        alert("Hello!");
    }
    // Adds a json event to event_data
    function new_event_json(booktype, name, count, date, day, stime) {
        var event = {
            "booktype": booktype,
            "booktitle": name,
            "bookvisitors": count,
            "bookyear": date.getFullYear(),
            "bookmonth": date.getMonth() + 1,
            "bookday": day,
            "booktime": stime
        };
        event_data["events"].push(event);
    }

    // Display all events of the selected date in card views
    function show_events(events, month, day,year) {
        // Clear the dates container
        $(".events-container").empty();
        $(".events-container").show(250);
        console.log(event_data["events"]);
        var sMnth = 1;
        if (month == "January") sMnth = 1;
        else if (month == "February") sMnth = 2;
        else if (month == "Mar") sMnth = 3;
        else if (month == "April") sMnth = 4;
        else if (month == "May") sMnth = 5;
        else if (month == "June") sMnth = 6;
        else if (month == "July") sMnth = 7;
        else if (month == "August") sMnth = 8;
        else if (month == "September") sMnth = 9;
        else if (month == "October") sMnth = 10;
        else if (month == "November") sMnth = 11;
        else if (month == "December") sMnth = 12;
        //alert(month);
        //alert(sMnth);
        //alert(year)
        // If there are no events for this date, notify the user
        if (events.length === 0) {
            var event_card = $("<div class='event-card'></div>");
            var event_name = $("<div class='event-name'>Select your timing for " + month + " " + day + " " + year + ".</div> <br/><br/>");
            var event_buttons = $("<div style='margin-left: 10px;'><div class='button-group-area' id='btngrpx'> " +
                "<input type='button' value='10:00AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1000'  name='but1000' > " +
                "<input type='button' value='10:30AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1030' name='but1030' > " +
                "<input type='button' value='11:00AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1100' name='but1100' > " +
                "<input type='button' value='11:30AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1130' name='but1130' > " +
                "<input type='button' value='12:00PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1200' name='but1200' > " +
                "<input type='button' value='12:30PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1230' name='but1230' > " +
                "<input type='button' value='01:00PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1300' name='but1300' > " +
                "<input type='button' value='01:30PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1330' name='but1330' > " +
                "</div ></div > ");

            $(event_card).css({ "border-left": "10px solid #FF1744" });
            $(event_card).append(event_name);
            $(event_card).append(event_buttons);
            $(".events-container").append(event_card);
        }
        else {
            //// Go through and add each event as a card to the events container
            //for (var i = 0; i < events.length; i++) {
            //    var event_card = $("<div class='event-card'></div>");
            //    var event_name = $("<div class='event-name'>" + events[i]["occasion"] + ":</div>");
            //    var event_count = $("<div class='event-count'>" + events[i]["invited_count"] + " Invited</div>");
            //    if (events[i]["cancelled"] === true) {
            //        $(event_card).css({
            //            "border-left": "10px solid #FF1744"
            //        });
            //        event_count = $("<div class='event-cancelled'>Cancelled</div>");
            //    }
            //    $(event_card).append(event_name).append(event_count);
            //    $(".events-container").append(event_card);
            //}
            //alert(events.length);
            var strTimingSched = "<div style='margin-left: 10px;'><div class='button-group-area' id='btngrpx'>";
            var s1000, s1030, s1100, s1130, s1200, s1230, s1300, s1330, s1400, s1430;
            s1000 = 1;
            s1030 = 1;
            s1100 = 1;
            s1130 = 1;
            s1200 = 1;
            s1230 = 1;
            s1300 = 1;
            s1330 = 1;
            s1400 = 1;
            s1430 = 1;
            for (var i = 0; i < events.length; i++) {
                var dsTime = events[i].booktime;
                //alert(dsTime);
                if (dsTime == "1000") s1000 = 0;
                else if (dsTime == "1030") s1030 = 0;
                else if (dsTime == "1100") s1100 = 0;
                else if (dsTime == "1130") s1130 = 0;
                else if (dsTime == "1200") s1200 = 0;
                else if (dsTime == "1230") s1230 = 0;
                else if (dsTime == "1300") s1300 = 0;
                else if (dsTime == "1330") s1330 = 0;
                else if (dsTime == "1400") s1400 = 0;
                else if (dsTime == "1430") s1430 = 0;
            }
            if (s1000 == 0) strTimingSched = strTimingSched + "<input type='button' value='10:00AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1000'  name='but1000' disabled > ";
            else strTimingSched = strTimingSched + "<input type='button' value='10:00AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1000'  name='but1000' > " ;
            if (s1030 == 0) strTimingSched = strTimingSched + "<input type='button' value='10:30AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1030'  name='but1030' disabled > ";
            else strTimingSched = strTimingSched + "<input type='button' value='10:30AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1030'  name='but1030' > ";
            if (s1100 == 0) strTimingSched = strTimingSched + "<input type='button' value='11:00AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1100'  name='but1100' disabled > ";
            else strTimingSched = strTimingSched + "<input type='button' value='11:00AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1100'  name='but1100' > ";
            if (s1130 == 0) strTimingSched = strTimingSched + "<input type='button' value='11:30AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1130'  name='but1130' disabled > ";
            else strTimingSched = strTimingSched + "<input type='button' value='11:30AM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1130'  name='but1130' > ";
            if (s1200 == 0) strTimingSched = strTimingSched + "<input type='button' value='12:00PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1200'  name='but1200' disabled > ";
            else strTimingSched = strTimingSched + "<input type='button' value='12:00PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1200'  name='but1200' > ";
            if (s1230 == 0) strTimingSched = strTimingSched + "<input type='button' value='12:30PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1230'  name='but1230' disabled > ";
            else strTimingSched = strTimingSched + "<input type='button' value='12:30PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1230'  name='but1230' > ";
            if (s1300 == 0) strTimingSched = strTimingSched + "<input type='button' value='01:00PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1300'  name='but1300' disabled > ";
            else strTimingSched = strTimingSched + "<input type='button' value='01:00PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1300'  name='but1300' > ";
            if (s1330 == 0) strTimingSched = strTimingSched + "<input type='button' value='01:30PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1330'  name='but1330' disabled > ";
            else strTimingSched = strTimingSched + "<input type='button' value='01:30PM' onclick='setTiming(this.value," + day + "," + year + "," + sMnth + ")' id='but1330'  name='but1330' > ";


            strTimingSched = strTimingSched + "</div ></div >";

            var event_card = $("<div class='event-card'></div>");
            var event_name = $("<div class='event-name'>Select your timing for " + month + " " + day + ".</div> <br/><br/>");
            var event_buttonsx = $(strTimingSched);

            $(event_card).css({ "border-left": "10px solid #FF1744" });
            $(event_card).append(event_name);
            $(event_card).append(event_buttonsx);
            $(".events-container").append(event_card);
        }
    }

    // Checks if a specific date has any events
    function check_events(day, month, year, stime) {
        var events = [];
        for (var i = 0; i < event_data["events"].length; i++) {
            var event = event_data["events"][i];
            if (event["bookday"] === day &&
                event["bookmonth"] === month &&
                event["bookyear"] === year) {
                events.push(event);
            }
        }
        return events;
    }


    const months = [
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December"
    ];

})(jQuery);
