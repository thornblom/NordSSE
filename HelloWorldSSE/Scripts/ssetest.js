function main() {
    var eventURL = location.protocol + "//" + location.host + "/SSE";
    console.log(eventURL);

    var evtSrc = new EventSource(eventURL);

    evtSrc.onmessage = function (msg) {
        listWrite(msg.data);
    }
}

function listWrite(data) {
    $("#messageHolder").append("<li>" + data + "</li>");
}

$(document).ready(main);