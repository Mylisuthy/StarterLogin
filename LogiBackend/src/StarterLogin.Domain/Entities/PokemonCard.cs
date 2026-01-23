using System;

namespace StarterLogin.Domain.Entities;

public class PokemonCard
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string ImageUrl { get; private set; }
    public string Description { get; private set; }
    public int HP { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public bool IsPublished { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected PokemonCard() { }

    private PokemonCard(
        string title, 
        string imageUrl, 
        string description, 
        int hp, 
        int attack, 
        int defense)
    {
        Id = Guid.NewGuid();
        Title = title;
        ImageUrl = imageUrl;
        Description = description;
        HP = hp;
        Attack = attack;
        Defense = defense;
        IsPublished = false;
        CreatedAt = DateTime.UtcNow;
    }

    public static PokemonCard Create(
        string title, 
        string imageUrl, 
        string description, 
        int hp, 
        int attack, 
        int defense)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        if (string.IsNullOrWhiteSpace(imageUrl)) throw new ArgumentException("Image URL is required.");
        if (hp < 0) throw new ArgumentException("HP cannot be negative.");
        if (attack < 0) throw new ArgumentException("Attack cannot be negative.");
        if (defense < 0) throw new ArgumentException("Defense cannot be negative.");

        return new PokemonCard(title, imageUrl, description, hp, attack, defense);
    }

    public void Update(
        string title, 
        string imageUrl, 
        string description, 
        int hp, 
        int attack, 
        int defense)
    {
        Title = title;
        ImageUrl = imageUrl;
        Description = description;
        HP = hp;
        Attack = attack;
        Defense = defense;
    }

    public void SetPublished(bool isPublished)
    {
        IsPublished = isPublished;
    }
}
