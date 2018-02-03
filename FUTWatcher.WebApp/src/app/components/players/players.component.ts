import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { ToasterService } from 'angular2-toaster/src/toaster.service';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
    selector: 'players',
    templateUrl: './players.component.html',
    styleUrls: ['./players.component.css']
})
export class PlayersComponent {

    totalCost: number = 0;
    totalPlayers: number = 0;
    totalOwnedPlayers: number = 0;

    constructor(
        private toaster: ToasterService,
        @Inject('API_URL') private apiUrl,
        private authService: AuthenticationService
    ) {
        this.getTotalPlayersInformation();
    }

    getTotalPlayersInformation() {
        this.authService.get(this.apiUrl + 'Player/TotalplayersInformation').subscribe(result => {
            this.totalCost = result.json().TotalCost;
            this.totalPlayers = result.json().TotalPlayers;
            this.totalOwnedPlayers = result.json().TotalOwnedPlayers;
        }, error => {
            // console.log(error);
        })
    }
}