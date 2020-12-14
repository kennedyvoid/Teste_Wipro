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
    public class ClienteController : Controller
    {
        private readonly G360Context _context;

        public ClienteController(G360Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListClientes")]
        public async Task<ActionResult<IEnumerable<Cliente>>> ListClientes()
        {
            try
            {
                return await _context.Cliente.Where(X => X.Excluido == false).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Cliente")]
        public async Task<ActionResult<Cliente>> Cliente(int id)
        {
            try
            {
                
                Cliente cliente = await _context.Cliente.Where(x => x.ClienteId == id && x.Excluido == false).FirstOrDefaultAsync();

                if (cliente == null)
                {
                    return NotFound("Cliente não encontrado");
                }
                return cliente;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("CadastrarCliente")]
        public async Task<ActionResult> CadastrarCliente([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    return BadRequest("Nenhum dado informado");
                }

                if (_context.Cliente.Where(x => x.Documento == cliente.Documento).ToList().Count() > 0)
                {
                    return BadRequest("Cliente já cadastrado");
                }


                cliente.Excluido = false;
                cliente.DataCadastro = DateTime.Now;

                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();


                return Ok("Cliente cadastrado com sucesso");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut]
        [Route("EditarCliente")]
        public async Task<ActionResult> EditarCliente([FromBody] Cliente cliente)
        {

            try
            {
                if (cliente == null)
                {
                    return BadRequest("Nenhum dado informado");
                }

                _context.Entry(cliente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Dados alterados com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("DeletarCliente")]
        public async Task<ActionResult> DeletarCliente([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    return BadRequest("Nenhum dado informado");
                }

                cliente.Excluido = true;
                cliente.DataExcluido = DateTime.Now;
                
                _context.Entry(cliente).State = EntityState.Modified;

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
