import { Routes } from '@angular/router';
import { MainLayoutComponent } from './shared/main-layout/main-layout.component';
import { MainPageComponent } from './main-page/main-page.component';
import { ProductPageComponent } from './product-page/product-page.component';
import { CartPageComponent } from './cart-page/cart-page.component';
import { AdminModule } from './admin/admin.module';

export const routes: Routes = [
    { path: '', component: MainLayoutComponent, children: [
        { path: '', redirectTo: '/main', pathMatch: 'full' },
        { path: 'main', component: MainPageComponent },
        { path: 'product/:id', component: ProductPageComponent },
        { path: 'cart', component: CartPageComponent }
    ] },
    { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule) }
];
