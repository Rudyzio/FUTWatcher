import { Component, Inject, Input } from '@angular/core';
import { ToasterService } from 'angular2-toaster/src/toaster.service';
import { AuthenticationService } from '../../services/authentication.service';
import { PlayerOptionsService } from '../../services/player_options.service';

@Component({
    selector: 'players-table',
    templateUrl: './players_table.component.html',
    styleUrls: ['./players_table.component.css']
})
export class PlayersTableComponent {

    @Input() players: any[] = [];
    @Input() filterIdName: string;
    @Input() showLeague: boolean;
    @Input() showClub: boolean;
    @Input() showNation: boolean;
    @Input() showFilters: boolean;
    @Input() showSearch: boolean;

    positionsMap: Map<string, number> = new Map([['GK', 1], ['RB/RWB', 1], ['CB', 3], ['LB/LWB', 1], ['RM/RW/RF', 1], ['CDM/CM/CAM', 4], ['LM/LW/LF', 1], ['CF/ST', 2]]);

    filteredPlayers: any[] = [];
    searchPlayerString: string;

    editRow: number;
    previousCost: number;

    showAll: boolean = true;
    cardTypeFilter: string[] = [];
    cardTypeFilterBoolean: boolean = false;

    ownedPlayersFilter: boolean = false;
    notOwnedPlayersFilter: boolean = false;
    missingPositionsFilterBoolean: boolean = false;

    constructor(
        private toaster: ToasterService,
        @Inject('API_URL') private apiUrl,
        private authService: AuthenticationService,
        private playerOptionsService: PlayerOptionsService
    ) {
    }

    ngOnChanges() {
        this.filteredPlayers = this.players;
        this.searchPlayerString = '';
        if (this.missingPositionsFilterBoolean) {
            this.constructMissingPositions();
        }
        if (this.ownedPlayersFilter) {
            this.constructOwnedPlayers();
        }
        if (this.notOwnedPlayersFilter) {
            this.constructNowOwnedPlayers();
        }
    }

    playerBackgroundColor(player: any) {
        this.playerOptionsService.playerBackgroundColor(player);
    }

    playerIsOwned(player: any) {
        this.playerOptionsService.playerIsOwned(player);
    }

    onSavePlayer(player: any) {
        this.authService.post(this.apiUrl + "Player/Update", player).subscribe(result => {
            this.editRow = 0;
            this.toaster.pop('success', 'Success', 'Player Saved');
        }, error => {
            this.toaster.pop('error', 'Error', 'The Player could not be saved');
        })
    }

    onFilterCardType(event: any, cardType: string) {
        if (event.target.checked) {
            this.cardTypeFilter.push(cardType);
            this.cardTypeFilterBoolean = true;
            this.showAll = false;
        }
        else {
            this.cardTypeFilter = this.cardTypeFilter.filter(item => item !== cardType);
            if (this.cardTypeFilter.length == 0) {
                this.cardTypeFilterBoolean = false;
                this.showAll = true;
            }
        }
    }

    resetStringFilter() {
        this.filteredPlayers = this.players;
    }

    filterPlayer(value: string) {
        if (!value) {
            this.resetStringFilter();
        }
        this.filteredPlayers = this.players.filter(item =>
            item.PlayerType.FirstName.toLowerCase().indexOf(value.toLowerCase()) > -1 ||
            item.PlayerType.LastName.toLowerCase().indexOf(value.toLowerCase()) > -1 ||
            item.PlayerType.FirstName.concat(" ").concat(item.PlayerType.LastName).toLowerCase().indexOf(value.toLowerCase()) > -1
        );
    }

    onOwnedPlayers() {
        this.ownedPlayersFilter = !this.ownedPlayersFilter;
        if (this.ownedPlayersFilter) {
            this.constructOwnedPlayers();
        }
        else {
            this.filteredPlayers = this.players;
        }
    }

    constructOwnedPlayers() {
        this.notOwnedPlayersFilter = false;
        this.missingPositionsFilterBoolean = false;
        this.filteredPlayers = this.players.filter(player => player.OwnedByMe == true);
    }

    onNotOwnedPlayers() {
        this.notOwnedPlayersFilter = !this.notOwnedPlayersFilter;
        if (this.notOwnedPlayersFilter) {
            this.constructNowOwnedPlayers();
        }
        else {
            this.filteredPlayers = this.players;
        }
    }

    constructNowOwnedPlayers() {
        this.ownedPlayersFilter = false;
        this.missingPositionsFilterBoolean = false;
        this.filteredPlayers = this.players.filter(player => player.OwnedByMe == false);
    }

    onToggleMissingPositions() {
        this.missingPositionsFilterBoolean = !this.missingPositionsFilterBoolean;

        if (this.missingPositionsFilterBoolean) {
            this.ownedPlayersFilter = false;
            this.notOwnedPlayersFilter = false;
            this.constructMissingPositions();
        }
        else {
            this.filteredPlayers = this.players;
        }
    }

    constructMissingPositions() {
        this.filteredPlayers = [];

        this.positionsMap.forEach((amount: number, position: string) => {
            var toAddPlayers: any[] = [];
            var count = 0;
            if (position.indexOf('/') >= 0) {
                var various = position.split("/");
                various.forEach(element => {
                    toAddPlayers = toAddPlayers.concat(this.players.filter(player => player.Position == element && player.OwnedByMe == false));
                    count += this.players.filter(player => player.Position == element && player.OwnedByMe == true).length;
                });
            }
            else {
                toAddPlayers = toAddPlayers.concat(this.players.filter(player => player.Position == position && player.OwnedByMe == false));
                count += this.players.filter(player => player.Position == position && player.OwnedByMe == true).length;
            }
            if (count < amount) {
                this.filteredPlayers = this.filteredPlayers.concat(toAddPlayers);
            }
        })
    }
}