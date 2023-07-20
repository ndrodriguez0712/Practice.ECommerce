using Customer.Domain;
using Customer.Persistence.Database;
using Customer.Service.EventHandlers.Commands;
using MediatR;

namespace Customer.Service.EventHandlers
{
    public class ClientEventHandler : INotificationHandler<ClientCreateCommand>
    {
        #region Variables
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructor
        public ClientEventHandler(
            ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task Handle(ClientCreateCommand notification, CancellationToken cancellationToken)
        {
            await _context.AddAsync(new Client
            {
                Name = notification.Name
            });

            await _context.SaveChangesAsync();
        }
    }
}
