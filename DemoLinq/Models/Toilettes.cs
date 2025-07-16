namespace DemoLinq.Models
{
    public class Toilettes
    {
        public int Id { get; set; }
        public double Capacite { get; set; }

        public int LivraisonId { get; set; } // clé étrangère

        public Livraison Livraison { get; set; } //Navigation

    }
}
