/*Importa o namespace onde está o componente raiz App da aplicação. Sem isso,
o compilador não encontra a classe App usada mais adiante.*/
using BlazorStudyng.Components;

/*Cria o builder da aplicação, que é o objeto responsável por configurar serviços e o pipeline HTTP. 
O args são os argumentos de linha de comando passados ao executar a aplicação.*/
var builder = WebApplication.CreateBuilder(args);


// Adicione serviços ao contêiner.


// Habilita o suporte a componentes Razor (.razor)
builder.Services.AddRazorComponents();

/*Adiciona suporte ao modo Server-Side Blazor, onde a lógica dos componentes roda no servidor e a
UI é sincronizada via SignalR (WebSocket)*/
builder.Services.AddRazorComponents();

/*Finaliza o registro de serviços e constrói a instância da aplicação (WebApplication). 
Após essa linha, não é mais possível adicionar novos serviços.*/
var app = builder.Build();

// Configure o pipeline de requisição HTTP.
// Em ambiente de produção (não development)
if (!app.Environment.IsDevelopment())
{
    // Redireciona exceções não tratadas para a rota /Error
    // CreateScopeForErrors: true — cria um novo escopo de DI ao tratar o erro (evita problemas com serviços Scoped)
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // Adiciona o header Strict-Transport-Security, forçando o browser a usar HTTPS por 30 dias
    app.UseHsts();
}

/*Intercepta respostas com códigos de erro HTTP (404, 403, etc.) e re-executa o pipeline usando a rota /not-found,
renderizando uma página de erro amigável sem redirecionar o usuário.*/
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

//Redireciona automaticamente todas as requisições HTTP para HTTPS.
app.UseHttpsRedirection();

/*Habilita a proteção contra ataques CSRF (Cross-Site Request Forgery).
É obrigatório em aplicações Blazor que usam formulários interativos.*/
app.UseAntiforgery();

/*Mapeia e serve os arquivos estáticos da aplicação (CSS, JS, imagens, etc.) de forma otimizada.
No .NET 9+, substituiu o UseStaticFiles() com melhor performance e fingerprinting automático.*/
app.MapStaticAssets();


app.MapRazorComponents<App>() //// define App como o componente raiz da aplicação, mapeando as rotas dos demais componentes a partir dele
    .AddInteractiveServerRenderMode(); // habilita o modo de renderização interativa via SignalR para os componentes que usarem @rendermode InteractiveServer

/*/Inicia o servidor HTTP e bloqueia a thread principal,
mantendo a aplicação em execução e escutando as requisições.*/
app.Run();