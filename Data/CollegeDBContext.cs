using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data;

public class CollegeDBContext : DbContext
{
    public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
    {
        
    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePrivilage> RolePrivilages { get; set; }
    public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
    public DbSet<UserType> UserTypes { get; set; }

    //TO seed data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Table 1
        modelBuilder.ApplyConfiguration(new StudentConfig()); // Here we are importing data from the studentConfigi
        
        //Table 2: This is the easy way to create table and seed data
        modelBuilder.ApplyConfiguration(new DepartmentConfig());
        
        //Table 2: This is the easy way to create table and seed data
        modelBuilder.ApplyConfiguration(new UserConfig());
        
        //Table 2: This is the easy way to create table and seed data
        modelBuilder.ApplyConfiguration(new RoleConfig());
        
        //Table 2: This is the easy way to create table and seed data
        modelBuilder.ApplyConfiguration(new RolePrivilageConfig());
        
        //Table 2: This is the easy way to create table and seed data
        modelBuilder.ApplyConfiguration(new UserRoleMappingConfig());
        
        //Table 2: This is the easy way to create table and seed data
        modelBuilder.ApplyConfiguration(new UserTypeConfig());
    }
}