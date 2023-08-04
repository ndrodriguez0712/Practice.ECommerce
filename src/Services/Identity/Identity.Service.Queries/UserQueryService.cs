using Identity.Persistence.Database;
using Identity.Service.Queries.DTOs;
using Identity.Service.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Common.Collection;
using Service.Common.Mapping;
using Service.Common.Paging;

namespace Identity.Service.Queries
{
    public class UserQueryService : IUserQueryService
    {
        #region Variables
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructor
        public UserQueryService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<DataCollection<UserDto>> GetAllAsync(int page, int take)
        {
            var collection = await _context.Users
                .OrderBy(x => x.Id)
                .GetPagedAsync(page, take);

            return collection.MapTo<DataCollection<UserDto>>();
        }

        public async Task<UserDto> GetAsync(int id)
        {
            return (await _context.Users.SingleAsync(x => x.Id == id)).MapTo<UserDto>();
        }
    }
}
