using System;
using FluentAssertions;
using StarterLogin.Domain.Entities;
using Xunit;

namespace StarterLogin.UnitTests.Domain;

public class MediaContentTests
{
    [Fact]
    public void CreateMovie_WithValidData_ShouldSucceed()
    {
        // Arrange
        var title = "Inception";
        var description = "A thief who steals corporate secrets through the use of dream-sharing technology.";
        var genreId = Guid.NewGuid();
        var duration = TimeSpan.FromMinutes(148);

        // Act
        var movie = Movie.Create(title, description, genreId, duration: duration);

        // Assert
        movie.Should().NotBeNull();
        movie.Title.Should().Be(title);
        movie.Description.Should().Be(description);
        movie.GenreId.Should().Be(genreId);
        movie.Duration.Should().Be(duration);
        movie.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void CreateMovie_WithEmptyTitle_ShouldThrowException()
    {
        // Arrange
        var title = "";
        var description = "Test";
        var genreId = Guid.NewGuid();

        // Act
        Action act = () => Movie.Create(title, description, genreId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Title is required.");
    }

    [Fact]
    public void CreateSeries_WithValidData_ShouldSucceed()
    {
        // Arrange
        var title = "Breaking Bad";
        var description = "A high school chemistry teacher turned meth producer.";
        var genreId = Guid.NewGuid();

        // Act
        var series = Series.Create(title, description, genreId);

        // Assert
        series.Should().NotBeNull();
        series.Title.Should().Be(title);
        series.Seasons.Should().BeEmpty();
    }

    [Fact]
    public void AddSeasonToSeries_ShouldIncreaseSeasonCount()
    {
        // Arrange
        var series = Series.Create("Test Series", "Description", Guid.NewGuid());
        var season = Season.Create(1, series.Id, "Season 1");

        // Act
        series.AddSeason(season);

        // Assert
        series.Seasons.Should().HaveCount(1);
        series.Seasons.Should().Contain(season);
    }
}
