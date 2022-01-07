using Xunit;

namespace MVCBlog.Business.Test;

public class BusinessRuleExceptionTest
{
    [Fact]
    public void Constructor()
    {
        Assert.NotNull(new BusinessRuleException().Message);
    }

    [Fact]
    public void Constructor_MessageApplied()
    {
        Assert.Equal("Test", new BusinessRuleException("Test").Message);
    }
}