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
    public class Relatorios
    {
        public List<Models.ResultadoGeralItensModels> ListarResultadoGeral(long idBatalha)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();


                select.AppendLine("SET DATEFORMAT dmy; ");
                select.AppendLine("SELECT x.IdPlayer, ");
                select.AppendLine("Sum(X.totalbonus)                Total_Bonus, ");
                select.AppendLine("Count(X.total_batalhas)          Total_Batalhas, ");
                select.AppendLine("x.Vitorias,x.Empates,x.Derrotas, ");
                select.AppendLine("x.guildpoint Guild_Point, ");
                select.AppendLine("x.nagvg Na_Gvg ");
                select.AppendLine("FROM(SELECT A.codplayer, ");
                select.AppendLine("b.id,                              IdPlayer, ");
                select.AppendLine("(select count(Vitoria) from DB_SW.dbo.Lutas y where y.CodBatalhas = @idBatalha and y.CodPlayer = b.id and y.Vitoria = 2) Vitorias, ");
                select.AppendLine("(select count(Vitoria) from DB_SW.dbo.Lutas y where y.CodBatalhas = @idBatalha and y.CodPlayer = b.id and y.Vitoria = 1) Empates, ");
                select.AppendLine("(select count(Vitoria) from DB_SW.dbo.Lutas y where y.CodBatalhas = @idBatalha and y.CodPlayer = b.id and y.Vitoria = 0) Derrotas, ");
                select.AppendLine("A.id                                Total_Batalhas, ");
                
                select.AppendLine("C.bonus totalBonus, ");
                select.AppendLine("B.pontoarena GuildPoint, ");
                select.AppendLine("D.status NaGVG ");
                select.AppendLine("FROM player B ");
                select.AppendLine("LEFT JOIN lutas A ");
                select.AppendLine("ON A.codplayer = B.id and a.CodBatalhas = @idBatalha ");
                select.AppendLine("LEFT JOIN playeroponente C ");
                select.AppendLine("ON A.codplayeroponente = C.id and c.IdBatalha= a.CodBatalhas");

                select.AppendLine("Inner Join PlayerStatus D on B.ID = D.IdPlayer and d.IdBatalha = @idBatalha ");
                select.AppendLine(") X ");
                select.AppendLine("GROUP  BY x.IdPlayer,  ");
                select.AppendLine("x.nagvg,  ");
                select.AppendLine("x.guildpoint,x.Vitorias,x.Empates,x.Derrotas ");
                select.AppendLine("ORDER  BY x.nagvg DESC, ");
                select.AppendLine("x.guildpoint DESC ");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("@idBatalha", System.Data.SqlDbType.BigInt));
                command.Parameters["@idBatalha"].Value = idBatalha;


                List<ResultadoGeralItensModels> objRetorno = new List<ResultadoGeralItensModels>();
                ResultadoGeralItensModels objResultadoGeral;

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objResultadoGeral = new ResultadoGeralItensModels();
                    if (reader["Total_Bonus"].ToString() != string.Empty)
                    objResultadoGeral.Bonus = int.Parse(reader["Total_Bonus"].ToString());
                    objResultadoGeral.NumeroLutas = int.Parse(reader["Total_Batalhas"].ToString());
                    objResultadoGeral.Player = new DataAccess.Player().ObterPlayer(int.Parse(reader["IdPlayer"].ToString()));
                    objResultadoGeral.Vitoria = int.Parse(reader["Vitorias"].ToString());
                    objResultadoGeral.Empate = int.Parse(reader["Empates"].ToString());
                    objResultadoGeral.Derrota = int.Parse(reader["Derrotas"].ToString());

                    objRetorno.Add(objResultadoGeral);

                }

                conexao.Close();
                conexao.Dispose();

                return objRetorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
