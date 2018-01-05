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

        public List<AtaquesSiegeModels> ListarAtaquesConsolidado(int idGuilda, long idSiege)
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

        public List<AtaquesSiegeModels> ListarAtaques(int idGuilda)
        {

            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            select.AppendLine("a.IdPlayer,d.Nome, ");
            select.AppendLine("(select count(Vitoria) from dbo.SiegeAtaques b where b.IdPlayer = a.IdPlayer and b.Vitoria = 1) Vitoria, ");
            select.AppendLine("(select count(Vitoria) from dbo.SiegeAtaques c where c.IdPlayer = a.IdPlayer and c.Vitoria = 2) Derrota, ");
            select.AppendLine("(select count(distinct idSiege) from dbo.SiegeAtaques e where e.IdPlayer = a.IdPlayer) QtsSieges, ");
            select.AppendLine("(select AVG(UsedUnitCount) from dbo.SiegePlayers f where f.IdPlayer = a.IdPlayer) MediaMonstros, ");
            select.AppendLine("(select sum(1) from dbo.SiegeAtaques g where g.IdPlayer = a.IdPlayer) AtaquesRealizados ");

            select.AppendLine("from dbo.SiegePlayers a ");
            select.AppendLine("inner join dbo.Player d on d.ID = a.IdPlayer ");
            select.AppendLine("where 1 = 1 ");
            select.AppendLine("and a.idSiege in (select guild.IdSiege from dbo.SiegeGuilda guild where guild.IdGuilda = @idGuilda)  ");
            select.AppendLine("group by a.IdPlayer,d.Nome ");

            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@idGuilda"].Value = idGuilda;

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

                    objAtaque.Player = new PlayerModels()
                    {
                        Id = int.Parse(reader["IdPlayer"].ToString()),
                        Nome = reader["Nome"].ToString()
                    };
                    objAtaque.Vitoria = int.Parse(reader["Vitoria"].ToString());
                    objAtaque.QuantidadeSieges = int.Parse(reader["QtsSieges"].ToString());
                    objAtaque.MediaMonstros = int.Parse(reader["MediaMonstros"].ToString());
                    objAtaque.AtaquesRealizados = int.Parse(reader["AtaquesRealizados"].ToString());

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

        public List<DefesaSiegeModels> ListarDefesasConsolidado(int idGuilda, long idSiege)
        {

            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            select.AppendLine("a.IdSiege,a.IdPlayer, d.Nome ");
            select.AppendLine(", (select count(Vitoria) from dbo.SiegePlayerDefesa e where e.idSiege = a.idSiege and e.IdPlayer = a.IdPlayer and e.Vitoria = 1 ) Vitoria ");
            select.AppendLine(", (select count(Vitoria) from dbo.SiegePlayerDefesa e where e.idSiege = a.idSiege and e.IdPlayer = a.IdPlayer and e.Vitoria = 2 ) Derrota ");
            select.AppendLine("from dbo.SiegePlayerDefesa a ");
            select.AppendLine("inner join dbo.SiegePlayers c on c.IdPlayer = a.IdPlayer and c.IdSiege = a.IdSiege ");
            select.AppendLine("inner join dbo.Player d on d.ID = c.IdPlayer ");
            select.AppendLine("where ");
            select.AppendLine("a.IdSiege = @idSiege ");
            select.AppendLine("and a.idSiege in (select guild.IdSiege from dbo.SiegeGuilda guild where guild.IdGuilda = @idGuilda) ");
            select.AppendLine("group by a.IdSiege,a.IdPlayer, d.Nome ");
            select.AppendLine("order by 3 ");


            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@idSiege", System.Data.SqlDbType.BigInt));
            sqlCom.Parameters["@idSiege"].Value = idSiege;

            sqlCom.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@idGuilda"].Value = idGuilda;

            SiegeModels objSiege = ObterSiege(idSiege);

            try
            {
                DefesaSiegeModels objDefesa;
                List<Models.DefesaSiegeModels> objRetorno = new List<DefesaSiegeModels>();

                conn.Open();
                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    objDefesa = new DefesaSiegeModels();

                    objDefesa.Derrota = int.Parse(reader["Derrota"].ToString());
                    objDefesa.Player = new PlayerModels()
                    {
                        Id = int.Parse(reader["IdPlayer"].ToString()),
                        Nome = reader["Nome"].ToString()
                    };
                    objDefesa.Siege = objSiege;

                    objDefesa.Vitoria = int.Parse(reader["Vitoria"].ToString());

                    objRetorno.Add(objDefesa);

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

        public List<DefesaSiegeModels> ListarDefesas(int idGuilda)
        {

            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            select.AppendLine("a.IdPlayer,b.nome, ");
            select.AppendLine("(select count(Vitoria) from dbo.SiegePlayerDefesa b where b.IdPlayer = a.IdPlayer and b.Vitoria = 1) Vitoria, ");
            select.AppendLine("(select count(Vitoria) from dbo.SiegePlayerDefesa b where b.IdPlayer = a.IdPlayer and b.Vitoria = 2) Derrota ");
            select.AppendLine("from dbo.SiegePlayers a ");
            select.AppendLine("inner join dbo.Player b on b.ID = a.IdPlayer ");
            select.AppendLine("where 1 = 1 ");
            select.AppendLine("and a.idSiege in (select guild.IdSiege from dbo.SiegeGuilda guild where guild.IdGuilda = @idGuilda) ");
            select.AppendLine("group by a.IdPlayer,b.nome ");
            select.AppendLine("order by 3 desc ");


            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@idGuilda"].Value = idGuilda;

            try
            {
                DefesaSiegeModels objDefesa;
                List<Models.DefesaSiegeModels> objRetorno = new List<DefesaSiegeModels>();

                conn.Open();
                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    objDefesa = new DefesaSiegeModels();

                    objDefesa.Derrota = int.Parse(reader["Derrota"].ToString());
                    objDefesa.Player = new PlayerModels()
                    {
                        Id = int.Parse(reader["IdPlayer"].ToString()),
                        Nome = reader["Nome"].ToString()
                    };

                    objDefesa.Vitoria = int.Parse(reader["Vitoria"].ToString());

                    objRetorno.Add(objDefesa);

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

        public List<DeckSiegeModels> ListarDecksPlayer(int idGuilda, long idSiege, int idPlayer)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            select.AppendLine("a.Id,a.IdDeck,a.IdGuild,a.IdPlayer,pl.Nome,a.IdSiege,s.Data, ");
            select.AppendLine("c.Id IdMonstro1,c.Nome NomeMonstro1,c.Imagem Imagem1, ");
            select.AppendLine("d.Id IdMonstro2, d.Nome NomeMonstro2,d.Imagem Imagem2, ");
            select.AppendLine("e.Id IdMonstro3, e.Nome NomeMonstro3,e.Imagem Imagem3, ");
            select.AppendLine("( ");
            select.AppendLine("select count(1) ");
            select.AppendLine("from dbo.SiegePlayerDefesa f ");
            select.AppendLine("inner join dbo.SiegeDefenseDeckAssign g on g.Base = f.Base and g.IdSiege = f.IdSiege ");
            select.AppendLine("where g.IdSiege = a.IdSiege and f.IdPlayer = a.IdPlayer ");
            select.AppendLine("and g.IdDeck = a.IdDeck ");
            select.AppendLine("and f.Vitoria = 1 ");
            select.AppendLine(")Vitoria, ");
            select.AppendLine("( ");
            select.AppendLine("select count(1) ");
            select.AppendLine("from dbo.SiegePlayerDefesa f ");
            select.AppendLine("inner join dbo.SiegeDefenseDeckAssign g on g.Base = f.Base and g.IdSiege = f.IdSiege ");
            select.AppendLine("where g.IdSiege = a.IdSiege and f.IdPlayer = a.IdPlayer ");
            select.AppendLine("and g.IdDeck = a.IdDeck and f.Vitoria = 2)Derrota, ");
            select.AppendLine("(SELECT Stuff( ");
            select.AppendLine("(SELECT N', ' + CONVERT(varchar, base) FROM dbo.SiegeDefenseDeckAssign bases where bases.IdDeck = a.IdDeck and bases.IdSiege=a.IdSiege order by bases.base FOR XML PATH(''), TYPE) ");
            select.AppendLine(".value('text()[1]', 'nvarchar(max)'),1,2,N'') ");
            select.AppendLine(") as BasesDefendidas ");
            select.AppendLine("from dbo.SiegeDefenseDeck a ");
            select.AppendLine("inner join dbo.SiegeTimesDefesas b on b.IdDeck = a.id ");
            select.AppendLine("inner join dbo.Siege s on s.Id = a.IdSiege ");
            select.AppendLine("inner join dbo.Player pl on pl.ID = a.IdPlayer ");
            select.AppendLine("left join dbo.Monstro c on c.Id = b.Monstro1 ");
            select.AppendLine("left join dbo.Monstro d on d.Id = b.Monstro2 ");
            select.AppendLine("left join dbo.Monstro e on e.Id = b.Monstro3 ");
            select.AppendLine("where ");
            select.AppendLine("a.IdSiege = @idSiege ");
            select.AppendLine("and a.IdPlayer = @IdPlayer ");
            select.AppendLine("and a.idSiege in (select guild.IdSiege from dbo.SiegeGuilda guild where guild.IdGuilda = @idGuilda) ");

            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@idSiege", System.Data.SqlDbType.BigInt));
            sqlCom.Parameters["@idSiege"].Value = idSiege;

            sqlCom.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@idGuilda"].Value = idGuilda;

            sqlCom.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@IdPlayer"].Value = idPlayer;

            try
            {
                DeckSiegeModels objDeck;
                List<Models.DeckSiegeModels> objRetorno = new List<DeckSiegeModels>();

                SiegeModels objSiege = ObterSiege(idSiege);

                conn.Open();
                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    objDeck = new DeckSiegeModels();

                    objDeck.Id = long.Parse(reader["Id"].ToString());
                    objDeck.IdDeck = long.Parse(reader["IdDeck"].ToString());
                    objDeck.Monstro1 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro1"].ToString()),
                        Nome = reader["NomeMonstro1"].ToString(),
                        Imagem = reader["Imagem1"].ToString()

                    };

                    objDeck.Monstro2 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro2"].ToString()),
                        Nome = reader["NomeMonstro2"].ToString(),
                        Imagem = reader["Imagem2"].ToString()

                    };

                    objDeck.Monstro3 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro3"].ToString()),
                        Nome = reader["NomeMonstro3"].ToString(),
                        Imagem = reader["Imagem3"].ToString()

                    };

                    objDeck.Player = new PlayerModels()
                    {
                        Id = int.Parse(reader["IdPlayer"].ToString()),
                        Nome = reader["Nome"].ToString()

                    };

                    objDeck.Siege = objSiege;
                    objDeck.Vitoria = int.Parse(reader["Vitoria"].ToString());
                    objDeck.Derrota = int.Parse(reader["Derrota"].ToString());
                    objDeck.BasesDefendidas = reader["BasesDefendidas"].ToString();

                    objRetorno.Add(objDeck);

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

        public List<DeckSiegeModels> ListarVitoriasTimes(int idGuilda)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            select.AppendLine("a.IdDeck,b.IdPlayer,h.Nome, ");
            select.AppendLine("e.Id IdMonstro1, e.Nome NomeMonstro1, e.Imagem Imagem1, ");
            select.AppendLine("f.Id IdMonstro2, f.Nome NomeMonstro2, f.Imagem Imagem2, ");
            select.AppendLine("g.Id IdMonstro3, g.Nome NomeMonstro3, g.Imagem Imagem3, ");
            select.AppendLine("sum(d.Vitoria) Vitoria ");
            select.AppendLine(",(select ");
            select.AppendLine("COUNT(1) ");
            select.AppendLine("from dbo.SiegePlayerDefesa def ");
            select.AppendLine("inner join dbo.SiegeDefenseDeckAssign ass on ass.IdSiege = def.IdSiege and ass.Base = def.Base ");
            select.AppendLine("inner join dbo.SiegeDefenseDeck defd on defd.IdSiege = def.IdSiege and defd.IdPlayer = def.IdPlayer and defd.IdDeck = ass.IdDeck ");
            select.AppendLine("where def.IdPlayer = b.IdPlayer ");
            select.AppendLine("and ass.IdDeck = a.IdDeck ");
            select.AppendLine(")AtaquesRecebidos ");
            select.AppendLine("from dbo.SiegeDefenseDeckAssign a ");
            select.AppendLine("inner join dbo.SiegeDefenseDeck b on b.IdSiege = a.IdSiege and b.IdDeck = a.IdDeck ");
            select.AppendLine("inner join dbo.SiegeTimesDefesas c on c.IdDeck = b.Id ");
            select.AppendLine("inner join dbo.SiegePlayerDefesa d on d.IdSiege = a.IdSiege and d.Base = a.Base and d.IdPlayer=b.IdPlayer");
            select.AppendLine("inner join dbo.Monstro e on e.Id = c.Monstro1 ");
            select.AppendLine("inner join dbo.Monstro f on f.Id = c.Monstro2 ");
            select.AppendLine("inner join dbo.Monstro g on g.Id = c.Monstro3 ");
            select.AppendLine("inner join dbo.Player h on h.ID = b.IdPlayer ");
            select.AppendLine("where 1 = 1 ");
            select.AppendLine("and d.Vitoria = 1 ");
            select.AppendLine("and a.idSiege in (select guild.IdSiege from dbo.SiegeGuilda guild where guild.IdGuilda = @idGuilda) ");
            select.AppendLine("group by a.IdDeck,b.IdPlayer,h.Nome, ");
            select.AppendLine("e.Id,e.Nome,e.Imagem, ");
            select.AppendLine("f.Id,f.Nome,f.Imagem, ");
            select.AppendLine("g.Id,g.Nome,g.Imagem ");


            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@idGuilda"].Value = idGuilda;


            try
            {
                DeckSiegeModels objDeck;
                List<Models.DeckSiegeModels> objRetorno = new List<DeckSiegeModels>();

                conn.Open();
                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    objDeck = new DeckSiegeModels();

                    objDeck.IdDeck = long.Parse(reader["IdDeck"].ToString());
                    objDeck.Monstro1 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro1"].ToString()),
                        Nome = reader["NomeMonstro1"].ToString(),
                        Imagem = reader["Imagem1"].ToString()

                    };

                    objDeck.Monstro2 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro2"].ToString()),
                        Nome = reader["NomeMonstro2"].ToString(),
                        Imagem = reader["Imagem2"].ToString()

                    };

                    objDeck.Monstro3 = new MonstroModels()
                    {
                        Id = int.Parse(reader["IdMonstro3"].ToString()),
                        Nome = reader["NomeMonstro3"].ToString(),
                        Imagem = reader["Imagem3"].ToString()

                    };

                    objDeck.Player = new PlayerModels()
                    {
                        Id = int.Parse(reader["IdPlayer"].ToString()),
                        Nome = reader["Nome"].ToString()

                    };

                    objDeck.Vitoria = int.Parse(reader["Vitoria"].ToString());
                    objDeck.AtaquesRecebidos = int.Parse(reader["AtaquesRecebidos"].ToString());

                    objRetorno.Add(objDeck);

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
