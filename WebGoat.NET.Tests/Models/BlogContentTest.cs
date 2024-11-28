using WebGoat.NET.Models;

namespace WebGoat.NET.Tests.Models
{
    public class BlogContentTest
    {
        [Theory]
        [InlineData("HelloWorld")] // Without space
        [InlineData("Hello World")] // With space
        [InlineData("Lorem Ipsum is simply dummy text of the printing and typesetting industry. " +
         "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. " +
         "It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. " +
         "It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with " +
          "desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. " +
         "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text " +
         "ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. " +
         "It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. " +
         "It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software " +
         "like Aldus PageMaker including versions of Lorem Ipsum.")] // Long Text
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
        [InlineData("Hello æ ø å")] // No Danish characters
        [InlineData("<b onmouseover=\"alert(1)\">Bold Text</b>\r\n")]
        [InlineData("<script\\n>alert(1);</script>\r\n")]
        [InlineData("<b><script>alert(1);</b></script>\r\n")]
        [InlineData("<script>alert(String.fromCharCode(88,83,83));</script>\r\n")]
        [InlineData("<a href=\"javascript:alert(1)\">Click me</a>\r\n")]
        [InlineData("<svg><script>alert(1)</script></svg>\r\n")]
        [InlineData("<b title=\"<script>alert(1)</script>\">Bold</b>\r\n")]
        [InlineData("<b>{{alert(1)}}</b>\r\n")]
        [InlineData("&#x3C;&#x73;&#x63;&#x72;&#x69;&#x70;&#x74;&#x3E;alert('XSS')&#x3C;&#x2F;&#x73;&#x63;&#x72;&#x69;&#x70;&#x74;&#x3E;\r\n")]
        [InlineData("<script>alert('XSS')</script>\r\n")]
        [InlineData("<b style=\"background:url(javascript:alert(1))\">Bold</b>\r\n")]
        [InlineData("&lt;scr&lt;ipt&gt;alert(1)&lt;/scr&lt;ipt&gt;\r\n")]
        [InlineData("<p onclick=\"eval('alert(1)')\">Click me</p>\r\n")]
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

        [Fact]
        public void ShouldThrowArgumentExceptionWhenInputStringIsTooLong()
        {
            // Arrange
            int numberOfCharInTestString = 5001;
            string veryLongBlogContentMethodInput = new string('a', numberOfCharInTestString);
            
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new BlogContent(veryLongBlogContentMethodInput));
        }

        [Fact]
        public void ShouldThrowAnyExceptionWhenInputStringIsExtremlyLong()
        {
            // Arrange
            int numberOfCharInTestString = 1000000000;
            string veryLongBlogContentMethodInput = new string('a', numberOfCharInTestString);

            // Act
            // Assert
            Assert.ThrowsAny<Exception> (() => new BlogContent(veryLongBlogContentMethodInput));
        }
    }
}
