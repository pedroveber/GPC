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
    public class Siege
    {
        public List<Models.SiegeModels> ListarSieges(int idGuilda)
        {

            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            select.AppendLine("a.Id,a.data, ");
            select.AppendLine("b.Posicao Posicao1, b.IdGuilda guild1, g1.Nome nome1, b.MatchScore MatchScore1, b.Rating Rating1, ");
            select.AppendLine("c.Posicao Posicao2, c.IdGuilda guild2, g2.Nome nome2, c.MatchScore MatchScore2, c.Rating Rating2, ");
            select.AppendLine("d.Posicao Posicao3, d.IdGuilda guild3, g3.Nome nome3, d.MatchScore MatchScore3, d.Rating Rating3 ");
            select.AppendLine("from dbo.Siege a ");
            select.AppendLine("inner join dbo.SiegeGuilda b on b.IdSiege = a.Id and b.Posicao = 1 ");
            select.AppendLine("left join dbo.Guilda g1 on g1.Id = b.IdGuilda ");
            select.AppendLine("inner join dbo.SiegeGuilda c on c.IdSiege = a.Id and c.Posicao = 2 ");
            select.AppendLine("left join dbo.Guilda g2 on g2.Id = c.IdGuilda ");
            select.AppendLine("inner join dbo.SiegeGuilda d on d.IdSiege = a.Id and d.Posicao = 3 ");
            select.AppendLine("left join dbo.Guilda g3 on g3.Id = d.IdGuilda ");
            select.AppendLine("where a.id in (select d.IdSiege from dbo.SiegeGuilda d where IdGuilda = @IdGuilda) ");

            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@IdGuilda", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@IdGuilda"].Value = idGuilda;

            try
            {
                Models.SiegeModels objSiege;
                List<Models.SiegeModels> objRetorno = new List<SiegeModels>();


                conn.Open();
                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    objSiege = new Models.SiegeModels();

                    objSiege.IdGuildaUsuarioLogado = idGuilda;
                    if (reader["Data"].ToString() != string.Empty)
                        objSiege.Data = Convert.ToDateTime(reader["Data"].ToString());

                    objSiege.Id = long.Parse(reader["Id"].ToString());

                    objSiege.Guilda = new List<SiegeGuildaModels>();

                    objSiege.Guilda.Add(new SiegeGuildaModels()
                    {
                        MatchScore = double.Parse(reader["MatchScore1"].ToString()),
                        Posicao = int.Parse(reader["Posicao1"].ToString()),
                        Rating = int.Parse(reader["Rating1"].ToString()),
                        Guilda = new GuildaModels()
                        {
                            Id = long.Parse(reader["guild1"].ToString()),
                            Nome = reader["nome1"].ToString()
                        }
                    });

                    objSiege.Guilda.Add(new SiegeGuildaModels()
                    {
                        MatchScore = double.Parse(reader["MatchScore2"].ToString()),
                        Posicao = int.Parse(reader["Posicao2"].ToString()),
                        Rating = int.Parse(reader["Rating2"].ToString()),
                        Guilda = new GuildaModels()
                        {
                            Id = long.Parse(reader["guild2"].ToString()),
                            Nome = reader["nome2"].ToString()
                        }
                    });


                    objSiege.Guilda.Add(new SiegeGuildaModels()
                    {
                        MatchScore = double.Parse(reader["MatchScore3"].ToString()),
                        Posicao = int.Parse(reader["Posicao3"].ToString()),
                        Rating = int.Parse(reader["Rating3"].ToString()),
                        Guilda = new GuildaModels()
                        {
                            Id = long.Parse(reader["guild3"].ToString()),
                            Nome = reader["nome3"].ToString()
                        }
                    });


                    objRetorno.Add(objSiege);

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

        public SiegeModels ObterSiege(long idSiege)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            select.AppendLine("a.Id,a.data, ");
            select.AppendLine("b.Posicao Posicao1, b.IdGuilda guild1, g1.Nome nome1, b.MatchScore MatchScore1, b.Rating Rating1, ");
            select.AppendLine("c.Posicao Posicao2, c.IdGuilda guild2, g2.Nome nome2, c.MatchScore MatchScore2, c.Rating Rating2, ");
            select.AppendLine("d.Posicao Posicao3, d.IdGuilda guild3, g3.Nome nome3, d.MatchScore MatchScore3, d.Rating Rating3 ");
            select.AppendLine("from dbo.Siege a ");
            select.AppendLine("inner join dbo.SiegeGuilda b on b.IdSiege = a.Id and b.Posicao = 1 ");
            select.AppendLine("left join dbo.Guilda g1 on g1.Id = b.IdGuilda ");
            select.AppendLine("inner join dbo.SiegeGuilda c on c.IdSiege = a.Id and c.Posicao = 2 ");
            select.AppendLine("left join dbo.Guilda g2 on g2.Id = c.IdGuilda ");
            select.AppendLine("inner join dbo.SiegeGuilda d on d.IdSiege = a.Id and d.Posicao = 3 ");
            select.AppendLine("left join dbo.Guilda g3 on g3.Id = d.IdGuilda ");
            select.AppendLine("Where a.Id = @idSiege ");

            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@idSiege", System.Data.SqlDbType.BigInt));
            sqlCom.Parameters["@idSiege"].Value = idSiege;

            

            try
            {
                SiegeModels objRetorno = new SiegeModels();
                objRetorno.Guilda = new List<SiegeGuildaModels>();
                

                conn.Open();
                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    objRetorno.Id = long.Parse(reader["Id"].ToString());
                    objRetorno.Data = DateTime.Parse(reader["Data"].ToString());

                    objRetorno.Guilda.Add(new SiegeGuildaModels()
                    {
                        MatchScore = double.Parse(reader["MatchScore1"].ToString()),
                        Posicao = int.Parse(reader["Posicao1"].ToString()),
                        Rating = int.Parse(reader["Rating1"].ToString()),
                        Guilda = new GuildaModels()
                        {
                            Id = long.Parse(reader["guild1"].ToString()),
                            Nome = reader["nome1"].ToString()
                        }
                    });

                    objRetorno.Guilda.Add(new SiegeGuildaModels()
                    {
                        MatchScore = double.Parse(reader["MatchScore2"].ToString()),
                        Posicao = int.Parse(reader["Posicao2"].ToString()),
                        Rating = int.Parse(reader["Rating2"].ToString()),
                        Guilda = new GuildaModels()
                        {
                            Id = long.Parse(reader["guild2"].ToString()),
                            Nome = reader["nome2"].ToString()
                        }
                    });


                    objRetorno.Guilda.Add(new SiegeGuildaModels()
                    {
                        MatchScore = double.Parse(reader["MatchScore3"].ToString()),
                        Posicao = int.Parse(reader["Posicao3"].ToString()),
                        Rating = int.Parse(reader["Rating3"].ToString()),
                        Guilda = new GuildaModels()
                        {
                            Id = long.Parse(reader["guild3"].ToString()),
                            Nome = reader["nome3"].ToString()
                        }
                    });

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

        public List<AtaquesSiegeModels> ListarAtaquesConsolidado(int idGuilda,long idSiege)
        {

            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            select.AppendLine("a.idSiege,siege.Data, a.IdPlayer,c.Nome ");
            select.AppendLine(", (select count(Vitoria) from dbo.SiegeAtaques e where e.idSiege = a.idSiege and e.IdPlayer = a.IdPlayer and e.Vitoria = 1 ) Vitoria ");
            select.AppendLine(", (select count(Vitoria) from dbo.SiegeAtaques e where e.idSiege = a.idSiege and e.IdPlayer = a.IdPlayer and e.Vitoria = 2 ) Derrota ");
            select.AppendLine(", 30 - b.UsedUnitCount as 'MobNaoUsado' ");
            select.AppendLine("from dbo.SiegeAtaques a");
            select.AppendLine("inner ");
            select.AppendLine("join dbo.SiegePlayers b on b.IdSiege = a.idSiege and b.IdPlayer = a.IdPlayer ");
            select.AppendLine("inner join dbo.Player c on c.ID = b.IdPlayer ");
            select.AppendLine("inner join dbo.SiegePlayerOponente d on d.IdSiege = a.idSiege and d.Id = a.IdPlayerOponente and d.IdGuilda = a.IdGuildaOpp ");
            select.AppendLine("inner join dbo.Siege siege on siege.Id = a.idSiege ");
            select.AppendLine("where a.idSiege = @idSiege ");
            select.AppendLine("and a.idSiege in (select guild.IdSiege from dbo.SiegeGuilda guild where guild.IdGuilda = @idGuilda) ");
            select.AppendLine("group by a.idSiege,siege.Data,a.IdPlayer,c.Nome,b.UsedUnitCount ");
            select.AppendLine("order by c.Nome ");

            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@idSiege", System.Data.SqlDbType.BigInt));
            sqlCom.Parameters["@idSiege"].Value = idSiege;

            sqlCom.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@idGuilda"].Value = idGuilda;

            SiegeModels objSiege = ObterSiege(idSiege);

            try
            {
                AtaquesSiegeModels objAtaque;
                List<Models.AtaquesSiegeModels> objRetorno = new List<AtaquesSiegeModels>();

                conn.Open();
                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    objAtaque = new AtaquesSiegeModels();

                    objAtaque.Derrota = int.Parse(reader["Derrota"].ToString());
                    objAtaque.MonstrosNaoUsados = int.Parse(reader["MobNaoUsado"].ToString());
                    objAtaque.Player = new PlayerModels()
                    {
                        Id = int.Parse(reader["IdPlayer"].ToString()),
                        Nome = reader["Nome"].ToString()
                    };
                    objAtaque.Siege = objSiege;

                    objAtaque.Vitoria = int.Parse(reader["Vitoria"].ToString());
                    
                    objRetorno.Add(objAtaque);

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
    }
}
