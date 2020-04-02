using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ComeFrexco.Models;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Http;

namespace ComeFrexco.Repositors
{
    public class PersonasRepositor
    {
        SqlConnection conectar;

        /// <summary>
        /// Inyexión que permite pasar la peticion del cliente como argumento con los datos basicos para poder establecer la conexión al motor
        /// </summary>
        /// <param name="config">Interfaz de la clase config donde se encuentra la estructura de conexión y validación del token</param>
        /// <param name="recuest">Petición enviada desde el cliente con los datos necesarios</param>
        public PersonasRepositor(IConfig config, HttpRequest recuest)
        {
            conectar = config.ConectarSqlServer(recuest);
        }

        public DataSet Cargar()
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT " +
                                           " id = PER_ID, " +
                                           " nit = PER_NIT, " +
                                           " nombre1 = PER_NOMBRE1, " +
                                           " nombre2 = PER_NOMBRE2, " +
                                           " apellido1 = PER_APELLIDO1, " +
                                           " apellido2 = PER_APELLIDO2, " +
                                           " telefono = PER_TELEFONO " +
                                           " FROM COM.PERSONA ", conectar);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataSet ds = new DataSet();
            da.Fill(ds, "personas");
            conectar.Close();
            return ds;
        }
        public DataSet CargarPorId(string id)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT " +
                                           " id = PER_ID, " +
                                           " nit = PER_NIT, " +
                                           " nombre1 = PER_NOMBRE1, " +
                                           " nombre2 = PER_NOMBRE2, " +
                                           " apellido1 = PER_APELLIDO1, " +
                                           " apellido2 = PER_APELLIDO2, " +
                                           " telefono = PER_TELEFONO " +
                                           " FROM COM.PERSONA WHERE PER_NIT = " + id, conectar);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conectar.Close();
            return ds;
        }
        public bool Exist(string id)
        {
            bool exist = false;
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT 1 FROM COM.PERSONA WHERE PER_NIT = '" + id + "'", conectar);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataSet ds = new DataSet();

            if (da.Fill(ds, "persona") > 0)
                exist = true;

            conectar.Close();
            return exist;
        }
        public Personas Actualizar(Personas personas)
        {
            conectar.Open();
            string Query = "UPDATE COM.PERSONA SET PER_NOMBRE1 = '@NOMBRE1', PER_NOMBRE2 = '@NOMBRE2', PER_APELLIDO1 = '@APELLIDO1', PER_APELLIDO2 = '@APELLIDO2', PER_TELEFONO = '@TELEFONO' WHERE PER_NIT = '@NIT'";
            Query = Query.Replace("@NOMBRE1", personas.nombre1).Replace("@NOMBRE2", personas.nombre2).Replace("@APELLIDO1", personas.apellido1).Replace("@APELLIDO2", personas.apellido2).Replace("@TELEFONO", personas.telefono).Replace("@NIT", personas.nit);
            SqlCommand cm = new SqlCommand(Query, conectar);
            cm.ExecuteNonQuery();
            conectar.Close();
            return personas;
        }
        public Personas Agregar(Personas personas)
        {
            conectar.Open();
            string Query = "INSERT INTO COM.PERSONA(PER_NIT, PER_NOMBRE1, PER_NOMBRE2, PER_APELLIDO1, PER_APELLIDO2, PER_TELEFONO) VALUES ('@NIT', '@NOMBRE1', '@NOMBRE2', '@APELLIDO1', '@APELLIDO2', '@TELEFONO')";
            Query = Query.Replace("@NIT", personas.nit).Replace("@NOMBRE1", personas.nombre1).Replace("@NOMBRE2", personas.nombre2).Replace("@APELLIDO1", personas.apellido1).Replace("@APELLIDO2", personas.apellido2).Replace("@TELEFONO", personas.telefono);
            SqlCommand cm = new SqlCommand(Query, conectar);
            cm.ExecuteNonQuery();

            cm = new SqlCommand("SELECT " +
                               " id = PER_ID" +
                               " FROM COM.PERSONA WHERE PER_NIT = '" + personas.nit + "'", conectar);
            SqlDataReader reader = cm.ExecuteReader();
            while (reader.Read())
            {
                personas.id = reader.GetInt32(0);
            }
            reader.Close();
            conectar.Close();
            return personas;
        }
        public void Borrar(string id)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("DELETE FROM COM.PERSONA WHERE PER_NIT = '" + id + "'", conectar);
            cm.ExecuteNonQuery();
            conectar.Close();
        }
    }
}
