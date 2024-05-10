using Tributech.SensorManager.Domain.ValueTypes;

namespace Domain.Tests;

public class SensorTypeTests
{
    [Fact]
    public void Should_Return_Same_Instance_For_Same_Input()
    {
        var status = SensorType.Parse("abc");
        var status1 = SensorType.Parse("abc");
        var eq = object.ReferenceEquals(status, status1);

        Assert.True(eq);
    }

    [Fact]
    public void Field_Should_Be_Equal_To_Value()
    {
        var status = SensorType.Temperature;
        var status1 = SensorType.Parse("Temperature");

        var eq = object.ReferenceEquals(status, status1);

        Assert.True(eq);
    }

    // check == and != operators
    [Fact]
    public void EqualsOperator_Should_Return_True_When_Both_Instances_Are_Same()
    {
        var status = SensorType.Temperature;
        var status1 = SensorType.Parse("Temperature");

        var eq = status == status1;

        Assert.True(eq);
    }

    [Fact]
    public void EqualsOperator_Should_Return_False_When_Both_Instances_Are_Not_Same()
    {
        var status = SensorType.Temperature;
        var status1 = SensorType.Parse("Humidity");

        var eq = status == status1;

        Assert.False(eq);
    }
}