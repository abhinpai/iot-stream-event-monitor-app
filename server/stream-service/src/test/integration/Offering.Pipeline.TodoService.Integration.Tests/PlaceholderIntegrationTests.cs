// ************************************************************************
// *****      COPYRIGHT 2014 - 2018 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

namespace Offering.Pipeline.TodoService.Integration.Tests
{
    using Xunit;

    public class PlaceholderIntegrationTests
    {
        [Fact]
        public void PlaceholderTest()
        {
            // Arrange
            var n1 = 1;

            // Act
            var n2 = n1;

            // Assert
            Assert.Equal(n1, n2);
        }
    }
}