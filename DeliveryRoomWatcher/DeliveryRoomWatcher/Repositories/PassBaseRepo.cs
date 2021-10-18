using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.Passbase;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class PassBaseRepo
    {
        //public ResponseModel GET_VERIFICATIONS(PassBaseModel passbase)
        //{
        //    using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
        //    {
        //        con.Open();
        //        using (var tran = con.BeginTransaction())
        //        {
        //            try
        //            {
        //                var data = con.Query($@"", passbase,
        //                  transaction: tran
        //                    );

        //                return new ResponseModel
        //                {
        //                    success = true,
        //                    data = data
        //                };
        //            }
        //            catch (Exception e)
        //            {
        //                return new ResponseModel
        //                {
        //                    success = false,
        //                    message = $@"External server error. {e.Message.ToString()}",
        //                };
        //            }

        //        }
        //    }

        //}
    }
}
