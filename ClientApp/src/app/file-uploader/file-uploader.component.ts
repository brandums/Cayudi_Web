import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { fileDataDTO } from '../models/fileDataDTO';
import { FileUploaderService } from '../Services/file-uploader-service';
import { LoginService } from '../Services/login.service';
import { UserService } from '../Services/user-service';
import { VideoService } from '../Services/video.service';
import { PDFService } from '../Services/pdf-service';
import { CourseCategoryService } from '../Services/course-category-service';
import { CourseService } from '../Services/course-service';
import { CourseCreationService } from '../Services/course-creation-service';
import { CertService } from '../Services/cert.service';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.css']
})
export class FileUploaderComponent implements OnInit{
  @Input() fileInfo: fileDataDTO = new fileDataDTO(0, 0, 0, 0, '');
  @Input() imageType: number = 0;
  @Input() listSourceID: number = 0;

  selectedFiles?: FileList;
  currentFile?: File;
  filename: string = "";
  loading: boolean = true;
  waiting: boolean = false;

  progress: number = 0;
  @ViewChild('#uopload_btn') _upload_btn!: ElementRef;



  constructor(private fileService: FileUploaderService, private videoService: VideoService,
    private loginService: LoginService, private pdfServ: PDFService, private uService: UserService,
    private courseCategoryService: CourseCategoryService, private courseService: CourseService,
    private courseCreateServ: CourseCreationService, private certService:CertService ) { }

  ngOnInit(): void {
    this.loading  = true;
    this.waiting = false;
  }

  captureFile(event: any ): any
  {
    const capturedFile = event.target.files[0];

    //Verifying if file type matches with captured file type
    if (this.fileInfo.imputType == 'image/*')
    {
      const rImage = new RegExp('image')
      if (!rImage.test(capturedFile.type))
      {
        return;
      }
    }
    else if (capturedFile.type != this.fileInfo.imputType)
    {
      return;
    }

    if (this.listSourceID != 0) {
      this.fileInfo.sourceId =  this.listSourceID
    }

    const total = event.total
    const formData = new FormData();
    formData.append("sourceID", this.fileInfo.sourceId.toString());
    formData.append("sourceType", this.fileInfo.objectType.toString());
    formData.append("courseSource", this.fileInfo.courseSourceType.toString());
    formData.append("imageType", this.fileInfo.imgType.toString());
    formData.append("file", capturedFile);
    this.changeIconStatus();
    this.uploadData(formData, total);
  }

  uploadData(formData: FormData, total: any) {
    this.fileService.uploadFile(formData).subscribe((event: HttpEvent<any>) => {
      switch (event.type) {
        case HttpEventType.Sent:
          console.log('Request has been made!');
          break;
        case HttpEventType.ResponseHeader:
          console.log('Response header has been received!');
          this.changeIconStatus()
          break;
        case HttpEventType.UploadProgress:
          this.progress = Math.round(event.loaded / total * 100);
          console.log(`Uploaded! ${this.progress}%`);
          break;
        case HttpEventType.Response:
          console.log('User successfully created!', event.body);
          this.warnNewData()
          setTimeout(() => {
            this.progress = 0;
          }, 1500);
      }
    });
  }

  warnNewData()
  {
    if (this.fileInfo.objectType == 1) {
      this.loginService.checkAuthentication()
      this.uService.updateImagesList(this.fileInfo.sourceId);
    }
    else if (this.fileInfo.objectType == 2) {
      if (this.fileInfo.courseSourceType == 1) {
        this.videoService.updateList(this.fileInfo.sourceId)
      }
      else if (this.fileInfo.courseSourceType == 2) {
        this.courseService.getCourseByID(this.fileInfo.sourceId).subscribe(
          data => {
            this.courseService.newDataAdded(data.courseCategoryID);
          })

        this.courseCreateServ.emitChanges()
      }
      else if (this.fileInfo.courseSourceType == 3) {
        this.pdfServ.updateList(this.fileInfo.sourceId);
      } else if (this.fileInfo.courseSourceType == 4) {
        this.certService.getUserByID(this.listSourceID).subscribe(
          data => {
            this.certService.newDataAdded(data.courseID);
          })

      }

    } else if (this.fileInfo.objectType == 3)
    {
      this.courseCategoryService.newDataAdded()
    }
  }

  changeIconStatus() {
    if (this.loading) {
      this.loading = false;
      this.waiting = true;
    } else {
      this.loading = true;
      this.waiting = false;
    }
  }
}
