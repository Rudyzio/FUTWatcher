import { Component, Inject } from "@angular/core";
import { Http, Headers } from "@angular/http";
import 'rxjs/Rx';
import { AuthenticationService } from "../../services/authentication.service";

@Component({
    selector: 'statistics',
    templateUrl: './statistics.component.html',
    styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent {

    showGraph: boolean = false;

    totalBronzePackProfit: number = 0;

    totalBronzePacksOpened: number = 0;

    private dailyProfitData = {
        labels: [],
        datasets: [
            {
                label: "",
                data: [],
                fill: false,
                borderColor: 'blue',
                backgroundColor: 'blue'
            }
        ]
    };

    private bronzePacksData = {
        labels: [],
        datasets: [
            {
                label: "",
                data: [],
                fill: false,
                borderColor: 'red',
                backgroundColor: 'red'
            }
        ]
    };

    private coinsData = {
        labels: [],
        datasets: [
            {
                label: "",
                data: [],
                fill: false,
                borderColor: 'blue',
                backgroundColor: 'blue'
            },
            {
                label: "",
                data: [],
                fill: false,
                borderColor: 'yellow',
                backgroundColor: 'yellow'
            }
        ]
    };

    private bronzePacksProfit = {
        labels: [],
        datasets: [
            {
                label: "",
                data: [],
                fill: false,
                borderColor: 'blue',
                backgroundColor: 'blue'
            },
            {
                label: "",
                data: [],
                fill: false,
                borderColor: 'yellow',
                backgroundColor: 'yellow'
            }
        ]
    };

    private dailyProfitoptions: any = {
        responsive: true,
        maintainAspectRatio: false,
        title: {
            display: true,
            text: 'Daily balance profits',
            fontSize: 18
        },
        elements: {
            line: {
                tension: 0
            }
        }
    };

    private bronzePacksoptions: any = {
        responsive: true,
        maintainAspectRatio: false,
        title: {
            display: true,
            text: 'Bronze packs opened',
            fontSize: 18
        },
        elements: {
            line: {
                tension: 0
            }
        }
    };

    private coinsoptions: any = {
        responsive: true,
        maintainAspectRatio: false,
        title: {
            display: true,
            text: 'Daily coins balance',
            fontSize: 18
        },
        elements: {
            line: {
                tension: 0
            }
        }
    };

    private bronzePacksProfitOptions: any = {
        responsive: true,
        maintainAspectRatio: false,
        title: {
            display: true,
            text: 'Bronze Packs Profit',
            fontSize: 18
        },
        elements: {
            line: {
                tension: 0
            }
        }
    };

    private type: string = 'line';

    constructor(
        @Inject('API_URL') private apiUrl,
        private authService: AuthenticationService
    ) {
        authService.get(apiUrl + 'Profit/GetDailyProfits').subscribe(result => {
            var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

            let dailyProfit = result.json() as any[];
            let lineChartData: Array<any> = [
                { data: [], label: '' }
            ];
            let lineChartLabels: Array<any> = [];
            dailyProfit.forEach(element => {
                this.dailyProfitData.datasets[0].data.push(element.Profit);
                this.bronzePacksData.datasets[0].data.push(element.BronzePacksOpended);
                this.coinsData.datasets[0].data.push(element.MaxCoins);
                this.coinsData.datasets[1].data.push(element.MinCoins);
                this.bronzePacksProfit.datasets[0].data.push(element.BronzePacksEarned);
                this.bronzePacksProfit.datasets[1].data.push(element.BronzePacksSpent);
                this.totalBronzePackProfit += element.BronzePacksEarned - element.BronzePacksSpent;
                this.totalBronzePacksOpened += element.BronzePacksOpended;
                var date = new Date(element.Date)
                this.dailyProfitData.labels.push(date.toLocaleDateString() + " " + days[date.getDay()]);
                this.bronzePacksData.labels.push(date.toLocaleDateString() + " " + days[date.getDay()]);
                this.coinsData.labels.push(date.toLocaleDateString() + " " + days[date.getDay()]);
                this.bronzePacksProfit.labels.push(date.toLocaleDateString() + " " + days[date.getDay()]);
            });
            this.dailyProfitData.datasets[0].label = 'Daily Profits';
            this.bronzePacksData.datasets[0].label = 'Bronze packs opened';
            this.coinsData.datasets[0].label = 'Max Coins';
            this.coinsData.datasets[1].label = 'Min Coins';
            this.bronzePacksProfit.datasets[0].label = 'Bronze Packs Earned';
            this.bronzePacksProfit.datasets[1].label = 'Bronze Packs Spent';
            this.showGraph = true;
        }, error => {
            // console.log(error);
        });
    }
}