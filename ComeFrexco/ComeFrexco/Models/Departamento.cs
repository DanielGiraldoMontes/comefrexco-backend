namespace ComeFrexco.Models
{
	public class Departamento
	{
		public int dep_id { get; set; }
		public string dep_desc { get; set; }
	}
	public class Municipio
	{
		public int muni_id { get; set; }
		public string muni_desc { get; set; }
		public int munidep_id { get; set; }
	}
}
