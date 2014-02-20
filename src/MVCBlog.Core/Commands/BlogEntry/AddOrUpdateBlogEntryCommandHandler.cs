using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Commands
{
    public class AddOrUpdateBlogEntryCommandHandler : 
        ICommandHandler<AddBlogEntryCommand>,
        ICommandHandler<UpdateBlogEntryCommand>
    {
        private readonly IRepository repository;

        public AddOrUpdateBlogEntryCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(AddBlogEntryCommand command)
        {
            this.AddTags(command.Entity, command.Tags);

            this.repository.BlogEntries.Add(command.Entity);
            await this.repository.SaveChangesAsync();
        }

        public async Task HandleAsync(UpdateBlogEntryCommand command)
        {
            this.AddTags(command.Entity, command.Tags);

            var entry = this.repository.Entry(command.Entity);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.repository.BlogEntries.Attach(command.Entity);
            }

            entry.State = System.Data.Entity.EntityState.Modified;

            await this.repository.SaveChangesAsync();
        }

        /// <summary>
        /// Adds the tags to the given <see cref="BlogEntry"/>.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="tags">The tags.</param>
        private void AddTags(BlogEntry entry, IEnumerable<string> tags)
        {
            var existingTags = this.repository.Tags.ToList();

            if (entry.Tags == null)
            {
                entry.Tags = new Collection<Tag>();
            }

            foreach (var tag in entry.Tags.Where(t => !tags.Contains(t.Name)).ToArray())
            {
                entry.Tags.Remove(tag);
            }

            foreach (var tag in tags.Where(t => !entry.Tags.Select(et => et.Name).Contains(t)).ToArray())
            {
                var existingTag = existingTags.SingleOrDefault(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase));

                if (existingTag == null)
                {
                    existingTag = new Tag() { Name = tag };
                    existingTags.Add(existingTag);
                }

                entry.Tags.Add(existingTag);
            }
        }
    }
}
