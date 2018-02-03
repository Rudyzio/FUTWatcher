import { Injectable } from "@angular/core";

@Injectable()
export class PlayerOptionsService {

    constructor() {

    }

    playerBackgroundColor(player: any) {
        switch (player.Color) {
            case 'gold':
                return '#c7aa5f';
            case 'silver':
                return '#979ca4';
            case 'bronze':
                return '#a17d53';
            case 'rare_gold':
                return 'linear-gradient(45deg,#c6a009 0,#f2c40b 50%,#f2c40b 51%,#f0cf4a 100%)';
            case 'rare_silver':
                return 'linear-gradient(45deg,#a9aeb1 0,#c2c7ca 50%,#c2c7ca 51%,#e9ecf0 100%)';
            case 'rare_bronze':
                return 'linear-gradient(45deg,#6e4c35 0,#9f7f69 50%,#9f7f69 51%,#ffd9aa 100%)';
            case 'totw_gold':
                return 'linear-gradient(-290deg,#000,#8a710e)';
            case 'totw_silver':
                return 'linear-gradient(-290deg,#000,#8d9498)';
            case 'legend':
                return 'linear-gradient(45deg,#fffbe9 0,#958351 50%,#36517e 65%,#203a63 100%)';
            case 'gotm':
                return 'linear-gradient(45deg,#8cecff 10%,#3eacff 20%,#5d8cd9 51%,#19137e 100%)';
            case 'purple':
                return 'linear-gradient(45deg,#7962a6 0,#7962a6 50%,#7962a6 51%,#4f3580 100%)';
            case 'fut_mas':
                return 'linear-gradient(45deg,#0e5e01 0,#d9282e 50%,#d9282e 51%,#c51218 100%)';
            case 'ones_to_watch':
                return 'linear-gradient(45deg,#996931 10%,#a69423 20%,#666663 51%,#000 100%)';
            case 'halloween':
                return 'linear-gradient(45deg,#ebb484 10%,#e39f63 20%,#e69045 51%,#000 100%)';
            case 'award_winner':
                return 'linear-gradient(45deg,#ecdef6 10%,#c9a2e4 20%,#985ac5 51%,#823ab5 100%)';
            case 'marquee':
                return 'linear-gradient(45deg,#b2ed31 0,#eb6c33 50%,#eb6c33 51%,#131133 100%)';
            default:
                break;
        }
    }

    playerIsOwned(player: any) {
        if (player.OwnedByMe) {
            return 'green';
        }
        else {
            return 'white';
        }
    }
}