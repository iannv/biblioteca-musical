﻿using Proyecto_Discos.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Discos
{
    class DiscoConexion
    {
        public List<Disco> listar()
        {
            List<Disco> listaDiscos = new List<Disco>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=DISCOS_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT D.Id,D.Titulo,D.FechaLanzamiento AS Lanzamiento,D.CantidadCanciones,D.UrlImagenTapa,E.Descripcion AS Genero,T.Descripcion AS Edicion, D.IdEstilo, D.IdTipoEdicion FROM DISCOS AS D, ESTILOS AS E, TIPOSEDICION AS T WHERE D.IdEstilo = E.Id AND D.IdTipoEdicion = T.Id";
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read()) { 
                    Disco disco = new Disco();
                    disco.Id = (int)lector["Id"];
                    disco.Titulo = (string)lector["Titulo"];
                    disco.FechaLanzamiento = (DateTime)lector["Lanzamiento"];
                    disco.CantidadCanciones = (int)lector["CantidadCanciones"];
                    disco.UrlImagenTapa = (string)lector["UrlImagenTapa"];

                    disco.Genero = new Estilo();
                    disco.Genero.Id = (int)lector["IdEstilo"];
                    disco.Genero.Descripcion = (string)lector["Genero"];

                    disco.Edicion = new TipoEdicion();
                    disco.Edicion.Id = (int)lector["IdTipoEdicion"];
                    disco.Edicion.Descripcion = (string)lector["Edicion"];


                    listaDiscos.Add(disco);
                }

                conexion.Close();
                return listaDiscos;

            }
            catch (Exception ex) {
                throw ex;
            }

        }


        // Método para agregar un nuevo disco
        public void agregar(Disco disco) {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO DISCOS(Titulo,FechaLanzamiento,CantidadCanciones,UrlImagenTapa,IdEstilo,IdTipoEdicion)" +
                    "VALUES ('"+disco.Titulo+"','"+disco.FechaLanzamiento+"','"+disco.CantidadCanciones+"',@UrlImagenTapa ,@IdEstilo, @IdTipoEdicion)");
                datos.setearParametro("@UrlImagenTapa", disco.UrlImagenTapa);
                datos.setearParametro("@IdEstilo", disco.Genero.Id);
                datos.setearParametro("@IdTipoEdicion", disco.Edicion.Id);
                

                datos.ejecutarAccion();

            }
            catch (Exception ex) { throw ex; }
        }


        // Método para modificar un disco existente
        public void modificar(Disco disco) {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE DISCOS SET Titulo = @titulo, FechaLanzamiento = @fecha, CantidadCanciones = @cantCanc, UrlImagenTapa = @img, IdEstilo = @idEstilo, IdTipoEdicion = @idEdic WHERE Id = @id");
                datos.setearParametro("@titulo",disco.Titulo);
                datos.setearParametro("@fecha", disco.FechaLanzamiento);
                datos.setearParametro("@cantCanc", disco.CantidadCanciones);
                datos.setearParametro("@img", disco.UrlImagenTapa);
                datos.setearParametro("@idEstilo", disco.Genero.Id);
                datos.setearParametro("@idEdic", disco.Edicion.Id);
                datos.setearParametro("@id", disco.Id);

                datos.ejecutarAccion();

            }
            catch (Exception ex) { throw ex; }
        }


        // Método para eliminar un disco existente
        public void eliminar(int id) {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM DISCOS WHERE ID = @id");
                datos.setearParametro("@id", id);

                datos.ejecutarAccion();

            }
            catch (Exception ex) { throw ex; }
        }
       
    }
}
