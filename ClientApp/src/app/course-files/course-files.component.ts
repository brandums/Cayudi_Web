import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { CourseFilesData } from '../models/courseFilesData';
import { fileDataDTO } from '../models/fileDataDTO';
import { LoginService } from '../Services/login.service';
import { PDFService } from '../Services/pdf-service';
import { Subscription, catchError } from 'rxjs';
import { HttpEvent, HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-course-files',
  templateUrl: './course-files.component.html',
  styleUrls: ['./course-files.component.css']
})
export class CourseFilesComponent {
  files: any[] = [];
  @Input() fileInfo: fileDataDTO;
  @Input() courseId: number = 0;
  user: any;
  subscription: Subscription = new Subscription;
  current: any = ""
  deletedFile: any = ""

  defLink: any;

  @ViewChild('modalPDF') _modal!: ElementRef;
  constructor(private loginService: LoginService, private service: PDFService) {    
    this.fileInfo = new fileDataDTO(this.courseId, 2, 3, 0, 'application/pdf')
  }

  ngOnInit(): void {
    this.subscription = this.service.getByCourse(this.courseId).subscribe(data => {
      this.files = data;
    });
    this.fileInfo.sourceId = this.courseId

    this.loginService.$userData.subscribe(user => {
      this.user = user.userData;
    })
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  displayStyle = "none";
  openModal(index: any) {
    this.defLink = this.files[index].source;

    if (this.files[index].readOnly == 1) {
       this.defLink = `${this.defLink}#toolbar=0`
    }
   
    this._modal.nativeElement.setAttribute('src', this.defLink)   
    this.displayStyle = "block";
    
  }
   
  closeModal() {
    this.displayStyle = "none";
    this._modal.nativeElement.setAttribute('src', '')   
  }

  switchState(index: any) {
    var val = this.files[index].id
    var resp: any;
    if (val != 0) {
      try {
        this.service.switchReadOnly(val).subscribe(responce =>
          resp = responce
          )
      }
      catch (e) {
        console.log("Errror while trying to switch read only value", e)
      }
    }
  }

  delete() {
    if (this.current != 0) {
      try {
        this.service.delete(this.current).subscribe((event: HttpEvent<any>) => {
          switch (event.type) {
            case HttpEventType.Sent:
              console.log('Request has been made!');
              break;
            case HttpEventType.ResponseHeader:
              console.log('Response header has been received!');
              break;           
            case HttpEventType.Response:
              console.log('User successfully deleted!', event.body);
              this.service.updateList(this.courseId)
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
    this.current = this.files[index].id
    this.deletedFile = this.files[index].tittle
  }
}
