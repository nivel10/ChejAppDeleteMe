namespace ChejAppDeleteMe.Services
{
    using System.Threading.Tasks;

    public class DialogService
    {
        /// <summary>
        /// Metodo que hace el envio de mensajes de sistema
        /// </summary>
        /// <param name="title">String titulo del mensaje</param>
        /// <param name="message">String contenido del mensaje</param>
        /// <param name="button">String boton del mensaje</param>
        public async Task ShowMessage(string title, string message, string button)
        {
            await App.Current.MainPage.DisplayAlert(title, message, button);
        }
    }
}
