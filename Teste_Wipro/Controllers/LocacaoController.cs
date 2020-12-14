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
    public class LocacaoController : Controller
    {
        private readonly G360Context _context;

        public LocacaoController(G360Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListLocacoes")]
        public async Task<ActionResult<IEnumerable<Locacao>>> ListLocacoes()
        {
            try
            {
                return await _context.Locacao.Where(X => X.Excluido == false).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Locacao")]
        public async Task<ActionResult<Locacao>> Locacao(int id)
        {
            try
            {

                Locacao locacao = await _context.Locacao.Where(x => x.LocacaoId == id && x.Excluido == false).FirstOrDefaultAsync();

                if (locacao == null)
                {
                    return NotFound("Locação não encontrado");
                }
                return locacao;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("RegistrarLocacao")]
        public async Task<ActionResult> RegistrarLocacao([FromBody] Locacao locacao)
        {
            try
            {
                if (locacao == null)
                {
                    return BadRequest("Nenhum dado informado");
                }

                Filme filme = await _context.Filme.Where(x => x.FilmeId == locacao.FilmeId).FirstOrDefaultAsync();

                if (filme == null)
                {
                    return BadRequest("Não foi possivel encontrar o filme na base de dados");
                }

                if (filme.Alugado == true)
                {
                    return BadRequest("Filme ja locado");
                }

                locacao.DataCadastro = DateTime.Now;
                locacao.Devolvido = false;
                locacao.Excluido = false;

                _context.Locacao.Add(locacao);
                await _context.SaveChangesAsync();

                filme.Alugado = true;
                _context.Entry(filme).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Filme locado com sucesso");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut]
        [Route("RegistrarDevolucao")]
        public async Task<ActionResult> RegistrarDevolucao([FromBody] Locacao locacao)
        {

            try
            {
                if (locacao == null)
                {
                    return BadRequest("Nenhum dado informado");
                }

                // Grava os dados da locação
                locacao.Devolvido = true;
                _context.Entry(locacao).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                Filme filme = await _context.Filme.Where(x => x.FilmeId == locacao.FilmeId).FirstOrDefaultAsync();
                filme.Alugado = false;

                // Grava os dados do filme
                _context.Entry(filme).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Verifica se teve atraso na entrega do filme
                double diffDates = (DateTime.Now - locacao.DataCadastro).TotalDays;

                if (locacao.DiasLocacao < diffDates)
                {
                    return Ok("Filme devolvido com " + diffDates + " de atraso");
                }

                return Ok("Filme devolvido sem atraso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeletarLocacao")]
        public async Task<ActionResult> DeletarLocacao([FromBody] Locacao locacao)
        {
            try
            {
                if (locacao == null)
                {
                    return BadRequest("Nenhum dado informado");
                }

                locacao.Excluido = true;
                locacao.DataExcluido = DateTime.Now;

                _context.Entry(locacao).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                Filme filme = await _context.Filme.Where(x => x.FilmeId == locacao.FilmeId).FirstOrDefaultAsync();
                filme.Alugado = false;

                // Desvincula o filme da locação deletada
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
