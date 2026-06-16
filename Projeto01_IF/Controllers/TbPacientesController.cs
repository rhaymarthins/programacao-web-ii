
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto01_IF.Models;
// Antonio Ray Martins Vieira
public class TbPacientesController : Controller
{
    private readonly db_IFContext _context;

    public TbPacientesController(db_IFContext context)
    {
        _context = context;
    }

    // GET: TBPACIENTES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TbPaciente.ToListAsync());
    }

    // GET: TBPACIENTES/Details/5
    public async Task<IActionResult> Details(int? idpaciente)
    {
        if (idpaciente == null)
        {
            return NotFound();
        }

        var tbpaciente = await _context.TbPaciente
            .FirstOrDefaultAsync(m => m.IdPaciente == idpaciente);
        if (tbpaciente == null)
        {
            return NotFound();
        }

        return View(tbpaciente);
    }

    // GET: TBPACIENTES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TBPACIENTES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdPaciente,Nome,Rg,Cpf,DataNascimento,NomeResponsavel,Sexo,Etnia,Endereco,Bairro,IdCidade,TelResidencial,TelComercial,TelCelular,Profissao,FlgAtleta,FlgGestante")] TbPaciente tbpaciente)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tbpaciente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tbpaciente);
    }

    // GET: TBPACIENTES/Edit/5
    public async Task<IActionResult> Edit(int? idpaciente)
    {
        if (idpaciente == null)
        {
            return NotFound();
        }

        var tbpaciente = await _context.TbPaciente.FindAsync(idpaciente);
        if (tbpaciente == null)
        {
            return NotFound();
        }
        return View(tbpaciente);
    }

    // POST: TBPACIENTES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idpaciente, [Bind("IdPaciente,Nome,Rg,Cpf,DataNascimento,NomeResponsavel,Sexo,Etnia,Endereco,Bairro,IdCidade,TelResidencial,TelComercial,TelCelular,Profissao,FlgAtleta,FlgGestante")] TbPaciente tbpaciente)
    {
        if (idpaciente != tbpaciente.IdPaciente)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tbpaciente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TbPacienteExists(tbpaciente.IdPaciente))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(tbpaciente);
    }

    // GET: TBPACIENTES/Delete/5
    public async Task<IActionResult> Delete(int? idpaciente)
    {
        if (idpaciente == null)
        {
            return NotFound();
        }

        var tbpaciente = await _context.TbPaciente
            .FirstOrDefaultAsync(m => m.IdPaciente == idpaciente);
        if (tbpaciente == null)
        {
            return NotFound();
        }

        return View(tbpaciente);
    }

    // POST: TBPACIENTES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idpaciente)
    {
        var tbpaciente = await _context.TbPaciente.FindAsync(idpaciente);
        if (tbpaciente != null)
        {
            _context.TbPaciente.Remove(tbpaciente);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TbPacienteExists(int? idpaciente)
    {
        return _context.TbPaciente.Any(e => e.IdPaciente == idpaciente);
    }
}
