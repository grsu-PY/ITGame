using System.Data.Entity;

namespace ITGame.DBConnector
{
    public partial class ITGameDBContext : DbContext
    {
        public ITGameDBContext()
            : base("name=ITGameDBContextConnectionString")
        {
           
        }

        public virtual DbSet<Models.Entities.Armor> Armor { get; set; }
        public virtual DbSet<Models.Entities.Character> Character { get; set; }
        public virtual DbSet<Models.Entities.Humanoid> Humanoid { get; set; }
        public virtual DbSet<Models.Entities.HumanoidRace> HumanoidRace { get; set; }
        public virtual DbSet<Models.Entities.Spell> Spell { get; set; }
        public virtual DbSet<Models.Entities.Surface> Surface { get; set; }
        public virtual DbSet<Models.Entities.SurfaceRule> SurfaceRule { get; set; }
        public virtual DbSet<Models.Entities.Weapon> Weapon { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Entities.Armor>()
                .Ignore(e => e.IsSelectedModelItem)
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Models.Entities.Character>()
                .Ignore(e => e.IsSelectedModelItem)
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Models.Entities.Character>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Models.Entities.Character>()
                .HasMany(e => e.Humanoids)
                .WithRequired(e => e.Character)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Models.Entities.Humanoid>()
                .Ignore(e => e.IsSelectedModelItem)
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Models.Entities.Humanoid>()
                .HasMany(e => e.Armors)
                .WithMany(e => e.Humanoids)
                .Map(m => m.ToTable("Hum_Armor"));

            modelBuilder.Entity<Models.Entities.HumanoidRace>()
                .Ignore(e => e.IsSelectedModelItem)
                .Ignore(x => x.Id)
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Models.Entities.Spell>()
                .Ignore(e => e.IsSelectedModelItem)
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Models.Entities.Spell>()
                .HasMany(e => e.Humanoids)
                .WithMany(e => e.Spells)
                .Map(m => m.ToTable("Hum_Spell"));

            modelBuilder.Entity<Models.Entities.Surface>()
                .HasOptional(e => e.SurfaceRule)
                .WithRequired(e => e.Surface);
            modelBuilder.Entity<Models.Entities.Surface>()
                .Ignore(e => e.IsSelectedModelItem)
                .Ignore(x => x.Id)
                .Ignore(x => x.Name);

            modelBuilder.Entity<Models.Entities.SurfaceRule>()
                .Ignore(e => e.IsSelectedModelItem)
                .Ignore(x => x.Id)
                .Ignore(x => x.Name);

            modelBuilder.Entity<Models.Entities.Weapon>()
                .Ignore(e => e.IsSelectedModelItem)
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Models.Entities.Weapon>()
                .HasMany(e => e.Humanoids)
                .WithMany(e => e.Weapons)
                .Map(m => m.ToTable("Hum_Weapon"));
        }
    }
}
