using System;
using System.Linq;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;

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

        public void Handle(AddOrUpdateSingleFeedUserCommand command)
        {
            this.DeleteOldStatistics();

            DateTime currentDay = DateTime.Now.Date;
            DateTime nextDay = DateTime.Now.Date.AddDays(1);

            var existingFeedStatistic = this.repository.FeedStatistics
                .FirstOrDefault(f => f.Identifier == command.Identifier
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

            this.repository.SaveChanges();
        }

        public void Handle(AddOrUpdateFeedAggregatorFeedUserCommand command)
        {
            this.DeleteOldStatistics();

            DateTime currentDay = DateTime.Now.Date;
            DateTime nextDay = DateTime.Now.Date.AddDays(1);

            var existingFeedStatistic = this.repository.FeedStatistics
                .FirstOrDefault(f => f.Application == command.Application
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

            this.repository.SaveChanges();
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
