using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ComeFrexco.Models;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Http;

namespace ComeFrexco.Repositors
{
    public class UsuarioRepositor
    {
        private SqlConnection conectar { get; set; }
        public SqlDataReader reader { get; set; }

        /// <summary>
        /// Inyexión que permite pasar la peticion del cliente como argumento con los datos basicos para poder establecer la conexión al motor
        /// </summary>
        /// <param name="config">Interfaz de la clase config donde se encuentra la estructura de conexión y validación del token</param>
        /// <param name="recuest">Petición enviada desde el cliente con los datos necesarios</param>
        public UsuarioRepositor(IConfig config, HttpRequest recuest)
        {
            conectar = config.ConectarSqlServer(recuest);
        }
        public DataSet Cargar()
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT" +
                                           " 'userName' = USU_USU," +
                                           " 'password' = USU_CLAVE," +
                                           " 'isAdmin'  = USU_ADMIN," +
                                           " 'nombre'   = USU_NOMBRE " +
                                           " FROM COM.USUARIO ORDER BY 1 DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "usuarios");
            conectar.Close();
            return ds;
        }
        public DataSet CargarPorId(string userName)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT" +
                                           " 'userName' = USU_USU," +
                                           " 'password' = USU_CLAVE," +
                                           " 'isAdmin'  = USU_ADMIN," +
                                           " 'nombre'   = USU_NOMBRE " +
                                           " FROM COM.USUARIO" +
                                           " WHERE USU_USU = '" + userName +
                                           "' ORDER BY 1 DESC", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "usuario");
            conectar.Close();
            return ds;
        }
        public bool Exist(string userName)
        {
            bool exist = false;
            conectar.Open();
            SqlCommand cm = new SqlCommand("SELECT 1 FROM COM.USUARIO WHERE USU_USU = '" + userName + "'", conectar);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataSet ds = new DataSet();

            if (da.Fill(ds, "usuario") > 0)
                exist = true;

            conectar.Close();
            return exist;
        }

        public Usuario Agregar(Usuario usuario)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("INSERT INTO COM.USUARIO (USU_USU, USU_CLAVE, USU_ADMIN, USU_NOMBRE) VALUES ('" + usuario.userName + "', '" + usuario.password + "', '" + usuario.isAdmin.ToUpper() + "', '" + usuario.nombre + "')", conectar);
            cm.ExecuteNonQuery();
            conectar.Close();
            return usuario;
        }
        public Usuario Actualizar(Usuario usuario)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("UPDATE COM.USUARIO SET USU_NOMBRE = @NOMBRE, USU_ADMIN = @ADMIN WHERE USU_USU = @USUARIO", conectar);
            cm.Parameters.AddWithValue("NOMBRE", usuario.nombre);
            cm.Parameters.AddWithValue("ADMIN", usuario.isAdmin);
            cm.Parameters.AddWithValue("USUARIO", usuario.userName);
            cm.ExecuteNonQuery();
            conectar.Close();
            return usuario;
        }
        public void Borrar(string userName)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("DELETE FROM COM.USUARIO WHERE USU_USU = '" + userName + "'", conectar);
            cm.ExecuteNonQuery();
            conectar.Close();
        }
        public void GrantPermission(List<Permissions> permissions)
        {
            conectar.Open();

            foreach (Permissions permiso in permissions)
            {
                SqlCommand cm = new SqlCommand("EXEC COM.GRANT_ACCES @USER, @MODULE, @OPTION", conectar);
                cm.Parameters.AddWithValue("USER", permiso.userName);
                cm.Parameters.AddWithValue("MODULE", permiso.module);
                cm.Parameters.AddWithValue("OPTION", permiso.permission);
                cm.ExecuteNonQuery();
            }

            conectar.Close();
        }
        public void DenyPermission(List<Permissions> permissions)
        {
            conectar.Open();

            foreach (Permissions permiso in permissions)
            {
                SqlCommand cm = new SqlCommand("EXEC COM.DENY_ACCES @USER, @MODULE, @OPTION", conectar);
                cm.Parameters.AddWithValue("USER", permiso.userName);
                cm.Parameters.AddWithValue("MODULE", permiso.module);
                cm.Parameters.AddWithValue("OPTION", permiso.permission);
                cm.ExecuteNonQuery();
            }

            conectar.Close();
        }
        public DataSet Permissions(Usuario usuario)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand(" SELECT" +
                                           " 'userName'     = ACC_USUARIO," +
                                           " 'module'       = ACC_MODULO," +
                                           " 'permission'   = ACC_OPERACION," +
                                           " 'idPermission' = ID_OPERACION," +
                                           " 'grante'       = ACC_GRANTE," +
                                           " 'label'        = CASE" +
                                                            " WHEN ACC_OPERACION = 'SELECT' THEN 'SELECCIONAR'" +
                                                            " WHEN ACC_OPERACION = 'INSERT' THEN 'INSERTAR'" +
                                                            " WHEN ACC_OPERACION = 'DELETE' THEN 'BORRAR'" +
                                                            " WHEN ACC_OPERACION = 'UPDATE' THEN 'ACTUALIZAR' END" +
                                           " FROM COM.PERMISOS" +
                                           " WHERE ACC_USUARIO = '" + usuario.userName + "'" +
                                           " ORDER BY ACC_MODULO, ID_OPERACION", conectar);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(ds, "permissions");

            conectar.Close();
            return ds;
        }
        public void RestablecerContraseña(Usuario usuario)
        {
            conectar.Open();
            SqlCommand cm = new SqlCommand("UPDATE COM.USUARIO SET USU_CLAVE = @CLAVE WHERE USU_USU = @USUARIO", conectar);
            cm.Parameters.AddWithValue("CLAVE", usuario.password);
            cm.Parameters.AddWithValue("USUARIO", usuario.userName);
            cm.ExecuteNonQuery();
            conectar.Close();
        }
    }
}