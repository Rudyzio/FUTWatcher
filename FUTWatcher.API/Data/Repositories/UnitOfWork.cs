using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;
using Newtonsoft.Json.Linq;

namespace FUTWatcher.API.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IClubRepository Clubs { get; private set; }
        public ILeagueRepository Leagues { get; private set; }
        public INationRepository Nations { get; private set; }
        public IPlayerTypeRepository PlayerTypes { get; private set; }
        public IPlayerVersionRepository PlayerVersions { get; private set; }
        public IDailyProfitRepository DailyProfits { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Clubs = new ClubRepository(_context);
            Leagues = new LeagueRepository(_context);
            Nations = new NationRepository(_context);
            PlayerTypes = new PlayerTypeRepository(_context);
            PlayerVersions = new PlayerVersionRepository(_context);
            DailyProfits = new DailyProfitRepository(_context);
            Users = new UserRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public long GetTotalPlayers()
        {
            return _context.PlayerVersions.Count();
        }

        public long GetOwnedPlayers()
        {
            return _context.PlayerVersions.Where(pv => pv.OwnedByMe == true).ToList().Count();
        }

        public void FillDatabase()
        {
            for (int pageNumber = 1; pageNumber < 714; pageNumber++)
            {
                System.Console.WriteLine("Beginnning page number: " + pageNumber);
                Task<JObject> t = Task<JObject>.Factory.StartNew(() => DownloadFutPlayerPage(pageNumber).Result);
                JObject result = t.Result;
                for (int i = 0; i < (int)result["count"]; i++)
                {
                    string type = result["items"][i]["color"].ToString();
                    long playerVersionId = (long)result["items"][i]["id"];
                    if (!this.PlayerVersions.Contains(pv => pv.Id == playerVersionId))
                    {

                        long nationId = (long)result["items"][i]["nation"]["id"];
                        if (!this.Nations.Contains(n => n.Id == nationId))
                        {
                            var nation = new Nation
                            {
                                Id = nationId,
                                Name = result["items"][i]["nation"]["name"].ToString()
                            };

                            this.Nations.Add(nation);
                            this.Complete();
                        }

                        long leagueId = (long)result["items"][i]["league"]["id"];
                        if (!this.Leagues.Contains(l => l.Id == leagueId))
                        {
                            var league = new League
                            {
                                Id = leagueId,
                                Name = result["items"][i]["league"]["name"].ToString()
                            };

                            this.Leagues.Add(league);
                            this.Complete();
                        }

                        long clubId = (long)result["items"][i]["club"]["id"];
                        if (!this.Clubs.Contains(c => c.Id == clubId))
                        {
                            var club = new Club
                            {
                                Id = clubId,
                                Name = result["items"][i]["club"]["name"].ToString(),
                                LeagueId = leagueId
                            };

                            this.Clubs.Add(club);
                            this.Complete();
                        }

                        long playerTypeId = (long)result["items"][i]["baseId"];
                        if (!this.PlayerTypes.Contains(pt => pt.Id == playerTypeId))
                        {
                            var playerType = new PlayerType
                            {
                                Id = playerTypeId,
                                CommonName = result["items"][i]["commonName"].ToString(),
                                FirstName = result["items"][i]["firstName"].ToString(),
                                LastName = result["items"][i]["lastName"].ToString()
                            };

                            this.PlayerTypes.Add(playerType);
                            this.Complete();
                        }

                        var playerVersion = new PlayerVersion
                        {
                            Id = playerVersionId,
                            Position = result["items"][i]["position"].ToString(),
                            Quality = result["items"][i]["quality"].ToString(),
                            Color = result["items"][i]["color"].ToString(),
                            Rating = (long)result["items"][i]["rating"],
                            IsSpecialType = (bool)result["items"][i]["isSpecialType"],
                            OwnedByMe = false,
                            Cost = 0,
                            PlayerTypeId = playerTypeId,
                            NationId = nationId,
                            LeagueId = leagueId,
                            ClubId = clubId
                        };

                        this.PlayerVersions.Add(playerVersion);
                        this.Complete();
                    }
                }
                System.Console.WriteLine("Page number: " + pageNumber + " finished!");
            }
        }

        private static async Task<JObject> DownloadFutPlayerPage(int pageNumber)
        {
            string page = "http://www.easports.com/fifa/ultimate-team/api/fut/item?jsonParamObject={\"page\":\"" + pageNumber + "\"}";

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                JObject json = JObject.Parse(result);
                return json;
            }
        }

        public void UpdateNations()
        {
            var playersWithoutNation = _context.PlayerVersions.Where(pv => pv.NationId == null).ToList();
            foreach (var player in playersWithoutNation)
            {
                System.Console.WriteLine(player.Id);
                Task<JObject> t = Task<JObject>.Factory.StartNew(() => DownloadFutPlayerId(player.Id).Result);
                JObject result = t.Result;

                player.NationId = (long)result["items"][0]["nation"]["id"];
                this.Complete();
            }
            System.Console.WriteLine("FINISHED");
        }

        private static async Task<JObject> DownloadFutPlayerId(long playerId)
        {
            string page = "http://www.easports.com/fifa/ultimate-team/api/fut/item?jsonParamObject={\"id\":\"" + playerId + "\"}";

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                JObject json = JObject.Parse(result);
                return json;
            }
        }

        public void UpdateAllPrices()
        {
            var allPlayers = _context.PlayerVersions.ToList();
            this.UpdatePrices(allPlayers);
            System.Console.WriteLine("Finished Updating Prices");
        }

        public void UpdateBronzePrices()
        {
            var bronzePlayers = _context.PlayerVersions.Where(pv => pv.Quality == "bronze").ToList();
            this.UpdatePrices(bronzePlayers);
            System.Console.WriteLine("Finished Updating Bronze Prices");
        }

        private void UpdatePrices(IEnumerable<PlayerVersion> players)
        {
            foreach (var player in players)
            {
                Task<JObject> t = Task<JObject>.Factory.StartNew(() => DownloadFutbinPlayerPage(player.Id).Result);
                JObject result = t.Result;
                var mc = result[player.Id.ToString()]["prices"]["pc"]["LCPrice"].ToString();
                mc = mc.Replace(",", string.Empty);
                player.MarketCost = Convert.ToInt64(mc);
                player.UpdateDate = result[player.Id.ToString()]["prices"]["pc"]["updated"].ToString();
                this.Complete();
            }
        }

        private static async Task<JObject> DownloadFutbinPlayerPage(long playerId)
        {
            string page = "https://www.futbin.com/18/playerPrices?player=" + playerId;

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                JObject json = JObject.Parse(result);
                return json;
            }
        }

        public void ResetOwned()
        {
            var allPlayers = _context.PlayerVersions.ToList();
            foreach (var player in allPlayers)
            {
                player.OwnedByMe = false;
                player.LastPrice = 0;
            }
            this.Complete();
        }

        public long MyPlayers(string message)
        {
            long toReturn = 0;
            JObject json = JObject.Parse(message);

            for (int i = 0; i < ((JArray)json["itemData"]).Count; i++)
            {
                if (json["itemData"][i]["loans"] == null)
                {
                    long playerId = (long)json["itemData"][i]["resourceId"];
                    PlayerVersion pv = PlayerVersions.Get(playerId);
                    System.Console.WriteLine("Reading player:" + pv.Id);
                    pv.OwnedByMe = true;
                    pv.LastPrice = (long)json["itemData"][i]["lastSalePrice"];
                    if (pv.Cost == 0)
                    {
                        pv.Cost = pv.LastPrice;
                    }
                    if (pv.Amount < 1)
                    {
                        pv.Amount = 1;
                    }
                    toReturn++;
                    this.Complete();
                }
            }
            return toReturn;
        }

        public long Bought(string message)
        {
            long toReturn = 0;
            JObject json = JObject.Parse(message);

            this.UpdateCoins((long)json["credits"]);

            for (int i = 0; i < (int)json["total"]; i++)
            {
                if (json["auctionInfo"][i]["tradeState"].ToString() == "closed" && json["auctionInfo"][i]["bidState"].ToString() == "highest")
                {
                    if (json["auctionInfo"][i]["itemData"]["itemType"].ToString() == "player")
                    {
                        long playerId = (long)json["auctionInfo"][i]["itemData"]["resourceId"];
                        PlayerVersion pv = PlayerVersions.Get(playerId);
                        pv.OwnedByMe = true;
                        pv.Cost += (long)json["auctionInfo"][i]["itemData"]["lastSalePrice"];
                        pv.Amount++;
                    }
                    this.RemoveProfit((long)json["auctionInfo"][i]["itemData"]["lastSalePrice"]);
                    toReturn += (long)json["auctionInfo"][i]["itemData"]["lastSalePrice"];
                    this.Complete();
                }
            }
            return toReturn;
        }

        public long Sold(string message)
        {
            long toReturn = 0;
            JObject json = JObject.Parse(message);

            this.UpdateCoins((long)json["credits"]);

            for (int i = 0; i < ((JArray)json["auctionInfo"]).Count; i++)
            {
                if (json["auctionInfo"][i]["tradeState"].ToString() == "closed" &&
                    (long)json["auctionInfo"][i]["currentBid"] > 0)
                {
                    long profit = (long)json["auctionInfo"][i]["currentBid"];
                    this.AddProfit(profit);
                    toReturn += (profit - (long)(profit * 0.05));
                    if (json["auctionInfo"][i]["itemData"]["itemType"].ToString() == "player")
                    {
                        long playerId = (long)json["auctionInfo"][i]["itemData"]["resourceId"];
                        PlayerVersion pv = PlayerVersions.Get(playerId);
                        pv.Amount--;
                        if (pv.Amount < 1)
                        {
                            pv.OwnedByMe = false;
                        }
                        pv.Cost -= (profit - (long)(profit * 0.05));
                    }

                    if (json["auctionInfo"][i]["itemData"]["itemType"].ToString() == "health" &&
                        (long)json["auctionInfo"][i]["itemData"]["rating"] == 55)
                    {
                        this.addBronzeProfit(profit);
                    }

                    if (json["auctionInfo"][i]["itemData"]["itemType"].ToString() == "player" &&
                    (long)json["auctionInfo"][i]["itemData"]["rating"] < 65)
                    {
                        this.addBronzeProfit(profit);
                    }
                }
            }
            this.Complete();
            return toReturn;
        }

        private void addBronzeProfit(long profit)
        {
            DailyProfit dp = this.GetDailyProfit();
            dp.BronzePacksEarned += (profit - (long)(profit * 0.05));
        }

        private bool IsDuplicateItem(long itemId, JObject json)
        {
            for (int i = 0; i < ((JArray)json["duplicateItemIdList"]).Count; i++)
            {
                if (itemId == (long)json["duplicateItemIdList"][i]["itemId"])
                {
                    return true;
                }
            }
            return false;
        }

        private void AddProfit(long profit)
        {
            long profitWithTax = profit - (long)(profit * 0.05);
            if (!DailyProfits.Contains(dp => dp.Date.Date == DateTime.Now.Date))
            {
                DailyProfit dp = this.CreateDailyProfit();
                dp.Profit += profitWithTax;
                DailyProfits.Add(dp);
            }
            else
            {
                DailyProfit update = DailyProfits.SingleOrDefault(dp => dp.Date.Date == DateTime.Now.Date);
                update.Profit += profitWithTax;
            }
            this.Complete();
        }

        private void RemoveProfit(long profit)
        {
            if (!DailyProfits.Contains(dp => dp.Date.Date == DateTime.Now.Date))
            {
                DailyProfit dp = this.CreateDailyProfit();
                dp.Profit -= profit;
                DailyProfits.Add(dp);
            }
            else
            {
                DailyProfit update = DailyProfits.SingleOrDefault(dp => dp.Date.Date == DateTime.Now.Date);
                update.Profit -= profit;
            }
            this.Complete();
        }

        private DailyProfit CreateDailyProfit()
        {
            var dp = new DailyProfit
            {
                Id = 0,
                Date = DateTime.Now,
                Profit = 0,
                MaxCoins = 0,
                MinCoins = 0,
                BronzePacksEarned = 0,
                BronzePacksOpended = 0,
                BronzePacksSpent = 0
            };

            DailyProfits.Add(dp);
            this.Complete();
            
            return dp;
        }

        public void BronzePackOpened(long value)
        {
            DailyProfit dailyProfit = this.GetDailyProfit();
            dailyProfit.BronzePacksOpended++;
            dailyProfit.BronzePacksSpent += (400 - value);
            long cost = 400 - value;
            dailyProfit.Profit -= 400;
        }

        public void UpdateCoins(long coins)
        {
            DailyProfit dailyProfit = this.GetDailyProfit();
            if (coins > dailyProfit.MaxCoins)
            {
                dailyProfit.MaxCoins = coins;
            }
            if (coins < dailyProfit.MinCoins || dailyProfit.MinCoins == 0)
            {
                dailyProfit.MinCoins = coins;
            }
        }

        private DailyProfit GetDailyProfit()
        {
            DailyProfit dailyProfit;
            if (!DailyProfits.Contains(dp => dp.Date.Date == DateTime.Now.Date))
            {
                dailyProfit = this.CreateDailyProfit();
                DailyProfits.Add(dailyProfit);
            }
            else
            {
                dailyProfit = DailyProfits.SingleOrDefault(dp => dp.Date.Date == DateTime.Now.Date);
            }
            return dailyProfit;
        }

        public long ConsumablesSold(long amount, long costEach, bool bronze)
        {
            DailyProfit dp = this.GetDailyProfit();
            long toReturn = 0;
            for (int i = 0; i < amount; i++)
            {
                dp.Profit += (costEach - (long)(costEach * 0.05));
                if (bronze)
                {
                    dp.BronzePacksEarned += (costEach - (long)(costEach * 0.05));
                }
                toReturn += (costEach - (long)(costEach * 0.05));
            }
            return toReturn;
        }

        public void SoldPlayer(long id, long value)
        {
            var player = _context.PlayerVersions.SingleOrDefault(pv => pv.Id == id);
            player.Cost -= (value - (long)(value * 0.05));
            player.OwnedByMe = false;

            DailyProfit dp = this.GetDailyProfit();
            dp.Profit += (value - (long)(value * 0.05));

            if (player.Rating < 65)
            {
                dp.BronzePacksEarned += (value - (long)(value * 0.05));
            }
        }
        public void BoughtPlayer(long id, long value)
        {
            var player = _context.PlayerVersions.SingleOrDefault(pv => pv.Id == id);
            player.Cost += value;
            player.OwnedByMe = true;

            DailyProfit dp = this.GetDailyProfit();
            dp.Profit -= value;
        }
    }
}