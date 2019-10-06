import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-tag-giphy-dialog',
  templateUrl: './tag-giphy-dialog.component.html',
})
export class TagGiphyDialogComponent {
  selectedTag: string;

  constructor(
    public dialogRef: MatDialogRef<TagGiphyDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public tags: string[]) { }

  onBackClick(): void {
    this.dialogRef.close();
  }
}
