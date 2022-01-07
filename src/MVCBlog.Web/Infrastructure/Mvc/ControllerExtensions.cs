using Microsoft.AspNetCore.Mvc;

namespace MVCBlog.Web.Infrastructure.Mvc;

public static class ControllerExtensions
{
    public const string SuccessMessage = "SuccessMessage";

    public const string WarningMessage = "WarningMessage";

    public const string ErrorMessage = "ErrorMessage";

    public static void SetSuccessMessage(this Controller controller, string message)
    {
        controller.TempData[SuccessMessage] = message;
    }

    public static void SetWarningMessage(this Controller controller, string message)
    {
        controller.TempData[WarningMessage] = message;
    }

    public static void SetErrorMessage(this Controller controller, string message)
    {
        controller.TempData[ErrorMessage] = message;
    }
}
