using Microsoft.AspNetCore.Mvc;
using Taxdown.API.Mappers;
using Taxdown.API.Models;
using Taxdown.ApplicationServices;

namespace Taxdown.API.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService,
            ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<List<CustomerResponse>>> GetAll()
        {
            var domainCustomers = await _customerService.GetAllAsync();
            // Convert domain -> API response
            var response = domainCustomers
                .Select(c => c.ToResponseModel())
                .ToList();
            return Ok(response);
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetById([FromRoute] string id)
        {
            var domainCustomer = await _customerService.GetByIdAsync(id);
            if (domainCustomer == null) return NotFound();

            var response = domainCustomer.ToResponseModel();
            return Ok(response);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> Create([FromBody] CustomerRequest request)
        {
            // Convert API request -> domain model
            var domainCustomer = request.ToDomainModel();

            var created = await _customerService.CreateAsync(domainCustomer);
            var response = created.ToResponseModel();
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] CustomerRequest request)
        {
            // Map request -> domain
            var domainCustomer = request.ToDomainModel(existingId: id);

            try
            {
                await _customerService.UpdateAsync(domainCustomer);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            await _customerService.DeleteAsync(id);
            return NoContent();
        }

        // POST: api/customers/{id}/add-credit?amount=100
        [HttpPost("{id}/add-credit")]
        public async Task<ActionResult> AddCredit([FromRoute] string id, [FromQuery] decimal amount)
        {
            try
            {
                await _customerService.AddCreditAsync(id, amount);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/customers/sorted
        [HttpGet("sorted")]
        public async Task<ActionResult<List<CustomerResponse>>> GetAllSorted()
        {
            var domainCustomers = await _customerService.GetAllSortedByCreditAsync();
            var response = domainCustomers
                .Select(c => c.ToResponseModel())
                .ToList();
            return Ok(response);
        }
    }