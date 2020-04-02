using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ComeFrexco.Models;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Http;

namespace ComeFrexco.Repositors
{
    public class MonitoreoRepositor
    {
        private SqlConnection conectar { get; set; }
        public SqlDataReader reader { get; set; }

        /// <summary>
        /// Inyexión que permite pasar la peticion del cliente como argumento con los datos basicos para poder establecer la conexión al motor
        /// </summary>
        /// <param name="config">Interfaz de la clase config donde se encuentra la estructura de conexión y validación del token</param>
        /// <param name="recuest">Petición enviada desde el cliente con los datos necesarios</param>
        public MonitoreoRepositor(IConfig config, HttpRequest recuest)
        {
            conectar = config.ConectarSqlServer(recuest);
        }
        public List<CargarMonitoreo> Cargar()
        {
            List<CargarMonitoreo> monitoreos = new List<CargarMonitoreo>();
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT " +
                                           " 'id' = MONI_ID, " +
                                           " 'plaga_id' = PL.PLAG_ID, " +
                                           " 'plaga_nombre' = PL.PLAG_NOM, " +
                                           " 'plaga_descripcion' = PL.PLAG_DESC, " +
                                           " 'per_id' = PE.PER_ID, " +
                                           " 'per_nombres' = CONCAT(PE.PER_APELLIDO1, ' ', PE.PER_APELLIDO2, ' ', PE.PER_NOMBRE1, ' ', PE.PER_NOMBRE2), " +
                                           " 'finca_id' = F.FIN_ID, " +
                                           " 'finca_nombre' = F.FIN_DESC, " +
                                           " 'vereda_id' = V.VER_ID, " +
                                           " 'vereda_nombre' = V.VER_DESC, " +
                                           " 'fecha' = CONVERT(VARCHAR(20), M.MONI_FECHA, 120), " +
                                           " 'usuario' = M.MONI_USUMODIFI, " +
                                           " 'fecha_creacion' = CONVERT(VARCHAR(20), M.MONI_FECHAMODIFI, 120) " +
                                           " FROM COM.MONITOREO M " +
                                           " INNER JOIN COM.PLAGA PL ON PL.PLAG_ID = M.PLAG_ID " +
                                           " INNER JOIN COM.FINCA F ON F.FIN_ID = M.FIN_ID " +
                                           " INNER JOIN COM.PERSONA PE ON PE.PER_ID = F.PER_ID " +
                                           " INNER JOIN COM.VEREDA V ON V.VER_ID = F.VER_ID " +
                                           " INNER JOIN COM.MUNICIPIO MU ON MU.MUN_ID = V.MUN_ID " +
                                           " ORDER BY 1 DESC ", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "monitoreos");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                monitoreos.Add(ReturnMonitoreo(dr));
            }
            conectar.Close();
            return monitoreos;
        }
        public CargarMonitoreo CargarPorId(int id)
        {
            CargarMonitoreo monitoreo = null;
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT" +
                                           " 'id' = MONI_ID, " +
                                           " 'plaga_id' = PL.PLAG_ID, " +
                                           " 'plaga_nombre' = PL.PLAG_NOM, " +
                                           " 'plaga_descripcion' = PL.PLAG_DESC, " +
                                           " 'per_id' = PE.PER_ID, " +
                                           " 'per_nombres' = CONCAT(PE.PER_APELLIDO1, ' ', PE.PER_APELLIDO2, ' ', PE.PER_NOMBRE1, ' ', PE.PER_NOMBRE2), " +
                                           " 'finca_id' = F.FIN_ID, " +
                                           " 'finca_nombre' = F.FIN_DESC, " +
                                           " 'vereda_id' = V.VER_ID, " +
                                           " 'vereda_nombre' = V.VER_DESC, " +
                                           " 'fecha' = CONVERT(VARCHAR(20), M.MONI_FECHA, 120), " +
                                           " 'usuario' = M.MONI_USUMODIFI, " +
                                           " 'fecha_creacion' = CONVERT(VARCHAR(20), M.MONI_FECHAMODIFI, 120) " +
                                           " FROM COM.MONITOREO M " +
                                           " INNER JOIN COM.PLAGA PL ON PL.PLAG_ID = M.PLAG_ID " +
                                           " INNER JOIN COM.FINCA F ON F.FIN_ID = M.FIN_ID " +
                                           " INNER JOIN COM.PERSONA PE ON PE.PER_ID = F.PER_ID " +
                                           " INNER JOIN COM.VEREDA V ON V.VER_ID = F.VER_ID " +
                                           " INNER JOIN COM.MUNICIPIO MU ON MU.MUN_ID = V.MUN_ID " +
                                           " WHERE MONI_ID = " + id +
                                           " ORDER BY 1 DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "monitoreo");
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                monitoreo = ReturnMonitoreo(dr);
            }
            conectar.Close();
            return monitoreo;
        }
        public CargarMonitoreo Agregar(Monitoreo monitoreo)
        {
            CargarMonitoreo cargarMonitoreo = null;
            conectar.Open();
            SqlCommand cm = new SqlCommand("INSERT INTO COM.MONITOREO (PLAG_ID, PER_ID, FIN_ID, MONI_FECHA, MONI_USUMODIFI, MONI_FECHAMODIFI) VALUES (@PLAG_ID, @PER_ID, @FIN_ID, @MONI_FECHA, SUSER_SNAME(), GETDATE())", conectar);
            cm.Parameters.AddWithValue("PLAG_ID", monitoreo.plag_id);
            cm.Parameters.AddWithValue("PER_ID", monitoreo.per_id);
            cm.Parameters.AddWithValue("FIN_ID", monitoreo.fin_id);
            cm.Parameters.AddWithValue("MONI_FECHA", monitoreo.moni_fecha);
            cm.ExecuteNonQuery();

            cm = new SqlCommand("SELECT TOP 1" +
                                " 'id' = MONI_ID, " +
                                " 'plaga_id' = PL.PLAG_ID, " +
                                " 'plaga_nombre' = PL.PLAG_NOM, " +
                                " 'plaga_descripcion' = PL.PLAG_DESC, " +
                                " 'per_id' = PE.PER_ID, " +
                                " 'per_nombres' = CONCAT(PE.PER_APELLIDO1, ' ', PE.PER_APELLIDO2, ' ', PE.PER_NOMBRE1, ' ', PE.PER_NOMBRE2), " +
                                " 'finca_id' = F.FIN_ID, " +
                                " 'finca_nombre' = F.FIN_DESC, " +
                                " 'vereda_id' = V.VER_ID, " +
                                " 'vereda_nombre' = V.VER_DESC, " +
                                " 'fecha' = CONVERT(VARCHAR(20), M.MONI_FECHA, 120), " +
                                " 'usuario' = M.MONI_USUMODIFI, " +
                                " 'fecha_creacion' = CONVERT(VARCHAR(20), M.MONI_FECHAMODIFI, 120) " +
                                " FROM COM.MONITOREO M " +
                                " INNER JOIN COM.PLAGA PL ON PL.PLAG_ID = M.PLAG_ID " +
                                " INNER JOIN COM.FINCA F ON F.FIN_ID = M.FIN_ID " +
                                " INNER JOIN COM.PERSONA PE ON PE.PER_ID = F.PER_ID " +
                                " INNER JOIN COM.VEREDA V ON V.VER_ID = F.VER_ID " +
                                " INNER JOIN COM.MUNICIPIO MU ON MU.MUN_ID = V.MUN_ID " +
                               " ORDER BY 1 DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "monitoreo");
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                cargarMonitoreo = ReturnMonitoreo(dr);
            }
            conectar.Close();
            return cargarMonitoreo;
        }
        public List<MonitoreoDetalle> CargarDetalle(int moniId)
        {
            List<MonitoreoDetalle> detalle = new List<MonitoreoDetalle>();
            MonitoreoDetalle monitoreo;
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT " +
                                           " 'monide_id' = MONIDE_ID," +
                                           " 'monide_item' = MONDE_ITEM," +
                                           " 'monide_lotenum' = MONIDE_LOTENUM," +
                                           " 'monide_narbol' = MONIDE_NARBOL," +
                                           " 'monide_dramas' = MONIDE_DRAMAS," +
                                           " 'monide_dinflores' = MONIDE_DINFLORES," +
                                           " 'monide_ninflores' = MONIDE_NINFLORES," +
                                           " 'monide_dfrutos' = MONIDE_DFRUTOS," +
                                           " 'monide_nfrutos' = MONIDE_NFRUTOS," +
                                           " 'monide_tallo' = MONIDE_TALLO," +
                                           " 'monide_parasita' = MONIDE_PARASITA," +
                                           " 'monide_ovario' = MONIDE_OVARIO," +
                                           " 'monide_acaros' = MONIDE_ACAROS," +
                                           " 'monide_phytophatora' = MONIDE_PHYTOPHTORA" +
                                           " FROM COM.MONITOREO_DETALLE " +
                                           " WHERE MONIDE_ID = " + moniId, conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "monitoreoDetalle");
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                monitoreo = new MonitoreoDetalle
                {
                    monide_id = Convert.ToInt32(dr["monide_id"].ToString()),
                    monide_item = Convert.ToInt32(dr["monide_item"].ToString()),
                    monide_lotenum = dr["monide_lotenum"].ToString(),
                    monide_narbol = dr["monide_narbol"].ToString(),
                    monide_dramas = dr["monide_dramas"].ToString(),
                    monide_dinflores = dr["monide_dinflores"].ToString(),
                    monide_ninflores = Convert.ToInt32(dr["monide_ninflores"].ToString()),
                    monide_dfrutos = dr["monide_dfrutos"].ToString(),
                    monide_nfrutos = Convert.ToInt32(dr["monide_nfrutos"].ToString()),
                    monide_tallo = dr["monide_tallo"].ToString(),
                    monide_parasita = dr["monide_parasita"].ToString(),
                    monide_ovario = dr["monide_ovario"].ToString(),
                    monide_acaros = dr["monide_acaros"].ToString(),
                    monide_phytophatora = dr["monide_phytophatora"].ToString()
                };
                detalle.Add(monitoreo);
            }
            return detalle;
        }
        public void AgregarDetalle(int moniId, List<MonitoreoDetalle> monitoreoDetalle)
        {
            conectar.Open();
            foreach (MonitoreoDetalle moniDetalle in monitoreoDetalle)
            {
                SqlCommand cm = new SqlCommand("INSERT INTO COM.MONITOREO_DETALLE (MONIDE_ID, MONDE_ITEM, MONIDE_NARBOL, MONIDE_DRAMAS, MONIDE_DINFLORES, MONIDE_NINFLORES, MONIDE_DFRUTOS, MONIDE_NFRUTOS, MONIDE_TALLO, MONIDE_PARASITA, MONIDE_OVARIO, MONIDE_ACAROS, MONIDE_PHYTOPHTORA, MONIDE_LOTENUM) " +
                    "VALUES (@MONIDE_ID, @MONIDE_ITEM, @MONIDE_NARBOL, @MONIDE_DRAMAS, @MONIDE_DINFLORES, @MONIDE_NINFLORES, @MONIDE_DFRUTOS, @MONIDE_NINFRUTOS, @MONIDE_TALLO, @MONIDE_PARASITA, @MONIDE_OVARIO, @MONIDE_ACAROS, @MONIDE_PHYTOPHATORA, @MONIDE_LOTENUM)", conectar);
                cm.Parameters.AddWithValue("@MONIDE_ID", moniId);
                cm.Parameters.AddWithValue("@MONIDE_ITEM", moniDetalle.monide_item);
                cm.Parameters.AddWithValue("@MONIDE_NARBOL", moniDetalle.monide_narbol);
                cm.Parameters.AddWithValue("@MONIDE_DRAMAS", moniDetalle.monide_dramas);
                cm.Parameters.AddWithValue("@MONIDE_DINFLORES", moniDetalle.monide_dinflores);
                cm.Parameters.AddWithValue("@MONIDE_NINFLORES", moniDetalle.monide_ninflores);
                cm.Parameters.AddWithValue("@MONIDE_DFRUTOS", moniDetalle.monide_dfrutos);
                cm.Parameters.AddWithValue("@MONIDE_NINFRUTOS", moniDetalle.monide_ninflores);
                cm.Parameters.AddWithValue("@MONIDE_TALLO", moniDetalle.monide_tallo);
                cm.Parameters.AddWithValue("@MONIDE_PARASITA", moniDetalle.monide_parasita);
                cm.Parameters.AddWithValue("@MONIDE_OVARIO", moniDetalle.monide_ovario);
                cm.Parameters.AddWithValue("@MONIDE_ACAROS", moniDetalle.monide_acaros);
                cm.Parameters.AddWithValue("@MONIDE_PHYTOPHATORA", moniDetalle.monide_phytophatora);
                cm.Parameters.AddWithValue("@MONIDE_LOTENUM", moniDetalle.monide_lotenum);
                cm.ExecuteNonQuery();
            }
            conectar.Close();
        }
        public void Borrar(int id)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("DELETE FROM COM.MONITOREO WHERE MONI_ID = " + id, conectar);
            cm.ExecuteNonQuery();
            conectar.Close();
        }
        private CargarMonitoreo ReturnMonitoreo(DataRow dr)
        {
            CargarMonitoreo monitoreo;
            Plaga plaga = new Plaga
            {
                id = Convert.ToInt32(dr["plaga_id"]),
                nombre = dr["plaga_nombre"].ToString(),
                descripcion = dr["plaga_descripcion"].ToString()

            };
            CargarFinca finca = new CargarFinca
            {
                id = Convert.ToInt32(dr["finca_id"]),
                descripcion = dr["finca_nombre"].ToString(),
                persona = new Persona
                {
                    id = Convert.ToInt32(dr["per_id"]),
                    nombres = dr["per_nombres"].ToString()
                },
                vereda = new Vereda
                {
                    id = Convert.ToInt32(dr["vereda_id"]),
                    descripcion = dr["vereda_nombre"].ToString()
                }
            };
            monitoreo = new CargarMonitoreo
            {
                id = Convert.ToInt32(dr["id"]),
                plaga = plaga,
                finca = finca,
                moni_fecha = dr["fecha"].ToString(),
                usuario = dr["vereda_id"].ToString(),
                fecha_creacion = dr["fecha_creacion"].ToString()
            };
            return monitoreo;
        }
    }
}
