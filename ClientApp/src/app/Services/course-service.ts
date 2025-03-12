import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, map, switchMap } from 'rxjs';
import { BlogService } from './blog-service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  private apiUrl = environment.apiUrl + 'Course';
  public nextCourses: BehaviorSubject<any[]>;
  public $coursesList: BehaviorSubject<any[]>

  constructor(
    private http: HttpClient,
    private blogService: BlogService
  ) {
    this.nextCourses = new BehaviorSubject<any[]>([])
    this.$coursesList = new BehaviorSubject<any[]>([])
  }

  getAllCourses() {
    return this.http.get<any[]>(this.apiUrl).pipe(
      map((response: any) => response.result)
    );
  }

  getCourseByID(id: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  getCoursesByCategory(id: number) {
    return this.http.get<any[]>(`${this.apiUrl}/GetByCategory/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  getCoursesByDate(date: Date) {
    const formattedDate = date.toISOString();
    return this.http.get<any[]>(`${this.apiUrl}/date/${formattedDate}`).pipe(
      map((response: any) => response.result)
    );
  }

  createCourse(courseData: any): Observable<any> {
    return this.http.post(this.apiUrl, courseData).pipe(
      switchMap((data: any) => {
        const blog = {
          CourseID: data.result.id
        };
        return this.blogService.createBlog(blog).pipe(
          map(() => data)
        );
      })
    );
  }

  updateCourse(courseId: number, patchData: any[]): Observable<any> {
    const apiUrl = `${this.apiUrl}/${courseId}`;

    return this.http.patch(apiUrl, patchData);
  }

  getNextMonthsCourses() {
    return this.http.get<any[]>(`${this.apiUrl}/GetNextMonthsCourses`).pipe(
      map((response: any) => response.result)
    );
  }

  public updateFutureCoursesList() {
    this.getNextMonthsCourses().subscribe(data => {
        this.nextCourses.next(data);
    });
  }

  deleteCourse(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`).pipe(
        map((response: any) => response.result)
      );
  }

  getCoursesBySearch(text: string): Observable<any> {
    return this.http.get<any[]>(`${this.apiUrl}/getBySearch/${text}`).pipe(
      map((response: any) => response.result)
    );
  }

  newDataAdded(catId = 0, text = "") {
    if (catId == 0) {
      if (text !== "") {
        this.getCoursesBySearch(text).subscribe(data => {
          this.$coursesList.next(data)
        })
      }
      else {
        this.getAllCourses().subscribe(data => {
          this.$coursesList.next(data)
        })
      }
    }
    else {
      this.getCoursesByCategory(catId).subscribe(data => {
        this.$coursesList.next(data)
      })
    }
  }
}


