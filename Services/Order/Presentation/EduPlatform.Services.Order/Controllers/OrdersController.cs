using EduPlatform.Services.Order.Application.Features.Commands.Orders;
using EduPlatform.Services.Order.Application.Features.Queries.Orders;
using EduPlatform.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace EduPlatform.Services.Order.Controllers
{
	public class OrdersController : CustomBaseController
	{
		private readonly IMediator _mediator;
		private readonly ISharedIdentityService _identityService;
		public OrdersController(IMediator mediator, ISharedIdentityService identityService)
		{
			_mediator = mediator;
			_identityService = identityService;
		}
		[HttpGet]
		public async Task<IActionResult> GetOrdersByUser()
		{
			var response = await _mediator.Send(new GetOrdersByUserIdQuery(_identityService.GetUserId));
			return Ok(response);
		}
		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderCommand c)
		{
			var response = await _mediator.Send(c);
			return CreateActionResultInstance(response);
		}
	}
}