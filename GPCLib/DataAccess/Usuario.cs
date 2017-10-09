using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPCLib.Models;
using System.Data.SqlClient;
using System.Configuration;


namespace GPCLib.DataAccess
{
    public class Usuario
    {
        public UsuarioModels ObterUsario(string id)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT * FROM DB_SW.dbo.AspNetUsers WHERE ID = @ID");


            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;
            sqlCom.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.VarChar));
            sqlCom.Parameters["@ID"].Value = id;

            try
            {
                Models.UsuarioModels objUsuario = new UsuarioModels();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objUsuario = new Models.UsuarioModels();
                    objUsuario.Email = reader["Email"].ToString();
                    objUsuario.Id = reader["Id"].ToString();
                    objUsuario.UserName = reader["Username"].ToString();


                }
                conn.Close();
                conn.Dispose();
                return objUsuario;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<Models.UsuarioModels> ListarUsuarios()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT * FROM dbo.AspNetUsers");

            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            try
            {
                Models.UsuarioModels objUsuario;
                List<Models.UsuarioModels> objRetorno = new List<UsuarioModels>();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objUsuario = new Models.UsuarioModels();
                    objUsuario.Email = reader["Email"].ToString();
                    objUsuario.Id = reader["Id"].ToString();
                    objUsuario.UserName = reader["UserName"].ToString();
                    
                    objRetorno.Add(objUsuario);

                }
                conn.Close();
                conn.Dispose();
                return objRetorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void AtualizarCodGuilda(long idGuilda,string idUsuario)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();

            select.AppendLine("update dbo.AspNetUsers set idGuilda = @idGuilda where id= @idUsuario");

            command.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.BigInt));
            command.Parameters["@idGuilda"].Value = idGuilda;

            command.Parameters.Add(new SqlParameter("@idUsuario", System.Data.SqlDbType.VarChar));
            command.Parameters["@idUsuario"].Value = idUsuario;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            try
            {

                conexao.Open();

                command.Connection = conexao;
                command.ExecuteNonQuery();

                conexao.Close();
                conexao.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
