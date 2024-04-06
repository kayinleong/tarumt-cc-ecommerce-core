import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Flowbite } from '../../decorators/flowbite-decorator.decorator';
import { UserService } from '../../services/user.service';
import { PaginatedResponse } from '../../types/responses/paginated-response.response';
import { UserResponse } from '../../types/responses/user-response.response';
import { UserCreateRequest } from '../../types/requests/user.request';
import { DateService } from '../../services/date.service';

@Flowbite()
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent implements OnInit {
  pageNumber = 1;
  pageSize = 10;
  keyword = '';
  isDeleted = false;
  isSuspended = false;
  errors: string[] = [];
  user: UserCreateRequest = {
    username: '',
    password: '',
    first_name: '',
    last_name: '',
    gender: 0,
    date_of_birth: '',
    email: '',
    phone_number: '',
    culture: '',
    address: '',
    is_email_verified: false,
    is_phone_number_verified: false,
    is_admin: false,
    is_suspended: false,
  };
  users: PaginatedResponse<UserResponse[]> | undefined;

  constructor(
    private title: Title,
    private dateService: DateService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Users - Tarumt.CC.Ecommerce Dashboard');

    this.userService
      .getAll(
        this.pageNumber,
        this.pageSize,
        this.keyword,
        this.isDeleted,
        this.isSuspended
      )
      .subscribe({
        next: (response) => {
          this.users = response;
        },
      });
  }

  onSearch(): void {
    this.userService
      .getAll(
        this.pageNumber,
        this.pageSize,
        this.keyword,
        this.isDeleted,
        this.isSuspended
      )
      .subscribe({
        next: (response) => {
          this.users = response;
        },
      });
  }

  onCreate(): void {
    this.errors = [];

    if (this.user!.username === undefined || this.user!.username === '')
      this.errors.push('Username is required.');

    if (this.user!.first_name === undefined || this.user!.first_name === '')
      this.errors.push('First name is required.');

    if (this.user!.last_name === undefined || this.user!.last_name === '')
      this.errors.push('Last name is required.');

    if (
      this.user!.date_of_birth === undefined ||
      this.user!.date_of_birth === ''
    )
      this.errors.push('Date of birth is required.');

    if (this.user!.gender === undefined)
      this.errors.push('Gender is required.');

    if (this.user!.culture === undefined)
      this.errors.push('Culture is required.');

    if (this.user!.email === undefined || this.user!.email === '')
      this.errors.push('Email is required.');

    if (this.user!.is_email_verified === undefined)
      this.errors.push('Is email verified is required.');

    if (this.user!.phone_number === undefined || this.user!.phone_number === '')
      this.errors.push('Phone number is required.');

    if (this.user!.is_phone_number_verified === undefined)
      this.errors.push('Is phone number verified is required.');

    if (this.errors.length > 0) return;
    this.errors = [];

    this.user.date_of_birth = this.dateService.convertToServerDateFormat(
      this.user.date_of_birth
    );

    this.userService.create(this.user).subscribe({
      next: () => {
        this.onSearch();
        this.user = {
          username: '',
          password: '',
          first_name: '',
          last_name: '',
          gender: 0,
          date_of_birth: '',
          email: '',
          phone_number: '',
          culture: '',
          address: '',
          is_email_verified: false,
          is_phone_number_verified: false,
          is_admin: false,
          is_suspended: false,
        };
      },
    });
  }

  onNextPage(): void {
    if (!this.users?.has_next) return;

    this.pageNumber++;
    this.onSearch();
  }

  onPreviousPage(): void {
    if (!this.users?.has_previous) return;

    this.pageNumber--;
    this.onSearch();
  }
}
