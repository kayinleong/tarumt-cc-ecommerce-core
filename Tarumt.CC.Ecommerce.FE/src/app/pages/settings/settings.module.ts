import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingsComponent } from './settings.component';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared.module';
import { SettingsRoutingModule } from './settings.routes';
import { PaymentComponent } from './payment/payment.component';

@NgModule({
  declarations: [SettingsComponent, PaymentComponent],
  imports: [CommonModule, FormsModule, SharedModule, SettingsRoutingModule],
})
export class SettingsModule {}
