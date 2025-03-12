import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CourseService } from '../Services/course-service';
import { UserFavoriteCourseService } from '../Services/user-favorite-course-service';
import { UserCourseService } from '../Services/UserCourse.service';
import { VideoService } from '../Services/video.service';

@Component({
  selector: 'app-course-card',
  templateUrl: './course-card.component.html',
  styleUrls: ['./course-card.component.css']
})
export class CourseCardComponent implements OnInit {
  @Input() course: any;
  @Input() user: any;
  @Output() notifyParent: EventEmitter<void> = new EventEmitter<void>();
  editedText: string = '';
  isEditingPrice: boolean = false;
  isEditingTime: boolean = false;
  isFavorite: boolean = false;
  userFavoriteCourse: any;
  isBought: any;
  numberVideos: number = 0;

  constructor(
    private courseService: CourseService,
    private userFavoriteCourseService: UserFavoriteCourseService,
    private userCourseService: UserCourseService,
    private videoService: VideoService
  ) { }

  ngOnInit(): void {
    if (this.user) {
      this.getFavorite();
      this.getUserCourse();
    }

    this.videoService.getVideosByCourse(this.course.id).subscribe(data => {
      this.numberVideos = data.length;
    })
  }

  getUserCourse() {
    this.userCourseService.getUserCourseByID(this.course.id, this.user.id).subscribe(data => {
      this.isBought = (data) ? true : false;
    })
  }

  getFavorite() {
    this.userFavoriteCourseService.getByUserAndCourse(this.user.id, this.course.id).subscribe(data => {
      this.userFavoriteCourse = data;

      if (this.userFavoriteCourse.courseID === this.course.id) {
        this.isFavorite = true;
      } else {
        this.isFavorite = false;
      }
    })
  }

  GiveFavorite() {
    if (this.isFavorite) {
      this.userFavoriteCourseService.deleteUserFavoriteCourse(this.userFavoriteCourse.id).subscribe(
        response => {
          this.isFavorite = false;
          this.getFavorite();
          console.log('UserFavoriteCourse eliminado con éxito:', response);
        },
        error => {
          console.error('Error al eliminar el UserFavoriteCourse:', error);
          console.log('el userFavoriteCourse es: ', this.userFavoriteCourse);
        }
      );
    }
    else {
      const userFavoriteCourse = {
        UserID: this.user.id,
        courseID: this.course.id
      };
      this.userFavoriteCourseService.createUserFavoriteCourse(userFavoriteCourse)
        .subscribe(
          (response: any) => {
            this.getFavorite();
            console.log('UserFavoriteCourse creado exitosamente', response);
          },
          (error: any) => {
            console.error('Error al crear el UserFavoriteCourse', error);
          }
        );
    }
  }

  startEditing(field: string) {
    if (this.user.roleID == 1)
    {
      if (field === 'price') {
        this.editedText = this.course.price;
        this.isEditingPrice = true;
      } else if (field === 'time') {
        this.editedText = this.course.durationTime;
        this.isEditingTime = true;
      }
    }
  }

  handleEditClick(event: Event) {
    event.stopPropagation();
  }
  handleClick() {
    this.isEditingPrice = false;
    this.isEditingTime = false;
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

        if (field === '/Price') {
          this.course.price = this.editedText;
          this.isEditingPrice = false;
        } else if (field === '/DurationTime') {
          this.course.durationTime = this.editedText;
          this.isEditingTime = false;
        }
      }, error => {
        console.error('Error al guardar cambios en la descripción:', error);
      });
  }

  updateParent(): void {
    this.notifyParent.emit();
  }
}
