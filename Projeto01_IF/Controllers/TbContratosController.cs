
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto01_IF.Models;
// Antonio Ray Martins Vieira
[Authorize]
public class TbContratosController : Controller
{
    private readonly db_IFContext _context;

    public TbContratosController(db_IFContext context)
    {
        _context = context;
    }

    // GET: TBCONTRATOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TbContrato.ToListAsync());
    }

    // GET: TBCONTRATOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tbcontrato = await _context.TbContrato
            .FirstOrDefaultAsync(m => m.IdContrato == id);
        if (tbcontrato == null)
        {
            return NotFound();
        }

        return View(tbcontrato);
    }

    // GET: TBCONTRATOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TBCONTRATOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdContrato,IdPlano,DataInicio,DataFim")] TbContrato tbcontrato)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tbcontrato);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tbcontrato);
    }

    // GET: TBCONTRATOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tbcontrato = await _context.TbContrato.FindAsync(id);
        if (tbcontrato == null)
        {
            return NotFound();
        }
        return View(tbcontrato);
    }

    // POST: TBCONTRATOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("IdContrato,IdPlano,DataInicio,DataFim")] TbContrato tbcontrato)
    {
        if (id != tbcontrato.IdContrato)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tbcontrato);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TbContratoExists(tbcontrato.IdContrato))
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
        return View(tbcontrato);
    }

    // GET: TBCONTRATOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tbcontrato = await _context.TbContrato
            .FirstOrDefaultAsync(m => m.IdContrato == id);
        if (tbcontrato == null)
        {
            return NotFound();
        }

        return View(tbcontrato);
    }

    // POST: TBCONTRATOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var tbcontrato = await _context.TbContrato.FindAsync(id);
        if (tbcontrato != null)
        {
            _context.TbContrato.Remove(tbcontrato);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TbContratoExists(int? idcontrato)
    {
        return _context.TbContrato.Any(e => e.IdContrato == idcontrato);
    }
}
