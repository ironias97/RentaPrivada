using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RutinaenC.Modelos
{
    public class TipoAjustado
    {
        public int Mes { get; set; }
        public int Ki { get; set; }
        public int Paso { get; set; }
        public List<TipoAjustado> ListaTipoAjustado { get; set; }

        public TipoAjustado()
        {
            // Enero
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 1,
                Ki = 3,
                Paso = 3
            });

            // Febrero
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 2,
                Ki = 2,
                Paso = 3
            });

            // Marzo
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 3,
                Ki = 1,
                Paso = 2
            });

            // Abril
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 4,
                Ki = 3,
                Paso = 3
            });

            // Mayo
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 5,
                Ki = 2,
                Paso = 3
            });

            // Junio
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 6,
                Ki = 1,
                Paso = 2
            });

            // Julio
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 7,
                Ki = 3,
                Paso = 3
            });

            // Agosto
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 8,
                Ki = 2,
                Paso = 3
            });

            // Septiembre
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 9,
                Ki = 1,
                Paso = 2
            });

            // Octubre
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 10,
                Ki = 3,
                Paso = 3
            });

            // Noviembre
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 11,
                Ki = 2,
                Paso = 3
            });
            
            // Diciembre
            ListaTipoAjustado.Add(new TipoAjustado()
            {
                Mes = 12,
                Ki = 1,
                Paso = 2
            });
        }
    }
}
