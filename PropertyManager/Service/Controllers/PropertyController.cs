using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Service.property;
using Service.property.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropertyController : ControllerBase
    {
        ILogger<PropertyController> _logger;
        IPropertyManager _propertyManager;

        public PropertyController(ILogger<PropertyController> logger, IPropertyManager propertyManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _propertyManager = propertyManager ?? throw new ArgumentNullException(nameof(propertyManager));
        }

        [HttpGet(Name = "{id}")]
        public async Task<ActionResult<IProperty>> Get(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var property = await _propertyManager.GetAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return new OkObjectResult(property);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        { 
            if (id <= 0)
            {
                return NotFound();
            }

            bool result = await _propertyManager.DeleteAsync(id);

            if (result)
                return Ok();

            return NotFound();
        }
    }
}
