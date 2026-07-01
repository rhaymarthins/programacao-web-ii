
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto01_IF.Data;
using Projeto01_IF.Models;
using System.Data;
using System.Runtime.ConstrainedExecution;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Authorize]
public class TbProfissionalsController : Controller
{
    private readonly db_IFContext _context;

    public TbProfissionalsController(db_IFContext context)
    {
        _context = context;
    }

    public enum Plano
    {
        MedicoTotal = 1,
        MedicoParcial = 2,
        Nutricionista = 3
    }

    // GET: TBPROFISSIONALS
    [Authorize(Roles = "GerenteMedico,GerenteNutricionista,GerenteGeral")]
    //[Authorize(Roles = "Medico, Nutricionista")]
    public IActionResult Index()    
    {
        if (User.IsInRole("GerenteGeral"))
        {
            var db_IFContextGeral = (from pro in _context.TbProfissional
                                     select new ProfissionalResumido
                                     {
                                         IdProfissional = pro.IdProfissional,
                                         Nome = pro.Nome,
                                         NomeCidade = pro.IdCidadeNavigation.Nome,
                                         NomePlano = pro.IdContratoNavigation.IdPlanoNavigation.Nome,
                                         Cpf = pro.Cpf,
                                         CrmCrn = pro.CrmCrn,
                                         Especialidade = pro.Especialidade,
                                         Logradouro = pro.Logradouro,
                                         Numero = pro.Numero,
                                         Bairro = pro.Bairro,
                                         Cep = pro.Cep,
                                         Ddd1 = pro.Ddd1,
                                         Ddd2 = pro.Ddd2,
                                         Telefone1 = pro.Telefone1,
                                         Telefone2 = pro.Telefone2,
                                         Salario = pro.Salario,
                                     });
            return View(db_IFContextGeral);
        }
        else
        {
            if (User.IsInRole("GerenteMedico"))
            {
                var db_IFContext = (from pro in _context.TbProfissional
                                    where (Plano)pro.IdContratoNavigation.IdPlano == Plano.MedicoTotal || (Plano)pro.IdContratoNavigation.IdPlano == Plano.MedicoParcial
                                    select new ProfissionalResumido
                                    {
                                        IdProfissional = pro.IdProfissional,
                                        Nome = pro.Nome,
                                        NomeCidade = pro.IdCidadeNavigation.Nome,
                                        NomePlano = pro.IdContratoNavigation.IdPlanoNavigation.Nome,
                                        Cpf = pro.Cpf,
                                        CrmCrn = pro.CrmCrn,
                                        Especialidade = pro.Especialidade,
                                        Logradouro = pro.Logradouro,
                                        Numero = pro.Numero,
                                        Bairro = pro.Bairro,
                                        Cep = pro.Cep,
                                        Ddd1 = pro.Ddd1,
                                        Ddd2 = pro.Ddd2,
                                        Telefone1 = pro.Telefone1,
                                        Telefone2 = pro.Telefone2,
                                        Salario = pro.Salario,
                                    });
                return View(db_IFContext);
            }
            else
            {
                if (User.IsInRole("GerenteNutricionista"))
                {
                    var db_IFContext2 = (from pro in _context.TbProfissional
                                         where (Plano)pro.IdContratoNavigation.IdPlano == Plano.Nutricionista
                                         select new ProfissionalResumido
                                         {
                                             IdProfissional = pro.IdProfissional,
                                             Nome = pro.Nome,
                                             NomeCidade = pro.IdCidadeNavigation.Nome,
                                             NomePlano = pro.IdContratoNavigation.IdPlanoNavigation.Nome,
                                             Cpf = pro.Cpf,
                                             CrmCrn = pro.CrmCrn,
                                             Especialidade = pro.Especialidade,
                                             Logradouro = pro.Logradouro,
                                             Numero = pro.Numero,
                                             Bairro = pro.Bairro,
                                             Cep = pro.Cep,
                                             Ddd1 = pro.Ddd1,
                                             Ddd2 = pro.Ddd2,
                                             Telefone1 = pro.Telefone1,
                                             Telefone2 = pro.Telefone2,
                                             Salario = pro.Salario,
                                         });

                    return View(db_IFContext2);
                }
                else
                {
                    return Forbid();
                }
            }
        }
        //return View(await db_IFContext.ToListAsync());
    }

    // GET: TBPROFISSIONALS/Details/5
    //[Authorize(Roles = "Medico, Nutricionista")]
    [Authorize(Roles = "GerenteMedico,GerenteNutricionista,GerenteGeral")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        TbProfissional? tbProfissional = await _context.TbProfissional
            .Include(t => t.IdCidadeNavigation)
            .Include(t => t.IdContratoNavigation)
            .Include(t => t.IdTipoAcessoNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.IdProfissional == id);
        if (tbProfissional == null)
        {
            return NotFound();
        }

        return View(tbProfissional);
    }

    // GET: TBPROFISSIONALS/Create
    [Authorize(Roles = "Medico,Nutricionista")]
    public IActionResult Create()
    {
        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome");
        ViewData["IdPlano"] = new SelectList(_context.TbPlano, "IdPlano", "Nome");
        ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome");
        return View();
    }

    // POST: TBPROFISSIONALS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> Create([Bind("IdTipoProfissional,IdTipoAcesso,IdCidade,IdUser,Nome,Cpf,CrmCrn,Especialidade,Logradouro,Numero,Bairro,Cep,Ddd1,Ddd2,Telefone1,Telefone2,Salario")] TbProfissional tbProfissional, [Bind("IdPlano")] TbContrato IdContratoNavigation)
    {
        try
        {
            ModelState.Remove("IdUser");
            ModelState.Remove("IdContrato");

            if (ModelState.IsValid)
            {
                IdContratoNavigation.DataInicio = DateTime.UtcNow;
                IdContratoNavigation.DataFim = IdContratoNavigation.DataInicio.Value.AddMonths(1);
                _context.Add(IdContratoNavigation);
                await _context.SaveChangesAsync();

                var userManager = HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>();
                if (userManager != null)
                {
                    var email = User.Identity?.Name;
                    if (email != null)
                    {
                        var user = await userManager.FindByEmailAsync(email);
                        if (user != null)
                        {
                            tbProfissional.IdUser = user.Id;
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

                tbProfissional.IdContrato = IdContratoNavigation.IdContrato;
                _context.Add(tbProfissional);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
        catch (DbUpdateException dex)
        {
            ModelState.AddModelError("", "Incapaz de salvar." + dex.ToString());
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Erro Geral." + ex.ToString());
        }
        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
        ViewData["IdPlano"] = new SelectList(_context.TbPlano, "IdPlano", "Nome", IdContratoNavigation.IdPlano);
        ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome", tbProfissional.IdTipoAcesso);
        return View(tbProfissional);
    }

    // GET: TBPROFISSIONALS/Edit/5
    [Authorize(Roles = "GerenteMedico,GerenteNutricionista,GerenteGeral")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("Error", "Home");
        }

        var tbProfissional = await _context.TbProfissional.Include(t => t.IdContratoNavigation).FirstOrDefaultAsync(s => s.IdProfissional == id);
        if (tbProfissional == null)
        {
            return NotFound();
        }
        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
        ViewData["IdContrato"] = new SelectList(_context.TbPlano, "IdPlano", "Nome", tbProfissional.IdContratoNavigation.IdPlano);
        ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome", tbProfissional.IdTipoAcesso);
        return View(tbProfissional);
    }

    // POST: TBPROFISSIONALS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "GerenteMedico,GerenteNutricionista,GerenteGeral")]
    public async Task<IActionResult> EditPost(int? idprofissional)
    {
        if(idprofissional == null)
        {
            return NotFound();
        }
        var tbProfissional = await _context.TbProfissional.Include(s =>s.IdCidadeNavigation).FirstOrDefaultAsync(s => s.IdProfissional == idprofissional);
        //if (idprofissional != tbprofissional.IdProfissional)
        //{
        //    return NotFound();
        //}
        if (tbProfissional == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<TbProfissional>(
        tbProfissional,
        "",
        s => s.IdProfissional, s => s.IdTipoAcesso, s => s.IdCidade, s => s.Nome, s => s.Cpf, s => s.CrmCrn,
        s => s.Especialidade, s => s.Logradouro, s => s.Numero, s => s.Bairro, s => s.Cep,
        s => s.Ddd1, s => s.Ddd2, s => s.Telefone1, s => s.Telefone2, s => s.Salario))
        {
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator." + ex.ToString());
            }
        }
        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
        ViewData["IdContrato"] = new SelectList(_context.TbPlano, "IdPlano", "Nome", tbProfissional.IdContratoNavigation.IdPlano);
        ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome", tbProfissional.IdTipoAcesso);
        return View(tbProfissional);
    }

    // GET: TBPROFISSIONALS/Delete/5
    [Authorize(Roles = "GerenteMedico,GerenteNutricionista,GerenteGeral")]
    public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tbProfissional = await _context.TbProfissional
            .Include(t => t.IdCidadeNavigation)
            .ThenInclude(s => s.IdEstadoNavigation)
            .Include(t => t.IdTipoAcessoNavigation)
            .Include(t => t.IdContratoNavigation)
            .ThenInclude(s => s.IdPlanoNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.IdProfissional == id);
        if (tbProfissional == null)
        {
            return NotFound();
        }
        if (saveChangesError.GetValueOrDefault())
        {
            ViewData["ErrorMessage"] =
            "Não foi possível excluir. Este profissional possui pacientes cadastrados, ou ocorreu um erro ao salvar.";
        }
        return View(tbProfissional);
    }

    // POST: TBPROFISSIONALS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "GerenteMedico,GerenteNutricionista,GerenteGeral")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var tbProfissional = await _context.TbProfissional.FindAsync(id);
        if (tbProfissional == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var temPacientes = await _context.TbMedicoPaciente.AnyAsync(mp => mp.IdProfissional == id);
        if (temPacientes)
        {
            return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
        }

        try
        {
            _context.TbProfissional.Remove(tbProfissional);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
        }
    }

    // //////////////////////////////////////////////////////////////
    // Ações exclusivas do próprio profissional (Médico/Nutricionista)
    // Diferente das ações acima, que são de uso dos Gerentes
    // //////////////////////////////////////////////////////////////
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> MeuPerfil()
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
                        .Include(t => t.IdCidadeNavigation)
                        .Include(t => t.IdContratoNavigation)
                        .FirstOrDefaultAsync(p => p.IdUser == user.Id);

                    if (tbProfissional != null)
                    {
                        return View("Details", tbProfissional);
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

    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> EditarMeuPerfil()
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
                        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
                        return View(tbProfissional);
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

    [HttpPost, ActionName("EditarMeuPerfil")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Medico,Nutricionista")]
    public async Task<IActionResult> EditarMeuPerfilPost(int? IdProfissional)
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

                    if (tbProfissional != null && tbProfissional.IdProfissional == IdProfissional)
                    {
                        if (await TryUpdateModelAsync<TbProfissional>(
                            tbProfissional,
                            "",
                            p => p.IdTipoProfissional, p => p.IdTipoAcesso, p => p.IdCidade,
                            p => p.Nome, p => p.CrmCrn, p => p.Especialidade, p => p.Logradouro,
                            p => p.Numero, p => p.Bairro, p => p.Cep, p => p.Ddd1, p => p.Ddd2,
                            p => p.Telefone1, p => p.Telefone2))
                        {
                            try
                            {
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(MeuPerfil));
                            }
                            catch (DbUpdateException ex)
                            {
                                ModelState.AddModelError("", "Não foi possível salvar. " + ex.ToString());
                            }
                        }

                        ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
                        return View(tbProfissional);
                    }
                    else
                    {
                        return Forbid();
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
