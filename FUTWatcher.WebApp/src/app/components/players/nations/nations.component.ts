import { Component, Inject } from '@angular/core';
import { ToasterService } from 'angular2-toaster/src/toaster.service';
import { AuthenticationService } from '../../../services/authentication.service';
import { PlayerOptionsService } from '../../../services/player_options.service';

@Component({
    selector: 'player-nations',
    templateUrl: './nations.component.html',
    styleUrls: ['./nations.component.css']
})
export class NationsComponent {

    nations: any[] = [];
    actualNation: number;

    players: any[] = [];

    currentPage: number = 1;
    pageSize: number = 20;
    totalPlayers: number;

    private editRow: number;
    private previousCost: number;

    loading: boolean;

    constructor(
        private toaster: ToasterService,
        @Inject('API_URL') private apiUrl,
        private authService: AuthenticationService,
        private playerOptionsService: PlayerOptionsService
    ) {
        authService.get(this.apiUrl + 'Nation/All').subscribe(result => {
            this.nations = result.json() as any[];
            this.currentPage = 1;
            this.actualNation = this.nations[0].Id;
            this.onNationChange();
        }, error => {
            // console.log(error);
        });
    }

    getPage(page: number) {
        if (isNaN(page)) {
            this.currentPage++;
        }
        else {
            this.currentPage = page;
        }
        this.loading = true;
        this.onNationChange();
    }

    onNationChange() {
        this.authService.get(this.apiUrl + `Player/GetNationPage?pageIndex=${this.currentPage}&pageSize=${this.pageSize}&nationId=${this.actualNation}`).subscribe(result => {
            this.players = result.json().Data as any[];
            this.totalPlayers = result.json().Total;
            this.players = this.players.filter(player => player.Color !== 'fut_champions_gold');
            this.loading = false;
        }, error => {
            // console.log(error);
        })
    }

    playerBackgroundColor(player: any) {
        return this.playerOptionsService.playerBackgroundColor(player);
    }

    playerIsOwned(player: any) {
        return this.playerOptionsService.playerIsOwned(player);
    }
}