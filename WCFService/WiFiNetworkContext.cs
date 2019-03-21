using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Configuration;

namespace WCFService
{
    /*Пример https://docs.microsoft.com/ru-ru/ef/core/get-started/uwp/getting-started */
    public class WiFiNetworkContext : DbContext
    {
        public DbSet<WiFiNetwork> WiFiNetworks { get; set; }

        public WiFiNetworkContext(DbContextOptions<WiFiNetworkContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WiFiNetworkConfiguration());
        }
    }

    /* Настройки для WiFiNetwork (задают правила для отображения свойств в столбцы бд)
     * Fluent API https://www.learnentityframeworkcore.com/configuration/fluent-api */
    class WiFiNetworkConfiguration : IEntityTypeConfiguration<WiFiNetwork>
    {

        public void Configure(EntityTypeBuilder<WiFiNetwork> builder)
        {

            //BSSID
            builder.Property(t => t.BSSID).IsRequired();
            builder.HasKey(t => t.BSSID);
            builder.Property(t => t.BSSID).HasMaxLength(17);

            //SSID
            builder.Property(t => t.SSID).IsRequired();
            builder.Property(t => t.SSID).IsUnicode();
            builder.Property(t => t.SSID).HasMaxLength(32);

            //Encryption
            builder.Property(t => t.Encryption).IsRequired();
            builder.Property(t => t.Encryption).HasMaxLength(16);

            //Если для типа свойства невозможно значение null, то и в бд оно будет невозможно
        }
    }


    /* required when local database deleted
    создаёт базу данных по коду. Источники
    https://docs.microsoft.com/ru-ru/ef/core/miscellaneous/cli/dbcontext-creation
    https://docs.microsoft.com/en-us/ef/core/get-started/full-dotnet/new-db
    эта же фабрика создаёт миграции https://docs.microsoft.com/ru-ru/ef/core/managing-schemas/migrations/ 
    !!! для создания миграции нужно заполнить именя входа и пароль в AdminConnection !!! */
    public class ToDoContextFactory : IDesignTimeDbContextFactory<WiFiNetworkContext>
    {
        public WiFiNetworkContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<WiFiNetworkContext>();
            builder.UseSqlServer(ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString);
            return new WiFiNetworkContext(builder.Options);
        }
    }
}