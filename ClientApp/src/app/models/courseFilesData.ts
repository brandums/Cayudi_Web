export class CourseFilesData {
  src: string = '';
  type: string = 'application/pdf';
  name: string = '';

  constructor(src: string, name: string)
  {
    this.src = src;
    this.name = name;
  }
}
