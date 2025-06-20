﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Api.Data.Mapeamentos.Identity;

public class IdentityUserClaimMap : IEntityTypeConfiguration<IdentityUserClaim<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<long>> builder)
    {
        builder.ToTable("IdentityClaim");
        builder.HasKey(uc => uc.Id);
        builder.Property(u => u.ClaimType).HasMaxLength(255);
        builder.Property(u => u.ClaimValue).HasMaxLength(255);
    }
}