namespace ComeFrexco.Models
{
    public class Monitoreo
    {
        public int moni_id { get; set; }
        public int plag_id { get; set; }
        public int per_id { get; set; }
        public int fin_id { get; set; }
        public int lote_num { get; set; }
        public string moni_fecha { get; set; }
    }
    public class MonitoreoDetalle
    {
        public int monide_id { get; set; }
        public int monide_item { get; set; }
        public string monide_narbol { get; set; }
        public string monide_dramas { get; set; }
        public string monide_dinflores { get; set; }
        public int monide_ninflores { get; set; }
        public string monide_dfrutos { get; set; }
        public int monide_nfrutos { get; set; }
        public string monide_tallo { get; set; }
        public string monide_parasita { get; set; }
        public string monide_ovario { get; set; }
        public string monide_acaros { get; set; }
        public string monide_phytophatora { get; set; }
        public string monide_lotenum { get; set; }
    }
    public class CargarMonitoreo
    {
        public int id { get; set; }
        public object plaga { get; set; }
        public CargarFinca finca { get; set; }
        public string moni_fecha { get; set; }
        public string usuario { get; set; }
        public string fecha_creacion { get; set; }
    }
}
