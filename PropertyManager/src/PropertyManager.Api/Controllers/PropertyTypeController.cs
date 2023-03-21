using AutoMapper;

namespace PropertyManager.Api.Controllers
{
    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using PropertyManager.Api.Dto;
    using PropertyManager.Api.ErrorResults;
    using PropertyManager.Shared.EntityResults;
    using PropertyManager.Shared.PropertyType;
    using PropertyManager.Shared.PropertyType.Manager;

    /// <summary>
    /// An api controller for managing the crud operations of property types
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PropertyTypeController : ControllerBase
    {
        private IPropertyTypeManager _propertyTypeManager;
        private IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyTypeController"/> class.
        /// </summary>
        /// <param name="propertyTypeManager">The property type manager</param>
        /// <param name="mapper">The automapper instance for mapping objects</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PropertyTypeController(IPropertyTypeManager propertyTypeManager, IMapper mapper)
        {
            _propertyTypeManager = propertyTypeManager ?? throw new ArgumentNullException(nameof(propertyTypeManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets a property type
        /// </summary>
        /// <param name="id">The unique identifier of the property type to get</param>
        /// <returns>An individual property type</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyTypeDto>> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var propertyType = await _propertyTypeManager.GetAsync(id);

            if (propertyType == null)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(_mapper.Map<PropertyTypeDto>(propertyType));
        }

        /// <summary>
        /// Gets all property type
        /// </summary>
        /// <param name="id">The unique identifier of the property type to get</param>
        /// <returns>An individual property type</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PropertyTypeDto>>> Get()
        {
            var propertyTypes = await _propertyTypeManager.GetAsync();

            if (propertyTypes.IsNullOrEmpty())
            {
                return new NoContentResult();
            }

            return new OkObjectResult(_mapper.Map<IEnumerable<PropertyTypeDto>>(propertyTypes));
        }

        /// <summary>
        /// Deletes a property type
        /// </summary>
        /// <param name="id">The unique identifier of the property type to delete</param>
        /// <returns>A success return code if the property type was deleted</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var deleted = await _propertyTypeManager.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Creates a new propery type
        /// </summary>
        /// <param name="propertyType">The property type to create</param>
        /// <returns>The newly created property or an error message</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Save([FromBody]  PropertyTypeDto propertyType)
        {
            if (propertyType == null)
            {
                return BadRequest();
            }

            if (propertyType.Id.HasValue)
            {
                return BadRequest("Id cannot be provided when creating a property type.");
            }

            var savedPropertyType = await _propertyTypeManager.SaveAsync(_mapper.Map<IPropertyType>(propertyType));

            if (savedPropertyType.Type == ResultType.Error)
            {

                return new ErrorResult("The property type could not be created.", _mapper);
            }

            return new ObjectResult(savedPropertyType.Value) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Updates an existing property type
        /// </summary>
        /// <param name="propertyType">The property type to update</param>
        /// <returns>A 200 status code with the updated property type or an error message if the property type could not be updated</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] PropertyTypeDto propertyType)
        {
            if (propertyType == null)
            {
                return BadRequest();
            }

            if (!propertyType.Id.HasValue)
            {
                return BadRequest("Id must be provided when updating a property type.");
            }

            var savedPropertyType = await _propertyTypeManager.SaveAsync(_mapper.Map<IPropertyType>(propertyType));

            if (savedPropertyType.Type == ResultType.Error)
            {

                return new ErrorResult("The property type could not be updated.", _mapper);
            }

            return new ObjectResult(savedPropertyType.Value) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
