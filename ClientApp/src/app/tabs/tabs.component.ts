import { Component, ElementRef, EventEmitter, Input, OnInit } from '@angular/core';
import { UserCourseService } from '../Services/UserCourse.service';
import { ViewChild } from '@angular/core';
import { LoginService } from '../Services/login.service';
import { CourseCreationService } from '../Services/course-creation-service';

@Component({
  selector: 'app-tabs',
  templateUrl: './tabs.component.html',
  styleUrls: ['./tabs.component.css']
})
export class TabsComponent implements OnInit {
  @ViewChild('tab1') tab1Ref!: ElementRef;
  @ViewChild('tab2') tab2Ref!: ElementRef;
  @ViewChild('tab4') tab4Ref!: ElementRef;

  @Input() activeTab: string = '';
  @Input() courseId: number = 0;
  @Input() userId: number = 0;
  videoTitle: string = "";
  isAdmin = false;

  constructor(
    private userCourseService: UserCourseService,
    private loginService: LoginService,
    private emitServices: CourseCreationService
  ) { }

  ngOnInit(): void {
    this.emitServices.emitingChanges$.subscribe(() => {
      this.changeButtonsVisibility();
    });
  }

  ngAfterViewInit() {
    this.loginService.user$.subscribe(user => {
      if (user.roleID == 1) {
        this.isAdmin = true;
        this.tab4Ref.nativeElement.style.visibility = 'visible';
      } else {
        this.isAdmin = false;
        this.changeButtonsVisibility();
      }
    });
  }

  changeButtonsVisibility() {
    this.userCourseService.getUserCourseByID(this.courseId, this.userId).subscribe(data => {

      if (data && data.isFinished) {
        this.tab1Ref.nativeElement.classList.add('disabled');
        this.tab2Ref.nativeElement.classList.add('disabled');
        this.tab4Ref.nativeElement.style.visibility = 'visible';

        this.showTab("tab3");
      }
      else {
        this.tab1Ref.nativeElement.classList.remove('disabled');
        this.tab2Ref.nativeElement.classList.remove('disabled');
        this.tab4Ref.nativeElement.style.visibility = 'hidden';
      }
    });
  }
  
  showTab(tab: string): void {
      this.activeTab = tab;
  }
}
