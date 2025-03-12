  export class fileDataDTO {
    sourceId = 0;
    objectType = 0;
    courseSourceType = 0;
    imgType = 0;
    imputType = '';

    constructor(sID: number, oType: number, cSType: number, iType: number, _inputType: string) {
      this.sourceId = sID;
      this.objectType = oType;
      this.courseSourceType = cSType;
      this.imgType = iType;
      this.imputType = _inputType;
    }

  }
