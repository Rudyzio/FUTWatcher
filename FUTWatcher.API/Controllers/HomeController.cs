using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FUTWatcher.API.Controllers
{
    // [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        public IActionResult PopulateDB()
        {
            this.unitOfWork.FillDatabase();
            return Ok();
        }

        public IActionResult UpdateAllPrices()
        {
            this.unitOfWork.UpdateAllPrices();
            return Ok();
        }

        public IActionResult UpdateBronzePrices()
        {
            this.unitOfWork.UpdateBronzePrices();
            return Ok();
        }

        public IActionResult Bought([FromBody] JSONMessageViewModel viewModel) {
            var value = this.unitOfWork.Bought(viewModel.Message);
            this.unitOfWork.Complete();
            return Ok(value);
        }

        public IActionResult Sold([FromBody] JSONMessageViewModel viewModel) {
            var value = this.unitOfWork.Sold(viewModel.Message);
            this.unitOfWork.Complete();
            return Ok(value);
        }

        [HttpPost]
        public IActionResult Mine([FromBody] JSONMessageViewModel viewModel) {
            return Ok(this.unitOfWork.MyPlayers(viewModel.Message));
        }

        public IActionResult ResetOwned() {
            this.unitOfWork.ResetOwned();
            return Ok();
        }

        [HttpPost]
        public IActionResult BronzePackOpened([FromBody] LongViewModel vm) {
            this.unitOfWork.BronzePackOpened(vm.Value);
            this.unitOfWork.Complete();
            return Ok();
        }

        [HttpPost]
        public IActionResult ConsumablesSold([FromBody] ConsumablesSoldViewModel vm) {
            var value = this.unitOfWork.ConsumablesSold(vm.Amount, vm.CostEach, vm.Bronze);
            this.unitOfWork.Complete();
            return Ok(value);
        }

        [HttpPost]
        public IActionResult UpdateMyCoins([FromBody] LongViewModel vm) {
            this.unitOfWork.UpdateCoins(vm.Value);
            this.unitOfWork.Complete();
            return Ok();
        }

        [HttpPost]
        public IActionResult SoldPlayer([FromBody] PlayerValueViewModel vm) {
            this.unitOfWork.SoldPlayer(vm.Id, vm.Value);
            this.unitOfWork.Complete();
            return Ok();
        }

        [HttpPost]
        public IActionResult BoughtPlayer([FromBody] PlayerValueViewModel vm) {
            this.unitOfWork.BoughtPlayer(vm.Id, vm.Value);
            this.unitOfWork.Complete();
            return Ok();
        }
    }
}