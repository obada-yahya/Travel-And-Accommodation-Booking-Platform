using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Common.Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder
            .Property(image => image.Format)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<ImageFormat>());

        builder
            .Property(image => image.EntityId)
            .IsRequired();

        builder
            .Property(image => image.Url)
            .IsRequired();
    }
}