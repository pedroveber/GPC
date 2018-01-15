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

        public List<TimeDefesaConsolidadoModels> ListarDefesasGVGConsolidado(long idPlayer)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();


                select.AppendLine("with Defesas(IdPlayer, monstro1, monstro2, monstro3, monstro4, monstro5, monstro6, Vitoria, Empate, Derrota) ");
                select.AppendLine("as (select ");
                select.AppendLine("a.IdPlayer,a.monstro1,a.monstro2,a.monstro3,a.monstro4,a.monstro5,a.monstro6, ");
                select.AppendLine("(select COUNT(1) from dbo.PlayerDefesas b where b.IdPlayer = a.IdPlayer ");
                select.AppendLine("-- >= Segunda ");
                select.AppendLine("and b.DataHora >= DATEADD(wk, DATEDIFF(wk, 0,case DATEPART(dw, a.data) when 1 then dateadd(d, -1, a.data) else a.data end), 0) ");
                select.AppendLine("-- <= Domingo ");
                select.AppendLine("and b.DataHora <= DATEADD(d, 6, DATEADD(wk, DATEDIFF(wk, 0,case DATEPART(dw, a.data) when 1 then dateadd(d, -1, a.data) else a.data end), 0)) ");
                select.AppendLine("and b.Vitoria = 1 ");
                select.AppendLine(")Vitoria, ");
                select.AppendLine("(select COUNT(1) from dbo.PlayerDefesas b where b.IdPlayer = a.IdPlayer ");
                select.AppendLine("-- >= Segunda ");
                select.AppendLine("and b.DataHora >= DATEADD(wk, DATEDIFF(wk, 0,case DATEPART(dw, a.data) when 1 then dateadd(d, -1, a.data) else a.data end), 0) ");
                select.AppendLine("-- <= Domingo ");
                select.AppendLine("and b.DataHora <= DATEADD(d, 6, DATEADD(wk, DATEDIFF(wk, 0,case DATEPART(dw, a.data) when 1 then dateadd(d, -1, a.data) else a.data end), 0)) ");
                select.AppendLine("and b.Vitoria = 0 ");
                select.AppendLine(")Empate, ");
                select.AppendLine("(select COUNT(1) from dbo.PlayerDefesas b where b.IdPlayer = a.IdPlayer ");
                select.AppendLine("-- >= Segunda ");
                select.AppendLine("and b.DataHora >= DATEADD(wk, DATEDIFF(wk, 0,case DATEPART(dw, a.data) when 1 then dateadd(d, -1, a.data) else a.data end), 0) ");
                select.AppendLine("-- <= Domingo ");
                select.AppendLine("and b.DataHora <= DATEADD(d, 6, DATEADD(wk, DATEDIFF(wk, 0,case DATEPART(dw, a.data) when 1 then dateadd(d, -1, a.data) else a.data end), 0)) ");
                select.AppendLine("and b.Vitoria = 2 ");
                select.AppendLine(")Derrota ");
                select.AppendLine("from dbo.TimeDefesa a where 1 = 1 ");
                //select.AppendLine("and a.IdPlayer = @idPlayer ");
                select.AppendLine("and Data = (select MAX(td.data) from dbo.TimeDefesa td where td.IdPlayer = a.IdPlayer ");
                select.AppendLine("and td.Monstro1 = a.Monstro1 ");
                select.AppendLine("and td.Monstro2 = a.Monstro2 ");
                select.AppendLine("and td.Monstro3 = a.Monstro3 ");
                select.AppendLine("and td.Monstro4 = a.Monstro4 ");
                select.AppendLine("and td.Monstro5 = a.Monstro5 ");
                select.AppendLine("and td.Monstro6 = a.Monstro6 ");
                select.AppendLine("and DATEADD(wk, DATEDIFF(wk,0,case DATEPART(dw, td.data) when 1 then dateadd(d,-1,td.data) else td.data end ), 0) = DATEADD(wk, DATEDIFF(wk, 0,case DATEPART(dw, a.data) when 1 then dateadd(d, -1, a.data) else a.data end), 0) ");
                select.AppendLine("and DATEADD(d,6,DATEADD(wk, DATEDIFF(wk, 0,case DATEPART(dw, td.data) when 1 then dateadd(d, -1, td.data) else td.data end), 0) ) = DATEADD(d, 6, DATEADD(wk, DATEDIFF(wk, 0,case DATEPART(dw, a.data) when 1 then dateadd(d, -1, a.data) else a.data end), 0)) ");
                select.AppendLine(")) ");
                select.AppendLine("select a.idPlayer,p.Nome, ");
                select.AppendLine("sum(a.Vitoria)Vitoria,sum(a.Empate)Empate,sum(a.Derrota)Derrota ");
                select.AppendLine(", a.Monstro1 IdMonstro1, b.Nome NomeMonstro1, b.imagem ImagemMonstro1, ");
                select.AppendLine("a.Monstro2 IdMonstro2, c.Nome NomeMonstro2, c.imagem ImagemMonstro2, ");
                select.AppendLine("a.Monstro3 IdMonstro3, d.Nome NomeMonstro3, d.imagem ImagemMonstro3, ");
                select.AppendLine("a.Monstro4 IdMonstro4, e.Nome NomeMonstro4, e.imagem ImagemMonstro4, ");
                select.AppendLine("a.Monstro5 IdMonstro5, f.Nome NomeMonstro5, f.imagem ImagemMonstro5, ");
                select.AppendLine("a.Monstro6 IdMonstro6, g.Nome NomeMonstro6, g.imagem ImagemMonstro6 ");
                select.AppendLine("from Defesas a ");
                select.AppendLine("inner join dbo.Player p on p.ID = a.IdPlayer ");
                select.AppendLine("inner ");
                select.AppendLine("join dbo.Monstro b on b.Id = a.Monstro1 ");
                select.AppendLine("inner join dbo.Monstro c on c.Id = a.Monstro2 ");
                select.AppendLine("inner join dbo.Monstro d on d.Id = a.Monstro3 ");
                select.AppendLine("inner join dbo.Monstro e on e.Id = a.Monstro4 ");
                select.AppendLine("inner join dbo.Monstro f on f.Id = a.Monstro5 ");
                select.AppendLine("inner join dbo.Monstro g on g.Id = a.Monstro6 ");
                select.AppendLine("group by ");
                select.AppendLine("a.idPlayer,p.Nome ");
                select.AppendLine(", a.Monstro1 ,b.Nome ,b.imagem , ");
                select.AppendLine("a.Monstro2 ,c.Nome ,c.imagem , ");
                select.AppendLine("a.Monstro3 ,d.Nome ,d.imagem , ");
                select.AppendLine("a.Monstro4 ,e.Nome ,e.imagem , ");
                select.AppendLine("a.Monstro5 ,f.Nome ,f.imagem , ");
                select.AppendLine("a.Monstro6 ,g.Nome ,g.imagem ");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;


                command.Parameters.Add(new SqlParameter("@idPlayer", System.Data.SqlDbType.BigInt));
                command.Parameters["@idPlayer"].Value = idPlayer;

                List<TimeDefesaConsolidadoModels> objRetorno = new List<TimeDefesaConsolidadoModels>();
                TimeDefesaConsolidadoModels objItem;

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objItem = new TimeDefesaConsolidadoModels();
                    objItem.Player = new PlayerModels()
                    {
                        Id = int.Parse(reader["idPlayer"].ToString()),
                        Nome = reader["Nome"].ToString()
                    };

                    objItem.Derrota = int.Parse(reader["Derrota"].ToString());
                    objItem.Empate = int.Parse(reader["Empate"].ToString());
                    objItem.Vitoria = int.Parse(reader["Vitoria"].ToString());

                    objItem.Monstro1 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro1"].ToString()),
                        Imagem = reader["ImagemMonstro1"].ToString(),
                        Nome = reader["NomeMonstro1"].ToString()
                    };

                    objItem.Monstro2 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro2"].ToString()),
                        Imagem = reader["ImagemMonstro2"].ToString(),
                        Nome = reader["NomeMonstro2"].ToString()
                    };

                    objItem.Monstro3 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro3"].ToString()),
                        Imagem = reader["ImagemMonstro3"].ToString(),
                        Nome = reader["NomeMonstro3"].ToString()
                    };

                    objItem.Monstro4 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro4"].ToString()),
                        Imagem = reader["ImagemMonstro4"].ToString(),
                        Nome = reader["NomeMonstro4"].ToString()
                    };

                    objItem.Monstro5 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro5"].ToString()),
                        Imagem = reader["ImagemMonstro5"].ToString(),
                        Nome = reader["NomeMonstro5"].ToString()
                    };

                    objItem.Monstro6 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro6"].ToString()),
                        Imagem = reader["ImagemMonstro6"].ToString(),
                        Nome = reader["NomeMonstro6"].ToString()
                    };

                    objRetorno.Add(objItem);
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
