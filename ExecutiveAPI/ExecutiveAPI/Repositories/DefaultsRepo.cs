﻿using Dapper;
using ExecutiveAPI.Config;
using ExecutiveAPI.Model.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveAPI.Repositories
{
    public class DefaultsRepo
    {
        public ResponseModel hospitalLogo()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        string logo = Convert.ToBase64String(con.QuerySingle<byte[]>($@"
                                        SELECT hosplogo  FROM hospitallogo WHERE hospcode = GetDefaultValue('hospinitial')
                                        "
                                         , null, transaction: tran));
                        return new ResponseModel
                        {
                            success = true,
                            data = logo
                        };
                    }
                }

            }
            catch (Exception err)
            {

                return new ResponseModel
                {
                    success = false,
                    message = err.Message
                };
            }
        }
        public ResponseModel GetHospitalName()
        {
            try
            {
                using MySqlConnection con = new MySqlConnection(DatabaseConfig.GetConnection());
                con.Open();
                using var tran = con.BeginTransaction();
                var data = con.QuerySingle<string>($@"SELECT val FROM `def_val` WHERE remarks = 'hosp_name'"
                                 , null, transaction: tran);
                return new ResponseModel
                {
                    success = true,
                    data = data
                };

            }
            catch (Exception err)
            {

                return new ResponseModel
                {
                    success = false,
                    message = err.Message
                };
            }
        }

        public ResponseModel GetHospitalInitial()
        {
            try
            {
                using MySqlConnection con = new MySqlConnection(DatabaseConfig.GetConnection());
                con.Open();
                using var tran = con.BeginTransaction();
                var data = con.QuerySingle<string>($@"SELECT val FROM `def_val` WHERE remarks = 'hosp_initial'"
                                 , null, transaction: tran);
                return new ResponseModel
                {
                    success = true,
                    data = data
                };

            }
            catch (Exception err)
            {

                return new ResponseModel
                {
                    success = false,
                    message = err.Message
                };
            }
        }
    }
}
