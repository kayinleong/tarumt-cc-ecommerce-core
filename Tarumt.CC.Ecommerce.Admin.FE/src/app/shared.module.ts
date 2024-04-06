import { NgModule } from '@angular/core';
import { LoaderComponent } from './components/loader/loader.component';
import { DateService } from './services/date.service';
import { UserFileService } from './services/user-file.service';

@NgModule({
  declarations: [LoaderComponent],
  providers: [DateService, UserFileService],
  exports: [LoaderComponent],
})
export class SharedModule {}
