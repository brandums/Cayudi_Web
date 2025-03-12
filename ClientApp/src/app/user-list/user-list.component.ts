import { Component, OnInit } from '@angular/core';
import { UserCourseService } from '../Services/UserCourse.service';
import { UserService } from '../Services/user-service';
import { Observable, Subscription, filter, map } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { UserCourse } from '../models/userCourse';
import { LoginService } from '../Services/login.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  courseID: any;
  usersOnline: any = [];
  usersPresential: any = [];
  users: any = [];
  filteredUsers: any[] = [];
  isOnline: any = '';
  searchTerm: string = '';
  userCourse = new UserCourse();
  userSubscription: Subscription | undefined;

  constructor(
    private userCourseService: UserCourseService,
    private userService: UserService,
    private loginService: LoginService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.courseID = params['courseID'];
    });

    this.userSubscription = this.loginService.user$.pipe(filter((user: any) => user !== undefined)).subscribe(user => {
      if (!user || user.roleID != 1) {
        this.router.navigate(['/']);
      }
    });

    this.getUserCourse();
  }

  getUserCourse() {
    if (this.courseID) {
      this.userCourseService.getAllUsersByCourseID(this.courseID).subscribe(data => {
        this.usersOnline = data.filter((userCourse: UserCourse) => userCourse.isOnline === true);
        this.usersPresential = data.filter((userCourse: UserCourse) => userCourse.isOnline === false);
      });
    }
  }

  getName(userID: any): Observable<string> {
    return this.userService.getUserByID(userID).pipe(
      map(user => user.name)
    );
  }

  selectedUserCourse: any;
  openConfirmationModal(userCourse: any) {
    this.selectedUserCourse = userCourse;
  }

  ToggleOnCourse(userCourse: any) {
    const patchData = [
      {
        "op": "replace",
        "path": "IsOppened",
        "value": !userCourse.isOppened
      }
    ];

    this.userCourseService.updateUserCourse(userCourse.id, patchData)
      .subscribe(response => {
        userCourse.isOppened = !userCourse.isOppened;
      }, error => {
        console.error('Error al guardar cambios del userCourse:', error);
      });
  }

  getUsers() {
    this.userService.getAllUser().subscribe(users => {
      this.users = users;
      this.filteredUsers = this.users;
    })
  }

  filterUsers(): void {
    if (!this.searchTerm) {
      this.filteredUsers = this.users;
    } else {
      const searchTermLower = this.searchTerm.toLowerCase();
      this.filteredUsers = this.users.filter((user: any) =>
        user.name.toLowerCase().includes(searchTermLower)
      );
    }
  }

  selectUser(user: any): void {
    this.userCourse.userID = user.id;
    this.searchTerm = user.name;
    this.filteredUsers = this.users;
  }

  AddUserCourse() {
    this.userCourse.courseID = this.courseID;
    this.userCourse.isOnline = this.isOnline;
    this.userCourseService.createUserCourse(this.userCourse).subscribe(response => {
      this.getUserCourse();
      this.searchTerm = "";
      this.isOnline = "";
    }, error => {
      console.error('Error al agregar al estudiante:', error);
    });
  }

  ngOnDestroy() {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }
}
