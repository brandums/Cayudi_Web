import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CourseCategoryService } from '../Services/course-category-service';
import { CourseService } from '../Services/course-service';
import { courseRatingResponse } from '../models/courseRatingResponse';
import { UserCourseService } from '../Services/UserCourse.service';
import { fileDataDTO } from '../models/fileDataDTO';

@Component({
  selector: 'app-course-lock',
  templateUrl: './course-lock.component.html',
  styleUrls: ['./course-lock.component.css']
})
export class CourseLockComponent implements OnInit {
  @Input() user: any;
  @Input() course: any;
  @Output() isAdminChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  trainerID: number | undefined;
  courseRate: courseRatingResponse = new courseRatingResponse();

  category: any;
  editedText: string = '';
  isEditingDescription = false;
  isEditingTitle = false;
  isEditingSubtitle = false;
  fileInfo: fileDataDTO = new fileDataDTO(0, 2, 2, 3, 'image/*');

  constructor(
    private courseCategoryService: CourseCategoryService,
    private courseService: CourseService,
    private userCourseService: UserCourseService
  ) { }


  ngOnInit(): void {
    this.trainerID = this.course.trainerID;
    this.courseCategoryService.getCategoryByID(this.course.courseCategoryID).subscribe(data => {
      this.category = data;
    });

    this.userCourseService.getCourseRating(this.course.id)
      .subscribe(
        (response) => {
          this.courseRate = response;
        },
        (error) => {
          console.error('Error al obtener la calificación del curso:', error);
        });

    this.fileInfo.sourceId = this.course.id
  }
  onNotifyParent(): void {
    this.isAdminChange.emit(true);
  }

  startEditing(field: string) {
    if (this.user.roleID == 1) {
      if (field === 'tittle') {
        this.editedText = this.course.tittle;
        this.isEditingTitle = true;
      } else if (field === 'description') {
        this.editedText = this.course.description;
        this.isEditingDescription = true;
      } else if (field === 'subtitle') {
        this.editedText = this.course.subtitle;
        this.isEditingSubtitle = true;
      }
    }
  }

  handleEditClick(event: Event) {
    event.stopPropagation();
  }
  handleClick() {
    this.isEditingTitle = false;
    this.isEditingDescription = false;
    this.isEditingSubtitle = false;
  }

  saveChanges(field: string) {
    const patchData = [
      {
        "op": "replace",
        "path": field,
        "value": this.editedText
      }
    ];

    this.courseService.updateCourse(this.course.id, patchData)
      .subscribe(response => {
        console.log('Cambios en la descripción guardados con éxito:', response);

        if (field === '/Tittle') {
          this.course.tittle = this.editedText;
          this.isEditingTitle = false;
        } else if (field === '/Description') {
          this.course.description = this.editedText;
          this.isEditingDescription = false;
        } else if (field === '/Subtitle') {
          this.course.subtitle = this.editedText;
          this.isEditingSubtitle = false;
        }
      }, error => {
        console.error('Error al guardar cambios en la descripción:', error);
      });
  }
}

