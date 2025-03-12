import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BlogPostAnswerService {
  private apiUrl = 'BlogPostAnswer';

  constructor(private http: HttpClient) { }

  getBlogPostAnswer() {
    return this.http.get<any[]>(`${environment.apiUrl}${this.apiUrl}`).pipe(
      map((response: any) => response.result)
    );
  }

  getBlogPostAnswerByID(id: number) {
    return this.http.get<any[]>(`${environment.apiUrl}${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  createBlogPostAnswer(blogPostAnswer: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}${this.apiUrl}`, blogPostAnswer);
  }
}
