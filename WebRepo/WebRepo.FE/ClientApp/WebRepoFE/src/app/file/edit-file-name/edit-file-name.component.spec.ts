import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFileNameComponent } from './edit-file-name.component';

describe('EditFileNameComponent', () => {
  let component: EditFileNameComponent;
  let fixture: ComponentFixture<EditFileNameComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditFileNameComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditFileNameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
