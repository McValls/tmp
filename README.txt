using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Acceso
    {
        private readonly string connectionString = "Data Source=DESKTOP-2MSA6DF;Initial Catalog=Control_Gastos;Integrated Security=True;";
        private SqlConnection connection;

        public Acceso()
        {

        }

        private void Conectar()
        {
            if (connection != null)
            {
                throw new Exception("Conexión abierta");
            }
            connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
        }

        private void Desconectar()
        {
            if (connection == null || connection.State == System.Data.ConnectionState.Closed)
            {
                throw new Exception("La conexión ya está cerrada");
            }
            connection.Close();
            connection = null;
        }

        private SqlTransaction BeginTransaction()
        {
            return connection.BeginTransaction();
        }

        private void CommitTransaction(SqlTransaction transaction)
        {
            transaction.Commit();
        }

        private void RollbackTransaction(SqlTransaction transaction)
        {
            transaction.Rollback();
        }

        public DataTable Leer(string storeProcedure, SqlParameter[] parameters)
        {
            Conectar();
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(storeProcedure, connection);
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            adapter.SelectCommand = command;
            adapter.Fill(table);

            Desconectar();

            return table;
        }

        public void Exportar(string storeProcedure, SqlParameter[] parameters, string filePath)
        {
            Conectar();
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(storeProcedure, connection);
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            adapter.SelectCommand = command;
            adapter.Fill(dataSet, "Usuarios");

            dataSet.WriteXml(filePath);

            Desconectar();
        }

        public int Escribir(string storeProcedure, SqlParameter[] parameters)
        {
            Conectar();
            int affectedRows = 0;
            SqlCommand command = new SqlCommand(storeProcedure, connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlTransaction tx = BeginTransaction();
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            try
            {
                affectedRows = command.ExecuteNonQuery();
                CommitTransaction(tx);
            }
            catch (Exception ex)
            {
                RollbackTransaction(tx);
            }
            finally
            {
                Desconectar();
            }
            return affectedRows;
        }
    }
}





 private void importBtn_Click(object sender, EventArgs e)
 {
     DataSet ds = new DataSet();
     ds.ReadXml("D:\\TEST_Usuarios.xml");
     dataGridView1.DataSource = ds;
     dataGridView1.DataMember = "Usuarios";
 }
