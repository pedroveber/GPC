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
    public class Player
    {
        public Models.PlayerModels ObterPlayer(int id)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT * FROM DB_SW.dbo.Player WHERE ID = @ID");


            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;
            sqlCom.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@ID"].Value = id;

            try
            {
                Models.PlayerModels objPlayer = new PlayerModels();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objPlayer = new Models.PlayerModels();
                    objPlayer.Id = Convert.ToInt32(reader["ID"].ToString());
                    objPlayer.Nome = reader["Nome"].ToString();
                    objPlayer.Level = Convert.ToInt32(reader["Level"].ToString());
                    objPlayer.PontoArena = Convert.ToInt32(reader["PontoArena"].ToString());
                    objPlayer.Ativo = (reader["Status"].ToString() == "S") ? true : false;

                }
                conn.Close();
                conn.Dispose();
                return objPlayer;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<Models.PlayerModels> ListarPlayers()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT * FROM dbo.Player WHERE Status = 'S' order by Nome");


            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            try
            {
                Models.PlayerModels objPlayer;
                List<Models.PlayerModels> objRetorno = new List<PlayerModels>();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objPlayer = new Models.PlayerModels();
                    objPlayer.Id = Convert.ToInt32(reader["ID"].ToString());
                    objPlayer.Nome = reader["Nome"].ToString();
                    objPlayer.Level = Convert.ToInt32(reader["Level"].ToString());
                    objPlayer.PontoArena = Convert.ToInt32(reader["PontoArena"].ToString());
                    objPlayer.Ativo = (reader["Status"].ToString() == "S") ? true : false;

                    objRetorno.Add(objPlayer);

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
        public List<Models.PlayerModels> ListarPlayers(long idGuilda)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT A.*FROM DBO.PLAYER A ");
            cmd.Append("INNER JOIN DBO.GUILDA_PLAYER B ON B.IDPLAYER = A.ID ");
            cmd.Append("WHERE ");
            cmd.Append("A.STATUS = 'S' ");
            cmd.Append("AND B.IDGUILDA = @idGuilda ");
            cmd.Append("ORDER BY NOME ");

            sqlCom.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.BigInt));
            sqlCom.Parameters["@idGuilda"].Value = idGuilda;

            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            try
            {
                Models.PlayerModels objPlayer;
                List<Models.PlayerModels> objRetorno = new List<PlayerModels>();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objPlayer = new Models.PlayerModels();
                    objPlayer.Id = Convert.ToInt32(reader["ID"].ToString());
                    objPlayer.Nome = reader["Nome"].ToString();
                    objPlayer.Level = Convert.ToInt32(reader["Level"].ToString());
                    objPlayer.PontoArena = Convert.ToInt32(reader["PontoArena"].ToString());
                    objPlayer.Ativo = (reader["Status"].ToString() == "S") ? true : false;

                    objRetorno.Add(objPlayer);

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
        public List<Models.PlayerModels> ListarPlayersSemUsuarios(long idGuilda)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT A.*FROM DBO.PLAYER A ");
            cmd.Append("INNER JOIN DBO.GUILDA_PLAYER B ON B.IDPLAYER = A.ID ");
            cmd.Append("WHERE ");
            cmd.Append("A.STATUS = 'S' ");
            cmd.Append("AND B.IDGUILDA = @idGuilda ");
            cmd.Append("AND B.IdUsuario IS NULL ");
            cmd.Append("ORDER BY NOME ");

            sqlCom.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.BigInt));
            sqlCom.Parameters["@idGuilda"].Value = idGuilda;

            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            try
            {
                Models.PlayerModels objPlayer;
                List<Models.PlayerModels> objRetorno = new List<PlayerModels>();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objPlayer = new Models.PlayerModels();
                    objPlayer.Id = Convert.ToInt32(reader["ID"].ToString());
                    objPlayer.Nome = reader["Nome"].ToString();
                    objPlayer.Level = Convert.ToInt32(reader["Level"].ToString());
                    objPlayer.PontoArena = Convert.ToInt32(reader["PontoArena"].ToString());
                    objPlayer.Ativo = (reader["Status"].ToString() == "S") ? true : false;

                    objRetorno.Add(objPlayer);

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


        public List<Models.PlayerModels> ListarPlayersSemGuild()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();

            cmd.Append("SELECT a.* FROM dbo.Player a ");
            cmd.Append("WHERE Status = 'S' ");
            cmd.Append("and not exists(select 0 from dbo.Guilda_Player b where b.IdPlayer = a.ID) ");
            cmd.Append("order by a.Nome");


            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            try
            {
                Models.PlayerModels objPlayer;
                List<Models.PlayerModels> objRetorno = new List<PlayerModels>();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objPlayer = new Models.PlayerModels();
                    objPlayer.Id = Convert.ToInt32(reader["ID"].ToString());
                    objPlayer.Nome = reader["Nome"].ToString();
                    objPlayer.Level = Convert.ToInt32(reader["Level"].ToString());
                    objPlayer.PontoArena = Convert.ToInt32(reader["PontoArena"].ToString());
                    objPlayer.Ativo = (reader["Status"].ToString() == "S") ? true : false;

                    objRetorno.Add(objPlayer);

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

        public CapivaraModels ObterPlayerCapivara(int idPlayer, long idGuilda)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();
            //multiguild
            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select a.Nome,a.Imagem,");
            select.AppendLine("(select count(vitoria) from dbo.Lutas b inner join dbo.Guilda_Player b1 on b1.idPlayer = b.CodPlayer and b1.ativo = 1 and b1.idGuilda = @idGuilda where b.CodPlayer = a.id and  b.vitoria = 2) Vitorias,");
            select.AppendLine("(select count(vitoria) from dbo.Lutas c inner join dbo.Guilda_Player b1 on b1.idPlayer = c.CodPlayer and b1.ativo = 1 and b1.idGuilda = @idGuilda where c.CodPlayer = a.id and  c.vitoria = 1) Empates,");
            select.AppendLine("(select count(vitoria) from dbo.Lutas d inner join dbo.Guilda_Player b1 on b1.idPlayer = d.CodPlayer and b1.ativo = 1 and b1.idGuilda = @idGuilda where d.CodPlayer = a.id and  d.vitoria = 0) Derrotas,");
            select.AppendLine("(select count(d1.vitoria) from dbo.PlayerDefesas d1 inner join dbo.Guilda_Player b1 on b1.idPlayer = d1.IdPlayer and b1.ativo = 1 and b1.idGuilda = @idGuilda where d1.vitoria = 2 and d1.IdPlayer = a.id) DefesaSucesso,");
            select.AppendLine("(select count(d2.vitoria) from dbo.PlayerDefesas d2 inner join dbo.Guilda_Player b1 on b1.idPlayer = d2.IdPlayer and b1.ativo = 1 and b1.idGuilda = @idGuilda where d2.vitoria = 1 and d2.IdPlayer = a.id) DefesaEmpate,");
            select.AppendLine("(select count(d3.vitoria) from dbo.PlayerDefesas d3 inner join dbo.Guilda_Player b1 on b1.idPlayer = d3.IdPlayer and b1.ativo = 1 and b1.idGuilda = @idGuilda where d3.vitoria = 0 and d3.IdPlayer = a.id ) DefesaDerrota,");
            select.AppendLine("(select count(1) from ");
            select.AppendLine("DB_SW.dbo.PlayerStatus e ");
            select.AppendLine("inner join DB_SW.dbo.Batalhas f on f.id = e.idBatalha and f.IdGuildaAtacante = @idGuilda");
            select.AppendLine("inner join dbo.Guilda_Player b1 on b1.idPlayer = e.IdPlayer and b1.ativo = 1 and b1.idGuilda = @idGuilda");
            select.AppendLine("where ");
            select.AppendLine("e.idPlayer = a.id and e.Status = 'S') Escalado,");
            select.AppendLine("(select count(1) from ");
            select.AppendLine("DB_SW.dbo.PlayerStatus g ");
            select.AppendLine("inner join DB_SW.dbo.Batalhas h on h.id = g.idBatalha and h.IdGuildaAtacante = @idGuilda");
            select.AppendLine("inner join dbo.Guilda_Player b1 on b1.idPlayer = g.IdPlayer and b1.ativo = 1 and b1.idGuilda = @idGuilda");
            select.AppendLine("where g.idPlayer = a.id and g.Status = 'S'");
            select.AppendLine("and not exists (select 0 from DB_SW.dbo.Lutas i inner join dbo.Guilda_Player b1 on b1.idPlayer = i.CodPlayer and b1.ativo = 1 and b1.idGuilda = @idGuilda where i.CodPlayer = g.IdPlayer and i.CodBatalhas = g.IdBatalha))NAtacou");
            select.AppendLine("from dbo.Player a");
            select.AppendLine("where ");
            select.AppendLine("a.id = @idPlayer");

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            command.Parameters.Add(new SqlParameter("@idPlayer", System.Data.SqlDbType.Int));
            command.Parameters["@idPlayer"].Value = idPlayer;

            command.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.BigInt));
            command.Parameters["@idGuilda"].Value = idGuilda;

            CapivaraModels objRetorno = new CapivaraModels();

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objRetorno = new CapivaraModels();
                objRetorno.Player = new Player().ObterPlayer(idPlayer);
                objRetorno.PercentualDefesa = 0;
                objRetorno.PercentualParticipacao = 0;
                objRetorno.PercentualVitoria = 0;
                objRetorno.Vitorias = Convert.ToInt32(reader["Vitorias"].ToString());
                objRetorno.Empates = Convert.ToInt32(reader["Empates"].ToString());
                objRetorno.Derrotas = Convert.ToInt32(reader["Derrotas"].ToString());
                objRetorno.Escalado = Convert.ToInt32(reader["Escalado"].ToString());
                objRetorno.NAtacou = Convert.ToInt32(reader["NAtacou"].ToString());
                objRetorno.DefesaVitorias = Convert.ToInt32(reader["DefesaSucesso"].ToString());
                objRetorno.DefesaEmpates = Convert.ToInt32(reader["DefesaEmpate"].ToString());
                objRetorno.DefesaDerrotas = Convert.ToInt32(reader["DefesaDerrota"].ToString());
                objRetorno.Imagem = reader["Imagem"].ToString();

            }

            //Ataques
            if (objRetorno.Vitorias > 0 || objRetorno.Empates > 0 || objRetorno.Derrotas > 0)
            {
                objRetorno.PercentualVitoria = Math.Round((Convert.ToDouble(objRetorno.Vitorias) / (objRetorno.Vitorias + objRetorno.Empates + objRetorno.Derrotas) * 100));
            }

            //Defesas
            if (objRetorno.DefesaVitorias > 0 || objRetorno.DefesaEmpates > 0 || objRetorno.DefesaDerrotas > 0)
            {
                objRetorno.PercentualDefesa = Math.Round((Convert.ToDouble(objRetorno.DefesaVitorias) / (objRetorno.DefesaVitorias + objRetorno.DefesaEmpates + objRetorno.DefesaDerrotas) * 100));
            }

            //Escalado
            if (objRetorno.Escalado > 0)
            {
                objRetorno.PercentualParticipacao = Math.Round(((objRetorno.Escalado - objRetorno.NAtacou) / Convert.ToDouble(objRetorno.Escalado)) * 100);
            }

            return objRetorno;

        }

        public List<PlayerUsuarioModels> ListarPlayerUsuario()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("select b.Ativo,b.IdGuilda,a.ID idPlayer,d.Id IdUsuario from dbo.Player a ");
            cmd.Append("left join dbo.Guilda_Player b on b.IdPlayer = a.ID ");
            cmd.Append("left join dbo.Guilda c on c.Id = b.IdGuilda ");
            cmd.Append("left join dbo.AspNetUsers d on d.Id = b.IdUsuario ");

            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            try
            {
                Models.PlayerUsuarioModels objPlayerUsuario;

                List<Models.PlayerUsuarioModels> objRetorno = new List<PlayerUsuarioModels>();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objPlayerUsuario = new Models.PlayerUsuarioModels();
                    if (reader["Ativo"].ToString() != string.Empty)
                        objPlayerUsuario.Ativo = bool.Parse(reader["Ativo"].ToString());

                    if (reader["IdGuilda"].ToString() != string.Empty)
                        objPlayerUsuario.Guilda = new Guilda().ObterGuilda(long.Parse(reader["IdGuilda"].ToString()));

                    objPlayerUsuario.Player = new Player().ObterPlayer(int.Parse(reader["idPlayer"].ToString())); ;

                    if (reader["IdUsuario"].ToString() != string.Empty)
                        objPlayerUsuario.Usuario = new Usuario().ObterUsario(reader["IdUsuario"].ToString());

                    objRetorno.Add(objPlayerUsuario);
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
        public PlayerUsuarioModels ObterPlayerUsuario(int idPlayer)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();


            StringBuilder cmd = new StringBuilder();
            cmd.Append("select b.Ativo,b.IdGuilda,a.ID idPlayer,d.Id IdUsuario from dbo.Player a ");
            cmd.Append("left join dbo.Guilda_Player b on b.IdPlayer = a.ID ");
            cmd.Append("left join dbo.Guilda c on c.Id = b.IdGuilda ");
            cmd.Append("left join dbo.AspNetUsers d on d.Id = b.IdUsuario ");
            cmd.Append("where a.Id = @id");

            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@id"].Value = idPlayer;

            try
            {
                Models.PlayerUsuarioModels objRetorno = new PlayerUsuarioModels();
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();


                while (reader.Read())
                {
                    objRetorno = new Models.PlayerUsuarioModels();
                    if (reader["Ativo"].ToString() != string.Empty)
                        objRetorno.Ativo = bool.Parse(reader["Ativo"].ToString());

                    if (reader["IdGuilda"].ToString() != string.Empty)
                        objRetorno.Guilda = new Guilda().ObterGuilda(long.Parse(reader["IdGuilda"].ToString()));

                    objRetorno.Player = new Player().ObterPlayer(int.Parse(reader["idPlayer"].ToString())); ;

                    if (reader["IdUsuario"].ToString() != string.Empty)
                        objRetorno.Usuario = new Usuario().ObterUsario(reader["IdUsuario"].ToString());

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



        public List<PlayerOponenteModels> ListarDefesasGVGOponente(long idBatalha)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
                StringBuilder select = new StringBuilder();

                select.AppendLine("SET DATEFORMAT dmy;");

                select.AppendLine("select ");
                select.AppendLine("a.idbatalha,a.idplayeroponente, ");
                select.AppendLine("p.Nome NomeOponente, p.Bonus, ");
                select.AppendLine("isnull(b.Id,0) Monstro1Id, isnull(b.Nome,'') Monstro1Nome, isnull(b.Imagem,'') Monstro1Imagem, ");
                select.AppendLine("isnull(c.Id,0) Monstro2Id, isnull(c.Nome,'') Monstro2Nome, isnull(c.Imagem,'') Monstro2Imagem, ");
                select.AppendLine("isnull(d.Id,0) Monstro3Id, isnull(d.Nome,'') Monstro3Nome, isnull(d.Imagem,'') Monstro3Imagem, ");
                select.AppendLine("isnull(e.Id,0) Monstro4Id, isnull(e.Nome,'') Monstro4Nome, isnull(e.Imagem,'') Monstro4Imagem, ");
                select.AppendLine("isnull(f.Id,0) Monstro5Id, isnull(f.Nome,'') Monstro5Nome, isnull(f.Imagem,'') Monstro5Imagem, ");
                select.AppendLine("isnull(g.Id,0) Monstro6Id, isnull(g.Nome,'') Monstro6Nome, isnull(g.Imagem,'') Monstro6Imagem ");

                select.AppendLine("from dbo.TimeDefesaGVG a ");

                select.AppendLine("inner join dbo.PlayerOponente p on p.ID = a.idplayeroponente and p.IdBatalha = a.idBatalha ");
                select.AppendLine("left join dbo.Monstro b on b.Id = a.Monstro1 ");
                select.AppendLine("left join dbo.Monstro c on c.Id = a.Monstro2 ");
                select.AppendLine("left join dbo.Monstro d on d.Id = a.Monstro3 ");
                select.AppendLine("left join dbo.Monstro e on e.Id = a.Monstro4 ");
                select.AppendLine("left join dbo.Monstro f on f.Id = a.Monstro5 ");
                select.AppendLine("left join dbo.Monstro g on g.Id = a.Monstro6 ");
                select.AppendLine("where a.idBatalha = @idBatalha ");
                select.AppendLine("order by p.Bonus desc, p.Nome ");

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                //Parametros

                command.Parameters.Add(new SqlParameter("@idBatalha", System.Data.SqlDbType.BigInt));
                command.Parameters["@idBatalha"].Value = idBatalha;

                TimeDefesaModels objTimeDefesa = new TimeDefesaModels();
                PlayerOponenteModels objPlayerOponente = new PlayerOponenteModels();
                List<PlayerOponenteModels> objRetorno = new List<PlayerOponenteModels>();

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objTimeDefesa = new TimeDefesaModels();
                    objTimeDefesa.Monstro1 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro1Id"].ToString()), Nome = reader["Monstro1Nome"].ToString(), Imagem = reader["Monstro1Imagem"].ToString() };
                    objTimeDefesa.Monstro2 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro2Id"].ToString()), Nome = reader["Monstro2Nome"].ToString(), Imagem = reader["Monstro2Imagem"].ToString() };
                    objTimeDefesa.Monstro3 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro3Id"].ToString()), Nome = reader["Monstro3Nome"].ToString(), Imagem = reader["Monstro3Imagem"].ToString() };
                    objTimeDefesa.Monstro4 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro4Id"].ToString()), Nome = reader["Monstro4Nome"].ToString(), Imagem = reader["Monstro4Imagem"].ToString() };
                    objTimeDefesa.Monstro5 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro5Id"].ToString()), Nome = reader["Monstro5Nome"].ToString(), Imagem = reader["Monstro5Imagem"].ToString() };
                    objTimeDefesa.Monstro6 = new MonstroModels() { Id = Convert.ToInt32(reader["Monstro6Id"].ToString()), Nome = reader["Monstro6Nome"].ToString(), Imagem = reader["Monstro6Imagem"].ToString() };

                    objPlayerOponente = new PlayerOponenteModels();
                    objPlayerOponente.Id = long.Parse(reader["idplayeroponente"].ToString());
                    objPlayerOponente.Bonus = int.Parse(reader["Bonus"].ToString());
                    objPlayerOponente.Nome = reader["NomeOponente"].ToString();
                    objPlayerOponente.TimeDefesa = objTimeDefesa;

                    objRetorno.Add(objPlayerOponente);

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
