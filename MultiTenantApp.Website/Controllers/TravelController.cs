using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.Application.Travels.Commands.CreateTravel;
using MultiTenantApp.Application.Travels.Commands.UpdateTravelDescription;
using MultiTenantApp.Application.Travels.Queries.GetAllTravels;
using MultiTenantApp.Application.Travels.Queries.GetTravel;
using MultiTenantApp.Application.Travels.Queries.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using MultiTenantApp.Application.Interfaces;

namespace MultiTenantApp.Website.Controllers
{
    [Authorize(Policy = "OnlyAzureGroupsForDomain")]
    public class TravelController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserAuthorizeService _authorizeService;
        private readonly bool isAdmin;
        private readonly Guid userId;
        private readonly string email;

        public TravelController(IMediator mediator, IUserAuthorizeService authorizeService)
        {
            _mediator = mediator;
            _authorizeService = authorizeService;
            isAdmin= _authorizeService.IsAdminForDomain();
            email = _authorizeService.GetEmail();
            userId = _authorizeService.GetId();
        }

        // GET: Travel
        public async Task<ActionResult> Index()
        {
            var result = await _mediator.Send(new GetAllTravelsQuery
            {
                UserId = userId,
                IsAdmin = isAdmin
            });

            return View(result);
        }

        // GET: Travel/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var result = await _mediator.Send(new GetTravelQuery
            {
                UserId = userId,
                IsAdmin = isAdmin,
                Id = id
            });

            return View(result);
        }

        // GET: Travel/Create
        public ActionResult Create()
        {
            var model = new TravelViewModel();

            return  View(model);
        }

        // POST: Travel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TravelViewModel travel)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new CreateTravelCommand
                {
                    Name = travel.Name,
                    Description = travel.Description,
                    Email = email,
                    UserId = userId
                });

                return RedirectToAction(nameof(Index));
            }

            return View(travel);
        }

        // GET: Travel/Update/5
        public async Task<ActionResult> Update(int id)
        {
            var model =await _mediator.Send(new GetTravelQuery
            {
                Id = id,
                UserId = userId,
                IsAdmin = isAdmin
            });

            return View(model);
        }

        // POST: Travel/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, TravelViewModel travel)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateTravelDescriptionCommand
                {
                    Id = id,
                    IsAdmin = isAdmin,
                    Email = email,
                    Description = travel.Description,
                    UserId = userId
                });

                return RedirectToAction(nameof(Index));
            }

            return View(travel);
        }
    }
}