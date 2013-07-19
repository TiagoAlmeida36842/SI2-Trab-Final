﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SI2_TP.Models
{
    public class OcorrenciaDao
    {
        public IEnumerable<Ocorrencia> GetAll(int idFunc)
        {
            var list = new LinkedList<Ocorrencia>();
            var conString = ConfigurationManager.ConnectionStrings[Environment.MachineName].ConnectionString;
            using(var con = new SqlConnection(conString))
            using(var cmd = new SqlCommand("GetOcurrenciasFromFunc",con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idFunc", idFunc);
                SqlDataAdapter adapter = new SqlDataAdapter {SelectCommand = cmd};
                var ds = new DataSet();
                adapter.Fill(ds);
                DataRowCollection drc = ds.Tables[0].Rows;
                foreach (DataRow row in drc)
                {
                    DateTime t1 = Convert.ToDateTime(row["dataHoraEnt"].ToString());
                    DateTime t2 = Convert.ToDateTime(row["dataHoraAct"].ToString());
                    list.AddLast(new Ocorrencia
                            {
                                id = Convert.ToInt32(row["id"]),
                                dataHoraEnt = t1,
                                dataHoraAct =t2 ,
                                tipo = (Tipo)Convert.ToInt32(row["tipo"]),
                                estado = (Estado)Convert.ToInt32(row["estado"]),
                                secInst = Convert.ToInt32(row["secInst"]),
                                secPiso = Convert.ToInt32(row["secPiso"]),
                                secZona = Convert.ToString(row["secZona"])
                            });
                }
            }
            return list;
        }

        public void UpdateResolucaoOcorrencia(int id)
        {
            var list = new LinkedList<Ocorrencia>();
            var conString = ConfigurationManager.ConnectionStrings[Environment.MachineName].ConnectionString;
            using (var con = new SqlConnection(conString))
            using (var cmd = new SqlCommand("GetOcurrenciasFromFunc", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }

        public Ocorrencia GetById(int idOcorrencia)
        {
            var conString = ConfigurationManager.ConnectionStrings[Environment.MachineName].ConnectionString;

            OcorrenciaDataSet ds = new OcorrenciaDataSet();
            var rows = ds.Ocorrencia.Where(row => row.id == idOcorrencia);
            return rows.Count() == 1 ? CreateOcorrencia(rows.First()) : null;


            //using (var con = new SqlConnection(conString))
            //using (var cmd = new SqlCommand("GetOcorrenciaById", con))
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@idOcorrencia", idOcorrencia);
            //    var adapter = new SqlDataAdapter {SelectCommand = cmd};
            //    var ds = new DataSet();
            //    adapter.Fill(ds);
            //    return ds.Tables[0].Rows.Count == 1 ? CreateOcorrencia(ds.Tables[0].Rows[0]) : null;
            //}
        }

        private static Ocorrencia CreateOcorrencia(OcorrenciaDataSet.OcorrenciaRow dataRow)
        {
            return new Ocorrencia
                       {
                           id = dataRow.id,
                           dataHoraAct = dataRow.dataHoraAct,
                           dataHoraEnt = dataRow.dataHoraEnt,
                           estado = (Estado)dataRow.estado,
                           tipo = (Tipo)dataRow.tipo,
                           secInst = dataRow.secInst,
                           secPiso = dataRow.secPiso,
                           secZona = dataRow.secZona
                       };
        }
    }
}