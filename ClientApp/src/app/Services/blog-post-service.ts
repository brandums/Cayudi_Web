import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BlogPostService {
  private apiUrl = 'BlogPost';

  constructor(private http: HttpClient) { }

  getBlogPost() {
    return this.http.get<any[]>(`${environment.apiUrl}${this.apiUrl}`).pipe(
      map((response: any) => response.result)
    );
  }

  getBlogPostByID(id: number) {
    return this.http.get<any[]>(`${environment.apiUrl}${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  createBlogPost(blogPost: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}${this.apiUrl}`, blogPost);
  }
}
