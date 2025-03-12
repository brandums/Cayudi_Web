import { Component, Input, OnInit } from '@angular/core';
import { BlogPostAnswerService } from '../Services/blog-post-answer-service';
import { UserService } from '../Services/user-service';
import { PostLikeService } from '../Services/post-like-service';
import { LoginService } from '../Services/login.service';

@Component({
  selector: 'app-comment-post',
  templateUrl: './comment-post.component.html',
  styleUrls: ['./comment-post.component.css']
})
export class CommentPostComponent implements OnInit {
  @Input() blogPost: any;
  blogPostAnswer: any;
  user: any;
  userInitials: string = '';
  userInitialsAnswer: string = ''
  isReplying: boolean = false;
  SeeMore: boolean = false;
  likes: any;
  postLike: any;
  isLogin: boolean = false;
  userData: any;
  containsImage = false;
  newBlogPostAnswer: any = {
    BlogPostID: 0,
    Message: '',
    LikesNumber: 0,
    CreationDate: new Date(),
    Highlighted: false,
    UserID: 1
  };

  constructor(
    private blogPostAnswerService: BlogPostAnswerService,
    private userService: UserService,
    private postLikeService: PostLikeService,
    private loginService: LoginService,
  ) { }

  ngOnInit(): void {
    this.GetLoggedInUser();
    this.getLikes();

    this.blogPostAnswerService.getBlogPostAnswerByID(this.blogPost.id).subscribe(data => {
      this.blogPostAnswer = data;
    })

    this.GetUsers();
  }

  GetLoggedInUser() {
    this.loginService.isAuthenticated$.subscribe(isAuthenticated => {
      this.isLogin = isAuthenticated;
      this.postLike = false;
    });

    this.loginService.user$.subscribe(user => {
      this.userData = user;

      this.newBlogPostAnswer.UserID = user.id;
      this.userInitialsAnswer = this.userService.calculateUserInitials(user.name);

      this.getLikes();
    });
  }

  GetUsers() {
    this.userService.getUserByID(this.blogPost.userID).subscribe(data => {
      this.user = data;
      this.containsImage = this.user.profImage != '' && this.user.profImage != null ? true : false;
      this.userInitials = this.userService.calculateUserInitials(this.user.name);
    })
  }

  toggleReply() {
    this.isReplying = !this.isReplying;
  }

  SendComment() {
    this.newBlogPostAnswer.BlogPostID = this.blogPost.id;

    this.blogPostAnswerService.createBlogPostAnswer(this.newBlogPostAnswer).subscribe(response => {
      console.log('Comentario enviado con éxito:', response);

      this.blogPostAnswerService.getBlogPostAnswerByID(this.blogPost.id).subscribe(data => {
        this.blogPostAnswer = data;
      });
    });

    this.isReplying = false;
  }

  ActiveComments() {
    this.SeeMore = !this.SeeMore;
  }

  getLikes() {
    this.postLikeService.getPostLikeByID(this.blogPost.id).subscribe(data => {
      this.likes = data.length;

      this.postLike = data.find((c: any) => c.userID === this.userData.id);
    })
  }

  GiveLike() {
    if (this.postLike) {
      this.postLikeService.deletePostLike(this.postLike.id).subscribe(
        response => {
          this.getLikes();
          console.log('PostLike eliminado con éxito:', response);
        },
        error => {
          console.error('Error al eliminar el PostLike:', error);
        }
      );
    }
    else {
      const postLike = {
        UserID: this.userData.id,
        BlogPostID: this.blogPost.id
      };
      this.postLikeService.createPostLike(postLike)
        .subscribe(
          (response: any) => {
            this.getLikes();
            console.log('PostLike creado exitosamente', response);
          },
          (error: any) => {
            console.error('Error al crear el PostLike', error);
          }
        );
    }
  }
}
