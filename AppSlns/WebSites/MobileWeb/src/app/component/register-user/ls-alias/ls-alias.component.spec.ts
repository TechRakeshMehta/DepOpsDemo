import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LsAliasComponent } from './ls-alias.component';

describe('LsAliasComponent', () => {
  let component: LsAliasComponent;
  let fixture: ComponentFixture<LsAliasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LsAliasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LsAliasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
