import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BlogService } from '../Services/blog-service';
import { BlogPostService } from '../Services/blog-post-service';
import { CourseService } from '../Services/course-service';
import { UserService } from '../Services/user-service';
import { Course } from '../models/course';
import { UserCourseService } from '../Services/UserCourse.service';
import { courseRatingResponse } from '../models/courseRatingResponse';
import { CourseCreationService } from '../Services/course-creation-service';

@Component({
  selector: 'app-course-page',
  templateUrl: './course-page.component.html',
  styleUrls: ['./course-page.component.css']
})
export class CoursePageComponent implements OnInit, AfterViewInit {
  @Input() user: any;
  @Input() course!: Course;
  @Output() isAdminChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  blog: any;
  blogPost: any;
  activeTab: string = 'tab1';
  editedText: string = '';
  isEditingDescription: boolean = false;
  isEditingTitle: boolean = false;
  isEditingSubtitle: boolean = false;
  userInitials: string = '';
  courseId: number = 0;
  containsImage = false;
  courseRate: courseRatingResponse = new courseRatingResponse();

  userCourse: any;
  tab1Enabled: boolean = true;
  tab2Enabled: boolean = true;
  tab4Enabled: boolean = false;
  tab4Visible: boolean = true;

  newBlogPost: any = {
    BlogID: 0,
    Message: '',
    LikesNumber: 0,
    CreationDate: new Date(),
    Highlighted: false,
    UserID: 0,
  };

  constructor(
    private blogService: BlogService,
    private blogPostService: BlogPostService,
    private courseService: CourseService,
    private userService: UserService,
    private userCourseService: UserCourseService,
    private emitServices: CourseCreationService
  ) { }

  ngOnInit(): void {
    this.blogService.getBlogByID(this.course.id).subscribe(blogData => {
      this.blog = blogData;

      this.blogPostService.getBlogPostByID(this.blog.id).subscribe(blogPostData => {
        this.blogPost = blogPostData;
      });
    });

    this.getRateCourse();
    this.checkCompletedCourse()

    this.containsImage = this.user.profImage != '' && this.user.profImage != null ? true : false;
    this.userInitials = this.userService.calculateUserInitials(this.user.name);
    this.courseId = this.course.id;
  }

  getRateCourse() {
    this.userCourseService.getUserCourseByID(this.course.id, this.user.id).subscribe(data => {
      this.userCourse = data;
    });

    this.userCourseService.getCourseRating(this.course.id)
      .subscribe(
        (response) => {
          this.courseRate = response;
        },
        (error) => {
          console.error('Error al obtener la calificación del curso:', error);
        });
  }

  checkCompletedCourse() {
    this.userCourseService.getUserCourseByID(this.course.id, this.user.id).subscribe(data => {
      this.userCourse = data;

      if (this.userCourse.isFinished) {
        this.tab1Enabled = false;
        this.tab2Enabled = false;
        this.tab4Visible = false;
        this.tab4Enabled = true;
      }
      else {
        this.tab1Enabled = true;
        this.tab2Enabled = true;
        this.tab4Enabled = false;
        this.tab4Visible = true;
      }
    });
  }

  SendComment() {
    this.newBlogPost.BlogID = this.blog.id;
    this.newBlogPost.UserID = this.user.id;
    this.blogPostService.createBlogPost(this.newBlogPost).subscribe(response => {
      this.blogPostService.getBlogPostByID(this.blog.id).subscribe(data => {
        this.blogPost = data;
      });
    });
    this.newBlogPost.Message = '';
  }

  ngAfterViewInit(): void {
  }

  scrollToComments() {
    const commentsContent = document.getElementById('comments-content');
    if (commentsContent) {
      commentsContent.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }
  }

  changeTab(tab: string): void {
    this.activeTab = tab;
    const leftContent = document.querySelector('.left-content') as HTMLElement;
    if (leftContent) {
      leftContent.style.scrollBehavior = 'smooth';
      leftContent.scrollTop = 0;
    }
  }

  startEditing(field: string) {
    if (this.user.roleID == 1 && this.course)
    {
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

  onNotifyParent(): void {
    this.isAdminChange.emit(false);
  }

  UpdateFinishCourse() {
    if (this.user.roleID !== 1) {
      const patchData = [
        {
          "op": "replace",
          "path": "IsFinished",
          "value": true,
        }
      ];

      this.userCourseService.updateUserCourse(this.userCourse.id, patchData)
        .subscribe(() => {
          this.checkCompletedCourse();
          this.emitServices.emitChanges();
        }, error => {
          console.error('Error al guardar cambios del userCourse:', error);
        });
    }
  }

  updateRateCourse(star: number) {
    const patchData = [
      {
        op: 'replace',
        path: '/rating',
        value: star
      }
    ];

    this.userCourseService.updateUserCourse(this.userCourse.id, patchData).subscribe((response) => {
        console.log('Calificación actualizada exitosamente:', response);
      this.getRateCourse();
      },
      (error) => {
        console.error('Error al actualizar la calificación:', error);
    })
  }
}
