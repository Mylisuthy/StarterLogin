using System;
using FluentAssertions;
using StarterLogin.Domain.Entities;
using Xunit;

namespace StarterLogin.UnitTests.Domain;

public class GenreTests
{
    [Fact]
    public void CreateGenre_WithValidData_ShouldSucceed()
    {
        // Arrange
        var name = "Sci-Fi";
        var description = "Science Fiction movies";

        // Act
        var genre = Genre.Create(name, description);

        // Assert
        genre.Name.Should().Be(name);
        genre.Description.Should().Be(description);
    }

    [Fact]
    public void CreateGenre_WithEmptyName_ShouldThrowException()
    {
        // Act
        Action act = () => Genre.Create("");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateGenre_ShouldUpdatePropertiesAndMarkAsUpdated()
    {
        // Arrange
        var genre = Genre.Create("Action");
        var newName = "Action & Adventure";
        
        // Act
        genre.Update(newName, "Updated description");

        // Assert
        genre.Name.Should().Be(newName);
        genre.UpdatedAt.Should().NotBeNull();
    }
}
