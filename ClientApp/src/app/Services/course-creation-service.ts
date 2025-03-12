import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CourseCreationService {
  private courseCreated = new Subject<void>();
  private emitingChanges = new Subject<void>();

  courseCreated$ = this.courseCreated.asObservable();
  emitingChanges$ = this.emitingChanges.asObservable();

  emitCourseCreated() {
    this.courseCreated.next();
  }

  emitChanges() {
    this.emitingChanges.next();
  }
}
