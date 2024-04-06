import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Flowbite } from '../../../decorators/flowbite-decorator.decorator';
import { DateService } from '../../../services/date.service';
import { UserService } from '../../../services/user.service';
import { UserResponse } from '../../../types/responses/user-response.response';

@Flowbite()
@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrl: './details.component.css',
})
export class DetailsComponent implements OnInit {
  id: string | undefined;
  errors: string[] = [];
  user: UserResponse | undefined;

  constructor(
    private title: Title,
    private route: ActivatedRoute,
    private router: Router,
    private dateService: DateService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('User Details - Tarumt.CC.Ecommerce Dashboard');

    this.route.paramMap.subscribe((params) => {
      this.id = params.get('id')!;

      this.route.queryParamMap.subscribe((queryParams) => {
        let is_deleted = /^true$/i.test(queryParams.get('is_deleted')!);
        let is_suspended = /^true$/i.test(queryParams.get('is_suspended')!);

        this.userService
          .getById(this.id!, is_deleted, is_suspended)
          .subscribe((user) => {
            this.user = user;
            this.user.date_of_birth = this.dateService.convertToHtmlDateFormat(
              new Date(user.date_of_birth).toLocaleDateString('en-US')
            );
          });
      });
    });
  }

  onSubmit() {
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

    this.userService
      .updateById(this.id!, {
        username: this.user!.username,
        first_name: this.user!.first_name,
        last_name: this.user!.last_name,
        gender: this.user!.gender,
        date_of_birth: this.dateService.convertToServerDateFormat(
          this.user!.date_of_birth
        ),
        email: this.user!.email,
        phone_number: this.user!.phone_number,
        culture: this.user!.culture,
        is_email_verified: this.user!.is_email_verified,
        is_phone_number_verified: this.user!.is_phone_number_verified,
        address: this.user!.address ?? 'N/A',
        is_admin: this.user!.is_admin,
        is_suspended: this.user!.is_suspended,
      })
      .subscribe({
        next: () => {
          this.router.navigate(['/dashboard/user']);
        },
        error: (err) => {},
      });
  }

  onDelete() {
    this.userService.deleteById(this.id!).subscribe({
      next: () => {
        this.router.navigate(['/dashboard/user']);
      },
      error: (err) => {},
    });
  }
}
