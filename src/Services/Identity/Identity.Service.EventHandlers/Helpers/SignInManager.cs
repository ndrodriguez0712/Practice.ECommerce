using Identity.Domain;
using Identity.Persistence.Database;
using Identity.Persistence.Database.Interfaces;
using Identity.Service.EventHandlers.Helpers.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Service.EventHandlers.Helpers
{
    public class SignInManager : ISignInManager
    {
        #region Variables
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor        
        public SignInManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        public async Task<ApplicationUser> SingleAsync(string email)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IUnitOfWork<ApplicationDbContext> _UnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
            IBaseRepository<ApplicationUser> _UserAccessRepository = _UnitOfWork.GetRepository<ApplicationUser>();


        }

        public async Task<bool> CheckPasswordSignInAsync(string user, string password)
        {

        }
    }
}
