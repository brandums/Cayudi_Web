import { Component, ElementRef, ViewChild, AfterViewInit, OnInit } from '@angular/core';
import { BlogPostService } from '../Services/blog-post-service';
import { CourseService } from '../Services/course-service';
import { LoginService } from '../Services/login.service';
import { User } from '../models/user';
import { CourseCategoryService } from '../Services/course-category-service';
import { fileDataDTO } from '../models/fileDataDTO';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, AfterViewInit {

  showLeftComment: boolean = false;
  showRightComment: boolean = false;
  showLeftVideo: boolean = false;
  showRightVideo: boolean = false;
  commentsData: any;
  courseData: any;
  isLogin: any;
  userData: User = new User()
  showReservations: boolean[] = [true, true, true, true, true, true, true, true, true, true, true, true];

  selectedCategoryId: any;
  categories: any[] = [];
  showLeftCourses: boolean = false;
  showRightCourses: boolean = false;
  fileInfo: fileDataDTO = new fileDataDTO(0, 3, 0, 0, 'image/*');
  //@ViewChild('uploader_id') _upload!: ElementRef;

  @ViewChild('commentsContainer') commentsContainer!: ElementRef;
  @ViewChild('videosContainer') videosContainer!: ElementRef;
  @ViewChild('resCourses') resCourses!: ElementRef;
  
  constructor(
    private blogPostService: BlogPostService,
    private courseService: CourseService,
    private loginService: LoginService,
    private courseCategoryService: CourseCategoryService,
  ) { }

  scrollComments(direction: number) {
    const container = this.commentsContainer.nativeElement;
    const scrollAmount = 200;

    container.scrollLeft += scrollAmount * direction;
  }
  scrollVideos(direction: number) {
    const containerVideo = this.videosContainer.nativeElement;
    const scrollAmount = 200;

    containerVideo.scrollLeft += scrollAmount * direction;
  }
  scrollCourses(direction: number) {
    const _resCourses = this.resCourses.nativeElement;
    const scrollAmount = 200;
    _resCourses.scrollLeft += scrollAmount * direction;
  }

  ngOnInit(): void {
    this.blogPostService.getBlogPost().subscribe(data => {
      this.commentsData = data;
      this.ngAfterViewInit();
    })

    this.courseService.getAllCourses().subscribe(data => {
      this.courseData = data;
      this.ngAfterViewInit();
    })
    this.loginService.checkAuthentication();
     this.loginService.$userData.subscribe(data => {
       this.userData = data;
     })


    this.loginService.user$.subscribe(data => {
      this.isLogin = (!data || data.roleID != 1) ? false : true;
      
    })
    //var userData = userUtils.getUserInfo()
    //this.isAdmin = (userData && userData.roleID == 1) ? true : false;

    //this.getCategories();

    this.courseService.updateFutureCoursesList();

    //this.categoryCreationService.courseCreated$.subscribe(() => {
    //  this.getCategories();
    //});

    this.courseCategoryService.$categoriesList.subscribe(data => {
      this.categories = data;
    })
    this.courseCategoryService.newDataAdded();
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.checkScrollPosition();

      this.commentsContainer.nativeElement.addEventListener('scroll', () => {
        this.checkScrollPosition();
      });

      this.videosContainer.nativeElement.addEventListener('scroll', () => {
        this.checkScrollPosition();
      });

      this.resCourses.nativeElement.addEventListener('scroll', () => {
        this.checkScrollPosition();
      });
     
    }, 1000);

    if (this.resCourses.nativeElement.childElementCount > 4) {
      this.resCourses.nativeElement.classList.add('content-reservation-big')
    }
  }

  //getCategories() {
  //  this.courseCategoryService.getCategory().subscribe(data => {
  //    this.categories = data;
  //  });
  //}

  checkScrollPosition() {
    const containerComments = this.commentsContainer.nativeElement;
    const containerVideo = this.videosContainer.nativeElement;
    const _resCourses = this.resCourses.nativeElement;

    this.showLeftComment = containerComments.scrollLeft > 0;
    this.showRightComment = containerComments.scrollLeft + containerComments.clientWidth < containerComments.scrollWidth;
    this.showLeftVideo = containerVideo.scrollLeft > 0;
    this.showRightVideo = containerVideo.scrollLeft + containerVideo.clientWidth < containerVideo.scrollWidth;

    this.showLeftCourses = _resCourses.scrollLeft > 0;
    this.showRightCourses = _resCourses.scrollLeft + _resCourses.clientWidth < _resCourses.scrollWidth;
  }

  ChangeReservationView(eventData: { value: number, index: number }): void {
    this.showReservations[eventData.index] = eventData.value > 0;
  }

  openDeleteCategoryModal(categoryId: number) {
    this.selectedCategoryId = categoryId;
  }

  deleteCategory() {
    if (this.selectedCategoryId !== null) {
      this.courseCategoryService.deleteCategory(this.selectedCategoryId).subscribe(() => {
        this.selectedCategoryId = null;
        this.courseCategoryService.newDataAdded();
        const button = document.getElementById('bCloseCategory');
        if (button instanceof HTMLElement) {
          button.click();
        }
        //this.getCategories();
      });
    }
  }
}
