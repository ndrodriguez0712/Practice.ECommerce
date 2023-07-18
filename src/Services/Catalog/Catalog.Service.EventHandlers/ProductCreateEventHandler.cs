using Catalog.Domain;
using Catalog.Persistence.Database;
using Catalog.Service.EventHandlers.Commands;
using MediatR;

namespace Catalog.Service.EventHandlers
{
    internal class ProductCreateEventHandler : INotificationHandler<ProductCreateCommand>
    {
        #region Variables
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructor
        public ProductCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task Handle(ProductCreateCommand command, CancellationToken cancellationToken)
        {
            await _context.AddAsync(new Product
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
            });

            await _context.SaveChangesAsync();
        }
    }
}

