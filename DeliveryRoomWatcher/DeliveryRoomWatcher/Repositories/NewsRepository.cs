using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Hooks;
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
                        string newsheader = $@"SELECT pn.*,COUNT(pnr.reaction)AS likes,(SELECT news_image FROM prem_news_images WHERE news_id=pn.`id` LIMIT 1) AS Image,
                                            CASE WHEN (SELECT  COUNT(`comment`) FROM prem_news_comment WHERE news_id=pn.`id` GROUP BY news_id) IS NULL THEN 0 ELSE (SELECT  COUNT(`comment`) 
                                            FROM prem_news_comment WHERE news_id=pn.`id` GROUP BY news_id) END   AS totalcomments
                                            FROM prem_news pn  LEFT JOIN prem_news_reaction pnr ON pn.id=pnr.news_id GROUP BY pn.id 
                                            ORDER BY pn.dateEncoded DESC limit @offset";
                        //string newsheader = $@"SELECT pn.*,COUNT(pnr.reaction)AS likes,(SELECT news_image FROM prem_news_images WHERE news_id=pn.`id` LIMIT 1) AS Image,
                        //                    CASE WHEN (SELECT  COUNT(`comment`) FROM prem_news_comment WHERE news_id=pn.`id` GROUP BY news_id) IS NULL THEN 0 ELSE (SELECT  COUNT(`comment`) 
                        //                    FROM prem_news_comment WHERE news_id=pn.`id` GROUP BY news_id) END   AS totalcomments
                        //                    FROM prem_news pn  LEFT JOIN prem_news_reaction pnr ON pn.id=pnr.news_id WHERE YEAR(DATE(pn.dateEncoded))=YEAR(CURDATE()) AND MONTH(DATE(pn.dateEncoded))=MONTH(CURDATE()) GROUP BY pn.id 
                        //                    ORDER BY pn.dateEncoded DESC limit @offset";
                        var data = con.Query(newsheader,news,transaction: tran);
                     
                        return new ResponseModel
                        {
                            success = true,
                            data = data,
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
        public ResponseModel getallnewsimages(PNews.PGetNews news)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string newsheader = $@"SELECT * FROM prem_news_images WHERE news_id=@news_id";
                        var data = con.Query(newsheader, news, transaction: tran);

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
                        var data = con.Query($@"SELECT pnc.*,CONCAT(pu.firstname,' ',pu.middlename,' ',pu.lastname)fullname,pu.img FROM prem_news_comment pnc JOIN prem_usersinfo pu ON pnc.commentedby=pu.prem_id WHERE news_id=@news_id ORDER BY commentedat DESC  LIMIT @offset",
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
        public ResponseModel UpdateNews(setNewImage create_news)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int update_news = con.Execute($@"update prem_news set Title=@title,description=@description,author=@author,dateEncoded=now() where id=@news_id",
                                    create_news, transaction: tran);
                        if (update_news >= 0)
                        {
                            foreach (var f in create_news.news_image)
                            {
                                FileResponseModel file_upload_response = new FileResponseModel
                                {
                                    success = true
                                };

                                var proc_file_payload = new NewsFileEntity()
                                {
                                    news_id = create_news.news_id
                                };

                                file_upload_response = UseLocalFiles.UploadLocalFile(f, $@"Resources\News\{create_news.news_id}\", create_news.title); ;
                                if (!file_upload_response.success)
                                {
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = file_upload_response.message
                                    };
                                }
                                else
                                {
                                    proc_file_payload.news_image = file_upload_response.data.path;
                                    proc_file_payload.news_image_name = file_upload_response.data.name;
                                }


                                int update_file = con.Execute(@"
                                update  `prem_news_images` 
                                SET 
                                news_image=@news_image,
                                news_image_name=@news_image_name,
                                dateEncoded=NOW() where  news_id=@news_id;",
                                              proc_file_payload, transaction: tran);

                                if (update_file <= 0)
                                {
                                    tran.Rollback();
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = $"The {f.FileName} could not be saved! Please try again!"
                                    };
                                }


                                if (update_file <= 0)
                                {
                                    tran.Rollback();
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = $"The {create_news.title} could not be saved! Please try again!"
                                    };
                                }

                            }
                        }
                        tran.Commit();
                        return new ResponseModel
                        {
                            success = true,
                            message = "News updated sucessfully."
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

        public ResponseModel UpdateNewsWithoutImage(setNewImage create_news)
        {

            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        int update_news = con.Execute($@"update prem_news set Title=@title,description=@description,author=@author,dateEncoded=now() where id=@news_id",
                            create_news, transaction: tran);



                        if (update_news <= 0)
                        {
                            tran.Rollback();
                            return new ResponseModel
                            {
                                success = false,
                                message = $"The {create_news.title} could not be saved! Please try again!"
                            };
                        }


                        tran.Commit();
                        return new ResponseModel
                        {
                            success = true,
                            message = "News updated sucessfully."
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


    }
}
