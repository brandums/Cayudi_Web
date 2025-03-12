import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { EpVideoplayerComponent } from './ep-videoplayer/ep-videoplayer.component';
import { FavoriteCommentComponent } from './favorite-comment/favorite-comment.component';
import { CourseBoxComponent } from './course-box/course-box.component';
import { FooterComponent } from './footer/footer.component';
import { CourseListComponent } from './course-list/course-list.component';
import { CoursePageComponent } from './course-page/course-page.component';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { CarouselComponent } from './carousel/carousel.component';
import { CourseLockComponent } from './course-lock/course-lock.component';
import { CourseCardComponent } from './course-card/course-card.component';
import { CommentPostComponent } from './comment-post/comment-post.component';
import { CreateCourseComponent } from './create-course/create-course.component';
import { CourseSaleComponent } from './course-sale/course-sale.component';
import { AnswerComponent } from './blog-post-answer/answer.component';
import { TabsComponent } from './tabs/tabs.component';
import { InstructorProfileComponent } from './instructor-profile/instructor-profile.component';
import { ThanksPageComponent } from './thanks-page/thanks-page.component';
import { PerfilComponent } from './perfil/perfil.component';
import { CourseComponent } from './course/course.component';
import { ReservationContainerComponent } from './reservation-container/reservation-container.component';
import { AuthenticationInterceptor } from './Services/middleware';
import { CreateCategoryComponent } from './create-category/create-category.component';
import { SalePageComponent } from './sale-page/sale-page.component';
import { UserListComponent } from './user-list/user-list.component';
import { CourseTestComponent } from './course-test/course-test.component';


import { CommonModule } from '@angular/common';
import { VgCoreModule } from '@videogular/ngx-videogular/core';
import { VgControlsModule } from '@videogular/ngx-videogular/controls';
import { VgOverlayPlayModule } from '@videogular/ngx-videogular/overlay-play';
import { VgBufferingModule } from '@videogular/ngx-videogular/buffering';
import { PaymentService } from './Services/payment.service';
import { FileUploaderComponent } from './file-uploader/file-uploader.component';
import { CourseFilesComponent } from './course-files/course-files.component';
import { CourseCertsComponent } from './course-certs/course-certs.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { CreateCourseTestComponent } from './create-course-test/create-course-test-component';
import { UsersManagementComponent } from './users-management/users-management.component';
import { CreateCourseCertComponent } from './create-course-cert/create-course-cert.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FavoriteCommentComponent,
    CourseBoxComponent,
    FooterComponent,
    CourseListComponent,
    CoursePageComponent,
    LoginComponent,
    CarouselComponent,
    SignupComponent,
    CourseLockComponent,
    CourseCardComponent,
    CommentPostComponent,
    CreateCourseComponent,
    CourseSaleComponent,
    AnswerComponent,
    TabsComponent,
    InstructorProfileComponent,
    ThanksPageComponent,
    PerfilComponent,
    CourseComponent,
    EpVideoplayerComponent,
    ThanksPageComponent,
    ReservationContainerComponent,
    FileUploaderComponent,
    CreateCategoryComponent,
    SalePageComponent,
    UserListComponent,
    CourseFilesComponent,
    CourseCertsComponent,
    ChangePasswordComponent,
    CourseTestComponent,
    CreateCourseTestComponent,
    UsersManagementComponent,
    CreateCourseCertComponent
  ],
  imports: [
    CommonModule,
    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'video', component: EpVideoplayerComponent, pathMatch: 'full' },
      { path: 'especialidades', component: HomeComponent },
      { path: '', component: FavoriteCommentComponent, pathMatch: 'full' },
      { path: '', component: CourseBoxComponent, pathMatch: 'full' },
      { path: '', component: FooterComponent, pathMatch: 'full' },
      { path: 'course-list/:categoryName', component: CourseListComponent },
      { path: '', component: CoursePageComponent, pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'signup', component: SignupComponent },
      { path: '', component: CarouselComponent, pathMatch: 'full' },
      { path: '', component: CourseLockComponent, pathMatch: 'full' },
      { path: '', component: CourseCardComponent, pathMatch: 'full' },
      { path: '', component: CommentPostComponent, pathMatch: 'full' },
      { path: '', component: AnswerComponent, pathMatch: 'full' },
      { path: 'create-course', component: CreateCourseComponent },
      { path: 'course-sale', component: CourseSaleComponent },
      { path: '', component: TabsComponent, pathMatch: 'full' },
      { path: '', component: InstructorProfileComponent, pathMatch: 'full' },
      { path: 'thanks-page/:courseID', component: ThanksPageComponent },
      { path: 'perfil/:userName', component: PerfilComponent },
      { path: 'course/:id/:tittle', component: CourseComponent },
      { path: '', component: ReservationContainerComponent, pathMatch: 'full' },
      { path: 'create-category', component: CreateCategoryComponent },
      { path: 'sale-page/:id', component: SalePageComponent },
      { path: 'user-list', component: UserListComponent },
      { path: 'change-password/:id/:token', component: ChangePasswordComponent },
      { path: 'users-management', component: UsersManagementComponent },
    ])
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthenticationInterceptor,
    multi: true,
  },
    PaymentService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
