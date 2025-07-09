using Microsoft.AspNetCore.Identity;

namespace DemoLinq.Models
{
    public static class InitBD
    {
        private static readonly string[] Types = { "Parc", "Plage", "Montagne", "Ville", "Forêt", "Lac", "Musée", "Monument" };
        private static readonly string[] Noms = {
            "Verdure", "Azur", "Émeraude", "Soleil", "Rocheuse", "Brume", "Étoile", "Cristal",
            "Val d'Or", "Mystère", "Lumière", "Évasion", "Éden", "Cascade", "Oasis"
        };

        public static List<Lieu> GenererLieux(int nombre = 50)
        {
            var random = new Random();
            var lieux = new List<Lieu>();

            for (int i = 1; i <= nombre; i++)
            {
                var nom = $"{Noms[random.Next(Noms.Length)]} {i}";
                var type = Types[random.Next(Types.Length)];

                lieux.Add(new Lieu
                {
                    //Id = i,
                    Nom = nom,
                    Description = $"Un lieu nommé {nom}, de type {type}.",
                    Type = type,
                    Latitude = random.Next(-90000000, 90000000), // équivalent à -90.000000 à 90.000000
                    Longitude = random.Next(-180000000, 180000000), // équivalent à -180.000000 à 180.000000
                    Superficie = random.Next(100, 100000) // en m² (de 100 à 100 000 m²)
                });
            }

            return lieux;
        }


         public static async Task Initialiser(IApplicationBuilder applicationBuilder)
        {
            //Récupérer le contexte de la base de données à partir du service
            LieuDBContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<LieuDBContext>();

            if (!context.Lieux.Any())
            {
                List<Lieu> ll = GenererLieux();
                context.Lieux.AddRange(ll);
                context.SaveChanges();
            }

            RoleManager<IdentityRole> _roleManager = 
                applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            UserManager<IdentityUser> _userManager = 
                applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityRole role = await _roleManager.FindByNameAsync("admin");
            if (role == null)
            {
                //Seed un role admin s'il n'existe pas déjà
                IdentityResult resultRole = await _roleManager.CreateAsync(new IdentityRole("admin"));
            }

            IdentityUser adminUser = await _userManager.FindByNameAsync("admin2@cegep.ca");
            if (adminUser == null)
            {
                //Seed un utilisateur admin s'il n'esxiste pas déjà
                IdentityResult resultUser = await _userManager.CreateAsync(
                    new IdentityUser("admin2@cegep.ca"));
            }

            //Récupérer les objets user et admin de la BD
            IdentityUser userA = await _userManager.FindByNameAsync("admin2@cegep.ca");
            IdentityRole roleA = await _roleManager.FindByNameAsync("admin");

            //Configurer mon user
            userA.Email = "admin2@cegep.ca";
            userA.PasswordHash = "AQAAAAIAAYagAAAAEJ/Jl8lSDGFHgp1OVzJhis0aN9qCBhqelyGT2cFVOP1AXz4/FcUKL4IvFalxbvMjtw==";
            userA.EmailConfirmed = true;

            //Associer le rôle admin à notre user
            IdentityResult resultA = await _userManager.AddToRoleAsync(userA, roleA.Name);

        }
    }
}
