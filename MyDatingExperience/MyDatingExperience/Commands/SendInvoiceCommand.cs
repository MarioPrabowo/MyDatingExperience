using MediatR;
using MyDatingExperience.DTOs;
using MyDatingExperience.Repository;
using MyDatingExperience.Services;

namespace MyDatingExperience.Commands
{
    public class SendInvoiceCommand : IRequest<SendInvoiceResponse>
    {
        public SendInvoiceRequest Request { get; set; }

        public class Handler : IRequestHandler<SendInvoiceCommand, SendInvoiceResponse>
        {
            private DateDbContext _dbContext;
            private IDateTimeService _dateTimeService;

            public Handler(DateDbContext dbContext, IDateTimeService dateTimeService)
            {
                _dbContext = dbContext;
                _dateTimeService = dateTimeService;
            }

            public async Task<SendInvoiceResponse> Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
            {
                var entity = new InvoiceEntity
                {
                    InvoiceId = Guid.NewGuid(),
                    Sender = request.Request.Sender,
                    Amount = request.Request.Amount,
                    SentDate = _dateTimeService.GetUtcNow(),
                };
                _dbContext.Add(entity);
                await _dbContext.SaveChangesAsync();

                return new SendInvoiceResponse
                {
                   InvoiceId = entity.InvoiceId,
                   Sender = entity.Sender,
                   Amount= entity.Amount,
                   SentDate = entity.SentDate,
                };
            }
        }
    }
}
