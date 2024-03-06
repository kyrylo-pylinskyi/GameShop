import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-page-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  templateUrl: './admin-page-layout.component.html'
})
export class AdminPageLayoutComponent {

}
