using LeadProducer.Models;
using LeadProducer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Shared;

namespace LeadProducer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly IKafkaProducerService _kafkaProducerService;

    public LeadsController(IKafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostLead(RealEstateRequest request)
    {
        var message = new RealEstateLead
        {
            LeadId = Guid.NewGuid(),
            Address = request.Address,
            RealEstateType = (RealEstateType)request.RealEstateType,
            LeadType = (LeadType)request.LeadType,
            Price = request.Price
        };
        
        await _kafkaProducerService.ProduceAsync(message);

        return Ok(new { Message = "Lead published", message.LeadId });
    }
}