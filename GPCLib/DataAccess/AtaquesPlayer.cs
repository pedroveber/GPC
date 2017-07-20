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
    public class AtaquesPlayer
    {
        public List<AtaquesPlayerConsolidado> ListarAtaqueConsolidado(DateTime dataInicio, DateTime dataFim, int idPlayer)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();

                select.AppendLine("SET DATEFORMAT dmy;");
                select.AppendLine("with tabela (CodPlayer,Guilda,CodBatalhas,Datab) as(");
                select.AppendLine("select a.CodPlayer,d.Guilda,a.CodBatalhas,cast(d.data as date) as Datab");
                select.AppendLine("from dbo.Lutas a");
                select.AppendLine("inner join dbo.Batalhas d on d.id = a.CodBatalhas");
                select.AppendLine("where 1=1");
                select.AppendLine("and a.codPlayer = @idplayer");
                select.AppendLine("and cast(d.data as date) >= @dataini");
                select.AppendLine("and cast(d.data as date) <= @datafim");
                select.AppendLine("group by a.CodPlayer,d.Guilda,a.CodBatalhas,d.data)");
                select.AppendLine("select tabela.* ");
                select.AppendLine(", (select count(Vitoria) from dbo.Lutas y where y.CodPlayer = tabela.CodPlayer and y.CodBatalhas = tabela.CodBatalhas and y.Vitoria = 2) Vitoria ");
                select.AppendLine(", (select count(Vitoria) from dbo.Lutas y where y.CodPlayer = tabela.CodPlayer and y.CodBatalhas = tabela.CodBatalhas and y.Vitoria = 1) Empate");
                select.AppendLine(", (select count(Vitoria) from dbo.Lutas y where y.CodPlayer = tabela.CodPlayer and y.CodBatalhas = tabela.CodBatalhas and y.Vitoria = 0) Derrota");
                select.AppendLine("from tabela");
                select.AppendLine("order by datab,CodBatalhas");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("@dataini", System.Data.SqlDbType.Date));
                command.Parameters["@dataini"].Value = dataInicio;

                command.Parameters.Add(new SqlParameter("@datafim", System.Data.SqlDbType.Date));
                command.Parameters["@datafim"].Value = dataFim;

                command.Parameters.Add(new SqlParameter("@idplayer", System.Data.SqlDbType.Int));
                command.Parameters["@idplayer"].Value = idPlayer;

                List<AtaquesPlayerConsolidado> objRetorno = new List<AtaquesPlayerConsolidado>();
                AtaquesPlayerConsolidado objAtaque;

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objAtaque = new AtaquesPlayerConsolidado();
                    objAtaque.Data = Convert.ToDateTime(reader["Datab"].ToString());
                    objAtaque.Derrota = Convert.ToInt32(reader["Derrota"].ToString());
                    objAtaque.Empate = Convert.ToInt32(reader["Empate"].ToString());
                    objAtaque.Guilda = reader["Guilda"].ToString();
                    objAtaque.Vitoria = Convert.ToInt32(reader["Vitoria"].ToString());

                    objRetorno.Add(objAtaque);

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
        public List<Models.LutasModels> ListarAtaques(int idPlayer)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();

                select.AppendLine("Select * from dbo.Lutas where CodPlayer = @idPlayer");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("@idplayer", System.Data.SqlDbType.Int));
                command.Parameters["@idplayer"].Value = idPlayer;

                List<LutasModels> objRetorno = new List<LutasModels>();
                LutasModels objLuta;

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objLuta = new LutasModels();
                    objLuta.DataHora = Convert.ToDateTime(reader["DataHora"].ToString());
                    objLuta.Id = Convert.ToInt32(reader["ID"].ToString());
                    objLuta.MomentoVitoria = reader["MomentoVitoria"].ToString();
                    objLuta.ValorBarra = Convert.ToInt32(reader["ValorBarra"].ToString());
                    objLuta.Vitoria = Convert.ToInt32(reader["Vitoria"].ToString());

                    objRetorno.Add(objLuta);

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

        public List<Models.AtaquesSemana> ListarAtaquesPorSemana(int idPlayer)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();

                select.AppendLine("");

                select.AppendLine("select");
                select.AppendLine("a.CodPlayer, DATEPART(wk, a.DataHora)     weekno,");
                select.AppendLine("isnull(b.Vitorias, 0) Vitorias,isnull(c.Empates, 0) Empates,isnull(d.Derrotas, 0)Derrotas");
                select.AppendLine("from");
                select.AppendLine("dbo.Lutas a");
                select.AppendLine("left join (");
                select.AppendLine("select");
                select.AppendLine("DATEPART(wk, DataHora) weekno,");
                select.AppendLine("count(Vitoria) Vitorias");
                select.AppendLine("from DB_SW.dbo.Lutas y where y.CodPlayer = @idPlayer and y.Vitoria = 2");
                select.AppendLine("GROUP BY    DATEPART(wk, DataHora)");
                select.AppendLine(") ");
                select.AppendLine("b on b.weekno = DATEPART(wk, a.DataHora)");
                select.AppendLine("left join (");
                select.AppendLine("select");
                select.AppendLine("DATEPART(wk, DataHora) weekno,");
                select.AppendLine("count(Vitoria) Empates");
                select.AppendLine("from DB_SW.dbo.Lutas y where y.CodPlayer = @idPlayer and y.Vitoria = 1");
                select.AppendLine("GROUP BY    DATEPART(wk, DataHora)");
                select.AppendLine(") ");
                select.AppendLine("c on c.weekno = DATEPART(wk, a.DataHora)");
                select.AppendLine("left join (");
                select.AppendLine("select");
                select.AppendLine("DATEPART(wk, DataHora) weekno,");
                select.AppendLine("count(Vitoria) Derrotas");
                select.AppendLine("from DB_SW.dbo.Lutas y where y.CodPlayer = @idPlayer and y.Vitoria = 0");
                select.AppendLine("GROUP BY    DATEPART(wk, DataHora)");
                select.AppendLine(") ");
                select.AppendLine("d on d.weekno = DATEPART(wk, a.DataHora)");
                select.AppendLine("where a.CodPlayer = @idPlayer");
                select.AppendLine("GROUP BY    a.CodPlayer,DATEPART(wk, a.DataHora),b.Vitorias,c.Empates,d.Derrotas");
                
                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("@idplayer", System.Data.SqlDbType.Int));
                command.Parameters["@idplayer"].Value = idPlayer;

                List<AtaquesSemana> objRetorno = new List<AtaquesSemana>();
                AtaquesSemana objAtaqueSemana;

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objAtaqueSemana = new AtaquesSemana();

                    objAtaqueSemana.Semana = Convert.ToInt32(reader["weekno"].ToString());
                    objAtaqueSemana.Vitorias = Convert.ToInt32(reader["Vitorias"].ToString());
                    objAtaqueSemana.Empates = Convert.ToInt32(reader["Empates"].ToString());
                    objAtaqueSemana.Derrotas = Convert.ToInt32(reader["Derrotas"].ToString());


                    objRetorno.Add(objAtaqueSemana);

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
