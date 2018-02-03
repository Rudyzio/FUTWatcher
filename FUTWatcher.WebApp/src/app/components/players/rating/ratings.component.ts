import { Component, Inject } from '@angular/core';
import { ToasterService } from 'angular2-toaster/src/toaster.service';
import { AuthenticationService } from '../../../services/authentication.service';
import { PlayerOptionsService } from '../../../services/player_options.service';

@Component({
    selector: 'player-ratings',
    templateUrl: './ratings.component.html',
    styleUrls: ['./ratings.component.css']
})
export class RatingsComponent {
    players: any[] = [];
    currentRating = 83;

    ratings: number[] = [82, 83, 84, 85, 86, 87, 88, 89, 90];

    private editRow: number;
    private previousCost: number;

    loading: boolean;

    constructor(
        private toaster: ToasterService,
        @Inject('API_URL') private apiUrl,
        private authService: AuthenticationService,
        private playerOptionsService: PlayerOptionsService
    ) {
        this.onRatingChange();
    }

    onRatingChange() {
        this.authService.get(this.apiUrl + `Player/GetPlayersByRating?rating=${this.currentRating}`).subscribe(result => {
            this.players = result.json() as any[];
            this.players = this.players.filter(player => player.Color !== 'fut_champions_gold');
            this.loading = false;
        }, error => {
            // console.log(error);
        })
    }

    playerBackgroundColor(player: any) {
        this.playerOptionsService.playerBackgroundColor(player);
    }

    playerIsOwned(player: any) {
        this.playerOptionsService.playerIsOwned(player);
    }
}