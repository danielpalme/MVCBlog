using System.Threading.Tasks;

namespace MVCBlog.Business.Email
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Message message);
    }
}
