using System;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using System.Data.Entity;

namespace MVCBlog.Core.Commands
{
    public class AddOrUpdateFeedStatisticsCommandHandler : 
        ICommandHandler<AddOrUpdateSingleFeedUserCommand>,
        ICommandHandler<AddOrUpdateFeedAggregatorFeedUserCommand>
    {
        private readonly IRepository repository;

        public AddOrUpdateFeedStatisticsCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(AddOrUpdateSingleFeedUserCommand command)
        {
            this.DeleteOldStatistics();

            DateTime currentDay = DateTime.Now.Date;
            DateTime nextDay = DateTime.Now.Date.AddDays(1);

            var existingFeedStatistic = await this.repository.FeedStatistics
                .FirstOrDefaultAsync(f => f.Identifier == command.Identifier
                    && f.Application == command.Application
                    && f.Identifier != null
                    && f.Created >= currentDay
                    && f.Created < nextDay);

            if (existingFeedStatistic == null)
            {
                this.repository.FeedStatistics.Add(new FeedStatistic()
                {
                    Application = command.Application,
                    Identifier = command.Identifier,
                    Users = 1,
                    Visits = 1
                });
            }
            else
            {
                existingFeedStatistic.Visits++;
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task HandleAsync(AddOrUpdateFeedAggregatorFeedUserCommand command)
        {
            this.DeleteOldStatistics();

            DateTime currentDay = DateTime.Now.Date;
            DateTime nextDay = DateTime.Now.Date.AddDays(1);

            var existingFeedStatistic = await this.repository.FeedStatistics
                .FirstOrDefaultAsync(f => f.Application == command.Application
                    && f.Identifier == null
                    && f.Created >= currentDay
                    && f.Created < nextDay);

            if (existingFeedStatistic == null)
            {
                this.repository.FeedStatistics.Add(new FeedStatistic()
                {
                    Application = command.Application,
                    Users = command.Users,
                    Visits = 1
                });
            }
            else
            {
                existingFeedStatistic.Users = command.Users;
                existingFeedStatistic.Visits++;
            }

            await this.repository.SaveChangesAsync();
        }

        private void DeleteOldStatistics()
        {
            DateTime lastDay = DateTime.Now.Date.AddDays(-31);

            foreach (var feedStatistic in this.repository.FeedStatistics.Where(f => f.Created < lastDay))
            {
                this.repository.FeedStatistics.Remove(feedStatistic);
            }
        }
    }
}
