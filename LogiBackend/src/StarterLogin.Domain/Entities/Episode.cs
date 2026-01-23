using System;
using StarterLogin.Domain.Common;

namespace StarterLogin.Domain.Entities;

public class Episode : BaseEntity
{
    public int Number { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string? VideoUrl { get; private set; }
    public TimeSpan? Duration { get; private set; }

    public Guid SeasonId { get; private set; }
    public virtual Season Season { get; private set; }

    private Episode() { }

    private Episode(int number, string title, string description, Guid seasonId, string? videoUrl = null, TimeSpan? duration = null)
    {
        Number = number;
        Title = title;
        Description = description;
        SeasonId = seasonId;
        VideoUrl = videoUrl;
        Duration = duration;
    }

    public static Episode Create(int number, string title, string description, Guid seasonId, string? videoUrl = null, TimeSpan? duration = null)
    {
        if (number <= 0) throw new ArgumentException("Episode number must be greater than 0.");
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        return new Episode(number, title, description, seasonId, videoUrl, duration);
    }

    public void Update(int number, string title, string description, string? videoUrl, TimeSpan? duration)
    {
        Number = number;
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        Duration = duration;
        MarkAsUpdated();
    }
}
