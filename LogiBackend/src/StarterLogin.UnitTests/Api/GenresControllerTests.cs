using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StarterLogin.Api.Controllers;
using StarterLogin.Application.Multimedia.Common;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using Xunit;

namespace StarterLogin.UnitTests.Api;

public class GenresControllerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly GenresController _controller;

    public GenresControllerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _controller = new GenresController(_uowMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllGenres()
    {
        // Arrange
        var genres = new List<Genre> { Genre.Create("Action"), Genre.Create("Drama") };
        _uowMock.Setup(u => u.Genres.GetAllAsync()).ReturnsAsync(genres);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.As<OkObjectResult>();
        var response = okResult.Value.As<IEnumerable<GenreResponse>>();
        response.Should().HaveCount(2);
    }

    [Fact]
    public async Task Create_ShouldAddGenreAndReturnCreatedAt()
    {
        // Arrange
        var request = new CreateGenreRequest("Sci-Fi", "Description");
        
        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = result.Result.As<CreatedAtActionResult>();
        createdResult.ActionName.Should().Be(nameof(GenresController.GetById));
        _uowMock.Verify(u => u.Genres.AddAsync(It.IsAny<Genre>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
