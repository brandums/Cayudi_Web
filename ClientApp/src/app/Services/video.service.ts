import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, map, BehaviorSubject, catchError, throwError } from 'rxjs';
import { Video } from '../models/video';
import { environment } from 'src/environments/environment'


@Injectable({
  providedIn: 'root'
})
export class VideoService {
  private apiUrl = 'Video/';
  private videosList: Video[];
  private _videos: BehaviorSubject<Video[]>;
  private defVideo: Video = {
    id: 0,
    name: 'Vacio',
    src: 'empty',
    type: ''
  }
  constructor(
    private http: HttpClient,
  ) {
    this.videosList = []     
    this._videos = new BehaviorSubject<Video[]>([]);
  }

  getVideosList1(courseID: any){
    return this.http.get<Video[]>(`${environment.apiUrl}${this.apiUrl}GetByCourse/${courseID}`).pipe(
      map((response: any) => response.result),
      catchError(err => this.handleError(err))
    );
  }

  getVideosByCourse(courseID: any): Observable<Video[]> {
    this.cleanVideoListCache();
    this.updateList(courseID)
    this._videos.next(this.videosList)
    return this._videos;
  }

  updateList(courseID: any)
  {
    this.getVideosList1(courseID).subscribe(data => {
      this.videosList = data;      
      this._videos.next(this.videosList)
    })    
  }
   
  cleanVideoListCache() {
    this.videosList = [
      this.defVideo];
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}${this.apiUrl}${id}`, {
      reportProgress: true,
      observe: 'events'
    });
  }
  private handleError(error: HttpErrorResponse) {
    this.cleanVideoListCache()
    this._videos.next(this.videosList)
    return throwError(() => new Error('Not videos found'));
  }
}
