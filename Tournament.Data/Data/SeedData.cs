using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Data.Data;

namespace Tournament.Data.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(TournamentApiContext dbContext)
        {
            if (await dbContext.Tournament.AnyAsync()) return;

            var tournaments = GenerateTournaments(4);
            dbContext.AddRange(tournaments);
            await dbContext.SaveChangesAsync();
        }

        private static ICollection<TournamentDetails> GenerateTournaments(int count)
        {
            var faker = new Faker("sv");
            var tournaments = new List<TournamentDetails>();

            for(int i = 0; i < count; i++)
            {
                var tournament = new TournamentDetails
                {
                    Title = faker.Company.CompanyName(),
                    StartDate = faker.Date.Future(),
                };

                tournament.Games = GenerateGames(faker.Random.Int(3, 10)); // Skapa mellan 3 och 10 matcher
                tournaments.Add(tournament);
            }

            return tournaments;
        }

        private static ICollection<Game> GenerateGames(int count)
        {
            var faker = new Faker("sv");

            return Enumerable.Range(1, count).Select(_ => new Game
            {
                Title = faker.Company.CompanyName(),
                Time = faker.Date.Future(),
            }).ToList();
        }
    }
}
