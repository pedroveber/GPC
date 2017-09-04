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
    public class DefesaPlayer
    {
        public DefesaSemanaModels ListarDefesasSemana(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();

                select.AppendLine("SET DATEFORMAT dmy");
                select.AppendLine("select a.Nome,a.ID,");

                select.AppendLine("(select count(b.vitoria) from dbo.PlayerDefesas b where b.vitoria = 2 and b.IdPlayer = a.ID and");
                select.AppendLine("cast(dataHora as date) between Cast(@dataini as date) and Cast(@datafim as date)) Vitoria,");

                select.AppendLine("(select count(c.vitoria) from dbo.PlayerDefesas c where c.vitoria = 1 and c.IdPlayer = a.ID and");
                select.AppendLine("cast(dataHora as date) between Cast(@dataini as date) and Cast(@datafim as date)) Empate,");

                select.AppendLine("(select count(d.vitoria) from dbo.PlayerDefesas d where d.vitoria = 0 and d.IdPlayer = a.ID and");
                select.AppendLine("cast(dataHora as date) between Cast(@dataini as date) and Cast(@datafim as date)) Derrota");
                select.AppendLine("from dbo.Player a ");
                select.AppendLine("order by 3 desc,4 desc,5 desc");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("@dataini", System.Data.SqlDbType.Date));
                command.Parameters["@dataini"].Value = dataInicio;

                command.Parameters.Add(new SqlParameter("@datafim", System.Data.SqlDbType.Date));
                command.Parameters["@datafim"].Value = dataFim;

                //coly
                DefesaModels objDefesa;
                DefesaSemanaModels objRetorno = new DefesaSemanaModels();

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                objRetorno.Defesas = new List<DefesaModels>();

                while (reader.Read())
                {
                    objDefesa = new DefesaModels();
                    objDefesa.Player = new Player().ObterPlayer(Convert.ToInt32(reader["ID"].ToString()));
                    objDefesa.Vitoria = Convert.ToInt32(reader["Vitoria"].ToString());
                    objDefesa.Empate = Convert.ToInt32(reader["Empate"].ToString());
                    objDefesa.Derrota = Convert.ToInt32(reader["Derrota"].ToString());

                    objRetorno.Defesas.Add(objDefesa);

                }

                //Retornar somente os que tem pelo menos 1 em um campo.
                objRetorno.Defesas = objRetorno.Defesas.Where(x => x.Vitoria > 0 || x.Empate > 0 || x.Derrota > 0).ToList();

                conexao.Close();
                conexao.Dispose();

                objRetorno.DataInicio = dataInicio;
                objRetorno.DataFim = dataFim;

                return objRetorno;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<DefesasPlayerConsolidado> ListarDefesaConsolidado(DateTime dataInicio, DateTime dataFim, int idPlayer)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();

                select.AppendLine("SET DATEFORMAT dmy;");
                select.AppendLine("with teste (idplayer,nome, data,vitoria,empate,derrota)");
                select.AppendLine("as(");
                select.AppendLine("select idplayer,NOMEGUILDA,");
                select.AppendLine("dataHora data,");
                select.AppendLine("(select count(vitoria) from dbo.PlayerDefesas a where a.NomeGuilda = c.NOMEGUILDA and vitoria=2 and a.IdPlayer = c.idplayer and convert(date,a.DataHora) = convert(date,c.DataHora))vitoria,");
                select.AppendLine("(select count(vitoria) from dbo.PlayerDefesas a where a.NomeGuilda = c.NOMEGUILDA and vitoria=1 and a.IdPlayer = c.idplayer and convert(date,a.DataHora) = convert(date,c.DataHora))empate,");
                select.AppendLine("(select count(vitoria) from dbo.PlayerDefesas a where a.NomeGuilda = c.NOMEGUILDA and vitoria=0 and a.IdPlayer = c.idplayer and convert(date,a.DataHora) = convert(date,c.DataHora))derrota");
                select.AppendLine("from dbo.PlayerDefesas c");
                select.AppendLine("where idplayer = @idplayer");
                select.AppendLine(")");
                select.AppendLine("select ");
                select.AppendLine("Nome,vitoria,empate,derrota,");
                select.AppendLine("CAST(data AS DATE)as data2");
                select.AppendLine("from teste");
                select.AppendLine("where data >= @dataini");
                select.AppendLine("and data <= @datafim");
                select.AppendLine("group by Nome,vitoria,empate,derrota,CAST(data AS DATE)  ");
                select.AppendLine("order by 5");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("@dataini", System.Data.SqlDbType.Date));
                command.Parameters["@dataini"].Value = dataInicio;

                command.Parameters.Add(new SqlParameter("@datafim", System.Data.SqlDbType.Date));
                command.Parameters["@datafim"].Value = dataFim;

                command.Parameters.Add(new SqlParameter("@idplayer", System.Data.SqlDbType.Int));
                command.Parameters["@idplayer"].Value = idPlayer;

                List<DefesasPlayerConsolidado> objRetorno = new List<DefesasPlayerConsolidado>();
                DefesasPlayerConsolidado objDefesa;

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objDefesa = new DefesasPlayerConsolidado();
                    objDefesa.Data = Convert.ToDateTime(reader["data2"].ToString());
                    objDefesa.Derrota = Convert.ToInt32(reader["derrota"].ToString());
                    objDefesa.Empate = Convert.ToInt32(reader["empate"].ToString());
                    objDefesa.NomeGuild = reader["Nome"].ToString();
                    objDefesa.Vitoria = Convert.ToInt32(reader["vitoria"].ToString());

                    objRetorno.Add(objDefesa);

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
        public List<Models.DefesasSemana> ListarDefesasPorSemana(int idPlayer)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();

                select.AppendLine("");

                select.AppendLine("select");
                select.AppendLine("a.IdPlayer, DATEPART(wk, a.DataHora)     weekno,");
                select.AppendLine("isnull(b.Vitorias, 0) Vitorias,isnull(c.Empates, 0) Empates,isnull(d.Derrotas, 0)Derrotas");
                select.AppendLine("from");
                select.AppendLine("dbo.PlayerDefesas a");
                select.AppendLine("left join (");
                select.AppendLine("select");
                select.AppendLine("  DATEPART(wk, DataHora) weekno,");
                select.AppendLine("count(Vitoria) Vitorias");
                select.AppendLine("from dbo.PlayerDefesas y where y.IdPlayer = @idplayer and y.Vitoria = 2");
                select.AppendLine("GROUP BY    DATEPART(wk, DataHora)");
                select.AppendLine(") ");
                select.AppendLine("b on b.weekno = DATEPART(wk, a.DataHora)");
                select.AppendLine("left join (");
                select.AppendLine("            select");
                select.AppendLine("DATEPART(wk, DataHora) weekno,");
                select.AppendLine("count(Vitoria) Empates");
                select.AppendLine("             from dbo.PlayerDefesas y where y.IdPlayer = @idplayer and y.Vitoria = 1");
                select.AppendLine("GROUP BY    DATEPART(wk, DataHora)");
                select.AppendLine(") ");
                select.AppendLine("c on c.weekno = DATEPART(wk, a.DataHora)");
                select.AppendLine("left join (");
                select.AppendLine("select");
                select.AppendLine("            DATEPART(wk, DataHora) weekno,");
                select.AppendLine("count(Vitoria) Derrotas");
                select.AppendLine("             from dbo.PlayerDefesas y where y.IdPlayer = @idplayer and y.Vitoria = 0");
                select.AppendLine("GROUP BY    DATEPART(wk, DataHora)");
                select.AppendLine(") ");
                select.AppendLine("d on d.weekno = DATEPART(wk, a.DataHora)");
                select.AppendLine("where a.IdPlayer = @idplayer");
                select.AppendLine("GROUP BY    a.IdPlayer,DATEPART(wk, a.DataHora),b.Vitorias,c.Empates,d.Derrotas");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SqlParameter("@idplayer", System.Data.SqlDbType.Int));
                command.Parameters["@idplayer"].Value = idPlayer;

                List<DefesasSemana> objRetorno = new List<DefesasSemana>();
                DefesasSemana objDefesaSemana;

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objDefesaSemana = new DefesasSemana();

                    objDefesaSemana.Semana = Convert.ToInt32(reader["weekno"].ToString());
                    objDefesaSemana.Vitorias = Convert.ToInt32(reader["Vitorias"].ToString());
                    objDefesaSemana.Empates = Convert.ToInt32(reader["Empates"].ToString());
                    objDefesaSemana.Derrotas = Convert.ToInt32(reader["Derrotas"].ToString());


                    objRetorno.Add(objDefesaSemana);

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

        public TimeDefesaModels ObterTimeDefesaGVG(int idPlayer, DateTime data)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();

                select.AppendLine("SET DATEFORMAT dmy;");
                select.AppendLine("select");
                select.AppendLine("a.Id,");
                select.AppendLine("(select count(1) from dbo.PlayerDefesas b where a.ID = b.IdPlayer and Vitoria = 2 and DataHora>= @datainicio and DataHora<= @datafim)Vitoria,");
                select.AppendLine("(select count(1) from dbo.PlayerDefesas c where a.ID = c.IdPlayer and Vitoria = 1 and DataHora>= @datainicio and DataHora<= @datafim)Empate,");
                select.AppendLine("(select count(1) from dbo.PlayerDefesas d where a.ID = d.IdPlayer and Vitoria = 0 and DataHora>= @datainicio and DataHora<= @datafim)Derrota,");
                select.AppendLine("e.Monstro1 Monstro1Id, f.Nome Monstro1Nome, f.Imagem Monstro1Imagem,");
                select.AppendLine("e.Monstro2 Monstro2Id, g.Nome Monstro2Nome, g.Imagem Monstro2Imagem,");
                select.AppendLine("e.Monstro3 Monstro3Id, h.Nome Monstro3Nome, h.Imagem Monstro3Imagem,");
                select.AppendLine("e.Monstro4 Monstro4Id, i.Nome Monstro4Nome, i.Imagem Monstro4Imagem,");
                select.AppendLine("e.Monstro5 Monstro5Id, j.Nome Monstro5Nome, j.Imagem Monstro5Imagem,");
                select.AppendLine("e.Monstro6 Monstro6Id, k.Nome Monstro6Nome, k.Imagem Monstro6Imagem");
                select.AppendLine("from dbo.Player a");
                select.AppendLine("inner");
                select.AppendLine("join dbo.TimeDefesa e on e.IdPlayer = a.ID");
                select.AppendLine("left");
                select.AppendLine("join dbo.Monstro f on f.Id = e.Monstro1");
                select.AppendLine("left");
                select.AppendLine("join dbo.Monstro g on g.Id = e.Monstro2");
                select.AppendLine("left");
                select.AppendLine("join dbo.Monstro h on h.Id = e.Monstro3");
                select.AppendLine("left");
                select.AppendLine("join dbo.Monstro i on i.Id = e.Monstro4");
                select.AppendLine("left");
                select.AppendLine("join dbo.Monstro j on j.Id = e.Monstro5");
                select.AppendLine("left");
                select.AppendLine("join dbo.Monstro k on k.Id = e.Monstro6");
                select.AppendLine("where a.Id = @idplayer");
                select.AppendLine("and e.data = @datafim");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                //Parametros
                command.Parameters.Add(new SqlParameter("@idplayer", System.Data.SqlDbType.Int));
                command.Parameters["@idplayer"].Value = idPlayer;

                command.Parameters.Add(new SqlParameter("@datainicio", System.Data.SqlDbType.Date));
                command.Parameters["@datainicio"].Value = data.AddDays(-7);

                command.Parameters.Add(new SqlParameter("@datafim", System.Data.SqlDbType.Date));
                command.Parameters["@datafim"].Value = data;

                TimeDefesaModels objTimeDefesa = new TimeDefesaModels();

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objTimeDefesa = new TimeDefesaModels();
                    objTimeDefesa.Data = data;
                    objTimeDefesa.Monstro1 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro1Id"].ToString()), Nome = reader["Monstro1Nome"].ToString(), Imagem = reader["Monstro1Imagem"].ToString() };
                    objTimeDefesa.Monstro2 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro2Id"].ToString()), Nome = reader["Monstro2Nome"].ToString(), Imagem = reader["Monstro2Imagem"].ToString() };
                    objTimeDefesa.Monstro3 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro3Id"].ToString()), Nome = reader["Monstro3Nome"].ToString(), Imagem = reader["Monstro3Imagem"].ToString() };
                    objTimeDefesa.Monstro4 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro4Id"].ToString()), Nome = reader["Monstro4Nome"].ToString(), Imagem = reader["Monstro4Imagem"].ToString() };
                    objTimeDefesa.Monstro5 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro5Id"].ToString()), Nome = reader["Monstro5Nome"].ToString(), Imagem = reader["Monstro5Imagem"].ToString() };
                    objTimeDefesa.Monstro6 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro6Id"].ToString()), Nome = reader["Monstro6Nome"].ToString(), Imagem = reader["Monstro6Imagem"].ToString() };
                    objTimeDefesa.Vitoria = Convert.ToInt32(reader["Vitoria"].ToString());
                    objTimeDefesa.Empate = Convert.ToInt32(reader["Empate"].ToString());
                    objTimeDefesa.Derrota = Convert.ToInt32(reader["Derrota"].ToString());
                }

                conexao.Close();
                conexao.Dispose();

                return objTimeDefesa;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
