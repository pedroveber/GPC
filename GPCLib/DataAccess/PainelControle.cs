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
    public class PainelControle
    {
        public PainelControleModels ListaConsolidada(DateTime inicioSemana, DateTime fimSemana)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy");
            select.AppendLine("select pl.Nome,pl.Id, ");
            select.AppendLine("(select count(1) from ");
            select.AppendLine("DB_SW.dbo.PlayerStatus a ");
            select.AppendLine("inner ");
            select.AppendLine("join DB_SW.dbo.Batalhas b on b.id = a.idBatalha ");
            select.AppendLine("where ");
            select.AppendLine("a.idPlayer = pl.id and a.Status = 'S' ");
            select.AppendLine("and cast(b.data as date) >= @InicioSemana ");
            select.AppendLine("and cast(b.data as date) <= @FimSemana ");
            select.AppendLine("");
            select.AppendLine(") Escalado, ");
            select.AppendLine("");
            select.AppendLine("( ");
            select.AppendLine("select count(1) from ");
            select.AppendLine("DB_SW.dbo.PlayerStatus a ");
            select.AppendLine("inner ");
            select.AppendLine("join DB_SW.dbo.Batalhas c on c.id = a.idBatalha ");
            select.AppendLine("where a.idPlayer = pl.id and a.Status = 'S' ");
            select.AppendLine("and not exists(select 0 from DB_SW.dbo.Lutas b where b.CodPlayer = a.IdPlayer and b.CodBatalhas = a.IdBatalha) ");
            select.AppendLine("and cast(c.data as date) >= @InicioSemana ");
            select.AppendLine("and cast(c.data as date) <= @FimSemana ");
            select.AppendLine(")NAtacou ");
            select.AppendLine(", (select count(Vitoria) from DB_SW.dbo.Lutas y where y.CodPlayer = pl.id and y.Vitoria = 2 and cast(y.DataHora as date) >= @InicioSemana and cast(y.DataHora as date) <= @FimSemana ) Vitoria ");
            select.AppendLine(", (select count(Vitoria) from DB_SW.dbo.Lutas y where y.CodPlayer = pl.id and y.Vitoria = 1 and cast(y.DataHora as date) >= @InicioSemana and cast(y.DataHora as date) <= @FimSemana) Empate ");
            select.AppendLine(", (select count(Vitoria) from DB_SW.dbo.Lutas y where y.CodPlayer = pl.id and y.Vitoria = 0 and cast(y.DataHora as date) >= @InicioSemana and cast(y.DataHora as date) <= @FimSemana) Derrota ");
            select.AppendLine("from DB_SW.dbo.Player pl ");

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            command.Parameters.Add(new SqlParameter("@InicioSemana", System.Data.SqlDbType.Date));
            command.Parameters["@InicioSemana"].Value = inicioSemana;

            command.Parameters.Add(new SqlParameter("@FimSemana", System.Data.SqlDbType.Date));
            command.Parameters["@FimSemana"].Value = fimSemana;

            try
            {
                PainelControleAtaquesModels objAtaque;
                PainelControleModels objRetorno = new PainelControleModels();
                conexao.Open();

                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                objRetorno.Ataques = new List<PainelControleAtaquesModels>();
                while (reader.Read())
                {
                    objAtaque = new PainelControleAtaquesModels();
                    objAtaque.Derrota = Convert.ToInt32(reader["Derrota"].ToString());
                    objAtaque.Empate = Convert.ToInt32(reader["Empate"].ToString());
                    objAtaque.Escalado = Convert.ToInt32(reader["Escalado"].ToString());
                    objAtaque.NAtacou = Convert.ToInt32(reader["NAtacou"].ToString());
                    objAtaque.Player = new Player().ObterPlayer(Convert.ToInt32(reader["ID"].ToString()));
                    objAtaque.Vitoria = Convert.ToInt32(reader["Vitoria"].ToString());

                    objRetorno.Ataques.Add(objAtaque);

                }

                //Retornar somente os maiores que Zero.
                objRetorno.Ataques = objRetorno.Ataques.Where(x => x.Escalado > 0).ToList();

                conexao.Close();
                conexao.Dispose();

                objRetorno.DataInicio = inicioSemana;
                objRetorno.DataFim = fimSemana;

                return objRetorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}
