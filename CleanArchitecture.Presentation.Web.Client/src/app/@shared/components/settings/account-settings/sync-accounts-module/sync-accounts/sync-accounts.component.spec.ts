import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyncAccountsComponent } from './sync-accounts.component';

describe('SyncAccountsComponent', () => {
  let component: SyncAccountsComponent;
  let fixture: ComponentFixture<SyncAccountsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SyncAccountsComponent]
    });
    fixture = TestBed.createComponent(SyncAccountsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
