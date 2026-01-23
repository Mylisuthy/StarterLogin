using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using StarterLogin.Api.Controllers;
using StarterLogin.Application.Common.Interfaces;
using StarterLogin.Application.Multimedia.Common;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;
using Xunit;

namespace StarterLogin.UnitTests.Api;

public class MediaControllerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ICloudinaryService> _cloudinaryMock;
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly MediaController _controller;

    public MediaControllerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _cloudinaryMock = new Mock<ICloudinaryService>();
        _cacheMock = new Mock<IMemoryCache>();
        
        // Mocking IMemoryCache TryGetValue
        object? cacheEntry = null;
        _cacheMock.Setup(m => m.TryGetValue(It.IsAny<object>(), out cacheEntry)).Returns(false);
        _cacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

        _controller = new MediaController(_uowMock.Object, _cloudinaryMock.Object, _cacheMock.Object);
        
        // Mocking User Identity
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task GetAll_ShouldReturnMediaFromUnitOfWork_WhenCacheIsEmpty()
    {
        // Arrange
        var media = new List<MediaContent> { Movie.Create("Matrix", "Sci-fi", Guid.NewGuid()) };
        _uowMock.Setup(u => u.Media.GetAllAsync()).ReturnsAsync(media);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.As<OkObjectResult>();
        var response = okResult.Value.As<IEnumerable<MediaResponse>>();
        response.Should().HaveCount(1);
        _uowMock.Verify(u => u.Media.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_WithRestrictedRating_ShouldAllowAdmin()
    {
        // Arrange
        var mediaId = Guid.NewGuid();
        var restrictedMovie = Movie.Create("Restricted", "Desc", Guid.NewGuid(), rating: "R");
        _uowMock.Setup(u => u.Media.GetByIdAsync(mediaId)).ReturnsAsync(restrictedMovie);
        
        // Act
        var result = await _controller.GetById(mediaId);

        // Assert
        var okResult = result.Result.As<OkObjectResult>();
        okResult.Should().NotBeNull();
    }
}
