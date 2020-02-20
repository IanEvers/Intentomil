using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Intentomil.Modelos;

namespace Intentomil.Controllers
{
    [Route("api/HolaMundo")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            return config;
        }

        private SqlConnection Conexion()
        {
            var json = InitConfiguration(); //Traemos appsettings.json con datos de la DB
            var connetionString = string.Format("Data Source={0};Initial Catalog={1};MultipleActiveResultSets=true;User ID={2};Password={3}", json["DBIp"], json["DBName"], json["DBUsuario"], json["DBContrasena"]);
            var connectPersonal = new SqlConnection(connetionString);
            connectPersonal.Open();
            return connectPersonal;
        }
        private List<string> DevolverLicencias()
        {
            string queryString = "SELECT * FROM dbo.Licencias;";
            SqlCommand command = new SqlCommand(queryString, Conexion());
            List<string> broker = new List<string>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string broki = Convert.ToString(reader["Nombre"]);
                    broker.Add(broki);
                    broki = Convert.ToString(reader["ID"]);
                    broker.Add(broki);
                    broki = Convert.ToString(reader["Fecha_de_Inicio"]);
                    broker.Add(broki);
                    broki = Convert.ToString(reader["Fecha_de_Fin"]);
                    broker.Add(broki);
                    broki = Convert.ToString(reader["Tipo"]);
                    broker.Add(broki);
                    broki = Convert.ToString(reader["Provincia"]);
                    broker.Add(broki);
                    broki = Convert.ToString(reader["Localidad"]);
                    broker.Add(broki);
                    broki = Convert.ToString(reader["Dirección"]);
                    broker.Add(broki);
                    broki = Convert.ToString(reader["Orden_Del_Día"]);
                    broker.Add(broki);
                    broki = Convert.ToString(reader["dni"]);
                    broker.Add(broki);
                }
            }
            return broker;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<string> broki = DevolverLicencias();
            return broki;
        }
        [HttpGet("cantidad")]
        public ActionResult<string> Get([FromBody]Licencias destino)
        {
            var pepe = destino;
            var json = InitConfiguration(); //Traemos appsettings.json con datos de la DB
            var connetionString = string.Format("Data Source={0};Initial Catalog={1};MultipleActiveResultSets=true;User ID={2};Password={3}", json["DBIp"], json["DBName2"], json["DBUsuario"], json["DBContrasena"]);
            var connectPersonal = new SqlConnection(connetionString);
            connectPersonal.Open();
            string queryString = "";
            if (destino.destino == "" && destino.subyofi== "")
            {
                queryString = "SELECT COUNT (dni) FROM datos_head;";
            }
            else if (destino.destino != "" && destino.subyofi == "")
            {
                queryString = $"SELECT COUNT (dni) FROM datos_head WHERE destino = {destino.destino}";
            }
            else if (destino.destino != "" && destino.subyofi == "oficial")
            {
               queryString = $"SELECT COUNT (dni) FROM datos_head WHERE destino = {destino.destino} AND tipo_grado = 'OFICIAL'";
            }
            else if (destino.destino != "" && destino.subyofi == "suboficial")
            {
                queryString = $"SELECT COUNT (dni) FROM datos_head WHERE destino = {destino.destino} AND tipo_grado = 'SUBOFICIAL'";
            }
            else if (destino.destino == "" && destino.subyofi == "oficial")
            {
                queryString = $"SELECT COUNT (dni) FROM datos_head WHERE tipo_grado = 'OFICIAL'";
            }
            else if (destino.destino == "" && destino.subyofi == "suboficial")
            {
                queryString = $"SELECT COUNT (dni) FROM datos_head WHERE tipo_grado = 'SUBOFICIAL'";
            }

            SqlCommand command = new SqlCommand(queryString, connectPersonal);
            Int32 count = (Int32)command.ExecuteScalar();
            return $"el numero que busca es:  {count}";
        }
        // POST api/values
        [HttpPost]
        public string Post([FromBody] Licencias licencias)
        {
            licencias.FechaInicio = Convert.ToDateTime(licencias.preFechaInicio);
            licencias.FechaFin = Convert.ToDateTime(licencias.preFechaFin);
            string[] licenciasvariables = { licencias.Nombre, licencias.Localidad, licencias.Ordendeldia, licencias.Direccion, licencias.preFechaFin, licencias.preFechaInicio, licencias.Provincia, licencias.Tipo };
            bool cargar = true;
            int i = 0;
            foreach(string elemento in licenciasvariables)
            { 
                if (String.IsNullOrEmpty(elemento))
                {
                    cargar = false;
                }
                i++;
            }
            if(licencias.dni == 0)
            {
                cargar = false;
            }
            if(cargar)
            { 
                string queryString = $"INSERT INTO dbo.Licencias (Nombre, Fecha_de_Inicio, Fecha_de_Fin, Tipo, Provincia, Localidad, Dirección, Orden_del_día, dni) VALUES ('{licencias.Nombre}','{licencias.FechaInicio} ','{licencias.FechaFin}','{licencias.Tipo}','{licencias.Provincia} ','{licencias.Localidad} ','{licencias.Direccion} ','{licencias.Ordendeldia} ', '{licencias.dni}')";
                SqlCommand command = new SqlCommand(queryString, Conexion());
                List<string> broker = new List<string>();
                SqlDataReader reader = command.ExecuteReader();
                return "agregada, crack";
            }
            else
            {
                return "no esta completo";
            }
        }

        // PUT api/values/5
        [HttpPut]
        public string Put([FromBody] Licencias licencias)
        {
            licencias.FechaInicio = Convert.ToDateTime(licencias.preFechaInicio);
            licencias.FechaFin = Convert.ToDateTime(licencias.preFechaFin);
            string[] columnas = { "Nombre", "Fecha_de_Inicio", "Fecha_de_Fin", "Tipo", "Provincia", "Localidad", "Dirección", "Orden_del_día", "dni"};
            string[] sentencias = {"", "", "", "", "", "", "", "", ""};
            string[] licenciasvariables = { "licencias.Nombre", "licencias.FechaInicio", "licencias.FechaFin", "licencias.Tipo", "licencias.Provincia", "licencias.Localidad", "licencias.Direccion", "licencias.Ordendeldia", "licencias.dni" };
            string[] licenciasvariables2 = { licencias.Nombre, Convert.ToString(licencias.FechaInicio), Convert.ToString(licencias.FechaFin), licencias.Tipo, licencias.Provincia, licencias.Localidad, licencias.Direccion, licencias.Ordendeldia, Convert.ToString(licencias.dni) };
            bool hayDatoAnterior = false;
            if (String.IsNullOrEmpty(licencias.Nombre))
            {
                sentencias[0] = "";
            }
            else
            {
                sentencias[0] = $" {columnas[0]} = '{licenciasvariables[0]}' ";
                hayDatoAnterior = true;
            }
            if (Convert.ToString(licencias.FechaInicio) == "1/1/0001 00:00:00")
            {
                    sentencias[1] = "";
            }
            else
            {   
                if(hayDatoAnterior == true)
                {
                sentencias[1] = $", {columnas[1]} = '{licencias.FechaInicio}' ";
                }
                else
                {
                    sentencias[1] = $" {columnas[1]} = '{licencias.FechaInicio}' ";
                    hayDatoAnterior = true;
                }
            }
            if (Convert.ToString(licencias.FechaFin) == "1/1/0001 00:00:00")
            {
                    sentencias[2] = "";
            }
            else
            {
                if (hayDatoAnterior == true)
                {
                    sentencias[2] = $", {columnas[2]} = '{licencias.FechaFin}' ";

                }
                else
                {
                    sentencias[2] = $" {columnas[2]} = '{licencias.FechaFin}' ";
                    hayDatoAnterior = true;
                }
            }
            if (string.IsNullOrEmpty(licencias.Tipo))
            {
                sentencias[3] = "";
            }
            else
            {
                if (hayDatoAnterior == true)
                {
                    sentencias[3] = $", {columnas[3]} = '{licencias.Tipo}' ";

                }
                else
                {
                    sentencias[3] = $" {columnas[3]} = '{licencias.Tipo}' ";
                    hayDatoAnterior = true;
                }
            }
            if (String.IsNullOrEmpty(licencias.Provincia))
            {
                sentencias[4] = "";
            }
            else
            {
                if (hayDatoAnterior == true)
                {
                    sentencias[4] = $", {columnas[4]} ='{licencias.Provincia}' ";

                }
                else
                {
                    sentencias[4] = $" {columnas[4]} = '{licencias.Provincia}' ";
                    hayDatoAnterior = true;
                }
            }
            if (String.IsNullOrEmpty(licencias.Localidad))
            {
                sentencias[5] = "";
            }
            else
            {
                if (hayDatoAnterior == true)
                {
                    sentencias[5] = $", {columnas[5]} = '{licencias.Localidad}' ";

                }
                else
                {
                    sentencias[5] = $" {columnas[5]} = '{licencias.Localidad}' ";
                    hayDatoAnterior = true;
                }
            }
            if (String.IsNullOrEmpty(licencias.Direccion))
            {
                sentencias[6] = "";
            }
            else
            {
                if (hayDatoAnterior == true)
                {
                    sentencias[6] = $", {columnas[6]} = '{licencias.Direccion}' ";
                }
                else
                {
                    sentencias[6] = $" {columnas[6]} = '{licencias.Direccion}' ";
                    hayDatoAnterior = true;
                }
            }
            if (String.IsNullOrEmpty(licencias.Ordendeldia))
            {
                sentencias[7] = "";
            }
            else
            {
                if (hayDatoAnterior == true)
                {
                    sentencias[7] = $", {columnas[7]} = '{licencias.Ordendeldia}' ";

                }
                else
                {
                    sentencias[7] = $" {columnas[7]} = '{licencias.Ordendeldia}' ";
                    hayDatoAnterior = true;
                }
            }
            if (licencias.dni == 0)
            {
                sentencias[8] = "";
            }
            else
            {
                if (hayDatoAnterior == true)
                {
                    sentencias[8] = $", {columnas[8]} ='{licencias.dni}' ";

                }
                else
                {
                    sentencias[8] = $" {columnas[8]} = '{licencias.dni}' ";
                    hayDatoAnterior = true;
                }
            }
           
            string queryString = $"UPDATE dbo.Licencias SET {sentencias[0]}{sentencias[1]}{sentencias[2]}{sentencias[3]}{sentencias[4]}{sentencias[5]}{sentencias[6]}{sentencias[7]}{sentencias[8]} WHERE ID=" +licencias.Id;
            SqlCommand command = new SqlCommand(queryString, Conexion());
            List<string> broker = new List<string>();
            SqlDataReader reader = command.ExecuteReader();
            return "modificada, crack";
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<string>> Delete(int id)
        {
            string queryString = "DELETE FROM dbo.Licencias WHERE ID="+id;
            SqlCommand command = new SqlCommand(queryString, Conexion());
            List<string> broker = new List<string>();
            SqlDataReader reader = command.ExecuteReader();
            return  DevolverLicencias();
        }
    }
}