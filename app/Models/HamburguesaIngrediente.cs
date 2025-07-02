namespace app.Models
{
    // Tabla intermedia para relación muchos a muchos
    public class HamburguesaIngrediente
    {
        public int HamburguesaId { get; set; }
        public Hamburguesa Hamburguesa { get; set; }

        public int IngredienteId { get; set; }
        public Ingrediente Ingrediente { get; set; }
    }
}
