using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teste_Wipro.Models;


namespace Teste_Wipro.Controllers
{
    [Route("api/[controller]")]
    public class FilmeController : Controller
    {
        private readonly G360Context _context;

        public FilmeController(G360Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListFilmes")]
        public async Task<ActionResult<IEnumerable<Filme>>> ListFilmes()
        {
            try
            {
                return await _context.Filme.Where(X => X.Excluido == false).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Filme")]
        public async Task<ActionResult<Filme>> Filme(int id)
        {
            try
            {

                Filme filme = await _context.Filme.Where(x => x.FilmeId == id && x.Excluido == false).FirstOrDefaultAsync();

                if (filme == null)
                {
                    return NotFound("Filme não encontrado");
                }
                return filme;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CadastrarFilme")]
        public async Task<ActionResult> CadastrarFilme([FromBody] Filme filme)
        {
            try
            {
                if (filme == null)
                {
                    return BadRequest("Nenhum dado informado");
                }

                if (_context.Filme.Where(x => x.Nome.ToLower() == filme.Nome.ToLower()).ToList().Count() > 0)
                {
                    return BadRequest("Filme já cadastrado");
                }

                filme.Alugado = false;
                filme.Excluido = false;
                filme.DataCadastro = DateTime.Now;

                _context.Filme.Add(filme);
                await _context.SaveChangesAsync();


                return Ok("Filme cadastrado com sucesso");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut]
        [Route("EditarFilme")]
        public async Task<ActionResult> EditarFilme([FromBody] Filme filme)
        {

            try
            {
                if (filme == null)
                {
                    return BadRequest("Nenhum dado informado");
                }

                _context.Entry(filme).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Dados alterados com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeletarFilme")]
        public async Task<ActionResult> DeletarFilme([FromBody] Filme filme)
        {
            try
            {
                if (filme == null)
                {
                    return BadRequest("Nenhum dado informado");
                }

                filme.Excluido = true;
                filme.DataExcluido = DateTime.Now;

                _context.Entry(filme).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok("Excluido com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
