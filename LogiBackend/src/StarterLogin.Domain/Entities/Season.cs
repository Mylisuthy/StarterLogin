using System;
using System.Collections.Generic;
using StarterLogin.Domain.Common;

namespace StarterLogin.Domain.Entities;

public class Season : BaseEntity
{
    public int Number { get; private set; }
    public string? Title { get; private set; }
    
    public Guid SeriesId { get; private set; }
    public virtual Series Series { get; private set; }

    private readonly List<Episode> _episodes = new();
    public virtual IReadOnlyCollection<Episode> Episodes => _episodes.AsReadOnly();

    private Season() { }

    private Season(int number, Guid seriesId, string? title = null)
    {
        Number = number;
        SeriesId = seriesId;
        Title = title;
    }

    public static Season Create(int number, Guid seriesId, string? title = null)
    {
        if (number <= 0) throw new ArgumentException("Season number must be greater than 0.");
        return new Season(number, seriesId, title);
    }

    public void Update(int number, string? title)
    {
        Number = number;
        Title = title;
        MarkAsUpdated();
    }

    public void AddEpisode(Episode episode)
    {
        _episodes.Add(episode);
    }
}
