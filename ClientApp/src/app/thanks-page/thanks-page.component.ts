import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../Services/course-service';

@Component({
  selector: 'app-thanks-page',
  templateUrl: './thanks-page.component.html',
  styleUrls: ['./thanks-page.component.css']
})
export class ThanksPageComponent implements OnInit {
  course: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private courseService: CourseService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const courseID = params['courseID'];

      this.courseService.getCourseByID(courseID).subscribe(data => {
        this.course = data;
      });
    });
  }

  redirectToCourse() {
    this.router.navigate(['/course', this.course.id, this.course.tittle]);
  }
}
