import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
providedIn: 'root'
})
export class PostAnswerLikesService
{
  private apiUrl = environment.apiUrl+'PostAnswerLikes';

  constructor(private http: HttpClient) { }

  getPostAnswerLikesByID(id: number)
  {
    return this.http.get<any[]>(`${ this.apiUrl}/${ id}`).pipe(
      map((response: any) => response.result)
    );
  }

  createPostAnswerLikes(postLike: any) : Observable<any> {
    return this.http.post(this.apiUrl, postLike);
  }

  deletePostAnswerLikes(id: number) : Observable<any> {
    return this.http.delete(`${this.apiUrl}/${ id}`);
  }
}
