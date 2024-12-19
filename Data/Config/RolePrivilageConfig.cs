using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config;

public class RolePrivilageConfig : IEntityTypeConfiguration<RolePrivilage>
{
    public void Configure(EntityTypeBuilder<RolePrivilage> builder)
    {
        builder.ToTable("RolePrivilages");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        
        builder.Property(n =>n.RolePrivilageName).HasMaxLength(50).IsRequired();
        builder.Property(n => n.Description);
        builder.Property(n => n.IsActive).IsRequired();
        builder.Property(n => n.IsDeleted).IsRequired();
        builder.Property(n => n.CreatedDate).IsRequired();

        builder.HasOne(n => n.Role).WithMany(n => n.RolePrivilages)
            .HasForeignKey(n => n.RoleId).HasConstraintName("FK_RolePrivilages_Role");
    }
}