using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telepase_DAL
{
    public abstract class Mapper<T, K>
    {
        protected readonly Acceso2 acceso = new Acceso2();
        protected readonly string columnaPK;
        protected readonly string obtenerStoreProcedure;
        protected readonly string insertarStoreProcedure;
        protected readonly string editarStoreProcedure;
        protected readonly string eliminarStoreProcedure;

        protected Mapper(string columnaPK,
            string obtenerStoreProcedure,
            string insertarStoreProcedure,
            string editarStoreProcedure,
            string eliminarStoreProcedure)
        {
            this.columnaPK = columnaPK;
            this.obtenerStoreProcedure = obtenerStoreProcedure;
            this.insertarStoreProcedure = insertarStoreProcedure;
            this.editarStoreProcedure = editarStoreProcedure;
            this.eliminarStoreProcedure = eliminarStoreProcedure;
        }

        public List<T> Listar()
        {
            DataTable dataTable = acceso.Leer(obtenerStoreProcedure);
            List<T> list = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T t = ParseRow(row);
                list.Add(t);
            }

            return list;
        }

        public List<T> Buscar(string storeProcedure, SqlParameter[] parametros)
        {
            DataTable dataTable = acceso.Leer(storeProcedure, parametros);
            List<T> list = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T t = ParseRow(row);
                list.Add(t);
            }

            return list;
        }

        internal abstract T ParseRow(DataRow row);

        public int Insertar(T t)
        {

            int affectedRows = 0;
            SqlParameter[] parametro = ParseData(t);
            affectedRows = acceso.Escribir(insertarStoreProcedure, parametro);

            return affectedRows;
        }

        public int Editar(T t)
        {
            int affectedRows = 0;
            SqlParameter[] parametro = ParseData(t);
            affectedRows = acceso.Escribir(editarStoreProcedure, parametro);

            return affectedRows;
        }

        public int Eliminar(K pk)
        {
            int affectedRows = 0;
            SqlParameter[] parametro = new SqlParameter[1];
            parametro[0] = new SqlParameter($"@{columnaPK.ToLower()}", pk);
            affectedRows = acceso.Escribir(eliminarStoreProcedure, parametro);

            return affectedRows;
        }

        internal abstract SqlParameter[] ParseData(T data);
    }
}
