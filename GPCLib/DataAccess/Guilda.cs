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
    public class Guilda
    {
        public GuildaModels ObterGuilda(long id)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();

            select.AppendLine("select Id,Nome from");
            select.AppendLine("DB_SW.dbo.Guilda ");
            select.AppendLine("where id = @id ");

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));
            command.Parameters["@id"].Value = id;

            try
            {
                GuildaModels objGuilda = new GuildaModels();

                conexao.Open();

                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objGuilda = new GuildaModels();
                    objGuilda.Id = long.Parse(reader["Id"].ToString());
                    objGuilda.Nome = reader["Nome"].ToString();


                }

                conexao.Close();
                conexao.Dispose();

                return objGuilda;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<GuildaModels> ListarGuildas()
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();

            select.AppendLine("select Id,Nome from");
            select.AppendLine("DB_SW.dbo.Guilda ");

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            try
            {
                List<GuildaModels> lstGuilda = new List<GuildaModels>();

                conexao.Open();

                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    GuildaModels objGuilda = new GuildaModels();
                    objGuilda = new GuildaModels();
                    objGuilda.Id = long.Parse(reader["Id"].ToString());
                    objGuilda.Nome = reader["Nome"].ToString();

                    lstGuilda.Add(objGuilda);

                }

                conexao.Close();
                conexao.Dispose();

                return lstGuilda;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public GuildaPlayerModels ListarUsuariosGuilda(long idGuilda)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");

            select.AppendLine("a.Ativo,a.IdGuilda,a.IdPlayer,a.IdUsuario, ");
            select.AppendLine("b.Id,b.Nome, ");
            select.AppendLine("c.ID,c.Imagem,c.Level,c.Nome NomePlayer, c.PontoArena,c.Status, ");
            select.AppendLine("d.Id,d.Email,d.UserName ");
            select.AppendLine("from dbo.Guilda_Player a ");
            select.AppendLine("inner join dbo.Guilda b on b.Id = a.IdGuilda ");
            select.AppendLine("inner join dbo.Player c on c.ID = a.IdPlayer ");
            select.AppendLine("inner join dbo.AspNetUsers d on d.Id = a.IdUsuario ");
            select.AppendLine("where a.IdGuilda = @idGuilda ");

            command.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.Int));
            command.Parameters["@idGuilda"].Value = idGuilda;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            try
            {
                conexao.Open();

                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                GuildaPlayerModels objGuildaPlayer = new GuildaPlayerModels();

                objGuildaPlayer.Guilda = ObterGuilda(idGuilda);
                objGuildaPlayer.Usuarios = new List<UsuarioModels>();
                objGuildaPlayer.Players = new List<PlayerModels>();

                while (reader.Read())
                {
                    UsuarioModels objUsuario = new UsuarioModels();
                    objUsuario.Id = reader["IdUsuario"].ToString();
                    objUsuario.Email = reader["Email"].ToString();
                    objUsuario.Username = reader["UserName"].ToString();
                    objGuildaPlayer.Usuarios.Add(objUsuario);

                    PlayerModels objPlayer = new PlayerModels();
                    objPlayer.Id = Convert.ToInt32(reader["IdPlayer"].ToString());
                    objPlayer.Nome = reader["NomePlayer"].ToString();
                    objPlayer.Level = Convert.ToInt32(reader["Level"].ToString());
                    objPlayer.PontoArena = Convert.ToInt32(reader["PontoArena"].ToString());
                    objPlayer.Ativo = (reader["Status"].ToString() == "S") ? true : false;

                    objGuildaPlayer.Players.Add(objPlayer);

                }

                conexao.Close();
                conexao.Dispose();

                return objGuildaPlayer;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
