using System.Collections.Generic;
using AutoMapper;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUTWatcher.API.Controllers
{
    [Authorize]
    public class ProfitController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProfitController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetDailyProfits()
        {
            return Ok(this.unitOfWork.DailyProfits.GetAllOrdered());
        }
    }
}