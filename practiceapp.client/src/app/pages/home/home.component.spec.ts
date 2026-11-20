import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { HomeComponent } from './home.component';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HomeComponent],
      imports: [HttpClientTestingModule, RouterTestingModule]
    }).compileComponents();

    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should load the API status', () => {
    fixture.detectChanges();

    const req = httpMock.expectOne('/api/home');
    expect(req.request.method).toEqual('GET');
    req.flush({ title: 'AI Practice App', message: 'Ready to practice.' });

    expect(component.loaded).toBeTrue();
    expect(component.message).toEqual('Ready to practice.');
    expect(component.error).toEqual('');
  });

  it('should show an error when the API is unreachable', () => {
    fixture.detectChanges();

    httpMock.expectOne('/api/home').flush(null, { status: 504, statusText: 'Gateway Timeout' });

    expect(component.loaded).toBeTrue();
    expect(component.error).not.toEqual('');
  });
});
