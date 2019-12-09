using System.Collections.Generic;
using GearboxService.Models;

namespace GearboxService.Services
{
    public interface IShiftService
    {
        List<RedeemResponse> SubmitCodes(List<string> codes);
    }
}