import { Component, Input, OnInit } from '@angular/core';
import { CourseTestService } from '../Services/CourseTestService';
import { CourseTest } from '../models/CourseTest';
import { LoginService } from '../Services/login.service';

@Component({
  selector: 'app-course-test',
  templateUrl: './course-test.component.html',
  styleUrls: ['./course-test.component.css']
})
export class CourseTestComponent implements OnInit {
  @Input() courseID: number = 0;
  tests: CourseTest[] | undefined;
  isAdmin = false;
  testIdToDelete: number = 0;

  constructor(
    private courseTestService: CourseTestService,
    private loginService: LoginService
  ) { }

  ngOnInit(): void {
    this.loginService.user$.subscribe(data => {
      this.isAdmin = (data.roleID != 1) ? false : true;
    })

    this.getCourseTest();

    this.courseTestService.$testList.subscribe(data => {
      this.tests = data;
    });
  }

  getCourseTest() {
    this.courseTestService.getAllTestByCourseID(this.courseID).subscribe(data => {
      this.tests = data;
    })
  }

  selectedTest(testId: number): void {
    this.testIdToDelete = testId;
  }

  deleteCourseTest() {
    this.courseTestService.deleteCourseTest(this.testIdToDelete).subscribe(() => {
      this.courseTestService.newDataAdded(this.courseID)
    })
  }
}
