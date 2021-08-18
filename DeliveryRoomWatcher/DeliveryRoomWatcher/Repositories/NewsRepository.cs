using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Parameters;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class NewsRepository
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
        public ResponseModel getallnews(PNews.PGetNews news)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pn.*,COUNT(pnr.reaction)AS likes FROM prem_news pn  LEFT JOIN prem_news_reaction pnr ON pn.id=pnr.news_id GROUP BY pn.id limit @offset",
                                             news,transaction: tran);
                                    
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        } 
        public ResponseModel getallnewsinfo(PNews.PGetNewsInfo news)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_news  WHERE id=@id",
                                             news,transaction: tran);

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        } 
        public ResponseModel getallnewscoment(mdlNewsComment.getallcomment comment)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT pnc.*,CONCAT(pu.firstname,' ',pu.middlename,' ',pu.lastname)fullname,pu.img FROM prem_news_comment pnc JOIN prem_usersinfo pu ON pnc.commentedby=pu.prem_id WHERE news_id=@news_id",
                                             comment, transaction: tran);
                       

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        } 
        public ResponseModel getallnewsreaction(mdlNewsReaction.getallReaction reaction)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT COUNT(reaction) AS likes FROM prem_news_reaction WHERE news_id=@news_id",
                                             reaction, transaction: tran);

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }
        public ResponseModel getNewsBymonth(PNews.PGetNews month)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_news WHERE YEAR(DATE(dateEncoded))=@year AND MONTH(DATE(dateEncoded))=@month order by dateEncoded desc limit  @offset",
                           month, transaction: tran
                            );

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }
        public ResponseModel getallnewsbyweek(PNews.PGetNewsWeek newsweek)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_news WHERE  dateEncoded >= CURDATE() - INTERVAL DAYOFWEEK(CURDATE())+6 DAY limit @offset",
                                             newsweek, transaction: tran);

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        } 
        public ResponseModel getallnewstoday(PNews.PGetNewsToday newstoday)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT * FROM prem_news WHERE  dateEncoded >= CURDATE() limit @offset",
                                             newstoday,transaction: tran);

                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }

        public ResponseModel InsertCommentNews(mdlNewsComment.addcomment addcomment)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {


                        int insert_news = con.Execute($@"INSERT INTO prem_news_comment (news_id,comment,commentedby)VALUES(@news_id,@comment,@commentedby)",
                                               addcomment, transaction: tran);
                        if (insert_news >= 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "The news comment inserted sucessfully."
                            };

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! news insertion Failed."
                            };
                        }





                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }
        public ResponseModel InsertReactionNews(mdlNewsReaction.addReaction addReaction)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        string getreactionexist = $@"SELECT * FROM prem_news_reaction WHERE news_id=@news_id AND reactedby=@reactedby";
                        var data = con.Query<string>(getreactionexist, addReaction, transaction: tran);
                        if (data.Count() == 0)
                        {
                            int insert_news = con.Execute($@"INSERT INTO prem_news_reaction (news_id,reaction,reactedby)VALUES(@news_id,@reaction,@reactedby)",
                                                   addReaction, transaction: tran);
                            if (insert_news >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "The news reaction inserted sucessfully."
                                };

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! news insertion Failed."
                                };
                            }


                        }
                        else {
                            int insert_news = con.Execute($@"update prem_news_reaction set reaction=@reaction where news_id=@news_id and reactedby=@reactedby",
                                                      addReaction, transaction: tran);
                            if (insert_news >= 0)
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "The news reaction update sucessfully."
                                };

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! news insertion Failed."
                                };
                            }
                        }




                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }

                }
            }

        }


    }
}
