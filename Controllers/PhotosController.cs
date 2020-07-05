using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Vega.Controllers.Resources;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Controllers
{
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IHostingEnvironment host;
        private readonly IVehicleRepository repository;
        private readonly IMapper mapper;
        private readonly IPhotoRepository photoRepository;
        private readonly IPhotoService photoService;
        private readonly PhotoSettings photoSettings;

        [Obsolete]
        public PhotosController(IHostingEnvironment host, IVehicleRepository repository, IMapper mapper, IOptionsSnapshot<PhotoSettings> options, IPhotoRepository photoRepository, IPhotoService photoService)
        {
            this.photoSettings = options.Value;
            this.host = host;
            this.repository = repository;
            this.mapper = mapper;
            this.photoRepository = photoRepository;
            this.photoService = photoService;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {

            var photos = await photoRepository.GetPhotos(vehicleId);

            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await repository.GeVehicle(vehicleId, includeRelated: false);
            if (vehicle == null)
                return NotFound();

            if (file == null) return BadRequest("Null file");
            if (file.Length == 0) return BadRequest("Empty File");
            if (file.Length > photoSettings.MaxBytes) return BadRequest("Maximum file size exceeded!");
            if (!photoSettings.IsSupported(file.FileName))
                return BadRequest("Invalid FIle Type");

            var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");

            var photo = await photoService.UploadPhoto(vehicle, file, uploadsFolderPath);

            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}
