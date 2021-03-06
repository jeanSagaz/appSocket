var host = 'localhost:5002';

$(document).ready(function () {
    var uri = `wss://${host}/notifications`;
    var connection = new WebSocketManager.Connection(uri);
    connection.enableLogging = true;

    connection.connectionMethods.onConnected = (id) => {
		$('#connectionid').html(connection.connectionId);
        $('#statusWebSocket').html('WebSocket conectado');
        $('#statusWebSocket').addClass("label label-success");
		
        console.log(`opened connection to ${uri}`);
		// connection.connectionMethods.invoke('GetConnectionId', (result, error) => {
			// if (result) {
				// console.log("The server's GetConnectionId returned: " + result);
				// alert(result);
				// $('#connectionid').html(result);
			// }
			// if (error) console.error("The server's GetConnectionId errored: " + error);
		// });
    }

    connection.connectionMethods.onDisconnected = () => {
        $('#statusWebSocket').html('WebSocket desconectado');
        $('#statusWebSocket').addClass("label label-warning");
        console.log(`closed connection from ${uri}`);

        setInterval(connection.start(), 3000);
    };

    connection.clientMethods["webSocketMessage"] = (response) => {
        var html = '';
        console.log('response', response);
		
		const li = document.createElement("li");

        try {
            html = `<p><span class="label label-info">${response.id} - ${response.value}</span></p>`;
        } catch (err) {
            html = `<span class="label label-info">${err.message}</span>`;
        }

		li.textContent = html;
        document.getElementById("message").appendChild(li);
    };

    connection.onerror = function (evt) {
        $('#statusWebSocket').html(`WebSocket erro:${evt.message}`);
        $('#statusWebSocket').addClass("label label-warning");

        console.log(`${evt.message}`);

        setInterval(connection.start(), 3000);
    };

    connection.start();
});
