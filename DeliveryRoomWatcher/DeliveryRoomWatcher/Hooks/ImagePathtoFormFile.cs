using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Clinic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Hooks
{
    public class ImagePathtoFormFile
    {
      
        public IFormFile ConvertFIle(ClinicModel clinic)
        {
            ImageToFileModel productViewModel =new ImageToFileModel();
            string path =clinic.imagpath;
            using (var stream = File.OpenRead(path))
            {

                productViewModel.Name = clinic.imagename;

                return productViewModel.File = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));

                
            }
}
    }
}
