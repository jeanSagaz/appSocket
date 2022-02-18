var host = 'localhost:5001';

$(document).ready(function () {

    function generateUUID() { // Public Domain/MIT
        var d = new Date().getTime(); //Timestamp
        var d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now()*1000)) || 0; //Time in microseconds since page-load or 0 if unsupported
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = Math.random() * 16; //random number between 0 and 16
            if(d > 0) { //Use timestamp until depleted
                r = (d + r)%16 | 0;
                d = Math.floor(d/16);
            } else { //Use microseconds since page-load if supported
                r = (d2 + r)%16 | 0;
                d2 = Math.floor(d2/16);
            }
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
    }

    const uuId = generateUUID();
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`https://${host}/notificationsQuery?sessionId=${uuId}`)
        .configureLogging(signalR.LogLevel.Information)
        .build();

    async function start() {
        try {
            await connection.start();
            $('#statusWebSocket').addClass("label label-success");
            $('#statusWebSocket').html('SignalR conectado');
            $('#messagesList').html('');
            
            const connectionid = await connection.invoke("getconnectionid");
            $('#connectionid').html(connectionid);
            $('#uuId').html(uuId);
            console.log(`getconnectionid: ${connectionid}`);
            console.log(`SignalR connected to ${host}`);
        } catch (err) {
            console.log('error', err);
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
    
        const li = document.createElement("li");
        li.textContent = `Connection lost due to error "${error}". Reconnecting.`;
        document.getElementById("messagesList").appendChild(li);
    });

    connection.onclose(async (error) => {
        $('#statusWebSocket').html('SignalR desconectado');
        $('#statusWebSocket').addClass("label label-danger");
        await start();

        console.assert(connection.state === signalR.HubConnectionState.Disconnected);
    
        const li = document.createElement("li");
        li.textContent = `Connection closed due to error "${error}". Try refreshing this page to restart the connection.`;
        document.getElementById("messagesList").appendChild(li);
    });
});
