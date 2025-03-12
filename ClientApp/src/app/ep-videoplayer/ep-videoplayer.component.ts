import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { fileDataDTO } from '../models/fileDataDTO';
import { VideoService } from '../Services/video.service';
import { LoginService } from '../Services/login.service';
import { HttpEvent, HttpEventType } from '@angular/common/http';


@Component({
  selector: 'app-ep-videoplayer',
  templateUrl: './ep-videoplayer.component.html',
  styleUrls: ['./ep-videoplayer.component.css']
})
export class EpVideoplayerComponent implements OnInit {
  @Input() courseId: number = 0;
  @Output() videoName = new EventEmitter<string>();
  @Input() fileInfo: fileDataDTO;

  current: any = ""
  deletedFile: any = ""

  subscription: Subscription = new Subscription;


  hasVideos: boolean = true;
  videoItems: any = [
    {
    id:'',
    name: '',
    src: '',
    type: ''
    },];

  videoItemsUpdated: any = [
    {
      id: '',
      name: '',
      src: '',
      type: ''
    },];

  activeIndex = 0;
  fileType = "video/mp4"
  currentVideo: any;
  videos: any[] = [];
  user: any;

  constructor(
    private videoService: VideoService,
    private loginService: LoginService
  ) {
    this.fileInfo = new fileDataDTO(this.courseId, 2, 1, 0, 'video/mp4')
  }

  ngOnInit(): void {
    this.subscription = this.videoService.getVideosByCourse(this.courseId).subscribe(data => {
      this.videoItems = data;
    });
    this.currentVideo = this.videoItems[this.activeIndex];
    this.fileInfo.sourceId = this.courseId

    this.loginService.$userData.subscribe(user => {
      this.user = user.userData;
    })
  }

   
  startPlaylistVdo(item: any, index: number) {
    this.activeIndex = index;
    this.currentVideo = item;
    this.videoName.emit(item.name)
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  delete() {

    if (this.current != 0) {
      try {
        this.videoService.delete(this.current).subscribe((event: HttpEvent<any>) => {
          switch (event.type) {
            case HttpEventType.Sent:
              console.log('Request has been made!');
              break;
            case HttpEventType.ResponseHeader:
              console.log('Response header has been received!');
              break;
            case HttpEventType.Response:
              console.log('User successfully deleted!', event.body);
              this.videoService.updateList(this.courseId)
              setTimeout(() => {
              }, 1500);
          }
        })
      }
      catch (e) {
        console.log("Errror while trying to switch read only value", e)
      }
    }
  }

  openConfModal(index: any) {
    this.current = this.videoItems[index].id
    this.deletedFile = this.videoItems[index].name
  }
}
