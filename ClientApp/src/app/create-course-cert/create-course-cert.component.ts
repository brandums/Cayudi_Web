import { Component, Input } from '@angular/core';
import { CertData } from '../models/certData';
import { CertService } from '../Services/cert.service';


@Component({
  selector: 'app-create-course-cert',
  templateUrl: './create-course-cert.component.html',
  styleUrls: ['./create-course-cert.component.css']
})
export class CreateCourseCertComponent {
  @Input() courseID: number = 0;
  courseCert: CertData = new CertData;
  requeriments: string[] = [''];

  constructor(
    private certService: CertService,
  ) { }

  addRequeriment() {
    this.requeriments.push('');
  }

  createCert() {
    const requerimentsString = this.requeriments.filter(req => req.trim() !== '').join(',');

    if (this.courseCert) {
      this.courseCert.courseID = this.courseID;
      this.courseCert.requeriments = requerimentsString;
      this.certService.createCert(this.courseCert)
        .subscribe(
          (response: any) => {
            console.log('Cert creado exitosamente', response);
            this.certService.newDataAdded(this.courseID)
          },
          (error: any) => {
            console.error('Error al crear el test', error);
          }
        );
    }
  }
}
