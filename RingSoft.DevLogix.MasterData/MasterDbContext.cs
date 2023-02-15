using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DevLogix.MasterData
{
    public class MasterDbContext : DbContext
    {
        public virtual DbSet<Organization> Organizations { get; set; }
        public static string ProgramDataFolder
        {
            get
            {
#if DEBUG
                return RingSoftAppGlobals.AssemblyDirectory;
#else
                return
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\HomeLogix\\";
#endif
            }
        }



        public static string MasterFilePath => $"{ProgramDataFolder}{MasterFileName}";

        public const string MasterFileName = "MasterDb.sqlite";
        public const string DemoDataFileName = "DemoData.sqlite";

        public static void ConnectToMaster()
        {
            if (!Directory.Exists(ProgramDataFolder))
                Directory.CreateDirectory(ProgramDataFolder);

            var firstTime = !File.Exists(MasterFilePath);

            var context = new MasterDbContext();
            context.Database.Migrate();

            if (firstTime)
            {
                var filePath = ProgramDataFolder;
#if DEBUG
                var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
                if (directoryInfo != null)
                    if (directoryInfo.Parent != null)
                        filePath = directoryInfo.Parent.FullName;
#endif
                SaveOrganization(new Organization
                {
                    Name = "Demonstration Account Organization",
                    FilePath = filePath,
                    FileName = DemoDataFileName
                });
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={MasterFilePath}");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbConstants.ConstantGenerator = new SqliteDbConstants();
            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(hk => hk.Id);

                entity.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);

                entity.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.FilePath).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.FileName).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.IsDefault).HasColumnType(DbConstants.BoolColumnType);

                entity.Property(p => p.Platform).HasColumnType(DbConstants.ByteColumnType);

                entity.Property(p => p.Server).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.Database).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.AuthenticationType).HasColumnType(DbConstants.ByteColumnType);

                entity.Property(p => p.Username).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.Password).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.DefaultUser).HasColumnType(DbConstants.IntegerColumnType);

                entity.Property(p => p.MigrateDb).HasColumnType(DbConstants.BoolColumnType);

            });

            base.OnModelCreating(modelBuilder);
        }

        public static IEnumerable<Organization> GetOrganizations()
        {
            var context = new MasterDbContext();
            return context.Organizations.OrderBy(p => p.Name);
        }

        public static Organization GetDefaultOrganization()
        {
            var context = new MasterDbContext();
            return context.Organizations.FirstOrDefault(f => f.IsDefault);
        }

        public static bool SaveOrganization(Organization Organization)
        {
            var context = new MasterDbContext();
            return context.SaveEntity(context.Organizations, Organization, "Saving Organization");
        }

        public static bool DeleteOrganization(int OrganizationId)
        {
            var context = new MasterDbContext();
            var Organization = context.Organizations.FirstOrDefault(f => f.Id == OrganizationId);
            return context.DeleteEntity(context.Organizations, Organization, "Deleting Organization");
        }

    }
}
