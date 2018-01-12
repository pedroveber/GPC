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
   public class Batalha
    {
        public List<BatalhaModels> ListarBatalhas(int idGuilda,bool listarTudo)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            if (!listarTudo)
                select.AppendLine("top 100");

            select.AppendLine("Guilda,life,data,PontuacaoOponente,PontuacaoGuild,RankGuild,idGuilda,Id,IdGuildaAtacante ");
            select.AppendLine(",(select  case when count(1) >= 1 then 1 else 0 end from dbo.lutas b where b.CodBatalhas = a.ID and b.MomentoVitoria = 'Win') Vitoria, ");
            select.AppendLine("(select count(1) from dbo.Batalhas c where c.idGuilda = a.idguilda and c.IdGuildaAtacante = a.IdGuildaAtacante) QtsAtaques");
            select.AppendLine("from dbo.Batalhas a");
            select.AppendLine("where IdGuildaAtacante = @IdGuildaAtacante ");
            select.AppendLine("order by data desc ");

            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@IdGuildaAtacante", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@IdGuildaAtacante"].Value = idGuilda;

            try
            {
                Models.BatalhaModels objBatalha;
                List<Models.BatalhaModels> objRetorno = new List<BatalhaModels>();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();
                Guilda daGuilda = new Guilda();

                List<GuildaModels> guildas = new List<GuildaModels>();
                guildas = daGuilda.ListarGuildas();

                while (reader.Read())
                {
                    objBatalha = new Models.BatalhaModels();

                    if (reader["Data"].ToString() != string.Empty)
                        objBatalha.Data = Convert.ToDateTime(reader["Data"].ToString());
                    
                    objBatalha.GuildaAtacante = guildas.First(m => m.Id == long.Parse(reader["IdGuildaAtacante"].ToString()));
                    objBatalha.GuildaOponente = reader["Guilda"].ToString();
                    objBatalha.IdGuildaOponente = int.Parse(reader["idGuilda"].ToString());
                    objBatalha.RankGuild = int.Parse(reader["RankGuild"].ToString());
                    objBatalha.Vitoria = Convert.ToBoolean(int.Parse(reader["Vitoria"].ToString()));
                    objBatalha.Life = int.Parse(reader["life"].ToString());
                    objBatalha.PontuacaoGuild= int.Parse(reader["PontuacaoGuild"].ToString());
                    objBatalha.PontuacaoOponente = int.Parse(reader["PontuacaoOponente"].ToString());
                    objBatalha.Id = long.Parse(reader["Id"].ToString());
                    objBatalha.QuantidadeAtaques = int.Parse(reader["QtsAtaques"].ToString());

                    objRetorno.Add(objBatalha);

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

        public BatalhaModels ObterBatalha(long id)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();

            select.AppendLine("select Guilda,LIFE,DATA,PontuacaoOponente,PontuacaoGuild,RankGuild,idGuilda,Id,IdGuildaAtacante ");
            select.AppendLine(",(select  case when count(1) > 1 then 1 else 0 end from dbo.lutas b where b.CodBatalhas = a.ID and b.MomentoVitoria = 'Win') Vitoria ");
            select.AppendLine("from dbo.Batalhas a");
            select.AppendLine("where id = @id ");

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));
            command.Parameters["@id"].Value = id;

            try
            {
                BatalhaModels objBatalha = new BatalhaModels();

                conexao.Open();

                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                Guilda daGuilda = new Guilda();
                

                while (reader.Read())
                {
                    objBatalha = new BatalhaModels();
                    objBatalha.Data = Convert.ToDateTime(reader["Data"].ToString());
                    objBatalha.GuildaAtacante = daGuilda.ObterGuilda(long.Parse(reader["IdGuildaAtacante"].ToString()));
                    objBatalha.GuildaOponente = reader["Guilda"].ToString();
                    objBatalha.RankGuild = int.Parse(reader["RankGuild"].ToString());
                    objBatalha.Vitoria = Convert.ToBoolean(int.Parse(reader["Vitoria"].ToString()));
                    
                }

                conexao.Close();
                conexao.Dispose();

                return objBatalha;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
