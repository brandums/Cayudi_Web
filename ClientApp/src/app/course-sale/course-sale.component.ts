import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { CourseCategoryService } from '../Services/course-category-service';
import { Router } from '@angular/router';
import { fileDataDTO } from '../models/fileDataDTO';

@Component({
  selector: 'app-course-sale',
  templateUrl: './course-sale.component.html',
  styleUrls: ['./course-sale.component.css']
})
export class CourseSaleComponent implements OnInit {
  @Input() course: any;
  @Input() user: any;
  @Output() courseToDelete = new EventEmitter<number>();
  categoryName: string = '';

  fileInfo: fileDataDTO = new fileDataDTO(0, 2, 2, 4, 'image/*');

  constructor(
    private courseCategoryService: CourseCategoryService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.courseCategoryService.getCategoryByID(this.course.courseCategoryID).subscribe(data => {
      this.categoryName = data.name;
      this.fileInfo.sourceId = this.course.id
    });
  }

  redirectToCoursePage() {
    this.router.navigate(['/course', this.course.id, this.course.tittle]);
  }

  selectCourseToDelete() {
    this.courseToDelete.emit(this.course);
  }
}
