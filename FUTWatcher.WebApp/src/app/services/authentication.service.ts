import { Injectable, Inject, EventEmitter } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

@Injectable()
export class AuthenticationService {

    userLoggedInEvent: EventEmitter<any> = new EventEmitter<any>();

    constructor(
        private http: Http,
        @Inject('API_URL') private apiUrl
    ) { }

    login(username: string, password: string) {
        return this.http.post(this.apiUrl + 'users/authenticate', { username: username, password: password })
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                let user = response.json();
                if (user && user.Token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                    this.userLoggedInEvent.emit(user);
                }
            });
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.userLoggedInEvent.emit();
    }

    get(url: string): Observable<Response> {
        return this.http.get(url, this.appendHeaders());
    }

    put(url: string, data: any): Observable<Response> {
        let body = JSON.stringify(data);
        return this.http.put(url, body, this.appendHeaders());
    }

    post(url: string, data: any): Observable<Response> {
        let body = JSON.stringify(data);
        return this.http.post(url, body, this.appendHeaders());
    }

    delete(url: string): Observable<Response> {
        return this.http.delete(url, this.appendHeaders());
    }

    private appendHeaders() {
        let headers = new Headers({ 'Content-Type': 'application/json' });

        // create authorization header with jwt token
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser && currentUser.Token) {
            headers.append('Authorization', 'Bearer ' + currentUser.Token);
        }
        
        return new RequestOptions({ headers: headers });
    }
}