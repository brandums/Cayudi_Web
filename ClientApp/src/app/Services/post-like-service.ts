import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PostLikeService {
  private apiUrl = environment.apiUrl + 'PostLike';

  constructor(private http: HttpClient) { }

  getPostLikeByID(id: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${id}`).pipe(
      map((response: any) => response.result)
    );
  }

  createPostLike(postLike: any): Observable<any> {
    return this.http.post(this.apiUrl, postLike);
  }

  deletePostLike(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
