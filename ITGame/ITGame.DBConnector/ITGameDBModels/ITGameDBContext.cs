namespace ITGame.DBConnector.ITGameDBModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ITGameDBContext : DbContext
    {
        public ITGameDBContext()
            : base("name=ITGameDBContextConnectionString")
        {
           
        }

        public virtual DbSet<Armor> Armor { get; set; }
        public virtual DbSet<Character> Character { get; set; }
        public virtual DbSet<Humanoid> Humanoid { get; set; }
        public virtual DbSet<HumanoidRace> HumanoidRace { get; set; }
        public virtual DbSet<Spell> Spell { get; set; }
        public virtual DbSet<Surface> Surface { get; set; }
        public virtual DbSet<SurfaceRule> SurfaceRule { get; set; }
        public virtual DbSet<Weapon> Weapon { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Armor>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Character>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Character>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Character>()
                .HasOptional(e => e.Humanoid)
                .WithRequired(e => e.Character);

            modelBuilder.Entity<Humanoid>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Humanoid>()
                .HasMany(e => e.Armors)
                .WithMany(e => e.Humanoid)
                .Map(m => m.ToTable("Hum_Armor"));

            modelBuilder.Entity<HumanoidRace>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<HumanoidRace>()
                .HasMany(e => e.Humanoid)
                .WithRequired(e => e.HumanoidRace)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Spell>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Spell>()
                .HasMany(e => e.Humanoid)
                .WithMany(e => e.Spells)
                .Map(m => m.ToTable("Hum_Spell"));

            modelBuilder.Entity<Surface>()
                .HasOptional(e => e.SurfaceRule)
                .WithRequired(e => e.Surface);

            modelBuilder.Entity<Weapon>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Weapon>()
                .HasMany(e => e.Humanoid)
                .WithMany(e => e.Weapon)
                .Map(m => m.ToTable("Hum_Weapon"));
        }
    }
}
