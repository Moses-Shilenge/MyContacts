import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { environment } from "../../environments/environment";
import { ApiResult, BearerDetails, Contact } from "../models/general";

@Injectable()
export class ApiService {
  Bearer: string = localStorage.getItem('bearer');
  Session: SessionController;
  Contacts: ContactsController;
  constructor(private http: Http) {
    var headers =
      new Headers({ 'APIKey': '428f24f8-dad6-4d0d-8f3e-d38ca41ab491' });

    if (this.Bearer)
      headers.append('Authorization', `Bearer ${this.Bearer}`);

    headers.append('Accept', `application/json`);
    var config = new ControllerConfig(http, headers);
    this.Session = new SessionController(config, "auth");
    this.Contacts = new ContactsController(config, "contacts");
  }
}

export class Controller {
  protected headers: Headers;
  protected http: Http;
  protected url: string = environment.api;
  constructor(config: ControllerConfig, endpoint: string) {
    this.http = config.http;
    this.headers = config.headers;
    this.url += endpoint;
  }
  private ErrorCheck(result: ApiResult<any>): void {
    if (result.Error) {
      Observable.throw(result.Error);
    }
  }
  protected Get<T>(path: string = "", globalOverlay: boolean = true, OnError: () => void = null): Observable<ApiResult<T>> {
    return this.http.get(this.url + path, { headers: this.headers })
      .map(response => {
        var data = (response.json() as ApiResult<T>);
        return data
      });
  }
  protected Post<T>(path: string = "", body: any, globalOverlay: boolean = true, OnError: () => void = null): Observable<ApiResult<T>> {
    return this.http.post(this.url + path, body, { headers: this.headers })
      .map(response => {
        var data = (response.json() as ApiResult<T>);
        return data;
      });
  }
  protected Put<T>(path: string = "", body: any, globalOverlay: boolean = true, OnError: () => void = null): Observable<ApiResult<T>> {
    return this.http.put(this.url + path, body, { headers: this.headers })
      .map(response => {
        var data = (response.json() as ApiResult<T>);
        return data;
      });
  }
  protected Delete<T>(path: string = "", globalOverlay: boolean = true, OnError: () => void = null): Observable<ApiResult<T>> {
    return this.http.delete(this.url + path, { headers: this.headers })
      .map(response => {
        var data = (response.json() as ApiResult<T>);
        return data
      });
  }
}

export class ControllerConfig {
  headers: Headers;
  http: Http;
  constructor(http: Http, headers: Headers) {
    this.headers = headers;
    this.http = http;
  }
}

export class SessionController extends Controller {
  GetBearerDetails(code: string): Observable<ApiResult<BearerDetails>> {
    return this.Get<BearerDetails>(``);
  }
  RegisterUser(code: string): Observable<ApiResult<BearerDetails>> {
    return this.Get<BearerDetails>(`/create`);
  }
}

export class ContactsController extends Controller {
  GetContacts(): Observable<ApiResult<Contact[]>> {
    return this.Get<Contact[]>('');
  }
  DeleteContact(email: string): Observable<ApiResult<Contact>> {
    return this.Delete<Contact>(`/delete/${email}`);
  }
  GetContactByEmail(email: string): Observable<ApiResult<Contact>> {
    return this.Get<Contact>(`/${email}`);
  }
  CreateContact(identity: string, contact: Contact): Observable<ApiResult<Contact>> {
    return this.Post<Contact>(`/create`, contact);
  }
  EdiContact(email: string, contact: Contact): Observable<ApiResult<Contact>> {
    return this.Put<Contact>(`/update/${email}`, contact);
  }
}
