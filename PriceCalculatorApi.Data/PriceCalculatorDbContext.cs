using Microsoft.EntityFrameworkCore;

namespace PriceCalculatorApi.Data;

public class PriceCalculatorDbContext(DbContextOptions<PriceCalculatorDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<ProductIngredient> ProductIngredients { get; set; } = null!;
    public DbSet<ProductLabor> ProductLabors { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<ItemProduct> ItemProducts { get; set; } = null!;
    public DbSet<ItemLabor> ItemLabors { get; set; } = null!;
    public DbSet<ItemIngredient> ItemIngredients { get; set; } = null!;
    public DbSet<Settings> Settings { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
}
