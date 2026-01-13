using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Features.WIRCheckpoints.Queries
{
   
    public record GetPredefinedChecklistItemsByWIRCodeQuery(string WIRCode) : IRequest<Result<List<PredefinedChecklistItemWithChecklistDto>>>;



}
