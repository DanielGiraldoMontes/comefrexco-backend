using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ComeFrexco.Models;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Http;

namespace ComeFrexco.Repositors
{
    public class PlagaRepositor
    {
        private SqlConnection conectar { get; set; }
        public SqlDataReader reader { get; set; }

        /// <summary>
        /// Inyexión que permite pasar la peticion del cliente como argumento con los datos basicos para poder establecer la conexión al motor
        /// </summary>
        /// <param name="config">Interfaz de la clase config donde se encuentra la estructura de conexión y validación del token</param>
        /// <param name="recuest">Petición enviada desde el cliente con los datos necesarios</param>
        public PlagaRepositor(IConfig config, HttpRequest recuest)
        {
            conectar = config.ConectarSqlServer(recuest);
        }

        public bool Exist(int id)
        {
            bool exist = false;
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT 'id' = PLAG_ID, 'nombre' = PLAG_NOM, 'descripcion' = PLAG_DESC FROM COM.PLAGA WHERE PLAG_ID = " + id, conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            if (da.Fill(ds, "plaga") > 0)
                exist = true;

            conectar.Close();
            return exist;
        }
        public List<Plaga> Cargar()
        {
            List<Plaga> plagas = new List<Plaga>();
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT 'id' = PLAG_ID, 'nombre' = PLAG_NOM, 'descripcion' = PLAG_DESC FROM COM.PLAGA ORDER BY 1 DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "plagas");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                Plaga plaga = new Plaga
                {
                    id = Convert.ToInt32(dr["id"]),
                    nombre = dr["nombre"].ToString(),
                    descripcion = dr["descripcion"].ToString()
                };
                plagas.Add(plaga);
            }
            conectar.Close();
            return plagas;
        }
        public Plaga CargarPorId(int Id)
        {
            Plaga plaga = null;
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT 'id' = PLAG_ID, 'nombre' = PLAG_NOM, 'descripcion' = PLAG_DESC FROM COM.PLAGA WHERE PLAG_ID = " + Id, conectar);
            SqlDataReader reader = cm.ExecuteReader();
            while (reader.Read())
            {
                plaga = new Plaga
                {
                    id = reader.GetInt32(0),
                    nombre = reader.GetString(1),
                    descripcion = reader.GetString(1)
                };
            }
            reader.Close();
            conectar.Close();
            return plaga;
        }
        public Plaga Agregar(Plaga plaga)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("INSERT INTO COM.PLAGA (PLAG_DESC, PLAG_NOM) VALUES ('" + plaga.descripcion + "', '" + plaga.nombre + "')", conectar);
            cm.ExecuteNonQuery();

            cm = new SqlCommand("SELECT TOP 1 'id' = PLAG_ID FROM COM.PLAGA ORDER BY PLAG_ID DESC", conectar);
            SqlDataReader reader = cm.ExecuteReader();
            while (reader.Read())
            {
                plaga.id = reader.GetInt32(0);
            }
            reader.Close();
            conectar.Close();
            return plaga;
        }
        public Plaga Actualizar(Plaga plaga)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("UPDATE COM.PLAGA SET PLAG_NOM = @NOMBRE, PLAG_DESC = @DESCRIPCION WHERE PLAG_ID = @ID", conectar);
            cm.Parameters.AddWithValue("NOMBRE", plaga.nombre);
            cm.Parameters.AddWithValue("DESCRIPCION", plaga.descripcion);
            cm.Parameters.AddWithValue("ID", plaga.id);
            cm.ExecuteNonQuery();
            conectar.Close();
            return plaga;
        }
        public void Borrar(int Id)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("DELETE FROM COM.PLAGA WHERE PLAG_ID = " + Id, conectar);
            cm.ExecuteNonQuery();
            conectar.Close();
        }
    }
}
