import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { UserCourse } from '../models/userCourse';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserCourseService {
  private apiUrl = environment.apiUrl + 'UserCourse';

  constructor(private http: HttpClient) { }

  getAllUserCourseByUserID(userID: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${userID}`).pipe(
      map((response: any) => response.result)
    );
  }

  getAllUsersByCourseID(courseID: number): Observable<any> {
    return this.http.get<any[]>(`${this.apiUrl}/getAllUsersByCourseID/${courseID}`).pipe(
      map((response: any) => response.result)
    );
  }

  getUserCourseByID(courseID: number, userID: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${courseID}/${userID}`).pipe(
      map((response: any) => response.result)
    );
  }

  getCourseRating(courseID: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/getRating/${courseID}`).pipe(
      map((response: any) => response.result)
    );
  }

  getEndTime(courseID: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/GetEndTime/${courseID}`).pipe(
      map((response: any) => response.result)
    );
  }

  closeCourse(courseID: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/closeCourse/${courseID}`).pipe(
      map((response: any) => response.result)
    );
  }

  createUserCourse(userCourse: UserCourse): Observable<any> {
    return this.http.post(this.apiUrl, userCourse);
  }

  updateUserCourse(userCourseId: number, patchData: any[]): Observable<any> {
    const apiUrl = `${this.apiUrl}/${userCourseId}`;

    return this.http.patch(apiUrl, patchData);
  }
}
