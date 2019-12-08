using System.Collections.Generic;

namespace GearboxService.Services
{
    public interface IShiftService
    {
        void SubmitCodes(List<string> codes);
    }
}