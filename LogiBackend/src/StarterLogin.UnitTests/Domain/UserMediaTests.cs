using System;
using FluentAssertions;
using StarterLogin.Domain.Entities;
using Xunit;

namespace StarterLogin.UnitTests.Domain;

public class UserMediaTests
{
    [Fact]
    public void UpdateProgress_ShouldUpdateWatchedTimeAndStatus()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mediaId = Guid.NewGuid();
        var history = new UserMediaHistory(userId, mediaId, TimeSpan.Zero, false);
        var newProgress = TimeSpan.FromMinutes(30);

        // Act
        history.UpdateProgress(newProgress, true);

        // Assert
        history.WatchedTime.Should().Be(newProgress);
        history.IsFinished.Should().BeTrue();
        history.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void CreateFavorite_ShouldSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mediaId = Guid.NewGuid();

        // Act
        var favorite = new UserFavorite(userId, mediaId);

        // Assert
        favorite.UserId.Should().Be(userId);
        favorite.MediaContentId.Should().Be(mediaId);
    }
}
