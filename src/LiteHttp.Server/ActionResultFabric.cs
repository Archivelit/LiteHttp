namespace LiteHttp.Server;

public class ActionResultFabric : IActionResultFactory
{
    public IActionResult Ok() => 
        new ActionResult(ResponseCode.Ok);
    
    public IActionResult BadRequest() => 
        new ActionResult(ResponseCode.BadRequest);
    
    public IActionResult NotFound() => 
        new ActionResult(ResponseCode.NotFound);
    
    public IActionResult InternalServerError() => 
        new ActionResult(ResponseCode.InternalServerError);
}