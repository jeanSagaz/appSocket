import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NameDialogComponent } from 'src/app/shared/name-dialog/name-dialog.component';

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
  userName!: string;

  constructor(public dialog: MatDialog,
    public snackBar: MatSnackBar) {
    this.openDialog();    
  }

  openMatSnackBar(userName: string) {
    const message = userName == this.userName ? 'Você entrou na sala' : `${userName} acabou de entrar`;
    this.snackBar.open(message, 'Fechar', {
      duration: 5000,
      horizontalPosition: 'right',
      verticalPosition: 'top'
    })
  }

  openDialog() {
    const dialogRef = this.dialog.open(NameDialogComponent, {
      width: '250px',
      data: this.userName,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      this.userName = result;
      this.startConnection();
      this.openMatSnackBar(result);
    });
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
    });

    this._hubConnection.on("newUser", (userName: string) => {
      this.openMatSnackBar(userName);
    });

    this._hubConnection.on("previousMessage", (messages: Message[]) => {
      this.messages = messages;
    });

    this._hubConnection.start()
      .then(() => { 
        console.log('connection started');
        //this._hubConnection.send("newUser", this.userName, this.connectionId);        
      })
      .then(() => this.getConnectionId())
      .catch((err: any) => console.log('error while establishing signalr connection: ' + err));
  }

  public getConnectionId = () => {
    if (this._hubConnection)
      this._hubConnection.invoke('getconnectionid').then(
        (data: any) => {
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
