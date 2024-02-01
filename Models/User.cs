using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Malmasp.Models;

[EntityTypeConfiguration(typeof(UserEntityTypeConfiguration))]
public class User
{
    public ulong Id { get; set; }
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("bigint")
            .UseIdentityAlwaysColumn()
            .IsRequired();
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(128)")
            .IsRequired();
        builder.Property(x => x.Password)
            .HasColumnName("password")
            .HasColumnType("text")
            .IsRequired();
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();
    }
}