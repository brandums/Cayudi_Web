import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment'


@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'User';
  public _images: BehaviorSubject<any[]>;

  constructor(private http: HttpClient) {
    this._images = new BehaviorSubject<any[]>([]);
  }

  getAllUser() {
    return this.http.get<any[]>(`${environment.apiUrl}${this.apiUrl}`).pipe(
      map((response: any) => response.result)
    );
  }

  getUserByID(id: number) {
    return this.http.get<any[]>(`${environment.apiUrl}${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  createUser(user: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}${this.apiUrl}`, user);
  }

  calculateUserInitials(name: string): string {
    const nameParts = name.split(' ');
    const initials = nameParts.map(part => part[0].toUpperCase()).join('');
    return initials;
  }

  updateUser(userID: number, patchData: any[]): Observable<any> {
    const apiUrl = `${environment.apiUrl}${this.apiUrl}/${userID}`;

    return this.http.patch(apiUrl, patchData);
  }

  updatePassword(userID: number, patchData: any): Observable<any> {
    const apiUrl = `${environment.apiUrl}${this.apiUrl}/password/${userID}`;

    return this.http.patch(apiUrl, patchData);
  }

  sendConfirmationToChangePassword(email: string): Observable<any> {
    const apiUrl = `${environment.apiUrl}${this.apiUrl}/${email}`;

    return this.http.get<any>(apiUrl);
  }

  getImages(id: number): Observable<any> {
    return this.http.get<any[]>(`${environment.apiUrl}${this.apiUrl}/GetImagesByUser/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  updateImagesList(id: number) {
    this.getImages(id).subscribe(data => {
      this._images.next(data)
    })    
  }

  deleteImage(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}${this.apiUrl}/deleteImage/${id}`, {
      reportProgress: true,
      observe: 'events'
    })
  }
}
