import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnityGameLoaderComponent } from './unity-game-loader.component';

describe('UnityGameLoaderComponent', () => {
  let component: UnityGameLoaderComponent;
  let fixture: ComponentFixture<UnityGameLoaderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UnityGameLoaderComponent]
    });
    fixture = TestBed.createComponent(UnityGameLoaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
