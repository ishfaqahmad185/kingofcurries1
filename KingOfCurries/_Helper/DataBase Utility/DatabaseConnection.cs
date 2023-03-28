using API.Model;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Utility
{
    public static class DatabaseConnection
    {
 
        public static bool GetConnectionString(string _key, IConfiguration _configuration, IOptions<MySettingsModel> _app)
        {
            bool result = false;
            // for API Connection
            var apiSection = _configuration.GetSection("APIConnection");
            var apiSectionExists = apiSection.Exists();
            if (apiSectionExists)
            {
                var apiconfigDetails = _configuration.GetSection("APIConnection").
                    GetChildren().ToList();
                _app.Value.APIDbConnection = apiconfigDetails.
                   Where(x => x.Key == "APIDbConnection").
                   Select(X => X.Value).
                   FirstOrDefault();
                result = true;
            }
            // end API Connection

            //var ConnectionDetails = DbClientFactory<ConnectionDbClient>.Instance.GetClientConnection(_app.Value.APIDbConnection, _key);
            ////var section = _configuration.GetSection(_key);
            ////var sectionExists = section.Exists();
            //if (ConnectionDetails.Rows.Count > 0)
            ////if (sectionExists)
            //{
            //    //   var configDetails = _configuration.GetSection(_key).
            //    //      GetChildren().ToList();

            //    _app.Value.DbConnection = Convert.ToString(ConnectionDetails.Rows[0]["DbConnection"]); //configDetails. )IsReservationExists.Rows[0]["RoomId"]
            //                                                                                           //Where(x => x.Key == "DbConnection").
            //                                                                                           //Select(X => X.Value).
            //                                                                                           //FirstOrDefault();
            //                                                                                           // set above
            //                                                                                           //_app.Value.APIDbConnection =configDetails.
            //                                                                                           //    Where(x => x.Key == "APIDbConnection").
            //                                                                                           //    Select(X => X.Value).
            //                                                                                           //    FirstOrDefault();


            //    _app.Value.ClientName = Convert.ToString(ConnectionDetails.Rows[0]["ClientName"]);// configDetails.
            //                                                                                      //Where(x => x.Key == "ClientName").
            //                                                                                      //Select(X => X.Value).
            //                                                                                      //FirstOrDefault();


            //    _app.Value.UserName = Convert.ToString(ConnectionDetails.Rows[0]["UserName"]);

            //    _app.Value.Password = Convert.ToString(ConnectionDetails.Rows[0]["HashPassword"]);
            //    //_app.Value.Email = configDetails.
            //    //    Where(x => x.Key == "Email").
            //    //    Select(X => X.Value).
            //    //    FirstOrDefault();


            //    //_app.Value.SMTPPort = configDetails.
            //    //    Where(x => x.Key == "SMTPPort").
            //    //    Select(X => X.Value).
            //    //    FirstOrDefault();

            //    result = true;
            //}

            //else
            //{
            //    result = false;
            //}

            return result;
        }
    }
}
