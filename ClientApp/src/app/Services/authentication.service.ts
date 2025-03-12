import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Login } from '../models/login'
import { Register } from '../models/register'
import { JwtAuth } from '../models/jwtAuth'
import { Observable, map } from 'rxjs'
import { environment } from 'src/environments/environment'
import { LoginService } from './login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  regURL = 'User'
  loginURL = 'User/Login'

  constructor(
    private http: HttpClient,
    private loginService: LoginService,
  ) { }

  public register(user: Register): Observable<JwtAuth> {
    return this.http.post<JwtAuth>(`${environment.apiUrl}${this.regURL}`, user);
  }

  public login(user: Login): Observable<JwtAuth> {

    return this.http.post<JwtAuth>(`${environment.apiUrl}${this.loginURL}`, user)
      .pipe(
        map(jwtAuth => {
          localStorage.setItem('jwtToken', jwtAuth.token);
          this.loginService.checkAuthentication();
          return jwtAuth;
        })
      );
  }

  public logout() {
    localStorage.removeItem('jwtToken');
    this.loginService.checkAuthentication();
  }
}

