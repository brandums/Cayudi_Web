import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Payment } from '../models/payment';
import { environment } from '../../environments/environment';

@Injectable()
export class PaymentService {
  private apiUrl = environment.apiUrl + 'Payment';

  constructor(private http: HttpClient) { }

  createCheckoutSession(payment: Payment) {
    return this.http.post(this.apiUrl, payment);
  }
}
