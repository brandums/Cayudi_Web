import { Component, OnInit } from '@angular/core';
import { CourseCategoryService } from '../Services/course-category-service';

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
  styleUrls: ['./create-category.component.css']
})
export class CreateCategoryComponent {
  category: any = {
    Name: '',
    ImagePath: '',
  };

  constructor(
    private courseCategoryService: CourseCategoryService,
  ) { }

  removeInvalidClass(): void {
    const inputElement = document.getElementById('Name') as HTMLInputElement | null;
    inputElement?.classList.remove('invalid');
  }

  validationForm(): void {
    const input = document.getElementById('Name') as HTMLInputElement | null;
    let isValid = true;

    if (input && input.value.trim() === '') {
      isValid = false;
      input.classList.add('invalid');
    } else if(input){
      input.classList.remove('invalid');
    }
    if (isValid) {
      this.createCategory();
    }
  }

  cleanForm() {
    this.category.Name = ""
  }

  createCategory() {
    this.courseCategoryService.createCategory(this.category).subscribe(() => {
      this.courseCategoryService.newDataAdded();
      const button = document.getElementById('bClose');
      if (button instanceof HTMLElement) {
        button.click();
      }
    });
  }
}
