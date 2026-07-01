
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Projeto01_IF.Data;
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
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> Index()    
    {
        var userManager = HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>();
        if (userManager != null)
        {
            var email = User.Identity?.Name;
            if (email != null)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var tbProfissional = await _context.TbProfissional
                    .FirstOrDefaultAsync(p => p.IdUser == user.Id);

                    if (tbProfissional != null)
                    {
                        var pacientes = await _context.TbMedicoPaciente
                        .Where(mp => mp.IdProfissional == tbProfissional.IdProfissional)
                        .Include(mp => mp.IdPacienteNavigation)
                        .ThenInclude(p => p.IdCidadeNavigation)
                        .Select(mp => mp.IdPacienteNavigation)
                        .ToListAsync();

                        return View(pacientes);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }
        else
        {
            return NotFound();
        }
    }

    // GET: TBPACIENTES/Details/5
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pacienteAutorizado = await VerificarVinculoAsync(id);
        if (!pacienteAutorizado)
        {
            return Forbid();
        }

        var tbpaciente = await _context.TbPaciente
                            .Include(t => t.IdCidadeNavigation)
                            .FirstOrDefaultAsync(m => m.IdPaciente == id);
        if (tbpaciente == null)
        {
            return NotFound();
        }

        return View(tbpaciente);
    }

    // método auxiliar privado para verificar vinculo entre o profissional logado e o paciente
    private async Task<bool> VerificarVinculoAsync(int? idPaciente)
    {
        var userManager = HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>();
        if (userManager == null)
        {
            return false;
        }

        var email = User.Identity?.Name;
        if (email == null)
        {
            return false;
        }

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var tbProfissional = await _context.TbProfissional
            .FirstOrDefaultAsync(p => p.IdUser == user.Id);
        if (tbProfissional == null)
        {
            return false;
        }

        var vinculoExiste = await _context.TbMedicoPaciente
            .AnyAsync(mp => mp.IdPaciente == idPaciente && mp.IdProfissional == tbProfissional.IdProfissional);

        return vinculoExiste;
    }

    // GET: TBPACIENTES/Create
    [Authorize(Roles = "Medico,Nutricionista")]
    public IActionResult Create()
    {
        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome");
        return View();
    }

    // POST: TBPACIENTES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> Create([Bind("Nome,Rg,Cpf,DataNascimento,NomeResponsavel,Sexo,Etnia,Endereco,Bairro,IdCidade,TelResidencial,TelComercial,TelCelular,Profissao,FlgAtleta,FlgGestante")] TbPaciente tbPaciente)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>();
                if (userManager != null)
                {
                    var email = User.Identity?.Name;
                    if (email != null)
                    {
                        var user = await userManager.FindByEmailAsync(email);
                        if (user != null)
                        {
                            var tbProfissional = await _context.TbProfissional
                                                    .FirstOrDefaultAsync(p => p.IdUser == user.Id);
                            if (tbProfissional != null)
                            {
                                _context.Add(tbPaciente);
                                await _context.SaveChangesAsync();

                                var vinculo = new TbMedicoPaciente
                                {
                                    IdPaciente = tbPaciente.IdPaciente,
                                    IdProfissional = tbProfissional.IdProfissional,
                                    InformacaoResumida = "Paciente cadastrado por " + tbProfissional.Nome
                                };
                                _context.Add(vinculo);
                                await _context.SaveChangesAsync();

                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
        }
        catch (DbUpdateException dex)
        {
            ModelState.AddModelError("", "Incapaz de salvar." + dex.ToString());
        }
        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbPaciente.IdCidade);
        return View(tbPaciente);
    }

    // GET: TBPACIENTES/Edit/5
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pacienteAutorizado = await VerificarVinculoAsync(id);
        if (!pacienteAutorizado)
        {
            return Forbid();
        }

        var tbPaciente = await _context.TbPaciente.FindAsync(id);
        if (tbPaciente == null)
        {
            return NotFound();
        }
        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbPaciente.IdCidade);
        return View(tbPaciente);
    }

    // POST: TBPACIENTES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> EditPost(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pacienteAutorizado = await VerificarVinculoAsync(id);
        if (!pacienteAutorizado)
        {
            return Forbid();
        }

        var tbPaciente = await _context.TbPaciente.FindAsync(id);
        if (tbPaciente == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<TbPaciente>(
        tbPaciente,
        "",
        p => p.Nome, p => p.Rg, p => p.Cpf, p => p.DataNascimento, p => p.NomeResponsavel,
        p => p.Sexo, p => p.Etnia, p => p.Endereco, p => p.Bairro, p => p.IdCidade,
        p => p.TelResidencial, p => p.TelComercial, p => p.TelCelular, p => p.Profissao,
        p => p.FlgAtleta, p => p.FlgGestante))
        {
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator." + ex.ToString());
            }
        }
        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbPaciente.IdCidade);
        return View(tbPaciente);
    }

    // GET: TBPACIENTES/Delete/5
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pacienteAutorizado = await VerificarVinculoAsync(id);
        if (!pacienteAutorizado)
        {
            return Forbid();
        }

        var tbpaciente = await _context.TbPaciente
            .Include(t => t.IdCidadeNavigation)
            .FirstOrDefaultAsync(m => m.IdPaciente == id);
        if (tbpaciente == null)
        {
            return NotFound();
        }

        return View(tbpaciente);
    }

    // POST: TBPACIENTES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var pacienteAutorizado = await VerificarVinculoAsync(id);
        if (!pacienteAutorizado)
        {
            return Forbid();
        }

        var tbPaciente = await _context.TbPaciente.FindAsync(id);
        if (tbPaciente == null)
        {
            return RedirectToAction(nameof(Index));
        }
        try
        {
            var vinculo = await _context.TbMedicoPaciente
                            .FirstOrDefaultAsync(mp => mp.IdPaciente == id);

            if (vinculo != null)
            {
                _context.TbMedicoPaciente.Remove(vinculo);
            }
            _context.TbPaciente.Remove(tbPaciente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
        }
    }

    private bool TbPacienteExists(int? idpaciente)
    {
        return _context.TbPaciente.Any(e => e.IdPaciente == idpaciente);
    }
}
