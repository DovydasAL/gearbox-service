using System.Collections.Generic;
using GearboxService.Models;

namespace GearboxService.Services
{
    public interface IEmailService
    {
        void SendEmail(List<RedeemResponse> responses);
    }
}