import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

interface Message {
  userName: string;
  text: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private _hubConnection: HubConnection | undefined;
  messages: Message[] = [];
  connectionId : string = '';
  messageControl = new FormControl('');
  userName: string = '';

  constructor() {
    this.startConnection();
   }

  ngOnInit(): void {
  }

  startConnection() {
    this._hubConnection = new HubConnectionBuilder()
    .withUrl('https://localhost:5001/notifications')
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();

    this._hubConnection.on("newMessage", (userName: string, message: string) => {
      this.messages.push({
        text: message,
        userName: userName
      });

      console.log('entrou');

      //this._hubConnection.start();

      //console.log('connection', this.connection);      
    });

    this._hubConnection.start()
      .then(() => console.log('connection started'))
      .then(() => this.getConnectionId())
      .catch((err) => console.log('error while establishing signalr connection: ' + err));
  }

  public getConnectionId = () => {
    if (this._hubConnection)
      this._hubConnection.invoke('getconnectionid').then(
        (data) => {
          console.log(data);
            this.connectionId = data;
          }
      ); 
  }

  sendMessage(): void {
    if (this._hubConnection)
      this._hubConnection.send("newMessage", this.userName, this.messageControl.value)
        .then(() => {
          this.messageControl.setValue('');
        });
  }
}
