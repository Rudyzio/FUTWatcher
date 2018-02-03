import { Component, Inject } from "@angular/core";
import { ToasterService } from 'angular2-toaster/src/toaster.service';
import { AuthenticationService } from "../../services/authentication.service";

@Component({
    selector: 'search',
    templateUrl: './search.component.html'
})
export class SearchComponent {

    players: any[] = [];

    playerName: string;

    constructor(
        private toaster: ToasterService,
        @Inject('API_URL') private apiUrl,
        private authService: AuthenticationService
    ) {
    }

    searchPlayer() {
        this.authService.get(this.apiUrl + 'Player/NameHas/?name=' + this.playerName).subscribe(result => {
            this.players = result.json() as any[];
            this.players = this.players.filter(player => player.Color !== 'fut_champions_gold');
            this.players = this.players.filter(player => player.Color !== 'fut_champions_silver');
        }, error => {
            // console.log(error);
        });
    }
}