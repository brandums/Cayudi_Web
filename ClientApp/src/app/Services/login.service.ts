import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserService } from './user-service';
import { userUtils } from '../utils/userUtils';
import { User } from '../models/user';
import { UserCourseService } from './UserCourse.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  private userSubject = new BehaviorSubject<any>(undefined);
  private userData: User = new User();
  public $userData: BehaviorSubject<User>;

  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  user$ = this.userSubject.asObservable();
  userLoginData: any;

  constructor(private userService: UserService, private courseService: UserCourseService) {
    this.checkAuthentication();
    this.userLoginData = userUtils.getUserInfo()
    this.$userData = new BehaviorSubject<User>(new User())
  }

  checkAuthentication() {
    this.userLoginData = userUtils.getUserInfo()
    if (this.userLoginData) {
      this.userService.getUserByID(this.userLoginData.id).subscribe(data => {
        this.userSubject.next(data);
        this.userData.userData = data;
        this.isAuthenticatedSubject.next(true);
        this.updateUserCourseData();
        this.$userData.next(this.userData);
      });
    } else {
      this.isAuthenticatedSubject.next(false);
      this.userSubject.next(null);
    }
  }

  updateUserCourseData()
  {    
    this.courseService.getAllUserCourseByUserID(this.userLoginData.id).subscribe(data => {
      this.userData.userCourseData = data;
      this.userData.userCourseData.forEach(obj => {
        this.userData.userCourseIds.push(obj.id)
      });
    })
  }

  getUserDataREMOVEITTHEN(): Observable<User>{
    this.userLoginData = userUtils.getUserInfo()
    this.courseService.getAllUserCourseByUserID(this.userLoginData.id).subscribe(data => {
      this.userData.userCourseData = data;
      this.userData.userCourseData.forEach(obj => {
        this.userData.userCourseIds.push(obj.courseID)
      });
      this.$userData.next(this.userData);
    })
    return this.$userData
  }

}

