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
                select.AppendLine("Isnull(Sum(x.total_vitorias), 0) Total_Vitorias, ");
                select.AppendLine("x.guildpoint Guild_Point, ");
                select.AppendLine("x.nagvg Na_Gvg ");
                select.AppendLine("FROM(SELECT A.codplayer, ");
                select.AppendLine("b.id,                              IdPlayer, ");
                select.AppendLine("A.vitoria                           Total_Vitorias, ");
                select.AppendLine("A.id                                Total_Batalhas, ");
                select.AppendLine("Stuff((SELECT ' | ' + CONVERT(VARCHAR(1), d.bonus) ");
                select.AppendLine("FROM   lutas C ");
                select.AppendLine("INNER JOIN playeroponente D ");
                select.AppendLine("ON C.codplayeroponente = D.id ");

                select.AppendLine("LEFT join Batalhas E ");

                select.AppendLine("on c.CodBatalhas = E.ID ");
                select.AppendLine("WHERE  Isnull(C.codplayer, '') = Isnull(A.codplayer, '') ");

                select.AppendLine("and E.ID = @idBatalha ");
                select.AppendLine("ORDER  BY d.bonus DESC ");
                select.AppendLine("FOR xml path('')), 1, 2, '') AS Bonus, ");
                select.AppendLine("C.bonus totalBonus, ");
                select.AppendLine("B.pontoarena GuildPoint, ");
                select.AppendLine("D.status NaGVG ");
                select.AppendLine("FROM player B ");
                select.AppendLine("LEFT JOIN lutas A ");
                select.AppendLine("ON A.codplayer = B.id and a.CodBatalhas = @idBatalha ");
                select.AppendLine("LEFT JOIN playeroponente C ");
                select.AppendLine("ON A.codplayeroponente = C.id ");

                select.AppendLine("Inner Join PlayerStatus D on B.ID = D.IdPlayer and IdBatalha = @idBatalha ");
                select.AppendLine(") X ");
                select.AppendLine("GROUP  BY x.IdPlayer,  ");
                select.AppendLine("x.bonus,  ");
                select.AppendLine("x.nagvg,  ");
                select.AppendLine("x.guildpoint ");
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

                    //objResultadoGeral.Derrota = int.Parse(reader["0"].ToString());
                    //objResultadoGeral.Empate = int.Parse(reader["0"].ToString());
                    objResultadoGeral.NumeroLutas = int.Parse(reader["Total_Batalhas"].ToString());
                    objResultadoGeral.Player = new DataAccess.Player().ObterPlayer(int.Parse(reader["IdPlayer"].ToString()));
                    objResultadoGeral.Vitoria = int.Parse(reader["Total_Vitorias"].ToString());
                    
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
