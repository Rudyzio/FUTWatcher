using System.Collections.Generic;
using AutoMapper;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;
using FUTWatcher.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUTWatcher.API.Controllers
{
    [Authorize]
    public class LeagueController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LeagueController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult All()
        {
            var toReturn = unitOfWork.Leagues.GetAllOrdered();
            this.unitOfWork.Complete();
            return Ok(toReturn);
        }

        [HttpGet]
        public IActionResult Information(long id)
        {
            LeagueDataViewModel vm = new LeagueDataViewModel();
            vm.Cost = unitOfWork.Leagues.GetLeagueCost(id);
            vm.TotalPlayers = unitOfWork.PlayerVersions.GetTotalPlayersByLeague(id);
            vm.OwnedPlayers = unitOfWork.PlayerVersions.GetOwnedPlayersByLeague(id);
            vm.Clubs = this.unitOfWork.Clubs.GetClubsByLeague(id);
            this.unitOfWork.Complete();
            return Ok(vm);
        }
    }
}