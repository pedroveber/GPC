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
    public class Luta
    {
        public List<LutasModels> ListarLutas(int idBatalha, long idGuilda)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DB_SW"].ToString();
            StringBuilder select = new StringBuilder();

            select.AppendLine("select ");
            select.AppendLine("a.ID, a.CodBatalhas,a.CodPlayer,a.CodPlayerOponente,a.Vitoria, a.ValorBarra, a.DataHora, a.MomentoVitoria ");
            select.AppendLine("from dbo.lutas a ");
            select.AppendLine("where a.CodBatalhas = @idBatalha ");

            sqlCom.CommandText = select.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@idBatalha", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@idBatalha"].Value = idBatalha;

            List<LutasModels> objRetorno = new List<LutasModels>();

            try
            {
                conn.Open();

                sqlCom.Connection = conn;
                SqlDataReader reader = sqlCom.ExecuteReader();

                LutasModels objLuta = null;

                BatalhaModels objBatalha = new BatalhaModels();
                objBatalha = new DataAccess.Batalha().ObterBatalha(idBatalha);

                //Obter Players
                List<PlayerModels> lstPlayer = new List<PlayerModels>();
                lstPlayer = new Player().ListarPlayers(idGuilda);

                while (reader.Read())
                {
                    objLuta = new LutasModels();
                    objLuta.Batalhas = objBatalha;
                    objLuta.DataHora = Convert.ToDateTime(reader["DataHora"].ToString());
                    objLuta.Id = Convert.ToInt32(reader["ID"].ToString());
                    objLuta.MomentoVitoria = reader["MomentoVitoria"].ToString();
                    objLuta.Player = lstPlayer.First(x => x.Id == long.Parse(reader["CodPlayer"].ToString()));
                    objLuta.PlayerOponente = new PlayerOponenteModels() { Id = long.Parse(reader["CodPlayerOponente"].ToString()) };
                    objLuta.ValorBarra = int.Parse(reader["ValorBarra"].ToString());
                    objLuta.Vitoria = int.Parse(reader["Vitoria"].ToString());

                    objRetorno.Add(objLuta);

                }
                conn.Close();
                conn.Dispose();
            }
            catch (Exception)
            {

                throw;
            }


            return objRetorno;
        }

    }
}
