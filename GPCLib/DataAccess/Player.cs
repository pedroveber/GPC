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
    public class Player
    {
        public Models.PlayerModels ObterPlayer(int id)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT * FROM DB_SW.dbo.Player WHERE ID = @ID");


            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;
            sqlCom.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@ID"].Value = id;

            try
            {
                Models.PlayerModels objPlayer = new PlayerModels();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objPlayer = new Models.PlayerModels();
                    objPlayer.Id = Convert.ToInt32(reader["ID"].ToString());
                    objPlayer.Nome = reader["Nome"].ToString();
                    objPlayer.Level = Convert.ToInt32(reader["Level"].ToString());
                    objPlayer.PontoArena = Convert.ToInt32(reader["PontoArena"].ToString());
                    objPlayer.Ativo = (reader["Status"].ToString() == "S") ? true : false;

                }
                conn.Close();
                conn.Dispose();
                return objPlayer;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<Models.PlayerModels> ListarPlayers()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT * FROM dbo.Player WHERE Status = 'S' order by Nome");


            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;
            
            try
            {
                Models.PlayerModels objPlayer;
                List<Models.PlayerModels> objRetorno = new List<PlayerModels>();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objPlayer = new Models.PlayerModels();
                    objPlayer.Id = Convert.ToInt32(reader["ID"].ToString());
                    objPlayer.Nome = reader["Nome"].ToString();
                    objPlayer.Level = Convert.ToInt32(reader["Level"].ToString());
                    objPlayer.PontoArena = Convert.ToInt32(reader["PontoArena"].ToString());
                    objPlayer.Ativo = (reader["Status"].ToString() == "S") ? true : false;

                    objRetorno.Add(objPlayer);
                    
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

        public CapivaraModels ObterPlayerCapivara(int idPlayer)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select a.Nome,a.Imagem,");
            select.AppendLine("(select count(vitoria) from dbo.Lutas b where b.CodPlayer = a.id and  b.vitoria = 2) Vitorias,");
            select.AppendLine("(select count(vitoria) from dbo.Lutas c where c.CodPlayer = a.id and  c.vitoria = 1) Empates,");
            select.AppendLine("(select count(vitoria) from dbo.Lutas d where d.CodPlayer = a.id and  d.vitoria = 0) Derrotas,");
            select.AppendLine("(select count(d1.vitoria) from dbo.PlayerDefesas d1 where d1.vitoria = 2 and d1.IdPlayer = a.id) DefesaSucesso,");
            select.AppendLine("(select count(d2.vitoria) from dbo.PlayerDefesas d2 where d2.vitoria = 1 and d2.IdPlayer = a.id) DefesaEmpate,");
            select.AppendLine("(select count(d3.vitoria) from dbo.PlayerDefesas d3 where d3.vitoria = 0 and d3.IdPlayer = a.id ) DefesaDerrota,");
            select.AppendLine("(select count(1) from ");
            select.AppendLine("DB_SW.dbo.PlayerStatus e ");
            select.AppendLine("inner join DB_SW.dbo.Batalhas f on f.id = e.idBatalha");
            select.AppendLine("where ");
            select.AppendLine("e.idPlayer = a.id and e.Status = 'S') Escalado,");
            select.AppendLine("(select count(1) from ");
            select.AppendLine("DB_SW.dbo.PlayerStatus g ");
            select.AppendLine("inner join DB_SW.dbo.Batalhas h on h.id = g.idBatalha");
            select.AppendLine("where g.idPlayer = a.id and g.Status = 'S'");
            select.AppendLine("and not exists (select 0 from DB_SW.dbo.Lutas i where i.CodPlayer = g.IdPlayer and i.CodBatalhas = g.IdBatalha))NAtacou");
            select.AppendLine("from dbo.Player a");
            select.AppendLine("where ");
            select.AppendLine("a.id = @idPlayer");

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            command.Parameters.Add(new SqlParameter("@idPlayer", System.Data.SqlDbType.Int));
            command.Parameters["@idPlayer"].Value = idPlayer;

            CapivaraModels objRetorno = new CapivaraModels();

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objRetorno = new CapivaraModels();
                objRetorno.Player = new Player().ObterPlayer(idPlayer);
                objRetorno.PercentualDefesa = 0;
                objRetorno.PercentualParticipacao = 0;
                objRetorno.PercentualVitoria = 0;
                objRetorno.Vitorias = Convert.ToInt32(reader["Vitorias"].ToString());
                objRetorno.Empates = Convert.ToInt32(reader["Empates"].ToString());
                objRetorno.Derrotas = Convert.ToInt32(reader["Derrotas"].ToString());
                objRetorno.Escalado = Convert.ToInt32(reader["Escalado"].ToString());
                objRetorno.NAtacou = Convert.ToInt32(reader["NAtacou"].ToString());
                objRetorno.DefesaVitorias = Convert.ToInt32(reader["DefesaSucesso"].ToString()); 
                objRetorno.DefesaEmpates  = Convert.ToInt32(reader["DefesaEmpate"].ToString()); 
                objRetorno.DefesaDerrotas = Convert.ToInt32(reader["DefesaDerrota"].ToString());
                objRetorno.Imagem = reader["Imagem"].ToString();

            }

            //Ataques
            if (objRetorno.Vitorias > 0 || objRetorno.Empates > 0 || objRetorno.Derrotas > 0)
            {
                objRetorno.PercentualVitoria = Math.Round((Convert.ToDouble(objRetorno.Vitorias) / (objRetorno.Vitorias + objRetorno.Empates + objRetorno.Derrotas) * 100));
            }

            //Defesas
            if (objRetorno.DefesaVitorias > 0 || objRetorno.DefesaEmpates > 0 || objRetorno.DefesaDerrotas > 0)
            {
                objRetorno.PercentualDefesa = Math.Round((Convert.ToDouble(objRetorno.DefesaVitorias) / (objRetorno.DefesaVitorias + objRetorno.DefesaEmpates + objRetorno.DefesaDerrotas) * 100));
            }

            //Escalado
            if (objRetorno.Escalado > 0)
            {
                objRetorno.PercentualParticipacao = Math.Round(((objRetorno.Escalado - objRetorno.NAtacou) / Convert.ToDouble(objRetorno.Escalado)) * 100);
            }

            return objRetorno;

        }

    }
}
