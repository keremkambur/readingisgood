using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReadingIsGood.DataLayer.Extensions
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder HasDefaultNewId(this PropertyBuilder builder)
        {
            return builder.HasDefaultValueSql("NEWID()");
        }

        public static PropertyBuilder<Guid> HasDefaultNewId(this PropertyBuilder<Guid> builder)
        {
            return builder.HasDefaultValueSql("NEWID()");
        }

        public static PropertyBuilder<Guid?> HasDefaultNewId(this PropertyBuilder<Guid?> builder)
        {
            return builder.HasDefaultValueSql("NEWID()");
        }

        public static PropertyBuilder<DateTime> IsDatetime2(this PropertyBuilder<DateTime> builder)
        {
            return builder.HasColumnType("datetime2(7)");
        }

        public static PropertyBuilder<DateTime?> IsDatetime2(this PropertyBuilder<DateTime?> builder)
        {
            return builder.HasColumnType("datetime2(7)");
        }

        public static PropertyBuilder<DateTime> IsDate(this PropertyBuilder<DateTime> builder)
        {
            return builder.HasColumnType("DATE");
        }

        public static PropertyBuilder<DateTime?> IsDate(this PropertyBuilder<DateTime?> builder)
        {
            return builder.HasColumnType("DATE");
        }

        public static PropertyBuilder<DateTime> IsTime(this PropertyBuilder<DateTime> builder)
        {
            return builder.HasColumnType("TIME");
        }

        public static PropertyBuilder<DateTime?> IsTime(this PropertyBuilder<DateTime?> builder)
        {
            return builder.HasColumnType("TIME");
        }
    }
}