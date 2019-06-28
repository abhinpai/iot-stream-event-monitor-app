import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IStreamData } from 'src/app/models/IStreamData';
import { ApiService } from 'src/app/services/api.service';
import io from 'socket.io-client';
const socket = io('http://localhost:3020');
import { BehaviorSubject, Observable } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  @ViewChild('loginButton') loginButton: ElementRef;

  deviceList: IStreamData[];
  isDataPresent: boolean = false;
  dataData: object = {};

  device1: any = {
    name: 'Device 1',
    guid: 'b1738939-268d-41d1-ad67-6ac4cccb4f48',
    granted: 0,
    denayGrant: 0
  };

  device2: any = {
    name: 'Device 2',
    guid: 'bd2a5ef7-1013-4448-93e8-c73253161308',
    granted: 0,
    denayGrant: 0
  };

  device3: any = {
    name: 'Device 3',
    guid: 'ac95dc85-3f0c-4662-b4c4-08759f70ca6a',
    granted: 0,
    denayGrant: 0
  };

  public d1Granted: BehaviorSubject<number> = new BehaviorSubject<number>(0);

  abc = Observable.create(function(observer) {
    observer.onNext(0);
    observer.onCompleted();
    return function() {
      console.log('disposed');
    };
  });

  constructor(private apiSvc: ApiService) {
    let self = this;

    socket.on('message', function(data) {
      console.log(data);

      // let topic = data.topic.split('_')[0];
      // let type = data.topic.split('_')[1];
      // if (topic == self.device1.guid) {
      //   if (type == 'notgranted') {
      //     self.device1.denayGrant += 1;
      //   } else {
      //     self.device1.granted += 1;
      //     self.d1Granted.next(self.device1.granted);
      //   }
      //   console.log(self.d1Granted);
      // }

      // if (topic == self.device2.guid) {
      //   if (type == 'notgranted') {
      //     self.device2.denayGrant += 1;
      //   } else {
      //     self.device2.granted += 1;
      //   }
      //   console.log(self.device2.granted);
      // }

      // if (topic == self.device3.guid) {
      //   if (type == 'notgranted') {
      //     self.device3.denayGrant += 1;
      //   } else {
      //     self.device3.granted += 1;
      //   }
      //   console.log(self.device3);
      // }
    });
  }

  ngOnInit() {
    let self = this;
    this.apiSvc.getStreamData().subscribe((res: any) => {
      console.log(res);

      let topic = res.topic.split('_')[0];
      let type = res.topic.split('_')[1];
      if (topic == self.device1.guid) {
        if (type == 'notgranted') {
          self.device1.denayGrant += 1;
        } else {
          self.device1.granted += 1;
          self.d1Granted.next(self.device1.granted);
        }
        console.log(self.d1Granted);
      }

      if (topic == self.device2.guid) {
        if (type == 'notgranted') {
          self.device2.denayGrant += 1;
        } else {
          self.device2.granted += 1;
        }
        console.log(self.device2.granted);
      }

      if (topic == self.device3.guid) {
        if (type == 'notgranted') {
          self.device3.denayGrant += 1;
        } else {
          self.device3.granted += 1;
        }
        console.log(self.device3);
      }
    });

    // setInterval(() => {
    //  this.loginButton.nativeElement.click();
    // }, 2);
  }

  conter = 0;

  increase() {
    this.conter += 1;
  }
}
