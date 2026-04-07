using Xunit;

namespace AcessaAi.Tests
{
    public class Class1
    {
        [Fact]
        public void Sum_ShouldReturnCorrectValue()
        {
            // Arrange
            var a = 2;
            var b = 3;

            // Act
            var result = a + b;

            // Assert
            Assert.Equal(5, result);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("test@email.com", true)]
        public void Email_ShouldValidateCorrectly(string? email, bool expected)
        {
            // Act
            var result = !string.IsNullOrEmpty(email) && email.Contains("@");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void List_ShouldContainAddedItem()
        {
            // Arrange
            var list = new List<string>();

            // Act
            list.Add("AcessaAi");

            // Assert
            Assert.Single(list);
            Assert.Contains("AcessaAi", list);
        }
    }
}
