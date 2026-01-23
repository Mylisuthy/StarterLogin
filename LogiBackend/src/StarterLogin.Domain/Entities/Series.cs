using System;
using System.Collections.Generic;

namespace StarterLogin.Domain.Entities;

public class Series : MediaContent
{
    private readonly List<Season> _seasons = new();
    public virtual IReadOnlyCollection<Season> Seasons => _seasons.AsReadOnly();

    private Series() { }

    private Series(
        string title, 
        string description, 
        Guid genreId,
        string? imageUrl = null, 
        DateTime? releaseDate = null, 
        string? rating = null) 
        : base(title, description, genreId, imageUrl, null, null, releaseDate, rating)
    {
    }

    public static Series Create(
        string title, 
        string description, 
        Guid genreId,
        string? imageUrl = null, 
        DateTime? releaseDate = null, 
        string? rating = null)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        return new Series(title, description, genreId, imageUrl, releaseDate, rating);
    }

    public void Update(
        string title, 
        string description, 
        Guid genreId,
        string? imageUrl = null, 
        DateTime? releaseDate = null, 
        string? rating = null)
    {
        Title = title;
        Description = description;
        GenreId = genreId;
        ImageUrl = imageUrl;
        ReleaseDate = releaseDate;
        Rating = rating;
        MarkAsUpdated();
    }

    public void AddSeason(Season season)
    {
        _seasons.Add(season);
    }
}
