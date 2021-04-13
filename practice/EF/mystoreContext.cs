using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using practice.Models;

namespace practice.EF
{
    public partial class mystoreContext : DbContext
    {
        public mystoreContext()
        {
        }

        public mystoreContext(DbContextOptions<mystoreContext> options)
            : base(options)
        {
            
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public virtual DbSet<AccessLevel> AccessLevel { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<CartItem> CartItem { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductLinkProductOrder> ProductLinkProductOrder { get; set; }
        public virtual DbSet<ProductOrder> ProductOrder { get; set; }
        public virtual DbSet<Subcategory> Subcategory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(Startup.Connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLevel>(entity =>
            {
                entity.ToTable("Access_Level");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Building)
                    .IsRequired()
                    .HasColumnName("building")
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Address__person___01142BA1");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("Cart_Item");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Number).HasColumnName("number");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.CartItem)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cart_item_ibfk_2");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartItem)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cart_item_ibfk_1");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Midname)
                    .IsRequired()
                    .HasColumnName("midname")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("surname")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Customer__person__2D27B809");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasColumnName("filename")
                    .HasMaxLength(50);

                entity.Property(e => e.ImageData)
                    .IsRequired()
                    .HasColumnName("imageData");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("Order_Status");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccessLevelId).HasColumnName("access_level_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.HasOne(d => d.AccessLevel)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.AccessLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Person__access_l__2A4B4B5E");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__categor__6E01572D");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK__Product__image_i__5AEE82B9");
            });

            modelBuilder.Entity<ProductLinkProductOrder>(entity =>
            {
                entity.ToTable("Product_Link_Product_Order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Number).HasColumnName("number");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductLinkProductOrder)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product_L__order__3B75D760");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductLinkProductOrder)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product_L__produ__3A81B327");
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.ToTable("Product_Order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("date");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product_O__addre__36B12243");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product_O__custo__35BCFE0A");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product_O__statu__37A5467C");
            });

            modelBuilder.Entity<Subcategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Subcategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subcatego__categ__49C3F6B7");
            });

            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<AccessLevel>().HasData(
                new { Id = 1, Name = "admin" },
                new { Id = 2, Name = "user" }
            );

            modelBuilder.Entity<Person>().HasData(
                new { Id = 1, Email = "admin@admin.ru", Password = "admin", AccessLevelId = 1 }
            );

            modelBuilder.Entity<Category>().HasData(
                new { Id = 6, Name = "Компьютеры" },
                new { Id = 7, Name = "ТВ и развлечения" },
                new { Id = 8, Name = "Смартфоны" },
                new { Id = 9, Name = "Аксессуары" },
                new { Id = 10, Name = "Фото и видео" },
                new { Id = 11, Name = "Прочие устройства" }
            );

            modelBuilder.Entity<Subcategory>().HasData(
                new { Id = 1, Name = "Комплектующие ПК", CategoryId = 6 },
                new { Id = 2, Name = "Ноутбуки", CategoryId = 6 },
                new { Id = 3, Name = "Телевизоры", CategoryId = 7 },
                new { Id = 4, Name = "Игры и хобби", CategoryId = 7 },
                new { Id = 5, Name = "Фитнес-браслеты", CategoryId = 11 },
                new { Id = 6, Name = "Планшеты", CategoryId = 11 },
                new { Id = 7, Name = "Смартфоны", CategoryId = 8 },
                new { Id = 8, Name = "Экшн-камеры", CategoryId = 10 },
                new { Id = 9, Name = "Наушники и гарнитуры", CategoryId = 9 }
            );

            modelBuilder.Entity<OrderStatus>().HasData(
                new { Id = 1, Name = "в обработке" },
                new { Id = 2, Name = "выполнен" },
                new { Id = 3, Name = "отклонен" },
                new { Id = 4, Name = "в пункте самовывоза" }
            );
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
