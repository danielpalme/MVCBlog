using MVCBlog.Business.Email;
using Xunit;

namespace MVCBlog.Business.Test.Email;

public class RecipientTest
{
    [Fact]
    public void Recipient_Equals()
    {
        Assert.True(new Recipient("test@test.de").Equals(new Recipient("test@test.de")));
        Assert.True(new Recipient("test@test.de").Equals(new Recipient("TEST@TEST.DE")));
        Assert.True(new Recipient("test@test.de").Equals(new Recipient("Max Mustermann", "TEST@TEST.DE")));
        Assert.True(new Recipient("Erika Musterfrau", "test@test.de").Equals(new Recipient("Max Mustermann", "TEST@TEST.DE")));
    }

    [Fact]
    public void Recipient_GetHashCode()
    {
        Assert.Equal(new Recipient("test@test.de").GetHashCode(), new Recipient("test@test.de").GetHashCode());
        Assert.Equal(new Recipient("test@test.de").GetHashCode(), new Recipient("TEST@TEST.DE").GetHashCode());
        Assert.Equal(new Recipient("test@test.de").GetHashCode(), new Recipient("Max Mustermann", "TEST@TEST.DE").GetHashCode());
        Assert.Equal(new Recipient("Erika Musterfrau", "test@test.de").GetHashCode(), new Recipient("Max Mustermann", "TEST@TEST.DE").GetHashCode());
    }
}
