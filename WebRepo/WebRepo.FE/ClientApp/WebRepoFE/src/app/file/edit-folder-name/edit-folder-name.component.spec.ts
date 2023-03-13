import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFolderNameComponent } from './edit-folder-name.component';

describe('EditFolderNameComponent', () => {
  let component: EditFolderNameComponent;
  let fixture: ComponentFixture<EditFolderNameComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditFolderNameComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditFolderNameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
