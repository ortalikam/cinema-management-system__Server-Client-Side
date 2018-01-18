// days arr
var days = new Array();
days[days.length] = "ראשון";
days[days.length] = "שני";
days[days.length] = "שלישי";
days[days.length] = "רביעי";
days[days.length] = "חמישי";
days[days.length] = "שישי";
days[days.length] = "שבת";

// convert dates
function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

    var offset = date.getTimezoneOffset() / 60;
    var hours = date.getHours();

    newDate.setHours(hours - offset);

    return newDate.toJSON().slice(0, 17) + "00";;
}