﻿
(function () {

    console.log("hello");
}());


function openWebsocket(e) {
    var socket = new WebSocket("ws://localhost:17230/WebSocket/Index");
    socket.onopen = function (e, state) {
        //socket.send("Hello Socket");
        console.log("open");
    }
    socket.onclose = function (e, state) {
        console.log(e);
        console.log(state);
        console.log("close");
    }
    socket.onmessage = function (msg) {
        //onmessage中不应该发送消息。如果此时服务端回发消息，会导致死循环
        console.log(msg);
        console.log(msg.data);
        //socket.send("next message");
    }
    socket.onerror = function (e) {
        console.log(e);
    }
    window.socket = socket;
    console.log("click");
}

function sendFile() {
    if (window.socket === null)
        return;
    var file = document.querySelector("input[type='file']").files[0];
    if (file === undefined || file === null) {
        return;
    }
    window.socket.send(file);
}
