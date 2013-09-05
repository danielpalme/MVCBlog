using MVCBlog.Core.Database;

namespace MVCBlog.Core.Commands
{
    public class AddImageCommandHandler : ICommandHandler<AddImageCommand>
    {
        private readonly IRepository repository;

        public AddImageCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(AddImageCommand command)
        {
            int indexOfLastDot = command.FileName.LastIndexOf('.');
            string name = command.FileName.Substring(0, indexOfLastDot);
            string extension = command.FileName.Substring(indexOfLastDot + 1, command.FileName.Length - indexOfLastDot - 1);

            var image = new MVCBlog.Core.Entities.Image() { Name = name, Extension = extension };
            image.Data = command.Data;

            this.repository.Images.Add(image);
            this.repository.SaveChanges();
        }
    }
}
