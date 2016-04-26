function main() {
    var eventURL = location.protocol + "//" + location.host + "/api/SSE";
    console.log(eventURL);

    var evtSrc = new EventSource('/api/SSE');

    evtSrc.onmessage = function (msg) {
        console.log("recieved: " + msg.data);
        listWrite(msg.data);
    }
    evtSrc.onerror = function () {
        console.log("error");
    }
}

function listWrite(data) {
    $("#messageHolder").append("<li>" + data + "</li>");
}

$(document).ready(main);