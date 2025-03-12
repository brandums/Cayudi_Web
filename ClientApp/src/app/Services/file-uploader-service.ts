import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http'
import { Observable, map, catchError, throwError } from 'rxjs'
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileUploaderService {
  private url = environment.apiUrl + 'FilesHandler/UpdateFile'

  //${ environment.apiUrl }
  constructor(
    private http: HttpClient
  ) {}

  public uploadFile(fileData: FormData): Observable<any>{
    return this.http.post<any>(`${environment.apiUrl}FilesHandler`, fileData,{
      reportProgress: true,
      observe: 'events'
    }).pipe(
      catchError(this.error)
    )
  }


  updateFile(fileData: FormData): Observable<any> {
    return this.http.put(`${this.url}`, fileData).pipe(map((resp: any) => resp));

  }

  error(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
