import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, map, BehaviorSubject, throwError, catchError } from 'rxjs';
import { environment } from 'src/environments/environment'

@Injectable({
  providedIn: 'root'
})
export class PDFService {
  private apiUrl = 'PDFiles/';
  private fileList: any[];
  private _files: BehaviorSubject<any[]>;
  private defFile = {
    id : 0,
    tittle: 'Vacio',
    src: '',
    type: ''
  }
  constructor(
    private http: HttpClient,
  ) {
    this.fileList = [];
    this._files = new BehaviorSubject<any[]>([]);
  }

  getList1(courseID: any) {
    return this.http.get<any[]>(`${environment.apiUrl}${this.apiUrl}GetByCourse/${courseID}`).pipe(
      map((response: any) => response.result),
      catchError(err => this.handleError(err))
    );
  }

  getByCourse(courseID: any): Observable<any[]> {
    this.cleanCache();
    this.updateList(courseID)
    this._files.next(this.fileList)
    return this._files;
  }

  updateList(courseID: any) {
    this.getList1(courseID).subscribe(data => {
      this.fileList = data;
      this._files.next(this.fileList)
    });
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}${this.apiUrl}${id}`, {
      reportProgress: true,
      observe: 'events'
    })
  }

  switchReadOnly(id: number): Observable<any>  {
    return this.http.put(`${environment.apiUrl}${this.apiUrl}switchReadOnly/${id}`,id).pipe(
      map((response: any) => response.result)
    );
  }

  cleanCache() {
    this.fileList = [this.defFile];
  }

  private handleError(error: HttpErrorResponse) {
    this.cleanCache()
    this._files.next(this.fileList)
    return throwError(() => new Error('Not videos found'));
  }
}
