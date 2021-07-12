

namespace NSE.WebAPI.Core.Identidade
{
    //Aqui nos vamos definir o que vamos guardar lá no arquivo appsettings.json
    public class AppSettings
    {
        public string Secret { get; set; } //Este é o segredo que é a noss chave.

        public int ExpiracaoHoras { get; set; } //quanto tempo este token vai durar em espirar por horas.

        public string Emissor { get; set; } //Quem é o emissor e onde ele é válido.

        public string ValidoEm { get; set; } //E onde ele é válido "seria a audiência".
    }
}
