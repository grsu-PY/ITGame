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
                .HasMany(e => e.Humanoids)
                .WithRequired(e => e.Character)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Humanoid>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Humanoid>()
                .HasMany(e => e.Armors)
                .WithMany(e => e.Humanoids)
                .Map(m => m.ToTable("Hum_Armor"));

            modelBuilder.Entity<HumanoidRace>()
                .Property(e => e.Name)
                .IsUnicode(false);
            modelBuilder.Entity<HumanoidRace>().Ignore(x => x.Id);

            modelBuilder.Entity<Spell>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Spell>()
                .HasMany(e => e.Humanoids)
                .WithMany(e => e.Spells)
                .Map(m => m.ToTable("Hum_Spell"));

            modelBuilder.Entity<Surface>()
                .HasOptional(e => e.SurfaceRule)
                .WithRequired(e => e.Surface);
            modelBuilder.Entity<Surface>().Ignore(x => x.Id);
            modelBuilder.Entity<Surface>().Ignore(x => x.Name);

            modelBuilder.Entity<SurfaceRule>().Ignore(x => x.Id);
            modelBuilder.Entity<SurfaceRule>().Ignore(x => x.Name);

            modelBuilder.Entity<Weapon>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Weapon>()
                .HasMany(e => e.Humanoids)
                .WithMany(e => e.Weapons)
                .Map(m => m.ToTable("Hum_Weapon"));
        }
    }
}
