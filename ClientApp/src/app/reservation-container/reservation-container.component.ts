import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CourseService } from '../Services/course-service';
import { CourseCategoryService } from '../Services/course-category-service';
import { LoginService } from '../Services/login.service';
import { Course } from '../models/course';
import { User } from '../models/user';

@Component({
  selector: 'app-reservation-container',
  templateUrl: './reservation-container.component.html',
  styleUrls: ['./reservation-container.component.css']
})
export class ReservationContainerComponent implements OnInit {
  @Input() count: number = 0;
  @Output() showReservation: EventEmitter<{ value: number; index: number }> = new EventEmitter<{ value: number; index: number }>();
  userData: User = new User();
  courses: Course[] = [];
  coursesToDisplay: Course[] = [];
  category = '';
  date: Date = new Date();
  fullMonthNames: any;
  user: any;
  isLogin = false;

  constructor(
    private courseService: CourseService,
    private courseCategoryService: CourseCategoryService,
    private loginService: LoginService,
  ) {  }

  ngOnInit(): void {
    this.loginService.isAuthenticated$.subscribe(uData => {
      this.isLogin = uData;

      this.coursesToDisplay = [];
      this.courseService.nextCourses.subscribe(cData => {
        this.courses = cData;
        if (this.courses.length > 0) {
          this.updateCoursesToDisplay();
        }
      })
    })

    

    this.fullMonthNames = [
      'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
      'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'
    ];
  }

  updateCoursesToDisplay() {
      this.courses.forEach(obj => {
        const courseDate = new Date(obj.startDate)
        const auxDate = new Date()
        auxDate.setMonth(auxDate.getMonth() + this.count);

        if (courseDate.getMonth() == auxDate.getMonth() && courseDate.getFullYear() == auxDate.getFullYear())
        {
          this.date = auxDate;

          obj.day = courseDate.getDate();
          obj.startDate = new Date(obj.startDate);
          
          this.courseCategoryService.getCategoryByID(obj.courseCategoryID).subscribe(data => {
            obj.courseCategory = data.name;
          })

          if (this.isLogin) {
            this.loginService.getUserDataREMOVEITTHEN().subscribe(data => {
              this.userData = data;
              obj.bought = this.userData.userCourseIds.includes(obj.id);
            })
          }
          this.coursesToDisplay = [];
          this.coursesToDisplay.push(obj);
        }
        this.onNotifyParent();
      })
  }

  getStyle() {
    if (this.count % 3 == 0) {
      return {
        background: 'linear-gradient(186deg, #57baa3 0%, #57baa3 90%)' };
    } 
    return { background: 'linear - gradient(186deg, #6A95C8 0 %, #0D3A7E 80 %)' }; 
  }

  onNotifyParent(): void {
    const eventData = {
      value: this.coursesToDisplay.length,
      index: this.count
    };
    this.showReservation.emit(eventData);
  }
}
