import { Component } from '@angular/core';
import { Login } from '../models/login';
import { JwtAuth } from '../models/jwtAuth';
import { AuthenticationService } from '../Services/authentication.service';
import { UserService } from '../Services/user-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  loginDto = new Login();
  jwtDto = new JwtAuth();
  showPassword: boolean = false;
  errorMessages: string[] = [];


  constructor(
    private authService: AuthenticationService,
    private userService: UserService,
  ) {
    this.showPassword = false;
  }
   
  login(loginDto: Login) {
    this.authService.login(loginDto).subscribe(
      (jwtDto) => {
        const button = document.getElementById('CloseModal');
        if (button instanceof HTMLElement) {
          button.click();
        }
      },
      (error) => {
        if (error.status === 400) {
          this.errorMessages = [error.error.userMessage];
        } else if (error.status === 401) {
          this.errorMessages = [error.error.userMessage];
        }

        setTimeout(() => {
          this.errorMessages = [];
        }, 5000);
      }
    );
  }

  changePassword() {
    this.userService.sendConfirmationToChangePassword(this.loginDto.email).subscribe(() => { });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }
}
