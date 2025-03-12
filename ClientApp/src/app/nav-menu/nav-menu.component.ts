import { Component, OnInit } from '@angular/core';
import { LoginService } from '../Services/login.service';
import { UserService } from '../Services/user-service';
import { AuthenticationService } from '../Services/authentication.service';
import { CourseService } from '../Services/course-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  user: any;
  userInitials: string = '';
  isLogin: boolean = false;
  hasLoginImg = false;
  UseDefImg = true;
  search = "";

  constructor(
    private router: Router,
    private loginService: LoginService,
    private userService: UserService,
    private authService: AuthenticationService,
    private courseService: CourseService
  ) { }

  ngOnInit(): void {
    this.loginService.isAuthenticated$.subscribe(isAuthenticated => {
      this.isLogin = isAuthenticated;
    });

    this.loginService.user$.subscribe(data => {
      this.user = data;

      if (this.user) {
        this.userInitials = this.userService.calculateUserInitials(this.user.name);
        this.hasLoginImg = this.user.profImage != '' && this.user.profImage != null ? true : false;
        this.UseDefImg = this.hasLoginImg ? false : true ;
      }
    });
  }

  logout() {
    this.authService.logout();
  }

  searchCourse(): void {
    this.router.navigate(['/course-list/99999'])
    this.courseService.newDataAdded(0, this.search)
  }
}
