using System;
using Microsoft.EntityFrameworkCore;

namespace Teste_Wipro.Models
{
    public class G360Context : DbContext
    {
        public G360Context(DbContextOptions<G360Context> op)
            : base(op)
        {
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Filme> Filme { get; set; }
        public DbSet<Locacao> Locacao { get; set; }
    }
}