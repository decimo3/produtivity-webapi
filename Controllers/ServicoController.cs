using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private readonly Database _context;
        public ServicoController(Database context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult PostServico(IFormFile file)
        {
            if (file.Length == 0) return BadRequest("O arquivo enviado está vazio!");
            var filemanager = new FileManager(_context, file);
            var relatorio = filemanager.Relatorio();
            try
            {
              _context.AddRange(relatorio);
              _context.SaveChanges();
              return CreatedAtAction("GetServico", null, relatorio);
            }
            catch (System.InvalidOperationException erro)
            {
              return BadRequest(erro.Message);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException erro)
            {
              return BadRequest(erro.InnerException?.Message);
            }
            catch (System.Exception erro)
            {
              return Problem(erro.Message);
            }
        }
        [HttpGet]
        public ActionResult GetServico()
        {
          var relatorios = from f in _context.relatorio group f by f.filename into g select new {filename = g.Key, servicos = g.Count(x => x.servico > 0), recursos = g.Key.Distinct().Count(), dia = g.First().dia };
          Console.WriteLine(relatorios);
          return Ok(relatorios);
        }
        [HttpDelete("{filename}")]
        public ActionResult DeleteServico(string filename)
        {
          return NoContent();
        }
    }
}
