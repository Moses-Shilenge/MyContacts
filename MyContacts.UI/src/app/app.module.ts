import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { ContactViewComponent } from './contact/contact-view/contact-view.component';
import { ContactDeleteComponent } from './contact/contact-delete/contact-delete.component';
import { ContactCreateComponent } from './contact/contact-create/contact-create.component';
import { ContactUpdateComponent } from './contact/contact-update/contact-update.component';
import { LoginViewComponent } from './authenticate/login-view/login-view.component';
import { Routes, RouterModule } from "@angular/router";

//Material Design
import { CdkTableModule } from "@angular/cdk/table";

const routes: Routes = [
  { path: '', redirectTo: '/contact', pathMatch: 'full' },
  { path: 'contact', component: ContactViewComponent },
  { path: 'contact_create', component: ContactCreateComponent },
  { path: 'contact_delete', component: ContactDeleteComponent },
  { path: 'contact_update', component: ContactUpdateComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    ContactViewComponent,
    ContactDeleteComponent,
    ContactCreateComponent,
    ContactUpdateComponent,
    LoginViewComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    CdkTableModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
