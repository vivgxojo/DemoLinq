using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DemoLinq.Models
{
    public class Livraison
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int LieuId { get; set; } //Clé étrangère
        public Lieu Lieu { get; set; } // Navigation
        public List<Toilettes> Toilettes { get; set; } //Navigation

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
            
    }
}
