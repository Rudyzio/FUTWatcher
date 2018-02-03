using AutoMapper;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;
using FUTWatcher.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUTWatcher.API.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlayerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPlayersByLeague(long id)
        {
            var players = this.unitOfWork.PlayerVersions.GetPlayersByLeague(id);
            return Ok(players);
        }

        [HttpGet]
        public IActionResult GetPlayersByClub(long id)
        {
            var players = this.unitOfWork.PlayerVersions.GetPlayersByClub(id);
            return Ok(players);
        }

        [HttpPost]
        public IActionResult Update([FromBody] PlayerVersion pv)
        {
            this.unitOfWork.PlayerVersions.Update(pv);
            this.unitOfWork.Complete();
            return Ok();
        }

        [HttpGet]
        public IActionResult NameHas(string name)
        {
            var players = this.unitOfWork.PlayerVersions.NameHas(name);
            this.unitOfWork.Complete();
            return Ok(players);
        }

        [HttpGet]
        public IActionResult TotalPlayersInformation()
        {
            TotalPlayersDataViewModel vm = new TotalPlayersDataViewModel();
            vm.TotalCost = this.unitOfWork.PlayerVersions.TotalCost();
            vm.TotalPlayers = this.unitOfWork.GetTotalPlayers();
            vm.TotalOwnedPlayers = this.unitOfWork.GetOwnedPlayers();
            this.unitOfWork.Complete();
            return Ok(vm);
        }

        [HttpGet]
        // [Route("{pageIndex:int}/{pageSize:int}/{nationId:long}")]
        public IActionResult GetNationPage(int pageIndex, int pageSize, long nationId)
        {
            return Ok(new
            {
                Data = this.unitOfWork.PlayerVersions.GetNationPage(pageIndex, pageSize, nationId),
                Total = this.unitOfWork.PlayerVersions.GetPlayersByNation(nationId)
            });
        }

        [HttpGet]
        public IActionResult GetPlayersByRating(long rating) {
            return Ok(this.unitOfWork.PlayerVersions.GetPlayersByRating(rating));
        }

    }
}