﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDevSteamJau.Data;
using APIDevSteamJau.Models;

namespace APIDevSteamJau.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhosController : ControllerBase
    {
        private readonly APIContext _context;

        public CarrinhosController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Carrinhos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carrinho>>> GetCarrinhos()
        {
            return await _context.Carrinhos.ToListAsync();
        }

        // GET: api/Carrinhos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Carrinho>> GetCarrinho(Guid id)
        {
            var carrinho = await _context.Carrinhos.FindAsync(id);

            if (carrinho == null)
            {
                return NotFound();
            }

            return carrinho;
        }

        // PUT: api/Carrinhos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarrinho(Guid id, Carrinho carrinho)
        {
            if (id != carrinho.CarrinhoId)
            {
                return BadRequest();
            }

            _context.Entry(carrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarrinhoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Carrinhos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Carrinho>> PostCarrinho(Carrinho carrinho)
        {
            // Data e hora atual
            carrinho.DataCriacao = DateTime.Now;
            carrinho.DataFinalizacao = null; // Inicializa como nulo
            carrinho.Finalizado = false;

            //Valor total inicial
            carrinho.ValorTotal = 0;

            _context.Carrinhos.Add(carrinho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarrinho", new { id = carrinho.CarrinhoId }, carrinho);
        }

        // DELETE: api/Carrinhos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrinho(Guid id)
        {
            var carrinho = await _context.Carrinhos.FindAsync(id);
            if (carrinho == null)
            {
                return NotFound();
            }

            _context.Carrinhos.Remove(carrinho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarrinhoExists(Guid id)
        {
            return _context.Carrinhos.Any(e => e.CarrinhoId == id);
        }

        [HttpPost]
        [Route("FinalizarCompra/{id}")]
        public async Task<IActionResult> FinalizarCompra(Guid id)
        {
            //Se o carrinho de compra existe
            var carrinho = await _context.Carrinhos.FindAsync(id);
            if (carrinho == null)
            {
                return NotFound();
            }

            // Verifica se o carrinho já foi finalizado
            carrinho.Finalizado = true;
            carrinho.DataFinalizacao = DateTime.Now;
            _context.Entry(carrinho).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
