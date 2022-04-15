using FlameAndWax.Data.Constants;
using FlameAndWax.Services.Services;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlameAndWax.Images.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImagesController(IImageService imageService, IWebHostEnvironment webHostEnvironment)
        {
            _imageService = imageService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [Route("BasePath")]
        public IActionResult BasePath()
        {
            var basePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\products");
            return Ok(basePath);
        }      
        [HttpGet]
        [Route("BaseUrl")]
        public IActionResult BaseUrl()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}"; ;
            return Ok(baseUrl);
        }
    }
}
