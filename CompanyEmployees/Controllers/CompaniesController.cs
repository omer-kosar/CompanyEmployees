using Application.Commands.Company;
using Application.Notifications.Company;
using Application.Queries.Company;
using CompanyEmployees.ActionFilters;
using CompanyEmployees.Extensions;
using CompanyEmployees.ModelBinders;
using Entities.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController : ApiControllerBase
    {
        private readonly IServiceManager _service;
        private readonly ISender _sender;
        private readonly IPublisher _publisher;
        public CompaniesController(IServiceManager service, ISender sender, IPublisher publisher)
        {
            _service = service;
            _sender = sender;
            _publisher = publisher;
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _sender.Send(new GetCompaniesQuery(false));
            return Ok(companies);
        }
        [HttpGet("{id:guid}", Name = "CompanyById")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _sender.Send(new GetCompanyQuery(id, false));
            return Ok(company);
        }
        [HttpPost]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("Company object is null");
            var createdCompanyDto = await _sender.Send(new CreateCompanyCommand(company));
            return CreatedAtRoute("CompanyById", new { id = createdCompanyDto.Id }, createdCompanyDto);
        }
        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = await _service.CompanyService.GetByIdsAsync(ids, false);
            return Ok(companies);
        }
        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _publisher.Publish(new CompanyDeletedNotification(id, false));
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            if (company is null)
                return BadRequest("Company object is null");
            await _sender.Send(new UpdateCompanyCommand(id, company, true));
            return NoContent();
        }
    }
}
