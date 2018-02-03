import { Component, Inject } from "@angular/core";
import { Http, Headers } from "@angular/http";
import 'rxjs/Rx';
import { ToasterService } from "angular2-toaster/src/toaster.service";
import { AuthenticationService } from "../../services/authentication.service";
import { PlayerOptionsService } from "../../services/player_options.service";

@Component({
    selector: 'info_uploader',
    templateUrl: './info_uploader.component.html'
})
export class InfoUploaderComponent {
    buyPlayerCost: any;
    sellPlayerCost: any;
    selectedPlayer: any;
    actualCoins: any;
    quickSellValue: any;
    players: any[];
    playerNameSold: string;
    selling: boolean;
    consumablesCostEach: any;
    consumablesAmount: any;
    isBronzeConsumable: boolean = false;

    informationJSON: string;
    informationType: string;

    constructor(
        private authenticationService: AuthenticationService,
        @Inject('API_URL') private apiUrl,
        private toaster: ToasterService,
        private playerOptionsService: PlayerOptionsService
    ) {
    }

    uploadJSON() {
        let body = {
            message: this.informationJSON
        }
        switch (this.informationType) {
            case "Bought":
                this.authenticationService.post(this.apiUrl + 'Home/Bought', body).subscribe(result => {
                    let totalBought = result.json();
                    this.toaster.pop('success', 'Success', 'The bought value is ' + totalBought);
                }, error => {
                    this.toaster.pop('error', 'Error', 'An error occured');
                });
                break;
            case "Sold":
                this.authenticationService.post(this.apiUrl + 'Home/Sold', body).subscribe(result => {
                    let totalSold = result.json();
                    this.toaster.pop('success', 'Success', 'The sold value is ' + totalSold);
                }, error => {
                    this.toaster.pop('error', 'Error', 'An error occured');
                });
                break;
            case "Mine":
                this.authenticationService.post(this.apiUrl + 'Home/Mine', body).subscribe(result => {
                    let playersAdded = result.json();
                    this.toaster.pop('success', 'Success', 'There were ' + playersAdded + ' players added');
                }, error => {
                    this.toaster.pop('error', 'Error', 'An error occured');
                });
                break;
            default:
                break;
        }
    }

    saveConsumablesSold() {
        let requestBody = {
            amount: this.consumablesAmount,
            costEach: this.consumablesCostEach,
            bronze: this.isBronzeConsumable
        };

        this.authenticationService.post(this.apiUrl + 'Home/ConsumablesSold', requestBody).subscribe(result => {
            this.consumablesAmount = undefined;
            this.consumablesCostEach = undefined;
            let totalSold = result.json();
            this.toaster.pop('success', 'Success', 'The sold value is ' + totalSold);
        }, error => {
            this.toaster.pop('error', 'Error', 'An error occured');
        });
    }

    searchPlayerName(sold: boolean) {
        this.selling = sold;
        this.authenticationService.get(this.apiUrl + 'Player/NameHas/?name=' + this.playerNameSold).subscribe(result => {
            this.playerNameSold = undefined;
            this.players = result.json() as any[];
            this.players = this.players.filter(player => player.Color !== 'fut_champions_gold');
            this.players = this.players.filter(player => player.Color !== 'fut_champions_silver');
            if (this.players.length == 0) {
                this.toaster.pop('error', 'Error', 'No players were found');
            }
        }, error => {
            // console.log(error);
        });
    }

    bronzePackOpened() {
        let requestBody = {
            value: this.quickSellValue
        };
        this.authenticationService.post(this.apiUrl + 'Home/BronzePackOpened', requestBody).subscribe(result => {
            this.quickSellValue = undefined;
            this.toaster.pop('success', 'Success', 'Bronze pack registered');
        }, error => {
            this.toaster.pop('error', 'Error', 'An error occured');
        });
    }

    updateMyCoins() {
        let requestBody = {
            value: this.actualCoins
        }
        this.authenticationService.post(this.apiUrl + 'Home/UpdateMyCoins', requestBody).subscribe(result => {
            this.actualCoins = undefined;
            this.toaster.pop('success', 'Success', 'Coins updated');
        }, error => {
            this.toaster.pop('error', 'Error', 'An error occured');
        });
    }

    savePlayerOperation() {
        let requestBody = {
            Id: this.selectedPlayer
        }
        if (this.selling) {
            requestBody["Value"] = this.sellPlayerCost;
            this.authenticationService.post(this.apiUrl + 'Home/SoldPlayer', requestBody).subscribe(result => {
                this.players = [];
                this.sellPlayerCost = undefined;
                this.toaster.pop('success', 'Success', 'Player sold');
            }, error => {
                this.toaster.pop('error', 'Error', 'An error occured');
            });
        }
        else {
            requestBody["Value"] = this.buyPlayerCost;
            this.authenticationService.post(this.apiUrl + 'Home/BoughtPlayer', requestBody).subscribe(result => {
                this.players = [];
                this.buyPlayerCost = undefined;
                this.toaster.pop('success', 'Success', 'Player bought');
            }, error => {
                this.toaster.pop('error', 'Error', 'An error occured');
            });
        }
    }

    playerBackgroundColor(player: any) {
        this.playerOptionsService.playerBackgroundColor(player);
    }
}