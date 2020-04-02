using System.Collections.Generic;
using System.Data.SqlClient;
using ComeFrexco.Models;
using ComeFrexco.Services;

namespace ComeFrexco.Repositors
{
	public class LoginRepositor
	{
		private SqlConnection conectar { get; set; }
		public SqlDataReader reader { get; set; }

		/// <summary>
		/// Inyexión que permite pasar la peticion del cliente como argumento con los datos basicos para poder establecer la conexión al motor
		/// </summary>
		/// <param name="config">Interfaz de la clase config donde se encuentra la estructura de conexión y validación del token</param>
		/// <param name="recuest">Petición enviada desde el cliente con los datos necesarios</param>
		public LoginRepositor(IConfig config, Usuario usuario)
		{
			conectar = config.Login(usuario);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="usuario"></param>
		/// <returns>Retorna un objeto de tipo SqlDatareader para validar si se pudo realizar la autenticacion de manera exitosa</returns>
		public Usuario Login(Usuario usuario)
		{
			conectar.Open();
			SqlCommand cm = new SqlCommand("SELECT USU_USU, USU_CLAVE, USU_ADMIN, USU_NOMBRE FROM COM.USUARIO WHERE USU_USU = '" + usuario.userName + "' AND USU_CLAVE = '" + usuario.password + "'", conectar);
			SqlDataReader reader = cm.ExecuteReader();
			if (reader.Read())
			{
				usuario = new Usuario()
				{
					userName = reader.GetString(0),
					password = reader.GetString(1),
					isAdmin = reader.GetString(2),
                    nombre = reader.GetString(3)
                };
			}
			else
			{
				throw new System.Exception("No se pudo validar la autenticación en dos pasos");
			}
			reader.Close();
			conectar.Close();
			return usuario;
		}
		public List<Permissions> Permissions(Usuario usuario)
		{
			List<Permissions> permissions = new List<Permissions>();

			conectar.Open();
			SqlCommand cm = new SqlCommand("SELECT ACC_USUARIO, ACC_MODULO, ACC_OPERACION, ID_OPERACION, ACC_GRANTE, CASE WHEN ACC_OPERACION = 'SELECT' THEN 'SELECCIONAR' WHEN ACC_OPERACION = 'INSERT' THEN 'INSERTAR' WHEN ACC_OPERACION = 'DELETE' THEN 'BORRAR' WHEN ACC_OPERACION = 'UPDATE' THEN 'ACTUALIZAR' END AS LABEL FROM COM.PERMISOS WHERE ACC_USUARIO = '" + usuario.userName + "' ORDER BY ACC_MODULO, ID_OPERACION;", conectar);
			SqlDataReader reader = cm.ExecuteReader();
			while (reader.Read())
			{
				Permissions per = new Permissions()
				{
					userName = reader.GetString(0),
					module = reader.GetString(1),
					permission = reader.GetString(2),
					idPermission = reader.GetInt32(3),
					grante = reader.GetString(4),
					label = reader.GetString(5)
				};
				permissions.Add(per);
			}

			conectar.Close();
			reader.Close();
			return permissions;
		}
	}
}