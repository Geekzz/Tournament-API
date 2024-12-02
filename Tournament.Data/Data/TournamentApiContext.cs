using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class TournamentApiContext : DbContext
    {
        public TournamentApiContext (DbContextOptions<TournamentApiContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasOne<TournamentDetails>() // Specify the related entity without using a navigation property
                .WithMany(t => t.Games) // Use the collection in TournamentDetails
                .HasForeignKey(g => g.TournamentDetailsId) // Specify the foreign key
                .OnDelete(DeleteBehavior.Cascade); // Optional: Define delete behavior

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<TournamentDetails> Tournament { get; set; } = default!;
        public DbSet<Game> Games { get; set; } = default!;

    }
}
