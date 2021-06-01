using Microsoft.AspNetCore.Mvc;
using nginxconfigurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nginxconfigurator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NginxController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Helper.RestartNginx();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostNginxConfig([FromBody] NginxConfigModel nginxConfigModel)
        {

            try
            {
                string text = Helper.CreateServerConfigPart(nginxConfigModel);
                await Helper.WriteFile("test.txt", text);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}
