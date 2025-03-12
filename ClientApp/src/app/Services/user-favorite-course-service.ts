import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserFavoriteCourseService {
  private apiUrl = environment.apiUrl +'UserFavoriteCourse';

  constructor(private http: HttpClient) { }

  getAllByUser(id: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  getByUserAndCourse(userID: number, courseID: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${userID}/${courseID}`).pipe(
      map((response: any) => response.result)
    );
  }

  createUserFavoriteCourse(userFavoriteCourse: any): Observable<any> {
    return this.http.post(this.apiUrl, userFavoriteCourse);
  }

  deleteUserFavoriteCourse(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
