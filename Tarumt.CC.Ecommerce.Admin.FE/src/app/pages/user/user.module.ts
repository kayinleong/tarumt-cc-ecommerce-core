import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared.module';
import { UserRoutingModule } from './user.routes';

// Components
import { UserComponent } from './user.component';
import { DetailsComponent } from './details/details.component';

// Services
import { UserService } from '../../services/user.service';

@NgModule({
  declarations: [UserComponent, DetailsComponent],
  providers: [UserService],
  imports: [CommonModule, FormsModule, SharedModule, UserRoutingModule],
})
export class UserModule {}
