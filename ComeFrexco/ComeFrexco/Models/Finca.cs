namespace ComeFrexco.Models
{
    public class Finca
    {
        public int fin_id { get; set; }
        public int per_id { get; set; }
        public int ver_id { get; set; }
        public string fin_desc { get; set; }
    }
    public class Persona
    {
        public int id { get; set; }
        public string nombres { get; set; }
    }
    public class Vereda
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }
    public class CargarFinca
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public Persona persona { get; set; }
        public Vereda vereda { get; set; }
    }
}
