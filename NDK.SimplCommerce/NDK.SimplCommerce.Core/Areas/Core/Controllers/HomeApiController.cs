using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NDK.SimplCommerce.Core.Areas.Core.Controllers;
[Area("Core")]
[Route("api/home")]
public class HomeApiController : Controller
{
    private readonly ILogger<HomeApiController> _logger;

    public HomeApiController(ILogger<HomeApiController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    [HttpGet("index")]
    public IActionResult Index()
    {
        return View();
    }
}
