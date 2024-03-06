import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AdminPageLayoutComponent } from "./shared/admin-page-layout/admin-page-layout.component";
import { LoginPageComponent } from "../login-page/login-page.component";
import { AddProductPageComponent } from "./add-product-page/add-product-page.component";
import { EditProductPageComponent } from "./edit-product-page/edit-product-page.component";
import { DashboardPageComponent } from "./dashboard-page/dashboard-page.component";
import { OrdersPageComponent } from "./orders-page/orders-page.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

@NgModule({
    
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forChild([
            { path: '', component: AdminPageLayoutComponent, children: [
                { path: '', redirectTo: '/admin/login', pathMatch: 'full'},
                { path: 'login', component: LoginPageComponent },
                { path: 'dashboard', component: DashboardPageComponent },
                { path: 'orders', component: OrdersPageComponent },
                { path: 'product/add', component: AddProductPageComponent },
                { path: 'product/:id/edit', component: EditProductPageComponent }
            ]}
        ])
    ],
    exports: [RouterModule]
})

export class AdminModule{

}