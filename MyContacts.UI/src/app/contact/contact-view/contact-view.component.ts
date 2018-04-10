import { Component, OnInit } from '@angular/core';
import { ApiService } from "../../services/api.service";
import { ApiResult, Contact } from "../../models/general";

//Angular Material
import { DataSource } from '@angular/cdk/collections';

@Component({
  selector: 'app-contact-view',
  templateUrl: './contact-view.component.html',
  styleUrls: ['./contact-view.component.css']
})
export class ContactViewComponent implements OnInit {

  contactsList: Contact[] = [];
  displayedColumns = ['username', 'email', 'phonenumber'];
  dataSource;

  constructor(private apiservice: ApiService) { }

  ngOnInit() {
    this.dataSource = this.GetContactsList();
  }

  GetContactsList(): void {
    this.apiservice.Contacts.GetContacts()
      .subscribe((apiResult: ApiResult<Contact[]>) => {
        debugger;
        this.contactsList = apiResult.Result;
      });
  }
}
