using System.Collections.Generic;
using GearboxService.Models;

namespace GearboxService.Services
{
    public interface ITwitterService
    {
        List<string> GetCodes();
    }
}