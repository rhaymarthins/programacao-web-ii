# Projeto01_IF

## Tarefa de Acompanhamento 01

Seguindo as etapas da aula 1, foram encontradas dificuldades para acompanhar o que o professor estava fazendo nas videoaulas, sendo necessárias algumas modificações para o correto funcionamento do projeto. Inicialmente estava sendo utilizada a versão Visual Studio 2026 (18.6.2), e posteriormente foi tentado também com o Visual Studio Community 2022 (17.14.34). As alterações necessárias foram as mesmas nas duas versões.

---

### 1. Program.cs
De:
```csharp
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
```
Para:
```csharp
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
```

---

### 2. ApplicationDbContext.cs
De:
```csharp
public class ApplicationDbContext : IdentityDbContext
```
Para:
```csharp
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
```

---

### 3. Views/Shared/_LoginPartial.cshtml
De:
```csharp
@using Microsoft.AspNetCore.Identity
@inject SignInManager<IdentityUser> SignInManager
@inject UserManager<IdentityUser> UserManager
```
Para:
```csharp
@using Microsoft.AspNetCore.Identity
@using Projeto01_IF.Data
@inject SignInManager<ApplicationUser> SignInManager
@inject UserManager<ApplicationUser> UserManager
```

---

### 4. Areas/Identity/Pages/_ViewStart.cshtml
De:
```csharp
Layout = "/Pages/Shared/_Layout.cshtml";
```
Para:
```csharp
Layout = "/Views/Shared/_Layout.cshtml";
```

---

### 5. TbAlimentosController - parâmetro de rota
De:
```csharp
public async Task<IActionResult> Edit(int? idalimento)
public async Task<IActionResult> Delete(int? idalimento)
public async Task<IActionResult> DeleteConfirmed(int? idalimento)
```
Para:
```csharp
public async Task<IActionResult> Edit(int? id)
public async Task<IActionResult> Delete(int? id)
public async Task<IActionResult> DeleteConfirmed(int? id)
```

---

### 6. TbAlimentosController - Bind
De:
```csharp
[Bind("IdAlimento,IdTipoQuantidade,Nome,Carboidrato,VitaminaA,VitaminaB,TbReceitaAlimentarPadraoXAlimento")]
```
Para:
```csharp
[Bind("IdAlimento,IdTipoQuantidade,Nome,Carboidrato,VitaminaA,VitaminaB")]
```

---

### 7. TbAlimentosController - Views
Removido o campo `TbReceitaAlimentarPadraoXAlimento` das Views Create, Edit, Details, Delete e Index pois é uma propriedade de navegação (relacionamento com outra tabela) e não uma coluna direta.

---

### 8. TbContratosController - parâmetro de rota
De:
```csharp
public async Task<IActionResult> Edit(int? idcontrato)
public async Task<IActionResult> Delete(int? idcontrato)
public async Task<IActionResult> DeleteConfirmed(int? idcontrato)
```
Para:
```csharp
public async Task<IActionResult> Edit(int? id)
public async Task<IActionResult> Delete(int? id)
public async Task<IActionResult> DeleteConfirmed(int? id)
```

---

### 9. TbContratosController - Bind
De:
```csharp
[Bind("IdContrato,IdPlano,DataInicio,DataFim,IdPlanoNavigation,TbProfissional")]
```
Para:
```csharp
[Bind("IdContrato,IdPlano,DataInicio,DataFim")]
```

---

### 10. TbContratosController - Views
Removidos os campos `IdPlanoNavigation` e `TbProfissional` das Views Create, Edit, Details, Delete e Index pois são propriedades de navegação (relacionamentos com outras tabelas) e não colunas diretas.

---

### 11. Views/TbAlimentos/ e Views/TbContratos/
Pastas renomeadas de `TbAlimento` e `TbContrato` para `TbAlimentos` e `TbContratos`, pois o Scaffold gerou no singular mas os Controllers foram gerados no plural.

---
