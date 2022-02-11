var host = 'localhost:5001';

$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
         .withUrl(`https://${host}/notifications`)
        .configureLogging(signalR.LogLevel.Information)
        .build();

    async function start() {
        try {
            await connection.start();
            $('#statusWebSocket').addClass("label label-success");
            $('#statusWebSocket').html('SignalR conectado');            
            const connectionid = await connection.invoke("getconnectionid");
            $('#connectionid').html(connectionid);
            console.log(`getconnectionid: ${connectionid}`);
            console.log(`SignalR connected to ${host}`);
        } catch (err) {
            console.log(err);
            $('#statusWebSocket').addClass("label label-warning");
            $('#statusWebSocket').html('Erro ao conectar no SignalR');            
            setTimeout(start, 5000);
        }
    };    

    // Start the connection.
    start();

    connection.on("notify", (message) => {
        const li = document.createElement("li");
        //li.textContent = `${user}: ${message}`;
        li.textContent = `${message}`;
        document.getElementById("message").appendChild(li);
    });

    connection.onreconnecting(error => {
        console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
    
        //document.getElementById("messageInput").disabled = true;
    
        const li = document.createElement("li");
        li.textContent = `Connection lost due to error "${error}". Reconnecting.`;
        document.getElementById("messagesList").appendChild(li);
    });

    // connection.onclose(async () => {
    //     $('#statusWebSocket').html('SignalR desconectado');
    //     $('#statusWebSocket').addClass("label label-danger");
    //     await start();
    // });

    connection.onclose(async (error) => {
    //connection.onclose(error => {
        $('#statusWebSocket').html('SignalR desconectado');
        $('#statusWebSocket').addClass("label label-danger");
        await start();

        console.assert(connection.state === signalR.HubConnectionState.Disconnected);
    
        //document.getElementById("messageInput").disabled = true;
    
        const li = document.createElement("li");
        li.textContent = `Connection closed due to error "${error}". Try refreshing this page to restart the connection.`;
        document.getElementById("messagesList").appendChild(li);
    });
});
