namespace Kwetter.Core.Tests;

public record TestValueObject(string Value);

public class ValueObjectTests
{
    [Theory]
    [InlineData(null, null, true)]
    [InlineData("foo", "bar", false)]
    [InlineData("bar", "bar", true)]
    [InlineData("bar", "Bar", false)]
    [InlineData("bar", "foobar", false)]
    public void EqualityOperator_Tests(string leftArg, string rightArg, bool expected)
    {
        var left = new TestValueObject(leftArg);
        var right = new TestValueObject(rightArg);

        var result = left == right;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, null, false)]
    [InlineData("foo", "bar", true)]
    [InlineData("bar", "bar", false)]
    [InlineData("bar", "Bar", true)]
    [InlineData("bar", "foobar", true)]
    public void InEqualityOperator_Tests(string leftArg, string rightArg, bool expected)
    {
        var left = new TestValueObject(leftArg);
        var right = new TestValueObject(rightArg);

        var result = left != right;
        Assert.Equal(expected, result);
    }
}