using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inacap.Common.DAL
{
    public class Oracle
    {
        private string _stringAccess { get; set; }

        public Oracle() { }

        public Oracle(string key)
        {
            this._stringAccess = key;
        }

        public void ExecuteStoredProcedure(string NameStoredProcedure)
        {
            try
            {
                using (OracleConnection conn = GetConexion())
                {
                    OracleCommand cmd = new OracleCommand(NameStoredProcedure, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public void ExecuteStoredProcedure(string NameStoredProcedure, ref IDataParameter[] Params)
        {
            try
            {
                using (OracleConnection conn = GetConexion())
                {
                    OracleCommand cmd = new OracleCommand(NameStoredProcedure, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    TransferParameters(cmd, Params);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public void ExecuteStoredProcedure(string NameStoredProcedure, ref IDataParameter[] Params, ref DataTable DataResult)
        {
            try
            {
                OracleDataAdapter DataAdap = null;
                DataResult = new DataTable();

                using (OracleConnection conn = GetConexion())
                {
                    OracleCommand cmd = new OracleCommand(NameStoredProcedure, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    TransferParameters(cmd, Params);
                    DataAdap = new OracleDataAdapter(cmd);

                    conn.Open();
                    DataAdap.Fill(DataResult);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public void ExecuteStoredProcedure(string NameStoredProcedure, ref IDataParameter[] Params, ref DataSet DataResult)
        {
            try
            {
                OracleDataAdapter DataAdap = null;
                DataResult = new DataSet();

                using (OracleConnection conn = GetConexion())
                {
                    OracleCommand cmd = new OracleCommand(NameStoredProcedure, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    TransferParameters(cmd, Params);
                    DataAdap = new OracleDataAdapter(cmd);

                    conn.Open();
                    DataAdap.Fill(DataResult);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public void ExecuteSQL(string SQLString)
        {
            try
            {
                using (OracleConnection conn = GetConexion())
                {
                    OracleCommand cmd = new OracleCommand(SQLString, conn);
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public void ExecuteSQL(string SQLString, ref DataTable DataResult)
        {
            try
            {
                OracleDataAdapter DataAdap = null;
                DataResult = new DataTable();

                using (OracleConnection conn = GetConexion())
                {
                    DataAdap = new OracleDataAdapter(SQLString, conn);

                    conn.Open();
                    DataAdap.Fill(DataResult);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public void ExecuteSQL(string SQLString, ref IDataParameter[] Params)
        {
            try
            {
                using (OracleConnection conn = GetConexion())
                {
                    OracleCommand cmd = new OracleCommand(SQLString, conn);
                    cmd.CommandType = CommandType.Text;
                    TransferParameters(cmd, Params);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public void ExecuteSQL(string SQLString, ref IDataParameter[] Params, ref DataTable DataResult)
        {
            try
            {
                OracleDataAdapter DataAdap = null;
                DataResult = new DataTable();

                using (OracleConnection conn = GetConexion())
                {
                    OracleCommand cmd = new OracleCommand(SQLString, conn);
                    TransferParameters(cmd, Params);
                    DataAdap = new OracleDataAdapter(cmd);

                    conn.Open();
                    DataAdap.Fill(DataResult);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public object ExecuteStoredFunction(string NameStoredProcedure, ref IDataParameter[] Params)
        {
            object varOut = null;
            try
            {
                using (OracleConnection conn = GetConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(NameStoredProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName = true;
                        TransferParameters(cmd, Params);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        foreach (OracleParameter parameter in cmd.Parameters)
                        {
                            if (parameter.Direction == ParameterDirection.ReturnValue)
                            {
                                varOut = parameter.Value;
                                break;
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex.Message);
            }
            return varOut;
        }


        #region Privados
        private string StringConexion
        {
            get
            {
                if (string.IsNullOrEmpty(_stringAccess))
                {
                    return ConfigurationManager.AppSettings["AKORACLE"];
                }
                else
                {
                    return ConfigurationManager.AppSettings[_stringAccess];
                }
            }
        }
        private string GetStringConexion()
        {
            string NameKey = StringConexion;
            //TODO: Cambiar mensaje de error 1
            if (NameKey == null) throw new Exception("Clave de conexión no encontrada (NULL), revisar ubicación de string de conexión.");
            string StrConn = ConfigurationManager.AppSettings[NameKey];
            if (StrConn == null) throw new Exception("Clave de conexión no encontrada (NULL), revisar ubicación de string de conexión.");
            return StrConn;
        }
        private OracleConnection GetConexion()
        {
            string str = GetStringConexion();
            OracleConnection conn = new OracleConnection(str);

            return conn;
        }
        private void TransferParameters(OracleCommand cm, IDataParameter[] Params)
        {
            if (cm.Parameters.Count > 0)
            {
                cm.Parameters.Clear();
            }

            for (int i = 0; i < Params.Length; i++)
            {
                if (((OracleParameter)Params[i]).OracleDbType == OracleDbType.RefCursor)
                {
                    Params[i].Direction = ParameterDirection.Output;
                }

                cm.Parameters.Add(Params[i]);
            }
        }
        private void WriteToEventLog(string message)
        {
            string cs = ConfigurationManager.AppSettings["KEYLOG"];

            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    EventLog elog = new EventLog();

                    if (!EventLog.SourceExists(cs))
                    {
                        EventLog.CreateEventSource(cs, cs);
                    }

                    elog.Source = cs;
                    elog.EnableRaisingEvents = true;
                    elog.WriteEntry(message, EventLogEntryType.Error, 7637);
                    elog = null;
                }
                catch { }
            }
        }
        #endregion

    }
}
