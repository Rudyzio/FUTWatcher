import { Component } from '@angular/core';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { AuthenticationService } from '../../services/authentication.service';
import * as $ from 'jquery';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent implements OnInit {
    _loggedIn: boolean = false;

    userLoggedInSub: any;

    constructor(
        private authService: AuthenticationService,
    ) {
    }

    ngOnInit() {
        if (localStorage.getItem('currentUser')) {
            // logged in so return true
            this._loggedIn = true;
        }

        this.userLoggedInSub = this.authService.userLoggedInEvent.subscribe(user => {
            if (user != undefined) {
                this._loggedIn = true;
            }
        })
    }

    toggleNavBar() {
        $('.navbar-toggle').click();
    }

    logout() {
        this._loggedIn = false;
    }
}
