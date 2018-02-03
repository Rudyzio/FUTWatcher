using AutoMapper;
using FUTWatcher.API.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUTWatcher.API.Controllers
{
    [Authorize]
    public class NationController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public NationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult All()
        {
            var nations = this.unitOfWork.Nations.GetAllOrdered();
            this.unitOfWork.Complete();
            return Ok(nations);
        }
    }
}