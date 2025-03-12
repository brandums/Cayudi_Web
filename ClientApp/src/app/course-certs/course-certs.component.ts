import { Component, Input, OnInit } from '@angular/core';
import { CertData } from '../models/certData';
import { CertService } from '../Services/cert.service';
import { fileDataDTO } from '../models/fileDataDTO';

@Component({
  selector: 'app-course-certs',
  templateUrl: './course-certs.component.html',
  styleUrls: ['./course-certs.component.css']
})
export class CourseCertsComponent implements OnInit {
  @Input() courseID = 0;
  @Input() isAdmin = false;

  certs: CertData[] = [];
  certIdToDelete = 0;
  fileInfo_images: fileDataDTO;

  constructor(private certService: CertService)
  {
    this.fileInfo_images = new fileDataDTO(0, 2, 4, 0, 'image/*')
  }

  ngOnInit(): void {
    this.getCerts();

    this.certService.$certsList.subscribe(data => {
      this.certs = data.map((cert: CertData) => ({
        ...cert,
        splitedReq: cert.requeriments.split(',')
      }));
    })
  }

  getCerts() {
    this.certService.getAllCertByCourseID(this.courseID).subscribe(data => {
      this.certs = data.map((cert: CertData) => ({
        ...cert,
        splitedReq: cert.requeriments.split(',')
      }));
    })
  }

  selectedCert(certId: number): void {
    this.certIdToDelete = certId;
  }

  deleteCert() {
    console.log("el id del cert es: ", this.certIdToDelete);
    this.certService.deleteCert(this.certIdToDelete).subscribe(() => {
      this.certService.newDataAdded(this.courseID)
    })
  }
}
