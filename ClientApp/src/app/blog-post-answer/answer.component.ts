import { Component, Input, OnInit } from '@angular/core';
import { UserService } from '../Services/user-service';
import { PostAnswerLikesService } from '../Services/post-answer-likes-service';
import { LoginService } from '../Services/login.service';

@Component({
  selector: 'app-answer',
  templateUrl: './answer.component.html',
  styleUrls: ['./answer.component.css']
})
export class AnswerComponent implements OnInit {
  @Input() answer: any;
  user: any;
  userInitials: string = '';
  likes: any;
  postLike: any
  userData: any;
  isLogin: boolean = false;
  containsImage = false;

  constructor(
    private userService: UserService,
    private postAnswerLikesService: PostAnswerLikesService,
    private loginService: LoginService,
  ) { }

  ngOnInit(): void {
    this.userService.getUserByID(this.answer.userID).subscribe(data => {
      this.user = data;

      this.containsImage = this.user.profImage != '' && this.user.profImage != null ? true : false;
      this.userInitials = this.userService.calculateUserInitials(this.user.name);
    })

    this.GetLoggedInUser();
    this.getLikes();
  }

  GetLoggedInUser() {
    this.loginService.isAuthenticated$.subscribe(isAuthenticated => {
      this.isLogin = isAuthenticated;
      this.postLike = false;
    });

    this.loginService.user$.subscribe(user => {
      this.userData = user;

      this.getLikes();
    });
  }

  getLikes() {
    this.postAnswerLikesService.getPostAnswerLikesByID(this.answer.id).subscribe(data => {
      this.likes = data.length;

      this.postLike = data.find((c: any) => c.userID === this.userData.id);
    })
  }

  GiveLike() {
    if (this.postLike) {
      this.postAnswerLikesService.deletePostAnswerLikes(this.postLike.id).subscribe(
        response => {
          this.getLikes();
          console.log('PostAnswerLike eliminado con éxito:', response);
        },
        error => {
          console.error('Error al eliminar el PostAnswerLike:', error);
        }
      );
    }
    else {
      const postLike = {
        UserID: this.userData.id,
        BlogPostAnswerID: this.answer.id
      };
      this.postAnswerLikesService.createPostAnswerLikes(postLike)
        .subscribe(
          (response: any) => {
            this.getLikes();
            console.log('PostAnswerLike creado exitosamente', response);
          },
          (error: any) => {
            console.error('Error al crear el PostAnswerLike', error);
          }
        );
    }
  }
}
