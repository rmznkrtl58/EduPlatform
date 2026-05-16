using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Services.Order.Application.Dtos.OrderDtos;
using EduPlatform.Shared.Dtos;
using Mapster;
using MediatR;
using System.Net;
namespace EduPlatform.Services.Order.Application.Features.Queries.Orders
{
	public class GetOrdersByUserIdQuery:IRequest<ResponseDto<IEnumerable<GetOrderDto>>>
	{
		public string UserId { get; set; }

		public GetOrdersByUserIdQuery(string userId)
		{
			UserId = userId;
		}
	}
	public class GetOrderByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, ResponseDto<IEnumerable<GetOrderDto>>>
	{
		private readonly IOrderRepository _orderRepo;
		public GetOrderByUserIdQueryHandler(IOrderRepository orderRepo)
		{
			_orderRepo = orderRepo;
		}
		async Task<ResponseDto<IEnumerable<GetOrderDto>>> IRequestHandler<GetOrdersByUserIdQuery, ResponseDto<IEnumerable<GetOrderDto>>>.Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
		{
			var values =await _orderRepo.GetListByUserIdWithOrderItems(request.UserId);
			var mapValues = values.Adapt<IEnumerable<GetOrderDto>>();
			if (mapValues is null) return ResponseDto<IEnumerable<GetOrderDto>>.Success(new List<GetOrderDto>(), HttpStatusCode.OK.GetHashCode());
			return ResponseDto<IEnumerable<GetOrderDto>>.Success(mapValues, HttpStatusCode.OK.GetHashCode());
		}
	}
}
