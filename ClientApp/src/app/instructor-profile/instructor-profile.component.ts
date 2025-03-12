import { Component, Input, OnInit } from '@angular/core';
import { UserService } from '../Services/user-service';

@Component({
  selector: 'app-instructor-profile',
  templateUrl: './instructor-profile.component.html',
  styleUrls: ['./instructor-profile.component.css']
})
export class InstructorProfileComponent implements OnInit {
  @Input() trainerID: any;
  user: any;

  constructor(
    private userService: UserService,
  ) { }

  ngOnInit() {
    if (this.trainerID) {
      this.userService.getUserByID(this.trainerID).subscribe(data => {
        this.user = data;
      })
    }
  }
}
