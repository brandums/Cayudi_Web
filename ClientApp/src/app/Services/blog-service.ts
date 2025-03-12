import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  private apiUrl = environment.apiUrl + 'Blog';

  constructor(private http: HttpClient) { }

  getBlogByID(id: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  createBlog(blog: any): Observable<any> {
    return this.http.post(this.apiUrl, blog);
  }
}
