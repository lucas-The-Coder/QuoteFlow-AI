using Microsoft.EntityFrameworkCore;
using WorkerService.Models;

namespace WorkerService.Data.Repositories
{
    public class FollowUpRepository : IFollowUpRepository
    {
        private readonly WorkerDbContext _context;

        public FollowUpRepository(WorkerDbContext context)
        {
            _context = context;
        }

        public async Task<FollowUp?> GetByIdAsync(Guid id)
        {
            return await _context.FollowUps.FindAsync(id);
        }

        public async Task<IEnumerable<FollowUp>> GetDueFollowUpsAsync(DateTime asOf)
        {
            return await _context.FollowUps
                .Where(f => f.ScheduledDate <= asOf && f.Status == "Pending")
                .ToListAsync();
        }

        public async Task<FollowUp> CreateAsync(FollowUp followUp)
        {
            _context.FollowUps.Add(followUp);
            await _context.SaveChangesAsync();
            return followUp;
        }

        public async Task<FollowUp> UpdateAsync(FollowUp followUp)
        {
            _context.FollowUps.Update(followUp);
            await _context.SaveChangesAsync();
            return followUp;
        }

        public async Task UpdateStatusAsync(Guid id, string status)
        {
            var followUp = await _context.FollowUps.FindAsync(id);
            if (followUp != null)
            {
                followUp.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDeliveryStatusAsync(Guid id, string deliveryStatus)
        {
            var followUp = await _context.FollowUps.FindAsync(id);
            if (followUp != null)
            {
                followUp.DeliveryStatus = deliveryStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsSentAsync(Guid id, DateTime sentDate, string externalMessageId)
        {
            var followUp = await _context.FollowUps.FindAsync(id);
            if (followUp != null)
            {
                followUp.SentDate = sentDate;
                followUp.Status = "Sent";
                followUp.ExternalMessageId = externalMessageId;
                await _context.SaveChangesAsync();
            }
        }
    }
}