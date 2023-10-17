using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Form_CEDEM.BL
{
    public class PlataformaOracle : Inacap.Common.DAL.Oracle
    {
        private string NOMBREFORMULARIO = ConfigurationManager.AppSettings["NAMEFORM"];
        public DataTable ValidarSesion(string sesiCcod)
        {
            try
            {
                IDataParameter[] param = new IDataParameter[2];
                param[0] = new OracleParameter("i_sesi_ccod", OracleDbType.Varchar2);
                param[1] = new OracleParameter("o_cursor", OracleDbType.RefCursor);

                param[0].Value = sesiCcod;

                DataTable dt = null;
                this.ExecuteStoredProcedure("SIGA.PKG_RESUMEN_ACADEMICO.validar_sesion", ref param, ref dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetPreferencias(string sesiCcod)
        {
            try
            {
                IDataParameter[] param = new IDataParameter[2];
                param[0] = new OracleParameter("i_sesi_ccod", OracleDbType.Varchar2);
                param[1] = new OracleParameter("o_cursor", OracleDbType.RefCursor);

                param[0].Value = sesiCcod;

                DataTable dt = null;
                this.ExecuteStoredProcedure("SIGA.pkg_preferencias_personas.obtener_preferencias", ref param, ref dt);

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void GuardarPreferencias(string sesiCcod, string peprCdata)
        {
            try
            {
                IDataParameter[] param = new IDataParameter[2];
                param[0] = new OracleParameter("i_sesi_ccod", OracleDbType.Varchar2);
                param[1] = new OracleParameter("i_pepr_cdata", OracleDbType.Varchar2);

                param[0].Value = sesiCcod;
                param[1].Value = peprCdata;

                this.ExecuteStoredProcedure("SIGA.pkg_preferencias_personas.guardar_preferencias", ref param);
            }
            catch (Exception ex)
            {
        
            }
        }


        public DataTable GetParamsAPI(string pagen_tdesc)
        {

            IDataParameter[] param = new IDataParameter[2];
            param[0] = new OracleParameter("p_pagen_tdesc", OracleDbType.Varchar2);
            param[1] = new OracleParameter("outCur", OracleDbType.RefCursor);

            param[0].Value = pagen_tdesc;

            DataTable dt = null;
            this.ExecuteStoredProcedure("INTEGRACIONCRM.PKG_CRM_API_DATOS.GetParamsAPI", ref param, ref dt);
            return dt;
        }
        public string GetAccessToken(string apatTaplicacion)
        {
            try
            {
                DataTable dt = null;
                ExecuteSQL("select INTEGRACIONCRM.PKG_CRM_API_DATOS.GetAccessToken('" + apatTaplicacion + "') from dual", ref dt);
                return dt.Rows[0][0].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public void GuardarAccessToken(string apatTtoken, string apatTaplicacion, DateTime apatFguardado, DateTime apatFexpiracion, string apatIurl)
        {
            try
            {
                IDataParameter[] param = new IDataParameter[6];
                param[0] = new OracleParameter("i_apat_ttoken", OracleDbType.Varchar2);
                param[1] = new OracleParameter("i_apat_taplicacion", OracleDbType.Varchar2);
                param[2] = new OracleParameter("i_apat_fguardado", OracleDbType.Date);
                param[3] = new OracleParameter("i_apat_fexpiracion", OracleDbType.Date);
                param[4] = new OracleParameter("i_audi_tusuario", OracleDbType.Varchar2);
                param[5] = new OracleParameter("i_apat_iurl", OracleDbType.Varchar2);

                param[0].Value = apatTtoken;
                param[1].Value = apatTaplicacion;
                param[2].Value = apatFguardado;
                param[3].Value = apatFexpiracion;
                param[4].Value = NOMBREFORMULARIO;
                param[5].Value = apatIurl;

                this.ExecuteStoredProcedure("INTEGRACIONCRM.PKG_CRM_API_DATOS.GuardarAccessToken", ref param);
            }
            catch (Exception ex) { }
        }
        public DataTable GetParamsformulario(string pagen_tdesc)
        {

            IDataParameter[] param = new IDataParameter[2];
            param[0] = new OracleParameter("p_pagen_tdesc", OracleDbType.Varchar2);
            param[1] = new OracleParameter("outCur", OracleDbType.RefCursor);

            param[0].Value = pagen_tdesc;

            DataTable dt = null;
            this.ExecuteStoredProcedure("INTEGRACIONCRM.PKG_CRM_FORMULARIOS.GetParamsServicesformularios", ref param, ref dt);
            return dt;
        }
        public string GetVarApp(string p_pagen_tdesc, string var_name)
        {
            try
            {
                DataTable dt = null;
                ExecuteSQL("select INTEGRACIONCRM.PKG_CRM_API_DATOS.GetVarApp('" + p_pagen_tdesc + "','" + var_name + "') from dual", ref dt);
                return dt.Rows[0][0].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public void Insertar_log_aplicacion(string json, string servicio, string url, string url_api, string token, string aplicacion)
        {
            try
            {
                IDataParameter[] param = new IDataParameter[6];
                param[0] = new OracleParameter("i_json", OracleDbType.Varchar2);
                param[1] = new OracleParameter("i_servicio", OracleDbType.Varchar2);
                param[2] = new OracleParameter("i_url", OracleDbType.Varchar2);
                param[3] = new OracleParameter("i_url_api", OracleDbType.Varchar2);
                param[4] = new OracleParameter("i_token", OracleDbType.Varchar2);
                param[5] = new OracleParameter("i_aplicacion", OracleDbType.Varchar2);


                param[0].Value = json;
                param[1].Value = servicio;
                param[2].Value = url;
                param[3].Value = url_api;
                param[4].Value = token;
                param[5].Value = aplicacion;

                this.ExecuteStoredProcedure("INTEGRACIONCRM.PKG_CRM_FORMULARIOS.Insertar_log_aplicacion", ref param);
            }
            catch (Exception ex) { }
        }
        public void Insertar_log_error(string p_parametro, string p_aplicacion)
        {
            try
            {
                IDataParameter[] param = new IDataParameter[2];
                param[0] = new OracleParameter("i_parametro", OracleDbType.Varchar2);
                param[1] = new OracleParameter("i_aplicacion", OracleDbType.Varchar2);

                param[0].Value = p_parametro;
                param[1].Value = p_aplicacion;


                this.ExecuteStoredProcedure("INTEGRACIONCRM.PKG_CRM_API_DATOS.Insertar_log", ref param);
            }
            catch (Exception ex) { }
        }
    }
}
