using Microsoft.EntityFrameworkCore;
using Order.Persistence.Database;
using Order.Service.Queries.DTOs;
using Order.Service.Queries.Interfaces;
using Service.Common.Collection;
using Service.Common.Mapping;
using Service.Common.Paging;

namespace Order.Service.Queries
{
    public class OrderQueryService : IOrderQueryService
    {
        #region Variables
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructor
        public OrderQueryService(
            ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<DataCollection<OrderDto>> GetAllAsync(int page, int take)
        {
            var collection = await _context.Orders
                .Include(x => x.Items)
                .OrderByDescending(x => x.OrderId)
                .GetPagedAsync(page, take);

            return collection.MapTo<DataCollection<OrderDto>>();
        }

        public async Task<OrderDto> GetAsync(int id)
        {
            return (await _context.Orders.Include(x => x.Items).SingleAsync(x => x.OrderId == id)).MapTo<OrderDto>();
        }
    }
}
