using Microsoft.AspNetCore.Identity;
using MVCBlog.Localization;

namespace MVCBlog.Web.Infrastructure;

public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError()
    {
        return new IdentityError
        {
            Code = nameof(this.DefaultError),
            Description = Resources.IdentityErrorDefaultError
        };
    }

    public override IdentityError ConcurrencyFailure()
    {
        return new IdentityError
        {
            Code = nameof(this.ConcurrencyFailure),
            Description = Resources.IdentityErrorConcurrencyFailure
        };
    }

    public override IdentityError PasswordMismatch()
    {
        return new IdentityError
        {
            Code = nameof(this.PasswordMismatch),
            Description = Resources.IdentityErrorPasswordMismatch
        };
    }

    public override IdentityError InvalidToken()
    {
        return new IdentityError
        {
            Code = nameof(this.InvalidToken),
            Description = Resources.IdentityErrorInvalidToken
        };
    }

    public override IdentityError LoginAlreadyAssociated()
    {
        return new IdentityError
        {
            Code = nameof(this.LoginAlreadyAssociated),
            Description = Resources.IdentityErrorLoginAlreadyAssociated
        };
    }

    public override IdentityError InvalidUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(this.InvalidUserName),
            Description = string.Format(Resources.IdentityErrorInvalidUserName, userName)
        };
    }

    public override IdentityError InvalidEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(this.InvalidEmail),
            Description = string.Format(Resources.IdentityErrorInvalidEmail, email)
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(this.DuplicateUserName),
            Description = string.Format(Resources.IdentityErrorDuplicateUserName, userName)
        };
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(this.DuplicateEmail),
            Description = string.Format(Resources.IdentityErrorDuplicateEmail, email)
        };
    }

    public override IdentityError InvalidRoleName(string role)
    {
        return new IdentityError
        {
            Code = nameof(this.InvalidRoleName),
            Description = string.Format(Resources.IdentityErrorInvalidRoleName, role)
        };
    }

    public override IdentityError DuplicateRoleName(string role)
    {
        return new IdentityError
        {
            Code = nameof(this.DuplicateRoleName),
            Description = string.Format(Resources.IdentityErrorDuplicateRoleName, role)
        };
    }

    public override IdentityError UserAlreadyHasPassword()
    {
        return new IdentityError
        {
            Code = nameof(this.UserAlreadyHasPassword),
            Description = Resources.IdentityErrorUserAlreadyHasPassword
        };
    }

    public override IdentityError UserLockoutNotEnabled()
    {
        return new IdentityError
        {
            Code = nameof(this.UserLockoutNotEnabled),
            Description = Resources.IdentityErrorUserLockoutNotEnabled
        };
    }

    public override IdentityError UserAlreadyInRole(string role)
    {
        return new IdentityError
        {
            Code = nameof(this.UserAlreadyInRole),
            Description = string.Format(Resources.IdentityErrorUserAlreadyInRole, role)
        };
    }

    public override IdentityError UserNotInRole(string role)
    {
        return new IdentityError
        {
            Code = nameof(this.UserNotInRole),
            Description = string.Format(Resources.IdentityErrorUserNotInRole, role)
        };
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = nameof(this.PasswordTooShort),
            Description = string.Format(Resources.IdentityErrorPasswordTooShort, length)
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError
        {
            Code = nameof(this.PasswordRequiresNonAlphanumeric),
            Description = Resources.IdentityErrorPasswordRequiresNonAlphanumeric
        };
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError
        {
            Code = nameof(this.PasswordRequiresDigit),
            Description = Resources.IdentityErrorPasswordRequiresDigit
        };
    }

    public override IdentityError PasswordRequiresLower()
    {
        return new IdentityError
        {
            Code = nameof(this.PasswordRequiresLower),
            Description = Resources.IdentityErrorPasswordRequiresLower
        };
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return new IdentityError
        {
            Code = nameof(this.PasswordRequiresUpper),
            Description = Resources.IdentityErrorPasswordRequiresUpper
        };
    }
}
