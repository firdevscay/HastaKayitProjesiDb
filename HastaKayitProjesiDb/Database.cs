using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HastaKayitProjesiDb
{
    public class Database
    {

        private static string database = ConfigurationManager.ConnectionStrings["HastaKayitDb"].ConnectionString;


        public static DataTable GetDataTable(string query)
        {
            SqlConnection connection = new SqlConnection(database);
            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            connection.Close();

            return dataTable;
        }



        public static SqlParameter AddParameter(string parameterName, object value, SqlDbType sqlDbType)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                Value = value,
                SqlDbType = sqlDbType
            };
        }



        public static int ExecuteCommand(string query, List<SqlParameter> parameters)
        {
            SqlConnection connection = new SqlConnection(database);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddRange(parameters.ToArray());

            int res = command.ExecuteNonQuery();
            connection.Close();
            return res;
        }

    }
}
