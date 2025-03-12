import { Component, Input, Renderer2, ElementRef, OnInit } from '@angular/core';
import { UserService } from '../Services/user-service';

@Component({
  selector: 'app-course-box',
  templateUrl: './course-box.component.html',
  styleUrls: ['./course-box.component.css']
})
export class CourseBoxComponent implements OnInit {
  @Input() course: any;
  trainer: any;
  punctuation: string = '4.6';

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getUserByID(this.course.trainerID).subscribe(data => {
      this.trainer = data.name;
    })
  }
}
