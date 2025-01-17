﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PrimerProyecto.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
		public RepositorioInmueble(IConfiguration configuration) : base(configuration)
		{

		}

		public int Alta(Inmueble i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmueble (Direccion, Uso, Tipo, CantAmbientes, Precio, Estado, PropietarioId) " +
					$"VALUES (@direccion, @uso, @tipo, @cantAmbientes, @precio, @estado, @propietarioId);" +
					$"SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@uso", i.Uso);
					command.Parameters.AddWithValue("@tipo", i.Tipo);
					command.Parameters.AddWithValue("@cantAmbientes", i.CantAmbientes);
					command.Parameters.AddWithValue("@precio", i.Precio);
					command.Parameters.AddWithValue("@estado", i.Estado);
					command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					i.Id = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Inmueble WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Inmueble i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inmueble SET Direccion=@direccion, Uso=@uso, Tipo=@tipo, CantAmbientes=@cantAmbientes, Precio=@precio, Estado=@estado, PropietarioId=@propietarioId " +
					$"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@uso", i.Uso);
					command.Parameters.AddWithValue("@tipo", i.Tipo);
					command.Parameters.AddWithValue("@cantAmbientes", i.CantAmbientes);
					command.Parameters.AddWithValue("@precio", i.Precio);
					command.Parameters.AddWithValue("@estado", i.Estado);
					command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
					command.Parameters.AddWithValue("@id", i.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.Id, Direccion, Uso, Tipo, CantAmbientes, Precio, Estado, PropietarioId," +
					" p.Nombre, p.Apellido" +
					" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Uso = reader.GetString(2),
							Tipo = reader.GetString(3),
							CantAmbientes = reader.GetInt32(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetBoolean(6),
							PropietarioId = reader.GetInt32(7),
							Propietario = new Propietario
							{
								Id = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inmueble ObtenerPorId(int id)
		{
			Inmueble i = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.Id, Direccion, Uso, Tipo, CantAmbientes, Precio, Estado, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id" +
					$" WHERE i.Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Uso = reader.GetString(2),
							Tipo = reader.GetString(3),
							CantAmbientes = reader.GetInt32(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetBoolean(6),
							PropietarioId = reader.GetInt32(7),
							Propietario = new Propietario
							{
								Id = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
					}
					connection.Close();
				}
			}
			return i;
		}
		public IList<Inmueble> ObtenerTodosPorPropietarioId(int propietarioId)
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.Id, Direccion, Uso, Tipo, CantAmbientes, Precio, Estado, PropietarioId," +
					" p.Nombre, p.Apellido" +
					" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id" +
					$" WHERE i.PropietarioId = @propietarioId";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@propietarioId", SqlDbType.Int).Value = propietarioId;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Uso = reader.GetString(2),
							Tipo = reader.GetString(3),
							CantAmbientes = reader.GetInt32(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetBoolean(6),
							PropietarioId = reader.GetInt32(7),
							Propietario = new Propietario
							{
								Id = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}
		public IList<Inmueble> ObtenerTodosDisponibles(DateTime fechaInicio, DateTime fechaFinal)
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.Id, Direccion, Uso, Tipo, CantAmbientes, Precio, i.Estado, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id " +
					$"WHERE i.Id IN ( SELECT InmuebleId " + 
						$"FROM ContratoAlquiler ca "+
 						$"WHERE Estado = 1"+					
						$"AND((FechaInicio < @fechaInicio)AND(FechaFinalizacion < @fechaInicio))" +
						$"OR((FechaInicio > @fechaFinal)AND(FechaFinalizacion > @fechaFinal))" +
						$"AND((FechaInicio < @fechaInicio)AND(FechaFinalizacion > @fechaFinal))" +
						$"OR ((FechaInicio > @fechaInicio)AND(FechaFinalizacion < @fechaFinal))" +
						$"AND(FechaInicio NOT BETWEEN @fechaInicio AND @fechaFinal)" +
						$"AND(FechaFinalizacion NOT BETWEEN @fechaInicio AND @fechaFinal))" +
					$"OR i.Id NOT IN(SELECT InmuebleId FROM ContratoAlquiler);";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@fechaInicio", SqlDbType.DateTime).Value = fechaInicio;
					command.Parameters.Add("@fechaFinal", SqlDbType.DateTime).Value = fechaFinal;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Uso = reader.GetString(2),
							Tipo = reader.GetString(3),
							CantAmbientes = reader.GetInt32(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetBoolean(6),
							PropietarioId = reader.GetInt32(7),
							Propietario = new Propietario
							{
								Id = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

	}
}
