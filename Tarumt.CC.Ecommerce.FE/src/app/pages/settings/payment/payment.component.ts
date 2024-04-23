import {
  Component,
  ElementRef,
  OnInit,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { Title } from '@angular/platform-browser';
import { CardService } from '../../../services/card.service';
import { UserCardRequest } from '../../../types/requests/user-card.request';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css',
})
export class PaymentComponent implements OnInit {
  @ViewChildren('cardNumber1,cardNumber2,cardNumber3,cardNumber4')
  cardNumbers: QueryList<ElementRef> | undefined;
  cardNumbersIndex = 1;

  errors: string[] = [];
  card: UserCardRequest | undefined;
  isCardFound = false;

  constructor(private title: Title, private cardService: CardService) {}

  ngOnInit(): void {
    this.title.setTitle('Payment - Tarumt.CC.Ecommerce');

    this.cardService.get().subscribe({
      next: (response) => {
        this.card = {
          ...response,
          cvv: '',
        };

        setTimeout(() => {
          for (let i = 0; i < 4; i++) {
            const start = i * 4;
            const end = start + 4;
            const part = this.card!.card_number.substring(start, end);
            this.cardNumbers!.toArray()[i].nativeElement.value = part;
          }
        }, 1000);

        this.isCardFound = true;
      },
      error: () => {
        this.card = {
          card_holder_name: '',
          card_number: '',
          expiry_date: '',
          cvv: '',
        };
      },
    });
  }

  onInputExpiryDate() {
    let expiryDate = this.card!.expiry_date;

    if (expiryDate.length === 2) {
      this.card!.expiry_date = expiryDate + '/';
    }
  }

  onInputCardNumber(event: Event) {
    let cardNumber = (event.target as HTMLInputElement).value;

    if (cardNumber.length >= 4) {
      if (this.cardNumbersIndex < 4) {
        this.cardNumbers
          ?.toArray()
          [this.cardNumbersIndex].nativeElement.focus();
        this.cardNumbersIndex++;
      }
    }
  }

  onClickCardNumber(index: number) {
    this.cardNumbersIndex = index;
  }

  onSubmit() {
    this.errors = [];
    this.card!.card_number = '';

    this.cardNumbers?.toArray().forEach((cardNumber) => {
      this.card!.card_number += cardNumber.nativeElement.value;
    });

    if (this.card!.card_holder_name === '') {
      this.errors.push('Card Holder Name is required');
    }

    if (this.card!.card_number === '') {
      this.errors.push('Card Number is required');
    }

    if (this.card!.card_number.length !== 16) {
      this.errors.push('Card Number must is invalid');
    }

    if (this.card!.expiry_date === '') {
      this.errors.push('Card Expiry Date is required');
    }

    if (
      this.card!.expiry_date.length !== 5 ||
      this.card!.expiry_date[2] !== '/'
    ) {
      this.errors.push('Card Expiry Date is invalid');
    }

    if (this.card!.cvv === '') {
      this.errors.push('Card CVC is required');
    }

    if (this.card!.cvv.length !== 3) {
      this.errors.push('Card CVC is invalid');
    }

    if (this.errors.length > 0) {
      return;
    }

    if (this.isCardFound) {
      this.cardService.update(this.card!).subscribe({
        next: () => {},
        error: (error) => {
          this.errors.push('Card credentials are invalid');
        },
      });
    } else {
      this.cardService.create(this.card!).subscribe({
        next: () => {},
        error: (error) => {
          this.errors.push('Card credentials are invalid');
        },
      });
    }
  }
}
