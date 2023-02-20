namespace Service.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Service.property;
    using Service.property.Manager;

    /// <summary>
    /// An api controller for managing the crud operations of properties
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PropertyController : ControllerBase
    {
        ILogger<PropertyController> _logger;
        IPropertyManager _propertyManager;

        /// <summary>
        /// Initialises a new intance of the <see cref="PropertyController"/> class.
        /// </summary>
        /// <param name="logger"><The logger/param>
        /// <param name="propertyManager">The property manager</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PropertyController(ILogger<PropertyController> logger, IPropertyManager propertyManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _propertyManager = propertyManager ?? throw new ArgumentNullException(nameof(propertyManager));
        }

        /// <summary>
        /// Gets a property 
        /// </summary>
        /// <param name="id">The unique identifer of the property type to get</param>
        /// <returns>An individual property type</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<IProperty>> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var property = await _propertyManager.GetAsync(id);

            if (property == null)
            {
                return NoContent();
            }

            return new OkObjectResult(property);
        }

        /// <summary>
        /// Deletes a property
        /// </summary>
        /// <param name="id">The unique identifier of the property to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id) 
        { 
            if (id <= 0)
            {
                return BadRequest();
            }

            bool result = await _propertyManager.DeleteAsync(id);

            if (result)
                return Ok();

            return NotFound();
        }
    }
}
