import { Component, Inject } from '@angular/core';
import { ToasterService } from 'angular2-toaster/src/toaster.service';
import { AuthenticationService } from '../../../services/authentication.service';

@Component({
    selector: 'player-leagues',
    templateUrl: './leagues.component.html',
    styleUrls: ['./leagues.component.css']
})
export class LeaguesComponent {
    ownedClubPlayers: number;
    teamCost: number;
    totalClubPlayers: number;
    ownedLeaguePlayers: any;
    totalLeaguePlayers: any;
    leagueCost: any;
    clubs: any[] = [];
    players: any[] = [];
    leagues: any[] = [];

    teamHasAllPositions: boolean = false;
    teamHasMinimumPositions: boolean = false;

    allPositionsMap: Map<string, number> = new Map([['GK', 1], ['RB/RWB', 1], ['CB', 3], ['LB/LWB', 1], ['RM/RW/RF', 1], ['CDM/CM/CAM', 4], ['LM/LW/LF', 1], ['CF/ST', 2]]);
    minimumPositionsMap: Map<string, number> = new Map([['GK', 1], ['RB/RWB', 1], ['CB', 2], ['LB/LWB', 1], ['RM/RW/RF', 1], ['CDM/CM/CAM', 2], ['LM/LW/LF', 1], ['CF/ST', 2]]);

    teamRating: number;

    constructor(
        private toaster: ToasterService,
        @Inject('API_URL') private apiUrl,
        private authService: AuthenticationService
    ) {
        authService.get(apiUrl + 'League/All').subscribe(result => {
            this.leagues = result.json() as any[];
            this.getLeagueInformation(this.leagues[0].Id);
        }, error => {
            // console.log(error);
        });
    }

    getLeagueInformation(leagueId: number) {
        this.authService.get(this.apiUrl + 'League/Information/' + leagueId).subscribe(result => {
            this.leagueCost = result.json().Cost;
            this.totalLeaguePlayers = result.json().TotalPlayers;
            this.ownedLeaguePlayers = result.json().OwnedPlayers;
            this.clubs = result.json().Clubs;
            this.onClubChange(this.clubs[0].Id);
        }, error => {
            // console.log(error);
        })
    }

    onLeagueChange(leagueId: any) {
        this.players = [];
        this.clubs = [];
        this.getLeagueInformation(leagueId);
    }

    onClubChange(clubId: any) {
        this.authService.get(this.apiUrl + 'Player/GetPlayersByClub/' + clubId).subscribe(result => {
            var playersList = result.json() as any[];
            this.players = [];
            this.teamHasAllPositions = false;
            this.teamHasMinimumPositions = false;

            this.players = this.players.concat(playersList.filter(player => player.Position === 'GK'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'RB'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'RWB'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'CB'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'LB'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'LWB'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'RM'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'RW'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'RF'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'CDM'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'CM'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'CAM'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'LM'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'LW'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'LF'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'CF'));
            this.players = this.players.concat(playersList.filter(player => player.Position === 'ST'));
            this.players = this.players.filter(player => player.Color !== 'fut_champions_gold');
            this.players = this.players.filter(player => player.Color !== 'fut_champions_silver');

            this.calculateTeamRating();

            this.totalClubPlayers = this.players.length;
            this.getOwnedClubPlayers();

            this.getClubCost();
            this.verifyMissingPositions();
        })

    }

    getClubCost() {
        this.teamCost = 0;
        this.players.forEach(player => {
            this.teamCost += player.Cost;
        });
    }

    getOwnedClubPlayers() {
        this.ownedClubPlayers = 0;
        for (let i: number = 0; i < this.players.length; i++) {
            if (this.players[i].OwnedByMe) {
                this.ownedClubPlayers++;
            }
        }
    }

    calculateTeamRating() {
        let ratingSum: number = 0;
        let tempPlayers: any[] = this.players;
        tempPlayers = tempPlayers.filter(player => player.Color !== 'totw_gold');
        tempPlayers = tempPlayers.filter(player => player.Color !== 'totw_silver');
        tempPlayers = tempPlayers.filter(player => player.Color !== 'totw_bronze');

        for (let player of tempPlayers) {
            ratingSum += player.Rating;
        }

        this.teamRating = Math.round(ratingSum / tempPlayers.length);
    }

    verifyMissingPositions() {
        let filteredPlayers = [];

        this.allPositionsMap.forEach((amount: number, position: string) => {
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
                filteredPlayers = filteredPlayers.concat(toAddPlayers);
            }
        })

        if (filteredPlayers.length == 0) {
            this.teamHasAllPositions = true;
        }
        else {
            this.teamHasAllPositions = false;
        }

        filteredPlayers = [];

        this.minimumPositionsMap.forEach((amount: number, position: string) => {
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
                filteredPlayers = filteredPlayers.concat(toAddPlayers);
            }
        })

        if (filteredPlayers.length == 0) {
            this.teamHasMinimumPositions = true;
        }
        else {
            this.teamHasMinimumPositions = false;
        }
    }
}