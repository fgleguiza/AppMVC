namespace app.Models
{

    public class Ingrediente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relación muchos a muchos con Hamburguesa
        public ICollection<HamburguesaIngrediente> HamburguesaIngredientes { get; set; }
    }
}
