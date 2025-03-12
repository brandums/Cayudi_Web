import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserService } from '../Services/user-service';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit, OnDestroy {
  user: any;
  subscription: Subscription = new Subscription;
  password = "";
  repeatPassword = "";
  isChanged: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
  ) { }

  ngOnInit(): void {
    this.subscription = this.route.params.subscribe(params => {
      const id = params['id'];
      const token = params['token'];

      this.userService.getUserByID(id).subscribe(data => {
        if (data.confirmationToken == token) {
          this.user = data;
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onChangePassword() {
    const patchData = [
      {
        "op": "replace",
        "path": "Password",
        "value": this.password
      }
    ];

    this.userService.updatePassword(this.user.id, patchData)
      .subscribe(response => {
        console.log('Cambios en la contraseña guardados con éxito:', response);
        this.repeatPassword = "";
        this.password = "";
        this.isChanged = true;
      }, error => {
        console.error('Error al guardar cambios en la contraseña:', error);
      });
  }
}
