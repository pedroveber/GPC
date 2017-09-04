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
    public class Monstro
    {
        public MonstroModels ObterMostro(int id)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();

            select.AppendLine("select Id,Nome,Imagem from");
            select.AppendLine("DB_SW.dbo.Monstro ");
            select.AppendLine("where id = @id ");

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));
            command.Parameters["@id"].Value = id;

            try
            {
                MonstroModels objMonstro = new MonstroModels();

                conexao.Open();

                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objMonstro = new MonstroModels();
                    objMonstro.Id = Convert.ToInt32(reader["Id"].ToString());
                    objMonstro.Nome = reader["Nome"].ToString();
                    objMonstro.Imagem = reader["Imagem"].ToString();

                }

                conexao.Close();
                conexao.Dispose();

                return objMonstro;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
