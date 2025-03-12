import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { CourseTest } from '../models/CourseTest';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CourseTestService {
  private apiUrl = environment.apiUrl +'CourseTest';
  public $testList = new BehaviorSubject<any[]>([]);

  constructor(private http: HttpClient) { }

  getAllTestByCourseID(courseID: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${courseID}`).pipe(
      map((response: any) => response.result)
    );
  }

  createTest(courseTest: CourseTest): Observable<any> {
    return this.http.post(this.apiUrl, courseTest);
  }

  deleteCourseTest(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  newDataAdded(courseID: number) {
    this.getAllTestByCourseID(courseID).subscribe(data => {
      this.$testList.next(data);
    });
  }
}
