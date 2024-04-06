import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ServersettingsComponent } from './serversettings.component';

export const routes: Routes = [
  {
    path: '',
    component: ServersettingsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ServerSettingsRoutingModule {}
