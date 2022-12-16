using Entities.ErrorModel;
using Entities.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        public IActionResult ProcessError(ApiBaseResponse baseResponse)
        {
            return baseResponse switch
            {
                ApiNotFoundResponse => NotFound(new ErrorDetail
                {
                    Message = ((ApiNotFoundResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status404NotFound
                }),
                ApiBadRequestResponse => BadRequest(new ErrorDetail
                {
                    Message = ((ApiBadRequestResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status400BadRequest
                })
            };
        }
    }
}
