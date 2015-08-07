using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperProblem.Models;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace DapperProblem
{
    public class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            var clientSurvey = p.GetDefaultAsync(1).Result;
            Console.WriteLine("Id = {0}", clientSurvey.Id);
            Console.WriteLine("SurveyId = {0}", clientSurvey.SurveyId);
            Console.ReadLine();
        }

        public async Task<ClientSurvey> GetDefaultAsync(int clientId)
        {
            //  This returns correct data (Id = 3)                       
            const string sql2 =
                @"select *
                  from dbo.ClientSurveys cs
                  where cs.ClientId = @clientId
                    and cs.IsPatientDefault = 1
                    and isnull(cs.EndDate, '9999-1-1') > getdate() 
                    and isnull(cs.StartDate, '2000-1-1') < getdate()
                    and exists (select * from dbo.Surveys s where s.Id = cs.SurveyId and s.IsActive = 1);";

            //  This returns incorrect data (Id = 2)                       
            const string sql =
                @"select cs.*
                  from dbo.ClientSurveys cs
                  join dbo.Surveys s on s.Id = cs.SurveyId
                  where cs.ClientId = @clientId
                    and cs.IsPatientDefault = 1
                    and isnull(cs.EndDate, '9999-1-1') > getdate() 
                    and isnull(cs.StartDate, '2000-1-1') < getdate() 
                    and s.IsActive = 1;";

            using (var cnn = await OpenConnectionAsync())
            {
                var surveys = (await cnn.QueryAsync<ClientSurvey>(sql, new { ClientId = clientId })).ToList();
                if (!surveys.Any())
                {
                    return null;
                }
                if (surveys.Count() > 1)
                {
                    throw new Exception("Multiple default surveys for client " + clientId);
                }
                var clientSurvey = surveys.First();
                return clientSurvey;
            }
        }

        protected async Task<SqlConnection> OpenConnectionAsync()
        {
            var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            await cnn.OpenAsync();
            return cnn;
        }

    }
}
