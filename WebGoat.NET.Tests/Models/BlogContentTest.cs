using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGoat.NET.Models;

namespace WebGoat.NET.Tests.Models
{
    public class BlogContentTest
    {
        [Theory]
        [InlineData("Hello World")]
        [InlineData("Det er en kendsgerning, at man bliver distraheret af læsbart indhold på en side, når man betragter dens layout. Meningen med at bruge Lorem Ipsum er, at teksten indeholder mere eller mindre almindelig tekstopbygning i modsætning til")]
        public void ShouldCreateBlogContentObjectWithValidInputString(string blogContentMethodInput)
        {
            // Arrange

            // Act
            var actualObject = new BlogContent(blogContentMethodInput);

            // Assert
            Assert.Equal(blogContentMethodInput, actualObject.GetValue());
        }
    }
}
