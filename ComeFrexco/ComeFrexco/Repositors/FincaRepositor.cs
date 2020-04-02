using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ComeFrexco.Models;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Http;

namespace ComeFrexco.Repositors
{
    public class FincaRepositor
    {
        private SqlConnection conectar { get; set; }
        public SqlDataReader reader { get; set; }

        /// <summary>
        /// Inyexión que permite pasar la peticion del cliente como argumento con los datos basicos para poder establecer la conexión al motor
        /// </summary>
        /// <param name="config">Interfaz de la clase config donde se encuentra la estructura de conexión y validación del token</param>
        /// <param name="recuest">Petición enviada desde el cliente con los datos necesarios</param>
        public FincaRepositor(IConfig config, HttpRequest recuest)
        {
            conectar = config.ConectarSqlServer(recuest);
        }
        #region 'FINCA'
        public bool ExistFinca(int id)
        {
            bool exist = false;
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT 1 FROM COM.FINCA WHERE FIN_ID = " + id, conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            if (da.Fill(ds, "finca") > 0)
                exist = true;

            conectar.Close();
            return exist;
        }
        public List<CargarFinca> Cargar()
        {
            List<CargarFinca> fincas = new List<CargarFinca>();
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT " +
                                           " 'id' = F.FIN_ID," +
                                           " 'fin_descripcion' = F.FIN_DESC," +
                                           " 'per_id' = F.PER_ID," +
                                           " 'nombres' = CONCAT(PE.PER_APELLIDO1,' ', PE.PER_APELLIDO2, ' ', PE.PER_NOMBRE1, ' ', PE.PER_NOMBRE2)," +
                                           " 'ver_id' = F.VER_ID," +
                                           " 'ver_descripcion' =  V.VER_DESC" +
                                           " FROM COM.FINCA F" +
                                           " INNER JOIN COM.PERSONA PE ON PE.PER_ID = F.PER_ID" +
                                           " INNER JOIN COM.VEREDA V ON V.VER_ID = F.VER_ID" +
                                           " ORDER BY 1 DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "fincas");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                fincas.Add(getCargarFinca(dr));
            }
            conectar.Close();
            return fincas;
        }
        public CargarFinca CargarPorId(int id)
        {
            CargarFinca cargarFinca = null;
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT " +
                                           " 'id' = F.FIN_ID," +
                                           " 'fin_descripcion' = F.FIN_DESC," +
                                           " 'per_id' = F.PER_ID," +
                                           " 'nombres' = CONCAT(PE.PER_APELLIDO1,' ', PE.PER_APELLIDO2, ' ', PE.PER_NOMBRE1, ' ', PE.PER_NOMBRE2)," +
                                           " 'ver_id' = F.VER_ID," +
                                           " 'ver_descripcion' =  V.VER_DESC" +
                                           " FROM COM.FINCA F" +
                                           " INNER JOIN COM.PERSONA PE ON PE.PER_ID = F.PER_ID" +
                                           " INNER JOIN COM.VEREDA V ON V.VER_ID = F.VER_ID" +
                                           " WHERE F.FIN_ID = " + id, conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "fincas");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                cargarFinca = getCargarFinca(dr);
            }
            conectar.Close();
            return cargarFinca;
        }
        public List<CargarFinca> CargarPorPropietario(int id)
        {
            List<CargarFinca> fincas = new List<CargarFinca>();
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT " +
                                           " 'id' = F.FIN_ID," +
                                           " 'fin_descripcion' = F.FIN_DESC," +
                                           " 'per_id' = F.PER_ID," +
                                           " 'nombres' = CONCAT(PE.PER_APELLIDO1,' ', PE.PER_APELLIDO2, ' ', PE.PER_NOMBRE1, ' ', PE.PER_NOMBRE2)," +
                                           " 'ver_id' = F.VER_ID," +
                                           " 'ver_descripcion' =  V.VER_DESC" +
                                           " FROM COM.FINCA F" +
                                           " INNER JOIN COM.PERSONA PE ON PE.PER_ID = F.PER_ID" +
                                           " INNER JOIN COM.VEREDA V ON V.VER_ID = F.VER_ID" +
                                           " WHERE F.PER_ID = " + id, conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "fincas");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                fincas.Add(getCargarFinca(dr));
            }
            conectar.Close();
            return fincas;
        }
        public CargarFinca Agregar(Finca finca)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("INSERT INTO COM.FINCA (PER_ID, VER_ID, FIN_DESC) VALUES ('" + finca.per_id + "', '" + finca.ver_id + "', '" + finca.fin_desc + "')", conectar);
            cm.ExecuteNonQuery();

            CargarFinca cargarFinca = null;

            cm = new SqlCommand(" SELECT TOP 1 " +
                               " 'id' = F.FIN_ID," +
                               " 'fin_descripcion' = F.FIN_DESC," +
                               " 'per_id' = F.PER_ID," +
                               " 'nombres' = CONCAT(PE.PER_APELLIDO1,' ', PE.PER_APELLIDO2, ' ', PE.PER_NOMBRE1, ' ', PE.PER_NOMBRE2)," +
                               " 'ver_id' = F.VER_ID," +
                               " 'ver_descripcion' =  V.VER_DESC" +
                               " FROM COM.FINCA F" +
                               " INNER JOIN COM.PERSONA PE ON PE.PER_ID = F.PER_ID" +
                               " INNER JOIN COM.VEREDA V ON V.VER_ID = F.VER_ID" +
                               " ORDER BY F.FIN_ID DESC ", conectar);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "finca");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                cargarFinca = getCargarFinca(dr);
            }
            conectar.Close();
            return cargarFinca;
        }
        public CargarFinca Actualizar(Finca finca)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("UPDATE COM.FINCA SET PER_ID = @PERSONA, VER_ID = @VEREDA, FIN_DESC = @FINCA WHERE FIN_ID = @ID", conectar);
            cm.Parameters.AddWithValue("PERSONA", finca.per_id);
            cm.Parameters.AddWithValue("VEREDA", finca.ver_id);
            cm.Parameters.AddWithValue("FINCA", finca.fin_desc);
            cm.Parameters.AddWithValue("ID", finca.fin_id);
            cm.ExecuteNonQuery();

            CargarFinca cargarFinca = null;

            cm = new SqlCommand(" SELECT " +
                   " 'id' = F.FIN_ID," +
                   " 'fin_descripcion' = F.FIN_DESC," +
                   " 'per_id' = F.PER_ID," +
                   " 'nombres' = CONCAT(PE.PER_APELLIDO1,' ', PE.PER_APELLIDO2, ' ', PE.PER_NOMBRE1, ' ', PE.PER_NOMBRE2)," +
                   " 'ver_id' = F.VER_ID," +
                   " 'ver_descripcion' =  V.VER_DESC" +
                   " FROM COM.FINCA F" +
                   " INNER JOIN COM.PERSONA PE ON PE.PER_ID = F.PER_ID" +
                   " INNER JOIN COM.VEREDA V ON V.VER_ID = F.VER_ID" +
                   " WHERE F.FIN_ID =  " + finca.fin_id, conectar);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "finca");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                cargarFinca = getCargarFinca(dr);
            }
            conectar.Close();
            return cargarFinca;
        }
        public void Borrar(int id)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("DELETE FROM COM.FINCA WHERE FIN_ID = " + id, conectar);
            cm.ExecuteNonQuery();
            conectar.Close();
        }
        private CargarFinca getCargarFinca(DataRow dr)
        {
            Persona persona = new Persona
            {
                id = Convert.ToInt32(dr["per_id"]),
                nombres = dr["nombres"].ToString()
            };

            Vereda vereda = new Vereda
            {
                id = Convert.ToInt32(dr["ver_id"]),
                descripcion = dr["ver_descripcion"].ToString()
            };

            return new CargarFinca
            {
                id = Convert.ToInt32(dr["id"]),
                descripcion = dr["fin_descripcion"].ToString(),
                persona = persona,
                vereda = vereda
            };
        }
        #endregion
        #region 'VEREDA'
        public bool ExistVereda(int id)
        {
            bool exist = false;
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT 1 FROM COM.VEREDA WHERE VER_ID = " + id, conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            if (da.Fill(ds, "vereda") > 0)
                exist = true;

            conectar.Close();
            return exist;
        }
        public List<CargarVereda> CargarVereda()
        {
            List<CargarVereda> veredas = new List<CargarVereda>();
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT" +
                                           " 'ver_id' = V.VER_ID," +
                                           " 'ver_descripcion' = V.VER_DESC," +
                                           " 'mun_id' = M.MUN_ID," +
                                           " 'mun_descripcion' = M.MUN_DESC," +
                                           " 'dep_id' = D.DEP_ID," +
                                           " 'dep_descripcion' = D.DEP_DESC FROM COM.VEREDA V" +
                                           " INNER JOIN COM.MUNICIPIO M ON M.MUN_ID = V.MUN_ID" +
                                           " INNER JOIN COM.DEPARTAMENTO D ON D.DEP_ID = M.DEP_ID " +
                                           " ORDER BY V.VER_ID DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "veredas");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                object departamento = new
                {
                    id = Convert.ToInt32(dr["dep_id"]),
                    descripcion = dr["dep_descripcion"].ToString()
                };

                object municipio = new
                {
                    id = Convert.ToInt32(dr["mun_id"]),
                    descripcion = dr["mun_descripcion"].ToString()
                };

                CargarVereda cargarVereda = new CargarVereda
                {
                    id = Convert.ToInt32(dr["ver_id"]),
                    descripcion = dr["ver_descripcion"].ToString(),
                    departamento = departamento,
                    municipio = municipio
                };
                veredas.Add(cargarVereda);
            }
            conectar.Close();
            return veredas;
        }
        public List<CargarVereda> CargarVeredaById(int id)
        {
            List<CargarVereda> veredas = new List<CargarVereda>();
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT" +
                                           " 'ver_id' = V.VER_ID," +
                                           " 'ver_descripcion' = V.VER_DESC," +
                                           " 'mun_id' = M.MUN_ID," +
                                           " 'mun_descripcion' = M.MUN_DESC," +
                                           " 'dep_id' = D.DEP_ID," +
                                           " 'dep_descripcion' = D.DEP_DESC FROM COM.VEREDA V" +
                                           " INNER JOIN COM.MUNICIPIO M ON M.MUN_ID = V.MUN_ID" +
                                           " INNER JOIN COM.DEPARTAMENTO D ON D.DEP_ID = M.DEP_ID" +
                                           " WHERE V.VER_ID = " + id, conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "vereda");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                object departamento = new
                {
                    id = Convert.ToInt32(dr["dep_id"]),
                    descripcion = dr["dep_descripcion"].ToString()
                };

                object municipio = new
                {
                    id = Convert.ToInt32(dr["mun_id"]),
                    descripcion = dr["mun_descripcion"].ToString()
                };

                CargarVereda cargarVereda = new CargarVereda
                {
                    id = Convert.ToInt32(dr["ver_id"]),
                    descripcion = dr["ver_descripcion"].ToString(),
                    departamento = departamento,
                    municipio = municipio
                };
                veredas.Add(cargarVereda);
            }
            conectar.Close();
            return veredas;
        }
        public List<CargarVereda> CargarVeredaByMunicipio(int id)
        {
            List<CargarVereda> veredas = new List<CargarVereda>();
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT" +
                                           " 'ver_id' = V.VER_ID," +
                                           " 'ver_descripcion' = V.VER_DESC," +
                                           " 'mun_id' = M.MUN_ID," +
                                           " 'mun_descripcion' = M.MUN_DESC," +
                                           " 'dep_id' = D.DEP_ID," +
                                           " 'dep_descripcion' = D.DEP_DESC FROM COM.VEREDA V" +
                                           " INNER JOIN COM.MUNICIPIO M ON M.MUN_ID = V.MUN_ID" +
                                           " INNER JOIN COM.DEPARTAMENTO D ON D.DEP_ID = M.DEP_ID" +
                                           " WHERE M.MUN_ID = " + id, conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "vereda");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                object departamento = new
                {
                    id = Convert.ToInt32(dr["dep_id"]),
                    descripcion = dr["dep_descripcion"].ToString()
                };

                object municipio = new
                {
                    id = Convert.ToInt32(dr["mun_id"]),
                    descripcion = dr["mun_descripcion"].ToString()
                };

                CargarVereda cargarVereda = new CargarVereda
                {
                    id = Convert.ToInt32(dr["ver_id"]),
                    descripcion = dr["ver_descripcion"].ToString(),
                    departamento = departamento,
                    municipio = municipio
                };
                veredas.Add(cargarVereda);
            }
            conectar.Close();
            return veredas;
        }
        public CargarVereda AgregarVereda(AgregarVereda agregarVereda)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("INSERT INTO COM.VEREDA (VER_DESC, MUN_ID) VALUES ('" + agregarVereda.descripcion + "', '" + agregarVereda.municipio + "')", conectar);
            cm.ExecuteNonQuery();

            CargarVereda cargarVereda = null;
            cm = new SqlCommand(" SELECT TOP 1" +
                                " 'ver_id' = V.VER_ID," +
                                " 'ver_descripcion' = V.VER_DESC," +
                                " 'mun_id' = M.MUN_ID," +
                                " 'mun_descripcion' = M.MUN_DESC," +
                                " 'dep_id' = D.DEP_ID," +
                                " 'dep_descripcion' = D.DEP_DESC FROM COM.VEREDA V" +
                                " INNER JOIN COM.MUNICIPIO M ON M.MUN_ID = V.MUN_ID" +
                                " INNER JOIN COM.DEPARTAMENTO D ON D.DEP_ID = M.DEP_ID " +
                                " ORDER BY V.VER_ID DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "vereda");

            DataTable dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                object departamento = new
                {
                    id = Convert.ToInt32(dr["dep_id"]),
                    descripcion = dr["dep_descripcion"].ToString()
                };

                object municipio = new
                {
                    id = Convert.ToInt32(dr["mun_id"]),
                    descripcion = dr["mun_descripcion"].ToString()
                };

                cargarVereda = new CargarVereda
                {
                    id = Convert.ToInt32(dr["ver_id"]),
                    descripcion = dr["ver_descripcion"].ToString(),
                    departamento = departamento,
                    municipio = municipio
                };
            }
            conectar.Close();
            return cargarVereda;
        }
        public CargarVereda ActualizarVereda(AgregarVereda vereda, int id)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("UPDATE COM.VEREDA SET VER_DESC = @DESCRIPCION, MUN_ID = @MUNICIPIO WHERE VER_ID = @VEREDA", conectar);
            cm.Parameters.AddWithValue("DESCRIPCION", vereda.descripcion);
            cm.Parameters.AddWithValue("MUNICIPIO", vereda.municipio);
            cm.Parameters.AddWithValue("VEREDA", id);
            cm.ExecuteNonQuery();

            CargarVereda cargarVereda = null;

            cm = new SqlCommand(" SELECT" +
                                           " 'ver_id' = V.VER_ID," +
                                           " 'ver_descripcion' = V.VER_DESC," +
                                           " 'mun_id' = M.MUN_ID," +
                                           " 'mun_descripcion' = M.MUN_DESC," +
                                           " 'dep_id' = D.DEP_ID," +
                                           " 'dep_descripcion' = D.DEP_DESC FROM COM.VEREDA V" +
                                           " INNER JOIN COM.MUNICIPIO M ON M.MUN_ID = V.MUN_ID" +
                                           " INNER JOIN COM.DEPARTAMENTO D ON D.DEP_ID = M.DEP_ID" +
                                           " WHERE V.VER_ID = " + id, conectar);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "vereda");

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                object departamento = new
                {
                    id = Convert.ToInt32(dr["dep_id"]),
                    descripcion = dr["dep_descripcion"].ToString()
                };

                object municipio = new
                {
                    id = Convert.ToInt32(dr["mun_id"]),
                    descripcion = dr["mun_descripcion"].ToString()
                };

                cargarVereda = new CargarVereda
                {
                    id = Convert.ToInt32(dr["ver_id"]),
                    descripcion = dr["ver_descripcion"].ToString(),
                    departamento = departamento,
                    municipio = municipio
                };
            }
            conectar.Close();
            return cargarVereda;
        }
        public void BorrarVereda(int id)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("DELETE FROM COM.VEREDA WHERE VER_ID = " + id, conectar);
            cm.ExecuteNonQuery();
            conectar.Close();
        }
        #endregion
    }
}
