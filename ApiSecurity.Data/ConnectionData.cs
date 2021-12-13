using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using Npgsql;


namespace ApiSecurity.Data
{
    public class ConnectionData
    {
        //Codigo de Conexion configurada en la BD
        private int _CommandTimeout = 0;

        //Cadena de conecion por Default
        private string _ConectionString = "Server=192.168.56.101; Database=IOTCafe; User Id =postgres; Password=martin.ramos"; //Produccion

        //Constructor por default
        public ConnectionData()
        {
        }

        //Constructor que inicializa el codigo de conexion 
        public ConnectionData(string ConectionString)
        {
            _ConectionString = ConectionString;
        }

        #region Utilidades

        //Metodo Privado que generará la nueva conection string
        private NpgsqlConnection ObtenerConexion()
        {

            return new NpgsqlConnection(_ConectionString);
        }

        #endregion


        #region Consulta con ADO.NET

        //Variable para controlar las transacciones y manipular el cierre de conexion
        private Boolean _TransactionState = false;

        private NpgsqlConnection _Cnn = null;
        private NpgsqlCommand _sqlComand = null;
        private DataSet _DataSet = null;
        private NpgsqlDataAdapter _sqlDataAdapter = null;
        private NpgsqlTransaction _sqlTransaction = null;


        private NpgsqlConnection Cnn
        {
            get
            {

                //si es null se crea la instancia la primera vez
                if (_Cnn == null)
                    _Cnn = ObtenerConexion();

                //se abre la connexion en caso que este cerrada
                if (_Cnn.State != ConnectionState.Open)
                    _Cnn.Open();

                return _Cnn;
            }
        }

        //Metodo asincrono que retorna las filas afectadas a travez de una consulta o un proceso sin parametros
        public async Task<int> SqlExecuteAsync(string Query)
        {

            try
            {

                _sqlComand = new NpgsqlCommand(Query, Cnn, _sqlTransaction);
                _sqlComand.CommandTimeout = _CommandTimeout;

                int result = await _sqlComand.ExecuteNonQueryAsync();

                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }
        }

        //Metodo sincrono que retorna las filas afectadas a travez de una consulta o un proceso sin parametros
        public int SqlExecute(string Query)
        {

            try
            {
                //Realiza un exception en caso que el cliente halla realizado el cancel de la peticion

                _sqlComand = new NpgsqlCommand(Query, Cnn, _sqlTransaction);
                _sqlComand.CommandTimeout = _CommandTimeout;

                //Realiza un exception en caso que el cliente halla realizado el cancel de la peticion

                int result = _sqlComand.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }
        }

        //Metodo asincrono que retorna las filas afectadas a travez de un procedimiento almacenado con parametros
        public async Task<int> SqlExecuteAsync(string sp, params object[] args)
        {

            try
            {

                _sqlComand = new NpgsqlCommand();
                _sqlComand.CommandTimeout = _CommandTimeout;
                _sqlComand.CommandText = sp;
                _sqlComand.CommandType = CommandType.StoredProcedure;
                _sqlComand.Connection = Cnn;
                _sqlComand.Transaction = _sqlTransaction;

                NpgsqlCommandBuilder.DeriveParameters(_sqlComand);

                for (int i = 0; i < args.Length; i++)
                {
                    _sqlComand.Parameters[i].Value = (args[i] != null) ? args[i] : DBNull.Value;
                }

                int result = await _sqlComand.ExecuteNonQueryAsync();

                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }

        }

        //Metodo sincrono que retorna las filas afectadas a travez de un procedimiento almacenado con parametros
        public int SqlExecute(string sp, params object[] args)
        {

            try
            {

                _sqlComand = new NpgsqlCommand();
                _sqlComand.CommandTimeout = _CommandTimeout;
                _sqlComand.CommandText = sp;
                _sqlComand.CommandType = CommandType.StoredProcedure;
                _sqlComand.Connection = Cnn;
                _sqlComand.Transaction = _sqlTransaction;

                NpgsqlCommandBuilder.DeriveParameters(_sqlComand);

                for (int i = 0; i < args.Length; i++)
                {
                    _sqlComand.Parameters[i].Value = (args[i] != null) ? args[i] : DBNull.Value;
                }

                int result = _sqlComand.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }

        }

        //Metodo asincrono que retorna un escalar a travez de una consulta o un proceso sin parametros
        public async Task<object> SqlExecuteScalarAsync(string Query)
        {
            try
            {
                _sqlComand = new NpgsqlCommand();
                _sqlComand.CommandTimeout = _CommandTimeout;
                _sqlComand.CommandText = Query;
                _sqlComand.CommandType = CommandType.Text;
                _sqlComand.Connection = Cnn;
                _sqlComand.Transaction = _sqlTransaction;

                object result = await _sqlComand.ExecuteScalarAsync();

                return result;

            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }

        }

        //Metodo sincrono que retorna un escalar a travez de una consulta o un proceso sin parametros
        public object SqlExecuteScalar(string Query)
        {

            try
            {

                _sqlComand = new NpgsqlCommand();
                _sqlComand.CommandTimeout = _CommandTimeout;
                _sqlComand.CommandText = Query;
                _sqlComand.CommandType = CommandType.Text;
                _sqlComand.Connection = Cnn;
                _sqlComand.Transaction = _sqlTransaction;

                return _sqlComand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }

        }

        //Metodo asincrono que retorna un escalar a travez de un procedimiento almacenado con parametros
        public async Task<object> SqlExecuteScalarAsync(string sp, params object[] args)
        {

            try
            {
                _sqlComand = new NpgsqlCommand();
                _sqlComand.CommandTimeout = _CommandTimeout;
                _sqlComand.CommandText = sp;
                _sqlComand.CommandType = CommandType.StoredProcedure;
                _sqlComand.Connection = Cnn;
                _sqlComand.Transaction = _sqlTransaction;

                NpgsqlCommandBuilder.DeriveParameters(_sqlComand);

                for (int i = 0; i < args.Length; i++)
                {
                    _sqlComand.Parameters[i].Value = (args[i] != null) ? args[i] : DBNull.Value;
                }

                object result = await _sqlComand.ExecuteScalarAsync();

                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }
        }

        //Metodo sincrono que retorna un escalar a travez de un procedimiento almacenado con parametros
        public object SqlExecuteScalar(string sp, params object[] args)
        {

            try
            {
                _sqlComand = new NpgsqlCommand();
                _sqlComand.CommandTimeout = _CommandTimeout;
                _sqlComand.CommandText = sp;
                _sqlComand.CommandType = CommandType.StoredProcedure;
                _sqlComand.Connection = Cnn;
                _sqlComand.Transaction = _sqlTransaction;

                NpgsqlCommandBuilder.DeriveParameters(_sqlComand);

                for (int i = 0; i < args.Length; i++)
                {
                    _sqlComand.Parameters[i].Value = (args[i] != null) ? args[i] : DBNull.Value;
                }

                object result = _sqlComand.ExecuteScalar();

                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }

        }

        //Metodo que retorna una string Json desde una consuta o un proceso sin parametros
        public string JsonSqlQuery(string Query)
        {

            try
            {


                _sqlComand = new NpgsqlCommand(Query, Cnn, _sqlTransaction);
                _sqlComand.CommandTimeout = _CommandTimeout;

                _DataSet = new DataSet();
                _sqlDataAdapter = new NpgsqlDataAdapter(_sqlComand);
                _sqlDataAdapter.Fill(_DataSet);

                string result = JsonConvert.SerializeObject(_DataSet.Tables[0].Copy());

                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }

        }

        //Metodo que retorna una string Json desde una consuta sql que retorna mas de 1 resultado 
        public string JsonMultiSqlQuery(string Query)
        {

            try
            {

                _sqlComand = new NpgsqlCommand(Query, Cnn, _sqlTransaction);
                _sqlComand.CommandTimeout = _CommandTimeout;

                _DataSet = new DataSet();
                _sqlDataAdapter = new NpgsqlDataAdapter(_sqlComand);
                _sqlDataAdapter.Fill(_DataSet);

                string result = JsonConvert.SerializeObject(_DataSet.Tables);

                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }

        }

        //Metodo que retorna string json desde un proceso con parametros
        public string JsonSqlQuery(string sp, params object[] args)
        {

            try
            {

                _sqlComand = new NpgsqlCommand();
                _sqlComand.CommandTimeout = _CommandTimeout;
                _sqlComand.CommandText = sp;
                _sqlComand.CommandType = CommandType.StoredProcedure;
                _sqlComand.Connection = Cnn;
                _sqlComand.Transaction = _sqlTransaction;

                NpgsqlCommandBuilder.DeriveParameters(_sqlComand);

                for (int i = 0; i < args.Length; i++)
                {
                    _sqlComand.Parameters[i].Value = (args[i] != null) ? args[i] : DBNull.Value;
                }

                _DataSet = new DataSet();
                _sqlDataAdapter = new NpgsqlDataAdapter(_sqlComand);
                _sqlDataAdapter.Fill(_DataSet);

                string result = JsonConvert.SerializeObject(_DataSet.Tables[0].Copy());

                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }

        }

        //Metodo que retorna un string json desde un procedimiento con parametros que retorna mas de 1 consulta
        public string JsonMultiSqlQuery(string sp, params object[] args)
        {

            try
            {

                _sqlComand = new NpgsqlCommand();
                _sqlComand.CommandTimeout = _CommandTimeout;
                _sqlComand.CommandText = sp;
                _sqlComand.CommandType = CommandType.StoredProcedure;
                _sqlComand.Connection = Cnn;
                _sqlComand.Transaction = _sqlTransaction;

                NpgsqlCommandBuilder.DeriveParameters(_sqlComand);

                for (int i = 0; i < args.Length; i++)
                {
                    _sqlComand.Parameters[i].Value = (args[i] != null) ? args[i] : DBNull.Value;
                }

                _DataSet = new DataSet();
                _sqlDataAdapter = new NpgsqlDataAdapter(_sqlComand);
                _sqlDataAdapter.Fill(_DataSet);

                string result = JsonConvert.SerializeObject(_DataSet.Tables);


                return result;
            }
            catch (Exception ex)
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();

                throw new Exception(ex.Message);
            }
            finally
            {
                //Se cierra la conexion si esta no hay transaccion abierta
                CloseConnection();
            }

        }

        //Metodo que retorna una string Json desde una consuta o un proceso sin parametros
        public List<T> SqlQuery<T>(string Query)
        {

            List<T> result = default(List<T>);

            try
            {

                result = JsonConvert.DeserializeObject<List<T>>(JsonSqlQuery(Query));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;

        }

        //Metodo que retorna una string Json desde una consuta sql que retorna mas de 1 resultado 
        public T MultiSqlQuery<T>(string Query)
        {

            T result = default(T);

            try
            {

                result = JsonConvert.DeserializeObject<T>(JsonMultiSqlQuery(Query));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;

        }

        //Metodo que retorna string json desde un proceso con parametros
        public List<T> SqlQuery<T>(string sp, params object[] args)
        {

            List<T> result = new List<T>();

            try
            {

                result = JsonConvert.DeserializeObject<List<T>>(JsonSqlQuery(sp, args));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;

        }

        //Metodo que retorna un string json desde un procedimiento con parametros que retorna mas de 1 consulta
        public T MultiSqlQuery<T>(string sp, params object[] args)
        {

            T result = default(T);

            try
            {
                result = JsonConvert.DeserializeObject<T>(JsonMultiSqlQuery(sp, args));

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        //Metodo que inicializa una transaccion
        public void BeginTransaction()
        {
            _sqlTransaction = Cnn.BeginTransaction();
            _TransactionState = true;
        }

        //Metodo que realiza un RollBack a la transaccion
        public void Rollback()
        {
            if (_TransactionState == true)
            {
                _sqlTransaction.Rollback();
                _sqlTransaction = null;
                _TransactionState = false;

                //se cierra la conexion si se encuentra abierta
                CloseConnection();

            }
        }

        //Metodo que confirma la transaccion
        public void Commit()
        {
            _sqlTransaction.Commit();
            _sqlTransaction = null;
            _TransactionState = false;

            //se cierra la conexion si se encuentra abierta
            CloseConnection();

        }

        //Metodo que permite cerrar la conexion cuando su estado es Open
        private void CloseConnection()
        {
            if (!_TransactionState)
            {
                if (_Cnn != null)
                    if (_Cnn.State == ConnectionState.Open)
                    {
                        _Cnn.Close();
                        _Cnn.Dispose();
                    }
            }
        }

        #endregion
    }
}
