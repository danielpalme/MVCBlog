using MVCBlog.Data;

namespace MVCBlog.Web.Models.Administration;

public class ImagesSelectionViewModel
{
    public ImagesSelectionViewModel(List<Image> images)
    {
        this.Images = images;
    }

    public List<Image> Images { get; set; }
}
