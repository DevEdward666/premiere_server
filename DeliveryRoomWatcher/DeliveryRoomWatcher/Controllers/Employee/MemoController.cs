using DeliveryRoomWatcher.Repositories.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeliveryRoomWatcher.Models.Employee;

namespace DeliveryRoomWatcher.Controllers.Employee
{
    [ApiController]
   
    public class MemoController : ControllerBase
    {
        MemoRepo _memo = new MemoRepo();


        [HttpPost]
        [Route("api/memo/getallmemo")]
        public ActionResult getallmemo()
        {
            return Ok(_memo.getallmemo());
        }
        [HttpPost]
        [Route("api/memo/getmemoFilter")]
        public ActionResult getmemoFilter(MemoModel.FilterMemo getmemo)
        {
            return Ok(_memo.getmemoFilter(getmemo));
        }     
        [HttpPost]
        [Route("api/memo/getmemo")]
        public ActionResult getmemo(MemoModel.GetMemo getMemo)
        {
            return Ok(_memo.getmemo(getMemo));
        } 
        [HttpPost]
        [Route("api/memo/getmemoAttachMents")]
        public ActionResult getmemoAttachMents(MemoModel.GetMemoAttachments getMemo)
        {
            return Ok(_memo.getmemoAttachMents(getMemo));
        }
        [HttpPost]
        [Route("api/memo/UpdateMemoUnpublished")]
        public ActionResult UpdateMemoUnpublished([FromForm] MemoModel.Update_Memo_Published update_memo)
        {
            return Ok(_memo.UpdateMemoUnpublished(update_memo));
        }     
        [HttpPost]
        [Route("api/memo/UpdateMemo")]
        public ActionResult UpdateMemo([FromForm] MemoModel.Update_Memo update_memo)
        {
            return Ok(_memo.UpdateMemo(update_memo));
        }    
        [HttpPost]
        [Route("api/memo/UpdateMemoWithDepartment")]
        public ActionResult UpdateMemoWithDepartment([FromForm] MemoModel.Create_Memo update_memo)
        {
            return Ok(_memo.UpdateMemoWithDepartment(update_memo));
        }    [HttpPost]
        [Route("api/memo/UpdateMemoWithFiles")]
        public ActionResult UpdateMemoWithFiles([FromForm] MemoModel.Create_Memo update_memo)
        {
            return Ok(_memo.UpdateMemoWithFiles(update_memo));
        }  
        [HttpPost]
        [Route("api/memo/UpdateMemoWithFilesandDepartment")]
        public ActionResult UpdateMemoWithFilesandDepartment([FromForm] MemoModel.Create_Memo update_memo)
        {
            return Ok(_memo.UpdateMemoWithFilesandDepartment(update_memo));
        }    
        [HttpPost]
        [Route("api/memo/InsertNewMemo")]
        public ActionResult InsertNewMemo([FromForm] MemoModel.Create_Memo create_Memo)
        {
            return Ok(_memo.InsertNewMemo(create_Memo));
        }
    }
}
