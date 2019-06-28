import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import io from 'socket.io-client';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

//  data: Observable<any>;

const socket = io('http://localhost:3020');

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) {
    // socket.on('message', function(data){
    //   console.log(data);
    // });
  }

  getStreamData() {
    let observable = new Observable(observer => {
      socket.on('message', data => {
        observer.next(data);
      });
      return () => {
        socket.disconnect();
      };
    });
    return observable;
  }

  retrieveKafkaData() {}
}
