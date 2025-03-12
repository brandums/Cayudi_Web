import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseService } from '../Services/course-service';
import { LoginService } from '../Services/login.service';
import { UserCourseService } from '../Services/UserCourse.service';
import { Course } from '../models/course';
import { CourseCreationService } from '../Services/course-creation-service';


@Component({
  selector: 'app-course',
  templateUrl: './course.component.html'
})
export class CourseComponent implements OnInit {
  course: Course | undefined;
  user: any;
  isLock: any;
  isAdmin = false;
  userCourse: any;
  courseId = 0
  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private loginService: LoginService,
    private userCourseService: UserCourseService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params['id'];
      this.courseId = params['id'];
      if (id) {
        this.courseService.getCourseByID(id).subscribe(data => {
          this.course = data;

          this.loginService.isAuthenticated$.subscribe(isAuthenticated => {
            this.isLock = true;
            if (!isAuthenticated) {
              this.isLock = true;
            }
          });

          this.loginService.user$.subscribe(user => {
            this.user = user;

            if (this.user.roleID == 1) {
              this.isLock = false;
            }
            else if (this.user && this.course) {
              this.userCourseService.getUserCourseByID(this.course.id, this.user.id).subscribe(data => {
                this.userCourse = data;
                this.isLock = (data && data.isOppened) ? false : true;
                if (this.userCourse) this.dateControl();
              })
            }
          });
        });
      }
    });
  }

  dateControl() {
    if (this.user.roleID != 1 || this.user.id != this.course?.trainerID) {
      if (!this.userCourse.endTime || this.userCourse.endTime == '0001-01-01T00:00:00') {
        this.userCourseService.getEndTime(this.userCourse.id).subscribe(data => {
          this.userCourse = data;
          this.closeCourse();
        })
      }
      else {
        this.closeCourse();
      }
    }
  }

  closeCourse() {
    if (new Date(this.userCourse.endTime).getDate() <= Date.now()) {
      this.userCourseService.closeCourse(this.user.id).subscribe(data => {
        this.userCourse = data;
        this.isLock = true;
      })
    }
  }

  //changeView() {
  //  if (this.user.roleID == 1) {
  //    const patchData = [
  //      {
  //        "op": "replace",
  //        "path": "IsOppened",
  //        "value": this.isAdmin
  //      }
  //    ];

  //    this.userCourseService.updateUserCourse(this.userCourse.id, patchData)
  //      .subscribe(response => {
  //        this.isLock = !this.isAdmin;
  //      }, error => {
  //        console.error('Error al guardar cambios del userCourse:', error);
  //      });
  //  }
  //}

  onAdminChange(isAdmin: boolean): void {
    this.isLock = !isAdmin;
  }
}
