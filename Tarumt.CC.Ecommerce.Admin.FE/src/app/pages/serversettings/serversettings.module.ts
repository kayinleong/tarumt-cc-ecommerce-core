import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ServersettingsComponent } from './serversettings.component';
import { ServerSettingsRoutingModule } from './serversetings.routes';
import { FormsModule } from '@angular/forms';
import { ServerSettingsService } from '../../services/server-settings.service';
import { SharedModule } from '../../shared.module';

@NgModule({
  declarations: [ServersettingsComponent],
  providers: [ServerSettingsService],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    ServerSettingsRoutingModule,
  ],
})
export class ServersettingsModule {}
