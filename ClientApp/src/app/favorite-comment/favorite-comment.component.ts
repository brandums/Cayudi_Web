import { Component, Input, OnInit } from '@angular/core';
import { UserService } from '../Services/user-service';
import { BlogService } from '../Services/blog-service';
import { CourseService } from '../Services/course-service';

@Component({
  selector: 'app-favorite-comment',
  templateUrl: './favorite-comment.component.html',
  styleUrls: ['./favorite-comment.component.css']
})
export class FavoriteCommentComponent implements OnInit {
  @Input() blogPost: any;
  user: any;
  course: any;
  userInitials: string = '';
  containsImage = false;

  constructor(
    private blogService: BlogService,
    private userService: UserService,
    private courseService: CourseService,
  ) { }

  ngOnInit(): void {
    this.userService.getUserByID(this.blogPost.userID).subscribe(data => {
      this.user = data;
      this.containsImage = this.user.profImage != '' && this.user.profImage != null  ? true : false;
      this.userInitials = this.userService.calculateUserInitials(this.user.name);
    });

    this.blogService.getBlogByID(this.blogPost.blogID).subscribe(data => {
      this.courseService.getCourseByID(data.courseID).subscribe(data => {
        this.course = data
      });
    });
  }
}
