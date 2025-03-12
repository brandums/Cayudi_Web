import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { CertData } from '../models/certData';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CertService {
  private apiUrl = environment.apiUrl + 'Cert';
  public $certsList = new BehaviorSubject<any[]>([]);

  constructor(private http: HttpClient) { }

  getAllCertByCourseID(courseID: number) {
    return this.http.get<any[]>(`${this.apiUrl}/GetByCourseID/${courseID}`).pipe(
      map((response: any) => response.result)
    );
  }

  createCert(cert: CertData): Observable<any> {
    return this.http.post(this.apiUrl, cert);
  }

  deleteCert(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  newDataAdded(courseID: number) {
    this.getAllCertByCourseID(courseID).subscribe(data => {
      this.$certsList.next(data);
    });
  }

  getUserByID(id: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }
}
