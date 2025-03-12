import { Component, OnDestroy, OnInit } from '@angular/core';
import { LoginService } from '../Services/login.service';
import { UserService } from '../Services/user-service';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, filter } from 'rxjs';
import { fileDataDTO } from '../models/fileDataDTO';
import { HttpEvent, HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css']
})
export class PerfilComponent implements OnInit, OnDestroy {
  user: any;
  isEditingName = false;
  isEditingEmail = false;
  isEditingPhoneNumber = false;
  isEditingProfession = false;
  isEditingDescription = false;
  isEditingPassword = false;
  editedText = '';
  subscription: Subscription = new Subscription;
  error = false;
  userIcon = 1;
  userCert = 2;
  fileInfo: fileDataDTO;
  fileInfo_images: fileDataDTO;
  displayStyle = "none";

  userId = 0;
  userLoggedIn: any;

  images: any[] = [];
  current: any = ""
  deletedFile: any = ""
  defLink: any;
  currentImage: any = "";
  userSubscription: Subscription | undefined;

  constructor(
    private route: ActivatedRoute,
    private loginService: LoginService,
    private userService: UserService,
    private router: Router,
  ) {
    this.fileInfo = new fileDataDTO(0, 1, 0, 1, 'image/*')
    this.fileInfo_images = new fileDataDTO(0, 1, 0, 2, 'image/*')
  } 

  ngOnInit(): void {
    this.displayStyle = "none";
    this.route.params.subscribe(params => {
      const state = history.state;
      this.userId = state.userId;

      if (this.userId != null && this.userId != 0) {
        this.userService.getUserByID(this.userId).subscribe(user => {
          this.user = user
        })
      }
      else {
        this.loginService.$userData.subscribe(user => {
          this.user = user.userData;
        })
      }
    })

    this.loginService.checkAuthentication()

    this.subscription = this.loginService.user$.subscribe(user => {
      if (!user) {
        if (!user && this.error) {
          this.router.navigate(['/']);
        }
      }
      else {
        this.userLoggedIn = user;
        this.fileInfo.sourceId = user.id;
        this.fileInfo_images.sourceId = user.id;

      }
    });

    this.userService._images.subscribe(data => {
      this.images = data
    })

    this.userService.updateImagesList(this.userId);

    this.userSubscription = this.loginService.user$.pipe(filter((user: any) => user !== undefined)).subscribe(user => {
      if (!user || user.roleID != 1) {
        this.router.navigate(['/']);
      }
    });
  }

  startEditing(field: string) {
    if (this.user.id == this.userLoggedIn.id) {
      if (field === 'name') {
        this.editedText = this.user.name;
        this.isEditingName = true;
      } else if (field === 'email') {
        this.editedText = this.user.email;
        this.isEditingEmail = true;
      } else if (field === 'phoneNumber') {
        this.editedText = this.user.phoneNumber;
        this.isEditingPhoneNumber = true;
      } else if (field === 'profession') {
        this.editedText = this.user.profession;
        this.isEditingProfession = true;
      } else if (field === 'description') {
        this.editedText = this.user.description;
        this.isEditingDescription = true;
      }
    }
  }

  handleEditClick(event: Event) {
    event.stopPropagation();
  }
  handleClick() {
    this.isEditingName = false;
    this.isEditingEmail = false;
    this.isEditingPhoneNumber = false;
    this.isEditingProfession = false;
    this.isEditingDescription = false;
  }

  saveChanges(field: string) {
    const patchData = [
      {
        "op": "replace",
        "path": field,
        "value": this.editedText
      }
    ];

    this.userService.updateUser(this.user.id, patchData)
      .subscribe(response => {
        console.log('Cambios en la descripción guardados con éxito:', response);

        if (field === '/name') {
          this.user.name = this.editedText;
          this.isEditingName = false;
        } else if (field === '/email') {
          this.user.email = this.editedText;
          this.isEditingEmail = false;
        } else if (field === '/phoneNumber') {
          this.user.phoneNumber = this.editedText;
          this.isEditingPhoneNumber = false;
        } else if (field === '/profession') {
          this.user.profession = this.editedText;
          this.isEditingProfession = false;
        } else if (field === '/description') {
          this.user.description = this.editedText.trim();
          this.isEditingDescription = false;
        } else if (field === 'isVerified') {
          this.user.isVerified = this.editedText;
          this.loginService.checkAuthentication()

        }
      }, error => {
        console.error('Error al guardar cambios en la descripción:', error);
      });
  }
    
  openModal(index: any) {
    this.currentImage = this.images[index]
    this.displayStyle = "block";
  }
  closeModal() {
    this.displayStyle = "none";
  }

  openConfModal(index: any) {
    this.currentImage = this.images[index]
  }

  delete() {
    if (this.currentImage ) {
      try {
        this.userService.deleteImage(this.currentImage.id).subscribe((event: HttpEvent<any>) => {
          switch (event.type) {
            case HttpEventType.Sent:
              console.log('Request has been made!');
              break;
            case HttpEventType.ResponseHeader:
              console.log('Response header has been received!');
              break;
            case HttpEventType.Response:
              console.log('User successfully deleted!', event.body);
              this.userService.updateImagesList(this.userId)
              setTimeout(() => {
              }, 1500);
          }
        })
      }
      catch (e) {
        console.log("Errror while trying to switch read only value", e)
      }
    }
  }

  verifyUser() {
    this.editedText = "true"
    this.saveChanges("isVerified")
  }
  removeUserVerification() {
    this.editedText = "false"
    this.saveChanges("isVerified")
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();

    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }
}
