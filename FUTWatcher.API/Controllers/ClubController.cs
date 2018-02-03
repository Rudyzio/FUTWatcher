using AutoMapper;
using FUTWatcher.API.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FUTWatcher.API.Controllers
{
    public class ClubController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ClubController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult ClubsByLeague(long id) {
            return Ok(this.unitOfWork.Clubs.GetClubsByLeague(id));
        }
    }
}