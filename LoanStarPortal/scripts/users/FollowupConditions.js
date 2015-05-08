$(document).ready(function () {
    // accordion
    $("#accordion").accordion();

    //$("#dialog_followup_conditions").dialog({
    //    autoOpen: false,
    //    width: 400,
    //    close: function (event, ui) {
    //        $("#dialog_followup_conditions").dialog("close");
    //    }
    //});

    //$("#dialog_followup_conditions").html($("#modal_followup_conditions").html());
    //$("#modal_followup_conditions").html('');

    //$(".tdLinkToEdit a").on('click', function (event) {
    //    window.location = $(this).attr("href");
    //    $("#dialog_followup_conditions").show();
    //    return false;
    //});

    //var hdnCompleteds = $(".row_completed");
    //$.each(hdnCompleteds, function (index, item) {
    //    var diffDays = $(item).parent().find(".row_diffdays");

    //    if ($(item).val() == 'True') {
    //        $(item).parent().css("background-color", "white");
    //        $(item).parent().siblings('td').css("background-color", "white");
    //    }

    //    if ($(diffDays).val() == "-1") {
    //        $(item).parent().css("background-color", "coral");
    //        $(item).parent().siblings('td').css("background-color", "coral");
    //    }

    //    if ($(item).val() != "True" && $(diffDays).val() != "-1") {
    //        $(item).parent().css("background-color", "deepskyblue");
    //        $(item).parent().siblings('td').css("background-color", "deepskyblue");
    //    }
    //});

    $('#Tabs_CtrlTasks1_FollowupConditions1_gridConditions_ctl01').contextMenu({
        selector: 'tr',
        callback: function (key, options) {
            var id = $(this).find(".row_id").val();

            $("#conditionActiveID").val(id);

            if (key == "Advance") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/Advance.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-Everyday") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyEveryday.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-EveryOtherDay") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyEveryOtherDay.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-OnceWeek") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyOnceWeek.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-EveryOtherWeek") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyEveryOtherWeek.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Frequency-OnceMonth") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/FrequencyOnceMonth.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
            else if (key == "Completed") {
                $.ajax({
                    type: "POST",
                    url: "/Handlers/Completed.ashx",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        AjaxNS.AR('Tabs$CtrlTasks1$FollowupConditions1$btnRefresh', '', 'RadAjaxManager1', event);
                        return false;
                    },
                    error: function () {
                        alert("Error");
                    }
                });

                return false;
            }
        },
        items: {
            "Advance": { name: "Advance to the next follow-up date" },
            "Frequency": {
                name: "Change the follow-up frequency",
                items: {
                    "Frequency-Everyday": { name: "Everyday" },
                    "Frequency-EveryOtherDay": { name: "Every other day" },
                    "Frequency-OnceWeek": { name: "Once a week" },
                    "Frequency-EveryOtherWeek": { name: "Every other week" },
                    "Frequency-OnceMonth": { name: "Once a month" }
                }
            },
            "Completed": { name: "Change status to completed" }
        }
    });
});