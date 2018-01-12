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
                select.AppendLine("b.id,d.IdPlayer, ");
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
                select.AppendLine("inner join Guilda_Player e on e.IdPlayer = b.ID and e.Ativo = 1 ");
                select.AppendLine("where b.Status = 'S' ");
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

        public ResumoGuildaModels ListarResumoGuilda(int idGuildaAtacante, int idGuilda)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();

                select.AppendLine("select count(a.id) QtdGVGs,g.id,g.nome ");
                select.AppendLine(",(select count(1) from dbo.lutas z ");
                select.AppendLine("where z.CodBatalhas in (select y.id from dbo.Batalhas y where y.idGuilda = @idGuilda and y.IdGuildaAtacante = @idGuildaAtacante)  ");
                select.AppendLine("and z.MomentoVitoria = 'Win' ");
                select.AppendLine("and z.DataHora = (select max(c.DataHora) from dbo.Lutas c where c.CodBatalhas = z.CodBatalhas and c.MomentoVitoria = 'Win'))VitoriaGvg, ");
                select.AppendLine("(select count(l.MomentoVitoria) ");
                select.AppendLine("from dbo.lutas l ");
                select.AppendLine("where l.CodBatalhas in (select y.id from dbo.Batalhas y where y.idGuilda = @idGuilda and y.IdGuildaAtacante = @idGuildaAtacante)  ");
                select.AppendLine("and not exists(select 0 from dbo.Lutas b where b.CodBatalhas = l.CodBatalhas and MomentoVitoria = 'Win') ");
                select.AppendLine("and l.DataHora = (select max(c.DataHora) from dbo.Lutas c where c.CodBatalhas = l.CodBatalhas) ");
                select.AppendLine(") DerrotaGVG, ");
                select.AppendLine("(select ");
                select.AppendLine("count(1) qtd from ");
                select.AppendLine("dbo.SiegeGuilda s ");
                select.AppendLine("inner join dbo.Guilda b  on b.Id = s.IdGuilda ");
                select.AppendLine("where s.IdGuilda = @idGuilda ");
                select.AppendLine("group by s.IdGuilda) Siege, ");
                select.AppendLine("(select count(1) from dbo.SiegeGuilda s1 where s1.IdGuilda = g.id and posicao = 1) as Posicao1, ");
                select.AppendLine("(select count(1) from dbo.SiegeGuilda s2 where s2.IdGuilda = g.id and posicao = 2) as Posicao2, ");
                select.AppendLine("(select count(1) from dbo.SiegeGuilda s3 where s3.IdGuilda = g.id and posicao = 3) as Posicao3 ");
                select.AppendLine("from dbo.Guilda g ");
                select.AppendLine("left join dbo.Batalhas a on a.idGuilda = g.id and a.IdGuildaAtacante = @idGuildaAtacante ");
                select.AppendLine("where 1 = 1 ");
                select.AppendLine("and g.id = @idGuilda ");
                select.AppendLine("group by g.id,g.Nome ");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("@idGuildaAtacante", System.Data.SqlDbType.Int));
                command.Parameters["@idGuildaAtacante"].Value = idGuildaAtacante;

                command.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.Int));
                command.Parameters["@idGuilda"].Value = idGuilda;


                ResumoGuildaModels objRetorno = new ResumoGuildaModels();
                objRetorno.Guilda = new GuildaModels();


                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objRetorno.Guilda = new GuildaModels()
                    {
                        Id = long.Parse(reader["id"].ToString()),
                        Nome = reader["Nome"].ToString()
                    };
                    objRetorno.QtdGVGs = int.Parse(reader["QtdGVGs"].ToString());
                    objRetorno.DerrotaGVG = int.Parse(reader["DerrotaGVG"].ToString());
                    objRetorno.VitoriaGvg = int.Parse(reader["VitoriaGvg"].ToString());
                    
                    if (!string.IsNullOrEmpty(reader["Siege"].ToString()))
                    {
                        objRetorno.Siege = int.Parse(reader["Siege"].ToString());
                        objRetorno.SiegePosicao1 = int.Parse(reader["Posicao1"].ToString());
                        objRetorno.SiegePosicao2 = int.Parse(reader["Posicao2"].ToString());
                        objRetorno.SiegePosicao3 = int.Parse(reader["Posicao3"].ToString());
                    }
                    

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
