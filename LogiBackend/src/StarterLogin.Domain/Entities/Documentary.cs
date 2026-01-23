using System;

namespace StarterLogin.Domain.Entities;

public class Documentary : MediaContent
{
    private Documentary() { }

    private Documentary(
        string title, 
        string description, 
        Guid genreId,
        string? imageUrl = null, 
        string? videoUrl = null, 
        TimeSpan? duration = null, 
        DateTime? releaseDate = null, 
        string? rating = null) 
        : base(title, description, genreId, imageUrl, videoUrl, duration, releaseDate, rating)
    {
    }

    public static Documentary Create(
        string title, 
        string description, 
        Guid genreId,
        string? imageUrl = null, 
        string? videoUrl = null, 
        TimeSpan? duration = null, 
        DateTime? releaseDate = null, 
        string? rating = null)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        return new Documentary(title, description, genreId, imageUrl, videoUrl, duration, releaseDate, rating);
    }

    public void Update(
        string title, 
        string description, 
        Guid genreId,
        string? imageUrl = null, 
        string? videoUrl = null, 
        TimeSpan? duration = null, 
        DateTime? releaseDate = null, 
        string? rating = null)
    {
        Title = title;
        Description = description;
        GenreId = genreId;
        ImageUrl = imageUrl;
        VideoUrl = videoUrl;
        Duration = duration;
        ReleaseDate = releaseDate;
        Rating = rating;
        MarkAsUpdated();
    }
}
