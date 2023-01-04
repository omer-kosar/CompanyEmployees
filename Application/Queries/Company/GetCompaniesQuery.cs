using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Company
{
    public sealed record GetCompaniesQuery : IRequest<IEnumerable<CompanyDto>>
    {
        public bool TrackChanges { get; set; }
        public GetCompaniesQuery(bool trackChanges)
        {
            TrackChanges = trackChanges;
        }
    }
}
