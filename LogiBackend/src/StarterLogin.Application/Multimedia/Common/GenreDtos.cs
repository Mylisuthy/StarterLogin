using System;

namespace StarterLogin.Application.Multimedia.Common;

public record GenreResponse(Guid Id, string Name, string? Description);
public record CreateGenreRequest(string Name, string? Description);
public record UpdateGenreRequest(string Name, string? Description);
