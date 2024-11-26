using WebGoat.NET.Models;

namespace WebGoat.NET.Tests.Models
{
    public class BlogContentTest
    {
        [Theory]
        [InlineData("HelloWorld")] // Without space
        [InlineData("Hello World")] // With space
        [InlineData("Hello World æøå ÆØÅ")] // Danish characters
        [InlineData("Det er en kendsgerning, at man bliver distraheret af læsbart indhold på en side, når man betragter dens layout. " +
        "Meningen med at bruge Lorem Ipsum er, at teksten indeholder mere eller mindre almindelig tekstopbygning i modsætning til")] // Long text
        [InlineData(".,!?")] // Special characters, but allowed
        public void ShouldCreateBlogContentObjectWithValidInputString(string blogContentMethodInput)
        {
            // Arrange

            // Act
            var actualObject = new BlogContent(blogContentMethodInput);

            // Assert
            Assert.Equal(blogContentMethodInput, actualObject.GetValue());
        }

        [Theory]
        [InlineData("<b>")]
        [InlineData("<i>")]
        [InlineData("<p>")]
        [InlineData("<li>")]
        [InlineData("<ol>")]
        [InlineData("<p><b><i>hejsa</i></b></p>")]
        public void ShouldAllowSpecificHtmlTagsAsInputString(string blogContentMethodInput)
        {
            // Arrange

            // Act
            var actualObject = new BlogContent(blogContentMethodInput);

            // Assert
            Assert.Equal(blogContentMethodInput, actualObject.GetValue());
        }

        [Theory]
        [InlineData("<Script>")]
        [InlineData("-- //")]
        public void ShouldThrowArgumentExceptionWhenInvalidInputString(string blogContentMethodInput)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new BlogContent(blogContentMethodInput));
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWhenEmptyInputString()
        {
            // Arrange
            string blogContentMethodInput = string.Empty;

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new BlogContent(blogContentMethodInput));
        }
    }
}
