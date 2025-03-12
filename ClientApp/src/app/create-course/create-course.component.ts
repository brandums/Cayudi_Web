import { Component, OnInit } from '@angular/core';
import { CourseService } from '../Services/course-service';
import { CourseCategoryService } from '../Services/course-category-service';
import { CourseCreationService } from '../Services/course-creation-service';
import { UserService } from '../Services/user-service';


@Component({
  selector: 'app-create-course',
  templateUrl: './create-course.component.html',
  styleUrls: ['./create-course.component.css']
})
export class CreateCourseComponent implements OnInit {
  courseData: any = {
    Tittle: '',
    Subtitle: '',
    CourseCategoryID: '',
    Description: '',
    Content: '',
    Price: '',
    TrainerID: "",
    StartDate: "",
    IsOppened: false,
    ImagePath: '',
    BannerPath: '',
    DurationTime: ''
  };
  categories: any[] = [];
  users: any[] = [];
  isValid = true;

  constructor(
    private courseService: CourseService,
    private courseCategoryService: CourseCategoryService,
    private courseCreationService: CourseCreationService,
    private userService: UserService,
  ) { }

  ngOnInit(): void {
    this.courseCategoryService.getCategory().subscribe(data => {
      this.categories = data;
    });

    this.userService.getAllUser().subscribe(data => {
      this.users = data;
    });
  }

  minDate(): string {
    const fechaActual = new Date();
    const formatoFecha = fechaActual.toISOString().split('T')[0];

    return formatoFecha;
  }

  removeInvalidClass(fieldName: string): void {
    const inputElement = document.getElementById(fieldName) as HTMLInputElement | null;
    inputElement?.classList.remove('invalid');
  }


  validationForm(event: Event): void {
    event.preventDefault();
    this.isValid = true;

    const inputs = document.querySelectorAll('input[required], textarea[required], select[required]');

    inputs.forEach((value: Element) => {
      const input = value as HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement;
      if (input.value.trim() === '') {
        this.isValid = false;
        input.classList.add('invalid');
      } else {
        input.classList.remove('invalid');
      }
    });

    if (this.isValid) {
      this.createCourse();
    }
  }

  cleanForm() {
    this.courseData = {
      Tittle: '',
      Subtitle: '',
      CourseCategoryID: '',
      Description: '',
      Content: '',
      Price: '',
      TrainerID: "",
      StartDate: "",
      IsOppened: false,
      ImagePath: '',
      BannerPath: '',
      DurationTime: ''
    };

    const inputs = document.querySelectorAll('.invalid');
    inputs.forEach((input) => {
      input.classList.remove('invalid');
    });
  }

  createCourse() {
    this.courseService.createCourse(this.courseData)
      .subscribe(
        (response: any) => {
          const button = document.getElementById('bClose');
          if (button instanceof HTMLElement) {
            button.click();
          }
          console.log('Curso creado exitosamente', response);
          this.courseCreationService.emitCourseCreated();
        },
        (error: any) => {
          console.error('Error al crear el curso', error);
        }
      );
  }
}
