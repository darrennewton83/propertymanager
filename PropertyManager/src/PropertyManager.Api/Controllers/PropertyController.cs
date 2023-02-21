namespace PropertyManager.Api.Controllers
{
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using PropertyManager.Api.Dto;
    using PropertyManager.Api.ErrorResults;
    using PropertyManager.Shared.Property;
    using PropertyManager.Shared.Property.Manager;
    using PropertyManager.Shared.EntityResults;

    /// <summary>
    /// An api controller for managing the crud operations of properties
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PropertyController : ControllerBase
    {
        IPropertyManager _propertyManager;
        IMapper _mapper;

        /// <summary>
        /// Initialises a new intance of the <see cref="PropertyController"/> class.
        /// </summary>
        /// <param name="logger"><The logger/param>
        /// <param name="propertyManager">The property manager</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PropertyController(IPropertyManager propertyManager, IMapper mapper)
        {
            _propertyManager = propertyManager ?? throw new ArgumentNullException(nameof(propertyManager));
            _mapper = mapper ?? throw new ArgumentNullException();
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
        public async Task<ActionResult<PropertyDto>> Get(int id)
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

            return new OkObjectResult(_mapper.Map<PropertyDto>(property));
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

        /// <summary>
        /// Creates a new propery
        /// </summary>
        /// <param name="property">The property to create</param>
        /// <returns>The newly created property or an error message</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Save([FromBody] PropertyDto property)
        {
            if (property == null)
            {
                return BadRequest();
            }

            if (property.Id.HasValue)
            {
                return BadRequest("Id cannot be provided when creating a property.");
            }

            var savedProperty = await _propertyManager.SaveAsync(_mapper.Map<IProperty>(property));

            if (savedProperty.Type == ResultType.Error)
            {

                return new ErrorResult("The property could not be created.", _mapper);
            }

            return new ObjectResult(_mapper.Map<PropertyDto>(savedProperty.Value)) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Updates an existing propery
        /// </summary>
        /// <param name="property">The property to update</param>
        /// <returns>The newly created property or an error message</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] PropertyDto property)
        {
            if (property == null)
            {
                return BadRequest();
            }

            if (!property.Id.HasValue)
            {
                return BadRequest("Id must be provided when updating a property.");
            }

            var savedProperty = await _propertyManager.SaveAsync(_mapper.Map<IProperty>(property));

            if (savedProperty.Type == ResultType.Error)
            {

                return new ErrorResult("The property could not be updated.", _mapper);
            }

            return new ObjectResult(_mapper.Map<PropertyDto>(savedProperty.Value)) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
