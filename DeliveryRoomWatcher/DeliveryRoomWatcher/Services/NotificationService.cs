using CorePush.Google;
using Microsoft.Extensions.Options;
using DeliveryRoomWatcher.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Models.FCM;
using static DeliveryRoomWatcher.Models.FCM.GoogleNotification;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.IO;

namespace DeliveryRoomWatcher.Services
{
    public interface INotificationService
    {
        Task<FCMResponseModel> SendNotificationDoctors(NotificationModel notificationModel);
        Task<FCMResponseModel> SendNotificationEmployee(NotificationModel notificationModel);
        Task<FCMResponseModel> SendNotificationToPremiereClient(NotificationModel notificationModel);
        Task<FCMResponseModel> SendNotificationToPremiereWorkBench(NotificationModel notificationModel);
        FCMResponseModel SendMessage(NotificationModel notificationModel);
    }

    public class NotificationService : INotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        public NotificationService(IOptions<FcmNotificationSetting> settings)
        {
            _fcmNotificationSetting = settings.Value;
        }
        public FCMResponseModel SendMessage(NotificationModel notificationModel)
        {

            FCMResponseModel response = new FCMResponseModel();
            string serverKey = "AAAAmYbHshY:APA91bGqSGOZ_ux0NDEbpIAZsqX3upXPuNuuqQPKm2BQ--EvqpHFVlvCiMTwOdukzpX_yAIP2QSbHhfD1Q0FG7FMA525awvLh4XtdAKceB--CHTiWOv_EUVlRUeUBHufVau2_2Hlh5l1";
            foreach (var f in notificationModel.DeviceId)
            {
                var notificationInputDto = new
                {
                    to = notificationModel.DeviceId,
                    body = notificationModel.Body,
                    title = notificationModel.Title,
                    icon = ""
                };
                try
                {

                    var result = "";
                    var webAddr = "https://fcm.googleapis.com/fcm/send";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Headers.Add("Authorization", "key=" + serverKey);
                    httpWebRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(JsonConvert.SerializeObject(notificationInputDto));
                        streamWriter.Flush();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    response.IsSuccess = true;
                    response.Message = "Notification sent successfully";
                    return response;

                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = ex.Message;
                    return response;
                }

            }
            response.IsSuccess = true;
            response.Message = "Notification sent successfully";
            return response;
        }
        public async Task<FCMResponseModel> SendNotificationDoctors(NotificationModel notificationModel)
        {
            FCMResponseModel response = new FCMResponseModel();


            try
            {

                if (notificationModel.IsAndroiodDevice)
                {
                    var success = false;
                    foreach (var f in notificationModel.DeviceId)
                    {
                        /* FCM Sender (Android Device) */
                        FcmSettings settings = new FcmSettings()
                    {
                        SenderId = "659391230486",
                        ServerKey = "AAAAmYbHshY:APA91bGqSGOZ_ux0NDEbpIAZsqX3upXPuNuuqQPKm2BQ--EvqpHFVlvCiMTwOdukzpX_yAIP2QSbHhfD1Q0FG7FMA525awvLh4XtdAKceB--CHTiWOv_EUVlRUeUBHufVau2_2Hlh5l1"
                    };
                    HttpClient httpClient = new HttpClient();
                 
                        string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                        string deviceToken = f;
                              
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;
                  
                        var fcm = new FcmSender(settings, httpClient);

                        var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                        if (fcmSendResponse.IsSuccess())
                        {
                            success = true;
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    if (success)
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Notification Error";
                        return response;
                    }
                }

                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }

                   
                
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
           
        }   
        public async Task<FCMResponseModel> SendNotificationEmployee(NotificationModel notificationModel)
        {
            FCMResponseModel response = new FCMResponseModel();


            try
            {

                if (notificationModel.IsAndroiodDevice)
                {
                    var success = false;
                    var message = "";
                    foreach (var f in notificationModel.DeviceId)
                    {
                        /* FCM Sender (Android Device) */
                        FcmSettings settings = new FcmSettings()
                    {
                        SenderId = "121962556831",
                        ServerKey = "AAAAHGWI7Z8:APA91bFreoQk2EBOKfBjbG_APmI6oKyM1XSupLtzxAvTjrK08oM0H90ZcVfeLN-lpzXAjj15VKCNu5PaE2Sd_q0uOYaiqChphDOOJCHB6EF_mMR2dKIsgEjMnllbCrUylwpB6L4n0Hp4"
                        };
                    HttpClient httpClient = new HttpClient();
                 
                        string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                        string deviceToken = f;
                              
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;
                  
                        var fcm = new FcmSender(settings, httpClient);

                        var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                        if (fcmSendResponse.IsSuccess())
                        {
                            success = true;
                            message = fcmSendResponse.ToString();


                        }
                        else
                        {
                            success = false;
                            message = settings.ToString();
                        }
                    }
                    if (success)
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = message;
                        return response;
                    }
                }

                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }

                   
                
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
           
        }      
        public async Task<FCMResponseModel> SendNotificationToPremiereClient(NotificationModel notificationModel)
        {
            FCMResponseModel response = new FCMResponseModel();


            try
            {

                if (notificationModel.IsAndroiodDevice)
                {
                    var success = false;
                    var message = "";
                    foreach (var f in notificationModel.DeviceId)
                    {
                        /* FCM Sender (Android Device) */
                        FcmSettings settings = new FcmSettings()
                    {
                        SenderId = "761101685941",
                        ServerKey = "AAAAsTUyFLU761101685941:APA91bGMVwQqvE_aol--jviij5L0Zx4TprYa9INNB049VBzfxsrp0tvcMxYB323bOdi5yvjOdi8asa-slvRMZS4lfkmFFHs7p0zQ00sdzE27uCbyzY0u0kI58AzU_dXK96awe6ty-U2f"
                        };
                    HttpClient httpClient = new HttpClient();
                 
                        string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                        string deviceToken = f;
                              
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;
                  
                        var fcm = new FcmSender(settings, httpClient);

                        var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                        if (fcmSendResponse.IsSuccess())
                        {
                            success = true;
                            message = fcmSendResponse.ToString();


                        }
                        else
                        {
                            success = false;
                            message = settings.ToString();
                        }
                    }
                    if (success)
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = message;
                        return response;
                    }
                }

                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }

                   
                
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
           
        }  
        public async Task<FCMResponseModel> SendNotificationToPremiereWorkBench(NotificationModel notificationModel)
        {
            FCMResponseModel response = new FCMResponseModel();


            try
            {

                if (notificationModel.IsAndroiodDevice)
                {
                    var success = false;
                    var message = "";
                    foreach (var f in notificationModel.DeviceId)
                    {
                        /* FCM Sender (Android Device) */
                        FcmSettings settings = new FcmSettings()
                    {
                        SenderId = "316368513521",
                        ServerKey = "AAAASakIafE:APA91bHXdO-NammMU2rCJmSF3BrW0LCExscF9zib8pfDAX9J5XOndHIFClOEz1c4hOsTY8j058TX-dZns3-0z_FqSGeHXXD92GGAnG1ER9hIKgpgrnbDzU2XGuXiJ3tjxCk-B09i_8no"
                        };
                    HttpClient httpClient = new HttpClient();
                 
                        string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                        string deviceToken = f;
                              
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;
                  
                        var fcm = new FcmSender(settings, httpClient);

                        var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                        if (fcmSendResponse.IsSuccess())
                        {
                            success = true;
                            message = fcmSendResponse.ToString();


                        }
                        else
                        {
                            success = false;
                            message = settings.ToString();
                        }
                    }
                    if (success)
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = message;
                        return response;
                    }
                }

                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }

                   
                
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
           
        }


    }
}
