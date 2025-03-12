import { Component, OnInit } from '@angular/core';
import { UserCourseService } from '../Services/UserCourse.service';
import { UserService } from '../Services/user-service';
import { LoginService } from '../Services/login.service';
import { Router } from '@angular/router';
import { Subscription, filter } from 'rxjs';

@Component({
  selector: 'app-users-management',
  templateUrl: './users-management.component.html',
  styleUrls: ['./users-management.component.css']
})
export class UsersManagementComponent implements OnInit {
  users: any[] = [];
  userCourses: any[] = [];
  openedAccordionId: number | null = null;
  user: any;
  userSubscription: Subscription | undefined;

  constructor(
    private userCourseService: UserCourseService,
    private userService: UserService,
    private loginService: LoginService,
    private router: Router
  ) { }

  ngOnInit() {
    this.userSubscription = this.loginService.user$.pipe(filter((user: any) => user !== undefined)).subscribe(data => {
      this.user = data;
      if (!this.user || this.user.roleID != 1) {
        this.router.navigate(['/']);
      }
    });

    this.userService.getAllUser().subscribe(data => {
      this.users = data;
    })
  }

  loadUserCourses(userId: number): void {
    if (this.openedAccordionId === userId) {
      this.openedAccordionId = null;
      this.userCourses = [];
    } else {
      this.openedAccordionId = userId;
      this.userCourseService.getAllUserCourseByUserID(userId).subscribe(userCourses => {
        this.userCourses = userCourses;
      });
    }
  }

  ngOnDestroy() {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }
}
