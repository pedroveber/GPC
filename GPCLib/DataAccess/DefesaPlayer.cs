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

                objRetorno.Defesas= new List<DefesaModels>();

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
                objRetorno.Defesas = objRetorno.Defesas.Where(x => x.Vitoria > 0 || x.Empate>0 || x.Derrota>0).ToList();

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

        public  List<DefesasPlayerConsolidado> ListarDefesaConsolidado(DateTime dataInicio, DateTime dataFim,int idPlayer)
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
                select.AppendLine("(select count(vitoria) from dbo.PlayerDefesas a where a.NomeGuilda = c.NOMEGUILDA and vitoria=2 and a.IdPlayer = c.idplayer)vitoria,");
                select.AppendLine("(select count(vitoria) from dbo.PlayerDefesas a where a.NomeGuilda = c.NOMEGUILDA and vitoria=1 and a.IdPlayer = c.idplayer)empate,");
                select.AppendLine("(select count(vitoria) from dbo.PlayerDefesas a where a.NomeGuilda = c.NOMEGUILDA and vitoria=0 and a.IdPlayer = c.idplayer)derrota");
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
    }
}
