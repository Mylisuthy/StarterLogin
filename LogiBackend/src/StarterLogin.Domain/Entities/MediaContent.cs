using System;
using StarterLogin.Domain.Common;

namespace StarterLogin.Domain.Entities;

public abstract class MediaContent : BaseEntity
{
    public string Title { get; protected set; }
    public string Description { get; protected set; }
    public string? ImageUrl { get; protected set; }
    public string? VideoUrl { get; protected set; }
    public TimeSpan? Duration { get; protected set; }
    public DateTime? ReleaseDate { get; protected set; }
    public string? Rating { get; protected set; } // e.g., PG-13, R
    
    public Guid GenreId { get; protected set; }
    public virtual Genre Genre { get; protected set; }

    protected MediaContent() { }

    protected MediaContent(
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
    }
}
