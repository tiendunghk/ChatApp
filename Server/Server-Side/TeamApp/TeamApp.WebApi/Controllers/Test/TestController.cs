using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using TeamApp.Application.Exceptions;
using TeamApp.Application.Interfaces;
using TeamApp.Application.Wrappers;
using TeamApp.Infrastructure.Persistence.Entities;
using TeamApp.Infrastructure.Persistence.Helpers;
using TeamApp.WebApi.Extensions;
using static TeamApp.Application.Utils.Extensions;
using RadomString = TeamApp.Application.Utils.Extensions.RadomString;
using Task = System.Threading.Tasks.Task;

namespace TeamApp.WebApi.Controllers.Test
{
    [ApiController]
    [Route("api/test")]
    public class TestController : Controller
    {
        private readonly IAuthenticatedUserService authenticatedUserService;
        private readonly TeamAppContext _dbContext;
        private readonly IWebHostEnvironment _environment;


        public TestController(IAuthenticatedUserService _authenticatedUserService, TeamAppContext dbContext, IWebHostEnvironment environment)
        {
            authenticatedUserService = _authenticatedUserService;
            _dbContext = dbContext;
            _environment = environment;
        }

        [Authorize]
        [HttpGet]
        public IActionResult ShowTest()
        {
            return Ok(new
            {
                Text = "Okela",
            });
        }

        //[Authorize]
        [HttpGet("userId")]
        public IActionResult GetUserId()
        {
            return Ok(
                new
                {
                    UserId = authenticatedUserService.UserId,
                });
        }

        [HttpGet("test-time")]
        public IActionResult GetTime()
        {
            return Ok(DateTime.UtcNow);
        }

        [HttpPost("upload-file")]
        public IActionResult UpLoadFile(IFormFile file)
        {
            if (file.Length > 0)
            {
                var folder = Guid.NewGuid().ToString();
                Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\" + folder);


                using (FileStream fs = System.IO.File.Create(_environment.WebRootPath +
                    "\\Upload\\" + folder + "\\" + file.FileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            return Ok(file.FileName);
        }
    }
}
