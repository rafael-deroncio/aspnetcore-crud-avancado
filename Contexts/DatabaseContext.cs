using CRUDAvancado.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CrudAvancado.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<ItemPedidoModel> ItensPedido { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoriaModel>()
                .Property(categoria => categoria.DataCadastro).HasDefaultValueSql("datetime('now', 'locatime', 'start of day')")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<ProdutoModel>()
                .Property(produto => produto.DataCadastro).HasDefaultValueSql("datetime('now', 'locatime', 'start of day')")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<UsuarioModel>()
                .Property(usuario => usuario.DataCadastro).HasDefaultValueSql("datetime('now', 'locatime', 'start of day')")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<ItemPedidoModel>()
                .HasKey(itemPedido => new { itemPedido.IdPedido, itemPedido.IdProduto });

            modelBuilder.Entity<PedidoModel>()
                .OwnsOne(pedido => pedido.EnderecoEntrega,
                         endereco => 
                         {
                            endereco.Ignore(endereco=> endereco.IdEndereco);
                            endereco.Ignore(endereco=> endereco.Selecionado);
                            endereco.ToTable("Controle_Pedidos");
                         });

            modelBuilder.Entity<ClienteModel>()
                .OwnsMany(cliente => cliente.Enderecos,
                      endereco =>
                      {
                          endereco.WithOwner().HasForeignKey("IdUsuario");
                          endereco.HasKey("IdUsuario", "IdEndereco");
                      });

        }
    }
}