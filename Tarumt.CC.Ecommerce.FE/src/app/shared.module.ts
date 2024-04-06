import { NgModule } from '@angular/core';
import { LoaderComponent } from './components/loader/loader.component';
import { DateService } from './services/date.service';

@NgModule({
  declarations: [LoaderComponent],
  providers: [DateService],
  exports: [LoaderComponent],
})
export class SharedModule {}
