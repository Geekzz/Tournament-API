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

            for (int i = 0; i < count; i++)
            {
                var tournament = new TournamentDetails
                {
                    Title = Truncate(faker.Company.CompanyName(), 30), // Truncate to 30 characters
                    StartDate = faker.Date.Future(),
                };

                tournament.Games = GenerateGames(faker.Random.Int(3, 10), tournament); // Generate between 3 and 10 games
                tournaments.Add(tournament);
            }

            return tournaments;
        }

        private static ICollection<Game> GenerateGames(int count, TournamentDetails tournamentDetails)
        {
            var faker = new Faker("sv");

            return Enumerable.Range(1, count).Select(_ => new Game
            {
                Title = Truncate(faker.Company.CompanyName(), 30), // Truncate to 30 characters
                StartTime = faker.Date.Future(),
                TournamentDetailsId = tournamentDetails.Id
            }).ToList();
        }

        private static string Truncate(string value, int maxLength)
        {
            return string.IsNullOrEmpty(value) ? value : value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

    }
}
