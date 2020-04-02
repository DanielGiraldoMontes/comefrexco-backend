using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ComeFrexco.Models;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Http;

namespace ComeFrexco.Repositors
{
    public class DepartamentoRepositor
    {
        private SqlConnection conectar { get; set; }
        public SqlDataReader reader { get; set; }

        /// <summary>
        /// Inyexión que permite pasar la peticion del cliente como argumento con los datos basicos para poder establecer la conexión al motor
        /// </summary>
        /// <param name="config">Interfaz de la clase config donde se encuentra la estructura de conexión y validación del token</param>
        /// <param name="recuest">Petición enviada desde el cliente con los datos necesarios</param>
        public DepartamentoRepositor(IConfig config, HttpRequest recuest)
        {
            conectar = config.ConectarSqlServer(recuest);
        }
        public DataSet CargarDepartamento()
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT 'id' = DEP_ID, 'descripcion' = DEP_DESC FROM COM.DEPARTAMENTO ORDER BY DEP_ID DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "departamentos");
            conectar.Close();
            return ds;
        }
        public DataSet AgregarDepartamento(Departamento departamento)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("INSERT INTO COM.DEPARTAMENTO (DEP_DESC) VALUES (@DEP_DESC)", conectar);
            cm.Parameters.AddWithValue("DEP_DESC", departamento.dep_desc);
            cm.ExecuteNonQuery();

            cm = new SqlCommand("SELECT TOP 1 'id' = DEP_ID, 'descripcion' = DEP_DESC FROM COM.DEPARTAMENTO ORDER BY DEP_ID DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "departamento");
            conectar.Close();
            return ds;
        }
        public DataSet CargarMunicipio()
        {
            List<Municipio> municipios = new List<Municipio>();
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT 'id' = MUN_ID, 'descripcion' = MUN_DESC, 'departamento' = DEP_ID FROM COM.MUNICIPIO ORDER BY MUN_ID DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "municipios");
            conectar.Close();
            return ds;
        }
        public DataSet AgregarMunicipio(Municipio municipio)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("INSERT INTO COM.MUNICIPIO (MUN_DESC, DEP_ID) VALUES (@MUNI_DESC, DEP_ID)", conectar);
            cm.Parameters.AddWithValue("MUNI_DESC", municipio.muni_desc);
            cm.Parameters.AddWithValue("DEP_ID", municipio.munidep_id);
            cm.ExecuteNonQuery();

            cm = new SqlCommand("SELECT TOP 1 'id' = MUN_ID, 'descripcion' = MUN_DESC, 'departamento' = DEP_ID FROM COM.MUNICIPIO ORDER BY MUN_ID DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "municipio");
            conectar.Close();
            return ds;
        }
        public DataSet CargarMunicipioPorDepartamento(int depId)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT 'id' = MUN_ID, 'descripcion' = MUN_DESC, 'departamento' = DEP_ID FROM COM.MUNICIPIO WHERE DEP_ID = " + depId + " ORDER BY MUN_ID DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "municipios");
            conectar.Close();
            return ds;
        }
    }
}
