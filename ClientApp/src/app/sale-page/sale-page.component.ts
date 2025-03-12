import { Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseService } from '../Services/course-service';
import { Payment } from '../models/payment';
import { PaymentService } from '../Services/payment.service';
import { LoginService } from '../Services/login.service';
import { Course } from '../models/course';

@Component({
  selector: 'app-sale-page',
  templateUrl: './sale-page.component.html',
  styleUrls: ['./sale-page.component.css']
})
export class SalePageComponent implements OnInit {
  course: Course = new Course;
  user: any;
  opcionActiveMode: string | null = null;
  isBuyButtonDisabled: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private paymentService: PaymentService,
    private loginService: LoginService,
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];

      this.loginService.user$.subscribe(user => {
        this.user = user;
      });

      this.courseService.getCourseByID(id).subscribe(data => {
        this.course = data;
      });
    });
  }

  buyCourse() {
    const payment: Payment = {
      Amount: this.course.price,
      UserID: this.user.id,
      Email: this.user.email,
      CourseID: this.course.id,
      Title: this.course.tittle,
      Nit: 123,
      RazonSocial: 'Rivero',
      Price: this.course.price,
      IsOnline: (this.opcionActiveMode === 'virtual') ? true : false,
      IsOppened: true,
    };

    this.paymentService.createCheckoutSession(payment)
      .subscribe((response: any) => {

        if (response.url_pasarela_pagos) {
          window.location.href = response.url_pasarela_pagos;
        }
      })
  }

  changeOptionMode(option: string) {
    this.opcionActiveMode = this.opcionActiveMode === option ? null : option;
    this.updateBuyButtonState();
  }

  updateBuyButtonState() {
    this.isBuyButtonDisabled = !this.opcionActiveMode;
  }
}
