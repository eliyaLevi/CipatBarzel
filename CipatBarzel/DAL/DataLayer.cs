using CipatBarzel.Models;

using Microsoft.EntityFrameworkCore;



namespace CipatBarzel.DAL
{
    public class DataLayer : DbContext
    {
        public DataLayer(string cs) : base(GetOptions(cs))
        {
            Database.EnsureCreated();

            Seed();
        }

        private void Seed()
        {
            if (!DefenceAmmunitions.Any())
            {
                DefenceAmmunition defenceAmmunition = new DefenceAmmunition { Name = "כיפת ברזל", Amount = 4000 };
				DefenceAmmunitions.Add(defenceAmmunition);
				SaveChanges();
			}
            if (!TerrorOrgs.Any())
            {
                TerrorOrgs.AddRange(
                new TerrorOrg { Distance = 70, Name = "חמאס", Location = "עזה" },
                new TerrorOrg { Distance = 100, Name = "חיזבאללה", Location = "איראן" },
                new TerrorOrg { Distance = 2377, Name = "חותים", Location = "תימן" });

                SaveChanges();
            }
            if (!ThreatAmmunitions.Any())
            {
				ThreatAmmunitions.AddRange(
				new ThreatAmmunition { Name = "טיל", Speed = 880 },
				new ThreatAmmunition { Name = "כטבם", Speed = 300 },
				new ThreatAmmunition { Name = "טיל בליסטי", Speed = 1600 });
				SaveChanges();

            }
            if (!Threats.Any())
            {
                TerrorOrg? hamas = TerrorOrgs.FirstOrDefault(t => t.Name == "חמאס");
                ThreatAmmunition? rocket = ThreatAmmunitions.FirstOrDefault(t => t.Name == "טיל");
                if (hamas != null && rocket != null)
                {
                    Threats.AddRange(
                        new Threat
                        {
                            TerrorOrg = hamas,
                            Type = rocket,
                        }
                        );
                    SaveChanges();

                }
               
            }

			

        }



        public DbSet<DefenceAmmunition> DefenceAmmunitions { get; set; }
        public DbSet<TerrorOrg> TerrorOrgs { get; set; }
		public DbSet<Threat> Threats { get; set; }
		public DbSet<ThreatAmmunition> ThreatAmmunitions { get; set; }


		
		private static DbContextOptions GetOptions(string connectionString)
		{
			return SqlServerDbContextOptionsExtensions
				.UseSqlServer(new DbContextOptionsBuilder(), connectionString)
				.Options;
		}
	}
}
