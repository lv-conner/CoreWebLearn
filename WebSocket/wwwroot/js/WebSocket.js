
(function () {

    console.log("hello");
}());


function openWebsocket(e) {
    var socket = new WebSocket("ws://localhost:17230/WebSocket/Index");
    socket.onopen = function (e, state) {
        socket.send("Hello Socket");
        console.log("open");
    }
    socket.onclose = function (e, state) {
        console.log("close");
    }
    socket.onmessage = function (msg) {
        console.log(msg);
        console.log(msg.data);
        socket.send("next message");
    }
    socket.onerror = function (e, state) {
        console.log(e);
        console.log(state);
    }
    console.log("click");
}