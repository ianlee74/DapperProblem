using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperProblem.Models;
using System.Data.SqlClient;
using System.Configuration;
//using Dapper;

namespace DapperProblem
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** NOTE:  CORRECT results will be   Id = 3, SurveyId = 1 ***");
            Console.WriteLine("***        INCORRECT results will be Id = 1, SurveyId = 1 ***");
            Console.WriteLine();

            var p = new Program();

            //Console.WriteLine("*** [Dapper] Get POCO results via non-secure connection. ***");
            //var clientSurvey = p.GetDapperResultsAsync("NoDdmConnection").Result;
            //Console.WriteLine("Id = {0}", clientSurvey.Id);
            //Console.WriteLine("SurveyId = {0}", clientSurvey.SurveyId);
            //Console.WriteLine();

            //Console.WriteLine("*** [Dapper] Get POCO results via secure connection. ***");
            //clientSurvey = p.GetDapperResultsAsync("DdmConnection").Result;
            //Console.WriteLine("Id = {0}", clientSurvey.Id);
            //Console.WriteLine("SurveyId = {0}", clientSurvey.SurveyId);
            //Console.WriteLine();

            //Console.WriteLine("*** [Dapper] Get results using ExecuteReaderAsync() via non-secure connection. ***");
            //p.GetDapperDataReaderResultsAsync("NoDdmConnection").Wait();
            //Console.WriteLine();

            //Console.WriteLine("*** [Dapper] Get results using ExecuteReaderAsync() via secure connection. ***");
            //p.GetDapperDataReaderResultsAsync("DdmConnection").Wait();

            Console.WriteLine("*** [Native] Get results via non-secure connection. ***");
            p.GetNativeDataReaderResultsAsync("NoDdmConnection").Wait();
            Console.WriteLine();

            Console.WriteLine("*** [Native] Get results via secure connection. ***");
            p.GetNativeDataReaderResultsAsync("DdmConnection").Wait();

            Console.ReadLine();
        }

        // NOTE: This query does not reference the Patients table where the Dynamic Data Mask is applied.
        const string sql =
            @"select cs.*
                from dbo.ClientSurveys cs
                join dbo.Surveys s on s.Id = cs.SurveyId
                where cs.ClientId = 1
                and isnull(cs.EndDate, '9999-1-1') > getdate() 
                and isnull(cs.StartDate, '2000-1-1') < getdate() 
                and s.IsActive = 1;";

        //public async Task<ClientSurvey> GetDapperResultsAsync(string connectionStringName)
        //{
        //    using (var cnn = await OpenConnectionAsync(connectionStringName))
        //    {
        //        var surveys = (await cnn.QueryAsync<ClientSurvey>(sql)).ToList();
        //        var clientSurvey = surveys.First();
        //        return clientSurvey;
        //    }
        //}

        //public async Task GetDapperDataReaderResultsAsync(string connectionStringName)
        //{
        //    using (var cnn = await OpenConnectionAsync(connectionStringName))
        //    {
        //        var dr = await cnn.ExecuteReaderAsync(sql, cnn);
        //        while(dr.Read())
        //        {
        //            for (var i = 0; i < dr.FieldCount; i++)
        //            {
        //                Console.WriteLine(string.Format("{0}: {1}", dr.GetName(i), dr.GetValue(i)));
        //            }
        //        }
        //        dr.Close();
        //    }
        //}

        public async Task GetNativeDataReaderResultsAsync(string connectionStringName)
        {
            using (var cnn = await OpenConnectionAsync(connectionStringName))
            {
                var cmd = new SqlCommand(sql, cnn);
                var dr = await cmd.ExecuteReaderAsync();
                while (dr.Read())
                {
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        Console.WriteLine(string.Format("{0}: {1}", dr.GetName(i), dr.GetValue(i)));
                    }
                }
                dr.Close();
            }
        }

        protected async Task<SqlConnection> OpenConnectionAsync(string connectionStringName) 
        {
            var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString); 
            await cnn.OpenAsync(); 
            return cnn; 
        } 
    }
}
