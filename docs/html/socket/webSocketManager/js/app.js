var host = 'localhost:5001';

$(document).ready(function () {
    var uri = `wss://${host}/notifications`;
    var connection = new WebSocketManager.Connection(uri);
    connection.enableLogging = true;

    connection.connectionMethods.onConnected = () => {
        $('#statusWebSocket').html('WebSocket conectado');
        $('#statusWebSocket').addClass("label label-success");
        console.log(`opened connection to ${uri}`);
    }

    connection.connectionMethods.onDisconnected = () => {
        $('#statusWebSocket').html('WebSocket desconectado');
        $('#statusWebSocket').addClass("label label-warning");
        console.log(`closed connection from ${uri}`);

        setInterval(connection.start(), 3000);
    };

    connection.clientMethods["webSocketMessage"] = (response) => {

        var html = '';
        console.log(response);

        try {
            html = `<p><span class="label label-info">${response.id} - ${response.value}</span></p>`;
        } catch (err) {
            html = `<span class="label label-info">${err.message}</span>`;
        }

        $('#message').html(html);
    };

    connection.onerror = function (evt) {
        $('#statusWebSocket').html(`WebSocket erro:${evt.message}`);
        $('#statusWebSocket').addClass("label label-warning");

        console.log(`${evt.message}`);

        setInterval(connection.start(), 3000);
    };

    connection.start();
});
