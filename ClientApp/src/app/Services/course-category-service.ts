import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CourseCategoryService {
  private apiUrl = environment.apiUrl + 'CourseCategory';
  public $categoriesList = new BehaviorSubject<any[]>([]);
  constructor(private http: HttpClient) {

  }

  getCategory(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl).pipe(
      map((response: any) => response.result)
    );
  }

  getCategoryByID(id: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  createCategory(courseCategoryData: any): Observable<any> {
    return this.http.post(this.apiUrl, courseCategoryData);
  }

  deleteCategory(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result));
  }

  newDataAdded()
  {
    this.getCategory().subscribe(data => {
      this.$categoriesList.next(data);
    });
  }
}
