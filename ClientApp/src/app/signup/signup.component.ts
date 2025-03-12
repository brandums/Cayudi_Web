import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Register } from '../models/register';
import { AuthenticationService } from '../Services/authentication.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  signupForm: FormGroup;
  showPassword2: boolean = false;
  errorMessages: string[] = [];
  formSubmitted: boolean = false;
  isLoading: boolean = false;

  constructor(private formBuilder: FormBuilder, private authService: AuthenticationService) {
    this.signupForm = this.formBuilder.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&.]{8,}$/)
      ]],
      confirmPassword: ['', Validators.required],
    }, {
      validator: this.passwordMatchValidator,
    });
  }

  togglePasswordVisibility() {
    this.showPassword2 = !this.showPassword2;
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;

    if (password === confirmPassword) {
      return null;
    } else if (password != '') {
      form.get('confirmPassword')?.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
    return null;
  }

  register(form: FormGroup) {
    this.formSubmitted = true;
    if (form.get('confirmPassword')?.value === '') {
      form.get('confirmPassword')?.setErrors({ required: true });
    }

    if (form.valid) {
      this.isLoading = true;
      const registerDto: Register = form.value;
      this.authService.register(registerDto).subscribe(
        (response) => {
          console.log('Respuesta del servidor', response);
          this.isLoading = false;
          const button = document.querySelector('.close-signup');
          if (button instanceof HTMLElement) {
            button.click();
          }
        },
        (error) => {
          console.log('Error del servidor', error);
          this.isLoading = false;
          if (error.status === 400) {
            // Errores de validación del formulario
            this.errorMessages = [error.error.userMessage];
          }
        }
      );
    }
  }
}
