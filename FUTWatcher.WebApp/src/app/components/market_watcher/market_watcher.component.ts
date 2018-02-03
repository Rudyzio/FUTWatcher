import { Component, Inject } from '@angular/core';
import { ToasterService } from 'angular2-toaster/src/toaster.service';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
    selector: 'market-watcher',
    templateUrl: './market_watcher.component.html',
    styleUrls: ['./market_watcher.component.css']
})
export class MarketWatcherComponent {

    constructor(
        private toaster: ToasterService,
        @Inject('API_URL') private apiUrl,
        private authService: AuthenticationService
    ) {
        
    }
}