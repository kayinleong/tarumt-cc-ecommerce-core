import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { DetailsComponent } from './details/details.component';

export const routes: Routes = [
  {
    path: ':id',
    component: DetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProductRoutingModule {}
