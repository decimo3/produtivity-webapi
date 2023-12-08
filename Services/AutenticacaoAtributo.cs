namespace backend.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using backend.Models;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AutenticacaoAtributo : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (Funcionario?)context.HttpContext.Items["User"];
        // Se a variável 'user' for nulo, quer dizer que o usuário não está logado!
        if (user == null) context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
    }
}
