function main() {
    var eventURL = location.protocol + "//" + location.host + "/api/SSE";
    console.log(eventURL);

    
}

function listWrite(data) {
    $("#messageHolder").append("<li>" + data + "</li>");
}

$(document).ready(main);

$("#connectionForm").on("submit", function (event) {
    event.preventDefault();

    var clientID = $("#connectionForm").find("input[name='clientID']").val();

    var evtSrc = new EventSource('/api/SSE?id='+ encodeURIComponent(clientID));

    evtSrc.onmessage = function (msg) {
        console.log("recieved: " + msg.data);
        listWrite(msg.data);
    }
    evtSrc.onerror = function () {
        console.log("error");
    }
});