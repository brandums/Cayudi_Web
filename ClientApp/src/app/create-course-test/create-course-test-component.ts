import { Component, Input } from '@angular/core';
import { CourseTest } from '../models/CourseTest';
import { CourseTestService } from '../Services/CourseTestService';


@Component({
  selector: 'app-create-course-test',
  templateUrl: './create-course-test.component.html',
  styleUrls: ['./create-course-test.component.css']
})
export class CreateCourseTestComponent {
  @Input() courseID: number = 0;
  courseTest: CourseTest = new CourseTest;

  constructor(
    private courseTestService: CourseTestService
  ) { }

  createCourseTest() {
    if (this.courseTest) {
      this.courseTest.courseID = this.courseID;
      this.courseTestService.createTest(this.courseTest)
        .subscribe(
          (response: any) => {
            this.courseTestService.newDataAdded(this.courseID);
          },
          (error: any) => {
            console.error('Error al crear el test', error);
          }
        );
    }
  }
}
