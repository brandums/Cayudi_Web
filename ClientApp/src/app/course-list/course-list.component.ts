import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseService } from '../Services/course-service';
import { CourseCategoryService } from '../Services/course-category-service';
import { CourseCreationService } from '../Services/course-creation-service';
import { LoginService } from '../Services/login.service';
import { UserFavoriteCourseService } from '../Services/user-favorite-course-service';
import { UserCourseService } from '../Services/UserCourse.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
  optionList: string = '';
  courses: any[] = [];
  categories: any[] = [];
  filteredCourses: any[] = [];
  itemsPerPage: number = 8; // Número de cursos por página
  currentPage: number = 1; // Página actual
  isAdmin = false;
  user: any;
  category: any;
  visibilityCategory = true;
  search = "";
  selectedCourse: any;
  courseToDeleteTittle: string | undefined;

  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private courseCategoryService: CourseCategoryService,
    private courseCreationService: CourseCreationService,
    private loginService: LoginService,
    private userCourseFavoriteService: UserFavoriteCourseService,
    private userCourseService: UserCourseService,
  ) { }

  ngOnInit(): void {
    this.loginService.isAuthenticated$.subscribe(isAuthenticated => {
      this.isAdmin = false;
    });

    this.getParameter();

    this.loginService.user$.subscribe(user => {
      this.isAdmin = (user && user.roleID == 1) ? true : false;
      this.user = user;
    });

    this.courseCategoryService.getCategory().subscribe(data => {
      this.categories = data;
    });

    this.courseCreationService.courseCreated$.subscribe(() => {
      this.fillLists();
    });

    this.courseService.$coursesList.subscribe(data => {
      this.courses = data
    })
  }

  getParameter() {
    this.route.params.subscribe(params => {
      this.optionList = params['categoryName'];
      const state = history.state;
      this.category = state.category;

      this.fillLists();
    });
  }

  fillLists() {
    this.courses = [];
    this.filteredCourses = [];

    if (this.optionList === 'favorite') {
      this.loadFavoriteCourses();
      this.visibilityCategory = false;
    }
    else if (this.optionList === 'mis-cursos') {
      this.loadMyCourses();
      this.visibilityCategory = true;
    }
    else if (this.optionList === 'all' || !isNaN(Number(this.optionList))) {
      this.courseService.$coursesList.subscribe(
        data => {
          this.courses = data
        }
      )

      var catID = !isNaN(Number(this.optionList)) ? Number(this.optionList) : 0
      this.courseService.newDataAdded(catID)
      this.visibilityCategory = true;
    }
  }

  loadFavoriteCourses() {
    this.userCourseFavoriteService.getAllByUser(this.user.id).subscribe(data => {
      const userFavoriteCourse: any[] = data;
      const requests = userFavoriteCourse.map(course => this.courseService.getCourseByID(course.courseID));
      forkJoin(requests).subscribe(courses => {
        this.courses = courses;
        this.filteredCourses = courses;
      });
    });
  }

  loadMyCourses() {
    this.userCourseService.getAllUserCourseByUserID(this.user.id).subscribe(data => {
      const userCourse: any[] = data;
      const requests = userCourse.map(course => this.courseService.getCourseByID(course.courseID));
      forkJoin(requests).subscribe(courses => {
        this.courses = courses;
        this.filteredCourses = courses;
      });
    });
  }
  //loadAllCourses() {
  //  this.courseService. getAllCourses().subscribe(data => {
  //    this.courses = data;
  //    this.filteredCourses = data;
  //    this.filterCoursesByCategory(parseInt(this.category));
  //  });
  //}

  filterCoursesByCategory(categoryId: number) {
    this.courses = [];
    this.courseService.newDataAdded(categoryId)
    this.category = categoryId;
  }

  onPageChange(page: number) {
    this.currentPage = page;
  }

  get pagedCourses() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.courses.slice(startIndex, endIndex);
  }

  get pages() {
    const pageCount = Math.ceil(this.filteredCourses.length / this.itemsPerPage);
    return Array.from({ length: pageCount }, (_, i) => i + 1);
  }

  CourseToDelete(course: any) {
    this.selectedCourse = course;
    this.courseToDeleteTittle = course.tittle;
  }

  confirmDeleteCourse() {
    if (this.selectedCourse !== null) {
      this.courseService.deleteCourse(this.selectedCourse.id).subscribe(data => {
        this.courseCreationService.emitCourseCreated();

        this.courses = this.courses.filter(course => course.id !== this.selectedCourse);
      },
        (error) => {
          console.error('Error al eliminar el curso', error);
        });
    }
  }
}
