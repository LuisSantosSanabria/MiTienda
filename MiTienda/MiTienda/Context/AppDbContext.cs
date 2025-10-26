using Microsoft.EntityFrameworkCore;
using MiTienda.Entidades;


namespace MiTienda.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
            // confpor defecto para trabajar con entity framework
        {
            
        }

        //conf nuestras tablas
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //categoria
            modelBuilder.Entity<Categoria>(e =>
            {
                e.HasKey("CategoriaId");
                e.Property("CategoriaId").ValueGeneratedOnAdd();
                e.HasData(
                    new Categoria { CategoriaId = 1, Nombre = "Tecnologia" },
                    new Categoria { CategoriaId = 2, Nombre = "Accesorios" }
                    );
            });

            //Pedido
            modelBuilder.Entity<Pedido>(e =>
            {
                e.HasKey("PedidoId");
                e.Property("PedidoId").ValueGeneratedOnAdd();
                e.Property("Total").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Usuario).WithMany(p => p.Pedidos).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
                // usuario puede tener varios pero un pedido no varios usaurios

            });

            //prodcuto
            modelBuilder.Entity<Producto>(e =>
            {
                e.HasKey("ProdcutoId");
                e.Property("ProdcutoId").ValueGeneratedOnAdd();
                e.Property("Precio").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Categoria).WithMany(p => p.Productos).HasForeignKey(e => e.CategoriaId).OnDelete(DeleteBehavior.Restrict);
                // este prodcuto va contar con una categoria y esa cat tiene varios prod
                
            });

            //Usuario
            modelBuilder.Entity<Usuario>(e =>
            {
                e.HasKey("UserId");
                e.Property("UserId").ValueGeneratedOnAdd();

            });

            //artiulo
            modelBuilder.Entity<Articulo>(e =>
            {
                e.HasKey("ArticuloId");
                e.Property("ArticuloId").ValueGeneratedOnAdd();
                e.Property("Precio").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Pedido).WithMany(p => p.Articulos).HasForeignKey(e => e.PedidoId).OnDelete(DeleteBehavior.Restrict);
                // un articulo puede tener un pedido, pero un pedido puede tener varios articulos
                e.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId).OnDelete(DeleteBehavior.Restrict);
            });

        }

    }
}
