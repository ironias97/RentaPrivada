using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RutinaenC.Modelos
{
    public class Beneficiario
    {
        public int IdBeneficiario { get; set; }
        public int CodigoParentesco { get; set; }
        public string FechaNacimiento { get; set; }
        public int AniosFechaNacimiento { get; set; }
        public int MesesFechaNacimiento { get; set; }
        public int DiasFechaNacimiento { get; set; }
        public bool HijoMenor { get; set; }
        public double PorcentajeLegal { get; set; }
        public double PorcentajePension { get; set; }
        public string Sexo { get; set; }
        public string TipoInvalidez { get; set; }
        public string DerechoPension { get; set; }

    }
}
