using CompanyEmployees.ActionFilters;
using CompanyEmployees.Extensions;
using CompanyEmployees.ModelBinders;
using Entities.Responses;
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

        public CompaniesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompanies()
        {
            var baseResult = await _service.CompanyService.GetAllCompaniesAsync(false);
            var companies = baseResult.GetResult<IEnumerable<CompanyDto>>();
            return Ok(companies);
        }
        [HttpGet("{id:guid}", Name = "CompanyById")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var baseResult = await _service.CompanyService.GetCompanyAsync(id, false);
            if (!baseResult.Success)
                return ProcessError(baseResult);
            var company = baseResult.GetResult<CompanyDto>();
            return Ok(company);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
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
            await _service.CompanyService.DeleteCompanyAsync(id, false);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _service.CompanyService.UpdateCompanyAsync(id, company, true);
            return NoContent();
        }
    }
}
