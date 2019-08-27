using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RutinaenC.Modelos;

namespace RutinaenC
{
    class RutinaActuarial
    {
        // Objetos
        RutinaPorcentaje rutinaPorcentaje = new RutinaPorcentaje();

        // Propiedades
        public string Mensaje { get; set; }

        // Constantes
        public const decimal Exp = (decimal)1 / 12;
        public const int aniosBasTM = 2017;
        public int mesesCosto = 0;
        public int mesesDiferidos = 0;
        public int mesesGarantizados = 0;

        public List<beResultados> RutinaPension(
           beDatosModalidad modeloCotizacion, List<beDatosBen> modeloBeneficiarios,
           List<beDatosTasasPar> modeloTasas, List<beTasaMercado> modeloTasasMercado,
           List<beTasaAnclaje> modeloTasasAnclaje, List<beMortalidadDinVal> modeloMortalidad,
           List<beCPK> modeloCPK, List<beRentabilidad> modeloRentabilidad,
           List<beTasaFacVac> modeloFactorVac, List<bePorcenLegales> modeloPorcentajeLegal,
           List<beTasasPromedio> modeloTasasPromedio, List<beCurvaTasas> modeloCurvaTasas)
        {
            try
            {
                beResultados resultado = new beResultados();
                RutinaPorcentaje RP = new RutinaPorcentaje();
                beDatosTasasPar tasa = new beDatosTasasPar();

                List<beResultados> resultados = new List<beResultados>();
                List<beDatosBen> ModelBenTmp = new List<beDatosBen>();
                List<Beneficiario> beneficiarios = new List<Beneficiario>();
                List<beCPK> tasasCPK = new List<beCPK>();
                List<beRentabilidad> tasasRentabilidad = new List<beRentabilidad>();
                List<Mortalidad> tablasMortalidad = new List<Mortalidad>();

                List<double> factorReajuste = new List<double>();
                List<int> gratificacion = new List<int>();

                double tasaAnclaje;
                double tasaPromedio;
                double tasaMercado;
                double sumaPorcentajePension;

                string tipoPension = modeloCotizacion.Cobertura;

                int finTab = modeloCotizacion.FinTab;
                int tipoReajuste = modeloCotizacion.TipRea;

                ObtenerMesesCotizacion(modeloCotizacion);

                beneficiarios = CargarBeneficiarios(modeloBeneficiarios, tipoPension);
                tasa = CargarTasas(modeloTasas, ref modeloCotizacion);

                tasaMercado = CargarTasasMercado(modeloTasasMercado, modeloCotizacion);
                tasaAnclaje = CargarTasasAnclaje(modeloTasasAnclaje, modeloCotizacion);
                tasaPromedio = CargarTasasPromedio(modeloTasasPromedio, modeloCotizacion);
                tasasCPK = CargarTasasCPK(modeloCPK, modeloCotizacion);
                tasasRentabilidad = CargarTasasRentabilidad(modeloRentabilidad, modeloCotizacion);
                tablasMortalidad = CargarMortalidad(modeloMortalidad, beneficiarios, finTab);
                sumaPorcentajePension = CambiarPension(modeloBeneficiarios, modeloPorcentajeLegal, modeloCotizacion);
                gratificacion = ObtenerGratificacion(modeloCotizacion);

                if (tipoReajuste == 1)
                    factorReajuste = ObtenerTipoIndexado(modeloFactorVac, modeloCotizacion, beneficiarios, gratificacion);
                else
                    factorReajuste = ObtenerTipoAjustado(modeloCotizacion, beneficiarios, gratificacion);

                return resultados;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2019-07-30
        /// Cargar información de beneficiarios
        /// </summary>
        /// <param name="modeloBeneficiarios"> Lista de Beneficiarios </param>
        /// <returns> Regresa una lista con la información de los Beneficiarios </returns>

        public List<Beneficiario> CargarBeneficiarios(List<beDatosBen> modeloBeneficiarios, string tipoPension)
        {
            Beneficiario beneficiario;
            List<Beneficiario> beneficiarios = new List<Beneficiario>();

            int codigoParentesco;
            string fechaNacimiento;

            foreach (var item in modeloBeneficiarios)
            {
                beneficiario = new Beneficiario();

                codigoParentesco = int.Parse(item.CodPar);
                fechaNacimiento = item.FecNac;

                beneficiario.IdBeneficiario = item.NumOrd;
                beneficiario.CodigoParentesco = codigoParentesco;

                if (tipoPension == "S" && codigoParentesco == 99)
                    goto Next;

                beneficiario.PorcentajeLegal = (item.PrcLeg / 100);
                beneficiario.PorcentajePension = (item.PrcPen / 100);
                beneficiario.FechaNacimiento = fechaNacimiento;

                beneficiario.AniosFechaNacimiento = int.Parse(fechaNacimiento.Substring(0, 4));
                beneficiario.MesesFechaNacimiento = int.Parse(fechaNacimiento.Substring(4, 2));
                beneficiario.DiasFechaNacimiento = int.Parse(fechaNacimiento.Substring(6, 2));

                beneficiario.Sexo = item.TipSex;
                beneficiario.TipoInvalidez = item.TipInv;
                beneficiario.DerechoPension = item.DerPen;

            // Validar desde rutina porcentaje con un bool si es el hijo menor
            // beneficiario.HijoMenor = item.HijoMenor;

            Next:
                beneficiarios.Add(beneficiario);
            }

            return beneficiarios;
        }

        /// <summary>
        /// Julieta González / Antonio Quezada
        /// 2019-08-01
        /// Obtiene la información de las tasas
        /// </summary>
        /// <param name="modeloTasas"> Lista de Tasas </param>
        /// <param name="modeloCotizacion"> Modalidad </param>
        /// <returns> Regresa un objeto con la información de las Tasas </returns>

        public beDatosTasasPar CargarTasas(List<beDatosTasasPar> modeloTasas, ref beDatosModalidad modeloCotizacion)
        {
            try
            {
                beDatosTasasPar tasa = new beDatosTasasPar();

                string tipoPension = modeloCotizacion.Tippen;
                string moneda = modeloCotizacion.CodMon;
                string codigoRegion = modeloCotizacion.CodReg;
                int tipoReajuste = modeloCotizacion.TipRea;
                double montoGastoSepelio = modeloCotizacion.MtoGS;
                double montoMoneda = modeloCotizacion.ValCam;

                var tasaGeneral = modeloTasas.Where(x => x.CodMon == moneda && x.TipPen == tipoPension && x.CodReg == codigoRegion && x.TipRea == tipoReajuste).FirstOrDefault();

                tasa.PrcTir = tasaGeneral.PrcTir;
                tasa.MtoGad = tasaGeneral.MtoGad;
                tasa.PrcDeu = tasaGeneral.PrcDeu;
                tasa.MtoImp = tasaGeneral.MtoImp;
                tasa.PrcTas = tasaGeneral.PrcTas;
                tasa.MtoGem = tasaGeneral.MtoGem;
                tasa.PrcPer = tasaGeneral.PrcPer;

                if (moneda != "NS")
                {
                    tasa.MtoGad = tasa.MtoGad / montoMoneda;
                    tasa.MtoGem = tasa.MtoGem / montoMoneda;
                    modeloCotizacion.MtoGS = montoGastoSepelio / montoMoneda;
                }

                return tasa;
            }
            catch (Exception)
            {
                Mensaje = "Problemas al hallar tasas.";

                throw;
            }
        }

        /// <summary>
        /// Julieta González / Antonio Quezada
        /// 2019-08-01
        /// Obtiene la información de las tasas de Mercado
        /// </summary>
        /// <param name="modeloTasasMercado"> Lista de Tasas de Mercado </param>
        /// <param name="modeloCotizacion"> Modalidad </param>
        /// <returns> Regresa una variable que contiene la Tasa de Mercado </returns>

        public double CargarTasasMercado(List<beTasaMercado> modeloTasasMercado, beDatosModalidad modeloCotizacion)
        {
            try
            {
                double tasaMercado = 0;

                string moneda = modeloCotizacion.CodMon;
                int tipoReajuste = modeloCotizacion.TipRea;

                var tasasMercado = modeloTasasMercado.Where(x => x.CodMon == moneda && x.TipRea == tipoReajuste).FirstOrDefault();

                tasaMercado = tasasMercado.PrcVal;

                return tasaMercado;
            }
            catch (Exception)
            {
                Mensaje = "Problemas al hallar tasas de Mercado.";

                throw;
            }
        }

        /// <summary>
        /// Julieta González / Antonio Quezada
        /// 2019-08-01
        /// Obtiene la información de las Tasas de Anclaje
        /// </summary>
        /// <param name="modeloTasasAnclaje"> Lista de Tasas de Anclaje </param>
        /// <param name="modeloCotizacion"> Modalidad </param>
        /// <returns> Regresa una variable que contiene la Tasa de Anclaje </returns>

        public double CargarTasasAnclaje(List<beTasaAnclaje> modeloTasasAnclaje, beDatosModalidad modeloCotizacion)
        {
            try
            {
                double tasaAnclaje = 0;

                string moneda = modeloCotizacion.CodMon;
                int tipoReajuste = modeloCotizacion.TipRea;

                var tasasAnclaje = modeloTasasAnclaje.Where(x => x.CodMon == moneda && x.TipRea == tipoReajuste).FirstOrDefault();

                tasaAnclaje = tasasAnclaje.PrcVal;

                return tasaAnclaje;
            }
            catch (Exception)
            {
                Mensaje = "Problemas al hallar tasas de Anclaje.";

                throw;
            }
        }

        /// <summary>
        /// Julieta González / Antonio Quezada
        /// 2019-08-01
        /// Obtiene la información de las tasas de Promedio
        /// </summary>
        /// <param name="modeloTasasPromedio"> Lista de Tasas Promedio </param>
        /// <param name="modeloCotizacion"> Modalidad </param>
        /// <returns> Regresa una variable que contiene la Tasa Promedio </returns>

        public double CargarTasasPromedio(List<beTasasPromedio> modeloTasasPromedio, beDatosModalidad modeloCotizacion)
        {
            try
            {
                string moneda = modeloCotizacion.CodMon;
                int tipoReajuste = modeloCotizacion.TipRea;
                double tasaPromedio = 0;

                var tasasPromedio = modeloTasasPromedio.Where(x => x.COD_MONEDA == moneda && x.COD_TIPPREAJUSTE == tipoReajuste).FirstOrDefault();

                tasaPromedio = tasasPromedio.MTO_VTAPROM;
                tasaPromedio = (Math.Pow((1 + (tasaPromedio / 100)), (double)Exp)) - 1;

                return tasaPromedio;
            }
            catch (Exception)
            {
                Mensaje = "Problemas al hallar tasas de Promedio.";

                throw;
            }
        }
       
        /// <summary>
        /// Julieta González / Antonio Quezada
        /// 2019-08-01
        /// Obtiene la información de las tasas CPK
        /// </summary>
        /// <param name="modeloCPK"> Lista de tasas CPK </param>
        /// <param name="modeloCotizacion"> Modalidad </param>
        /// <returns> Regresa una lista con la información de las tasas CPK </returns>

        public List<beCPK> CargarTasasCPK(List<beCPK> modeloCPK, beDatosModalidad modeloCotizacion)
        {
            try
            {
                beCPK tasaCPK;
                List<beCPK> tasasCPK = new List<beCPK>();

                string moneda = modeloCotizacion.CodMon;
                int tipoReajuste = modeloCotizacion.TipRea;

                int intFila;

                var cpk = modeloCPK.Where(x => x.COD_MONEDA == moneda && x.COD_TIPREAJUSTE == tipoReajuste).ToList();

                foreach (var item in cpk)
                {
                    tasaCPK = new beCPK();
                    intFila = item.NUM_ANNO;

                    tasaCPK = (from cp in cpk where cp.NUM_ANNO == intFila select cp).FirstOrDefault();

                    tasaCPK.PRC_CPK = item.PRC_CPK;
                    tasasCPK.Add(tasaCPK);
                }

                return tasasCPK;
            }
            catch (Exception)
            {
                Mensaje = "Problemas al hallar tasas de CPK";

                throw;
            }
        }

        /// <summary>
        /// Julieta González / Josué Gutierrez / Antonio Quezada
        /// 2019-08-06
        /// Obtiene los porcentajes de las Tasas de Rentabilidad
        /// </summary>
        /// <param name="modeloRentabilidad"> Lista de Tasas de Rentabilidad </param>
        /// <param name="modeloCotizacion"> Modelo </param>
        /// <returns> Regresa una lista de porcentajes de las Tasas de Rentabilidad </returns>

        public List<beRentabilidad> CargarTasasRentabilidad(List<beRentabilidad> modeloRentabilidad, beDatosModalidad modeloCotizacion)
        {
            try
            {
                beRentabilidad tasaRentabilidad;
                List<beRentabilidad> tasasRentabilidad = new List<beRentabilidad>();
                List<beRentabilidad> porcentajesTasaRentabilidad = new List<beRentabilidad>();

                string moneda = modeloCotizacion.CodMon;
                int tipoReajuste = modeloCotizacion.TipRea;
                int finTab = modeloCotizacion.FinTab;
                int contador = 1;
                int numeroAnio;

                var rentabilidad = modeloRentabilidad.Where(x => x.COD_MONEDA == moneda && x.COD_TIPREAJUSTE == tipoReajuste).ToList();

                foreach (var item in rentabilidad)
                {
                    tasaRentabilidad = new beRentabilidad();
                    numeroAnio = item.NUM_ANNO;

                    tasaRentabilidad = (from renta in rentabilidad where renta.NUM_ANNO == numeroAnio select renta).FirstOrDefault();

                    tasaRentabilidad.PRC_TASAREN = item.PRC_TASAREN;
                    tasasRentabilidad.Add(tasaRentabilidad);
                }

                porcentajesTasaRentabilidad.Add(new beRentabilidad());

                for (int m = 1; m <= finTab; m++)
                {
                    tasaRentabilidad = new beRentabilidad();

                    tasaRentabilidad.PRC_TASAREN = (Math.Pow((1 + (tasasRentabilidad[contador].PRC_TASAREN / 100)), (double)Exp)) - 1;
                    porcentajesTasaRentabilidad.Add(tasaRentabilidad);

                    if ((m % 12) == 0)
                        contador++;
                }

                return porcentajesTasaRentabilidad;
            }
            catch (Exception)
            {
                Mensaje = "Problemas al hallar tasas de Rentabilidad";

                throw;
            }
        }

        /// <summary>
        /// Josué Gutierrez / Antonio Quezada
        /// 2019-08-06
        /// Otiene la Curva de las Tasas
        /// </summary>
        /// <param name="modeloTasasCurva"> Lista de Curva de Tasas </param>
        /// <param name="modeloCotizacion"> Modelo </param>
        /// <returns> Regresa una lista que contiene la Curva de las Tasas </returns>

        public List<beCurvaTasas> CargarCurvaTasas(List<beCurvaTasas> modeloTasasCurva, beDatosModalidad modeloCotizacion)
        {
            try
            {
                beCurvaTasas curvaTasa;
                List<beCurvaTasas> curvaTasas = new List<beCurvaTasas>();

                string moneda = modeloCotizacion.CodMon;
                int tipoReajuste = modeloCotizacion.TipRea;

                var curvas = modeloTasasCurva.Where(x => x.COD_MONEDA == moneda && x.COD_TIPPREAJUSTE == tipoReajuste).OrderBy(x => x.NUM_MES).ToList();

                foreach (var item in curvas)
                {
                    curvaTasa = new beCurvaTasas();

                    curvaTasa.NUM_MES = item.NUM_MES;
                    curvaTasa.MTO_VALOR = item.MTO_VALOR / 100;
                    curvaTasas.Add(curvaTasa);
                }

                return curvaTasas;
            }
            catch (Exception)
            {
                Mensaje = "Problemas al hallar la Curva de Tasas";

                throw;
            }
        }

        /// <summary>
        /// Josué Gutierrez / Antonio Quezada
        /// 2019-08-08
        /// Crea las Tablas de Mortalidad por Beneficiario
        /// </summary>
        /// <param name="modeloMortalidad"> Lista de datos de Mortalidad </param>
        /// <param name="beneficiarios"> Lista de Beneficiarios </param>
        /// <param name="finMortalidad"> Límite de meses para recibir una pensión </param>
        /// <returns> Regresa una lista que contiene los datos de Mortalidad por Beneficiario </returns>

        public List<Mortalidad> CargarMortalidad(List<beMortalidadDinVal> modeloMortalidad, List<Beneficiario> beneficiarios, int finMortalidad)
        {
            try
            {
                beMortalidadDinVal mortalidad;
                Mortalidad lxDin;

                List<beMortalidadDinVal> listaMortalidad = new List<beMortalidadDinVal>();
                List<Mortalidad> listaLxDin = new List<Mortalidad>();

                int anio;
                int anioAux;
                int tipoInvalidez;
                int sexo;

                decimal FxQx;
                decimal FxAx;
                decimal ExoQ;
                decimal ExpQ;
                decimal FxQxFm;

                decimal FxLxF = 0;
                decimal FxQxF = 0;
                decimal FxQxFmAux = 0;
                int mesAux = 0;

                foreach (var item in modeloMortalidad)
                {
                    mortalidad = new beMortalidadDinVal();

                    mortalidad.i = item.i;
                    mortalidad.j = item.j;
                    mortalidad.k = item.k;
                    mortalidad.MtoLx = item.MtoLx;
                    mortalidad.MtoAx = item.MtoAx;

                    listaMortalidad.Add(mortalidad);
                }

                foreach (var item in beneficiarios)
                {
                    anio = item.AniosFechaNacimiento;
                    tipoInvalidez = item.TipoInvalidez == null ? 0 : item.TipoInvalidez == "N" ? 2 : 1;
                    sexo = item.Sexo == null ? 0 : item.Sexo == "M" ? 1 : 2;

                    for (int mes = 0; mes <= finMortalidad; mes++)
                    {
                        lxDin = new Mortalidad();

                        if ((mes % 12) == 0)
                        {
                            anioAux = mes / 12;
                            mesAux = 0;

                            FxQx = (from m in listaMortalidad where m.i == sexo && m.j == tipoInvalidez && m.k == anioAux select m.MtoLx).FirstOrDefault();
                            FxAx = (from m in listaMortalidad where m.i == sexo && m.j == tipoInvalidez && m.k == anioAux select m.MtoAx).FirstOrDefault();

                            ExoQ = (1 - FxAx);
                            ExpQ = (anio + anioAux - aniosBasTM);
                            ExpQ = ExpQ < 0 ? 0 : ExpQ;
                            FxQxF = FxQx * (decimal)(Math.Pow((double)ExoQ, (double)ExpQ));
                        }

                        FxQxFm = Exp * FxQxF / ((12 - mesAux * FxQxF) / 12);
                        FxLxF = mes == 0 ? 100000 : FxLxF * (1 - FxQxFmAux);

                        FxQxFmAux = FxQxFm;
                        mesAux++;

                        lxDin.IdBeneficiario = item.IdBeneficiario;
                        lxDin.Mes = mes;
                        lxDin.LxDin = Math.Round(FxLxF, 9);

                        listaLxDin.Add(lxDin);
                    }
                }

                return listaLxDin;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Josué Gutierrez / Antonio Quezada
        /// 2019-08-08
        /// Cálcula el Porcentaje de Pensión de los beneficiarios
        /// </summary>
        /// <param name="modeloBeneficiarios"> Lista de Beneficiarios </param>
        /// <param name="modeloPorcentajeLegal"> Lista de Porcentajes Legales </param>
        /// <param name="modeloCotizacion"> Modalidad </param>
        /// <returns> Regresa una variable que contiene la suma de los Porcentajes de Pensión de los Beneficiarios </returns>

        public double CambiarPension(List<beDatosBen> modeloBeneficiarios, List<bePorcenLegales> modeloPorcentajeLegal, beDatosModalidad modeloCotizacion)
        {
            try
            {
                string tipoPension = modeloCotizacion.Tippen;
                string tipoRenta = modeloCotizacion.TipRen;
                string indiceCobertura = modeloCotizacion.IndCob;
                string fechaCotizacion = modeloCotizacion.FecCot;
                string fechaDevengue = modeloCotizacion.FecDev;
                string dias = fechaDevengue.Substring(6, 2);
                string meses = fechaDevengue.Substring(4, 2);
                string anios = fechaDevengue.Substring(0, 4);

                long edadLimite = modeloCotizacion.EdaLim;

                int mesesDiferencia = modeloCotizacion.MesDif;

                double sumaPorcentajePension = 0;

                string fechaRentaVitalicia = DateTime.Parse(dias + "/" + meses + "/" + anios).AddMonths(mesesDiferencia).ToString("yyyyMMdd");

                if (tipoPension != "S")
                {
                    if (tipoRenta == "D")
                    {
                        // Cálculo de porcentajes por Fecha de Renta Vitalicia
                        modeloBeneficiarios = rutinaPorcentaje.PorcentajeBen(modeloBeneficiarios, fechaRentaVitalicia, indiceCobertura, tipoPension, edadLimite, modeloPorcentajeLegal);
                        sumaPorcentajePension = (from b in modeloBeneficiarios select b.PrcPen).Sum() / 100;
                    }
                    else
                    {
                        // Cálculo de porcentajes por Fecha de Cotización
                        modeloBeneficiarios = rutinaPorcentaje.PorcentajeBen(modeloBeneficiarios, fechaCotizacion, indiceCobertura, tipoPension, edadLimite, modeloPorcentajeLegal);
                        sumaPorcentajePension = (from b in modeloBeneficiarios select b.PrcPen).Sum() / 100;
                    }
                }
                else
                {
                    if (tipoRenta == "I")
                    {
                        // Cálculo de porcentajes por Fecha de Devengue
                        modeloBeneficiarios = rutinaPorcentaje.PorcentajeBen(modeloBeneficiarios, fechaDevengue, indiceCobertura, tipoPension, edadLimite, modeloPorcentajeLegal);
                        sumaPorcentajePension = (from b in modeloBeneficiarios select b.PrcPen).Sum() / 100;
                    }
                    else
                        sumaPorcentajePension = (from b in modeloBeneficiarios select b.PrcPen).Sum() / 100;
                }

                return sumaPorcentajePension;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Factor Reajuste
        /// <summary>
        /// Josué Gutierrez / Antonio Quezada
        /// 2019-08-15
        /// Obtiene el Factor de Reajuste cuando el tipo es Indexado
        /// </summary>
        /// <param name="modeloFactorVac"> Lista de Factor Vac </param>
        /// <param name="modeloCotizacion"> Modalidad </param>
        /// <param name="beneficiarios"> Lista de Beneficiarios </param>
        /// <param name="gratificacion"> Lista de Gratificación </param>
        /// <returns> Regresa una lista que contiene el Factor de Reajuste </returns>

        public List<double> ObtenerTipoIndexado(List<beTasaFacVac> modeloFactorVac, beDatosModalidad modeloCotizacion, List<Beneficiario> beneficiarios, List<int> gratificacion)
        {
            List<double> factorReajuste = new List<double>();

            int finMortalidad = modeloCotizacion.FinTab;
            int anios = Convert.ToInt32(modeloCotizacion.FecDev.Substring(0, 4));
            int meses = Convert.ToInt32(modeloCotizacion.FecDev.Substring(4, 2));

            string tipoPension = modeloCotizacion.Cobertura;
            string moneda = modeloCotizacion.CodMon;

            DateTime fechaDevengue;
            DateTime fechaDevengueAux;

            double factorVac = 0;
            double factorVacMes = 0;
            double ipcMensual = 1;
            double factorVacMesAnterior = 0;
            double mescotoAlt = 0;
            double sumaCostTotalMes = 0;

            fechaDevengue = new DateTime(anios, meses, 1);
            fechaDevengueAux = new DateTime(anios, meses, 1);
            fechaDevengue = fechaDevengue.AddMonths(-1);

            factorVac = modeloFactorVac.Where(x => x.FEC_IPC == fechaDevengue).Select(x => x.MTO_IPC).SingleOrDefault();

            factorReajuste.Add(0);

            for (int mes = 1; mes <= finMortalidad; mes++)
            {
                if (factorVac == 0)
                {
                    factorVac = 1;
                    factorVacMes = 1;
                }
                else
                    factorVacMes = modeloFactorVac.Where(x => x.FEC_IPC == fechaDevengue).Select(x => x.MTO_IPC).SingleOrDefault();

                if (factorVacMes == 0)
                {
                    ipcMensual = factorVacMesAnterior / factorVac;
                    factorReajuste.Add(ipcMensual);

                    goto sale;
                }
                else
                {
                    if (fechaDevengueAux.Month == 4 || fechaDevengueAux.Month == 7 || fechaDevengueAux.Month == 10 || fechaDevengueAux.Month == 1)
                        ipcMensual = factorVacMes / factorVac;

                    factorReajuste.Add(ipcMensual);
                }

                factorVacMesAnterior = factorVacMes;

            sale:
                fechaDevengue = fechaDevengue.AddMonths(1);
                fechaDevengueAux = fechaDevengueAux.AddMonths(1);
            }

            if (mesesCosto > 0 && moneda == "NS")
            {
                if (tipoPension == "S")
                {
                    for (int ax = 0; ax <= mesesCosto - 1; ax++)
                    {
                        List<Beneficiario> hijos = new List<Beneficiario>();
                        List<Beneficiario> noHijos = new List<Beneficiario>();

                        hijos = (from b in beneficiarios where b.CodigoParentesco == 30 && b.TipoInvalidez != "N" select b).ToList();

                        foreach (var item in hijos)
                            sumaCostTotalMes = sumaCostTotalMes + (1 * item.PorcentajePension * factorReajuste[ax + 1] * gratificacion[ax + 1]);

                        noHijos = (from b in beneficiarios where b.CodigoParentesco != 30 select b).ToList();

                        foreach (var item in noHijos)
                            sumaCostTotalMes = sumaCostTotalMes + (1 * item.PorcentajePension * factorReajuste[ax + 1] * gratificacion[ax + 1]);

                        mescotoAlt = mescotoAlt + sumaCostTotalMes;
                    }
                }
                else
                {
                    for (int ax = 0; ax <= mesesCosto - 1; ax++)
                        mescotoAlt = mescotoAlt + (1 * beneficiarios[0].PorcentajePension * factorReajuste[ax + 1] * gratificacion[ax + 1]);
                }
            }

            return factorReajuste;
        }

        /// <summary>
        /// Josué Gutierrez / Antonio Quezada
        /// 2019-08-15
        /// Obtiene el Factor de Reajuste cuando el tipo es Ajustado
        /// </summary>
        /// <param name="modeloCotizacion"> Modalidad </param>
        /// <param name="beneficiarios"> Lista de Beneficiarios </param>
        /// <param name="gratificacion"> Lista de Gratificación </param>
        /// <returns> Regresa una lista que contiene el Factor de Reajuste </returns>

        public List<double> ObtenerTipoAjustado(beDatosModalidad modeloCotizacion, List<Beneficiario> beneficiarios, List<int> gratificacion)
        {
            TipoAjustado TA = new TipoAjustado();
            List<double> factorReajuste = new List<double>();
            List<TipoAjustado> listaTipoAjustado = TA.ObtenerListaTipoAjustado();

            int finMortalidad = modeloCotizacion.FinTab;
            int anios = Convert.ToInt32(modeloCotizacion.FecDev.Substring(0, 4));
            int meses = Convert.ToInt32(modeloCotizacion.FecDev.Substring(4, 2));
            int dias = Convert.ToInt32(modeloCotizacion.FecDev.Substring(6, 2));

            DateTime fechaDevengue = new DateTime(anios, meses, dias);

            int ki = 0;
            int paso = 0;

            double porcentajeMensual = modeloCotizacion.PrcMen;
            double porcentajeTrimestral = modeloCotizacion.PrcTri;

            double teMensual = 1 + porcentajeMensual / 100;
            double teTrimestral = 1 + porcentajeTrimestral / 100;

            double mescotoAlt = 0;
            double sumaCostTotalMes = 0;

            string tipoPension = modeloCotizacion.Cobertura;

            ki = (from ta in listaTipoAjustado where ta.Mes == fechaDevengue.Month select ta.Ki).First();
            paso = (from ta in listaTipoAjustado where ta.Mes == fechaDevengue.Month select ta.Paso).First();

            factorReajuste.Add(0);
            factorReajuste.Add(1);

            for (int mes = 2; mes <= finMortalidad; mes++)
            {
                if (mes <= ki)
                    factorReajuste.Add(factorReajuste[mes - 1]);
                else
                {
                    if (mes <= paso)
                        factorReajuste.Add(factorReajuste[mes - 1] * Math.Pow(teMensual, ki));
                    else
                    {
                        if (fechaDevengue.Month == 4 || fechaDevengue.Month == 7 || fechaDevengue.Month == 10 || fechaDevengue.Month == 1)
                            factorReajuste.Add(factorReajuste[mes - 1] * teTrimestral);
                        else
                            factorReajuste.Add(factorReajuste[mes - 1]);
                    }
                }

                fechaDevengue = fechaDevengue.AddMonths(1);
            }

            if (mesesCosto > 0)
            {
                if (tipoPension == "S")
                {
                    for (int ax = 0; ax <= mesesCosto - 1; ax++)
                    {
                        List<Beneficiario> hijos = new List<Beneficiario>();
                        List<Beneficiario> noHijos = new List<Beneficiario>();

                        hijos = (from b in beneficiarios where b.CodigoParentesco == 30 && b.TipoInvalidez != "N" select b).ToList();

                        foreach (var item in hijos)
                            sumaCostTotalMes = sumaCostTotalMes + (1 * item.PorcentajePension * factorReajuste[ax + 1] * gratificacion[ax + 1]);

                        noHijos = (from b in beneficiarios where b.CodigoParentesco != 30 select b).ToList();

                        foreach (var item in noHijos)
                            sumaCostTotalMes = sumaCostTotalMes + (1 * item.PorcentajePension * factorReajuste[ax + 1] * gratificacion[ax + 1]);

                        mescotoAlt = mescotoAlt + sumaCostTotalMes;
                    }
                }
                else
                {
                    for (int ax = 0; ax <= mesesCosto - 1; ax++)
                        mescotoAlt = mescotoAlt + (1 * beneficiarios[0].PorcentajePension * factorReajuste[ax + 1] * gratificacion[ax + 1]);
                }
            }

            return factorReajuste;
        }
        
        #endregion
        
        #region "Calcula Flujos de Pensión"
        public void CalcularFlujosPension(beDatosModalidad modeloCotizacion, List<Beneficiario> beneficiarios, List<Mortalidad> listaLxDin, List<Mortalidad> tablasMortalidad, List<int> gratificacion, List<double> factorReajuste)
        {
            int finMortalidad = modeloCotizacion.FinTab;

            int mesDiferido = modeloCotizacion.MesDif;

            string tipoPension = modeloCotizacion.Cobertura;
            string tipoRenta = modeloCotizacion.TipRen;

            double porcentajeRentaTmp = modeloCotizacion.RenTmp;

            Beneficiario titular = new Beneficiario();

            List<double> flujoTramos = new List<double>();
            List<double> flujosPension = new List<double>();
            List<double> fpx = new List<double>();

            List<Beneficiario> noTitular = new List<Beneficiario>();

            flujoTramos.Add(1);

            for (int i = 1; i < finMortalidad; i++)
            {
                if (tipoRenta == "E" && i > mesDiferido)
                    flujoTramos.Add(porcentajeRentaTmp);
                else
                    flujoTramos.Add(1);
            }

            titular = (from b in beneficiarios where b.CodigoParentesco == 99 select b).FirstOrDefault();
            noTitular = (from b in beneficiarios where b.CodigoParentesco != 99 select b).ToList();

            FlujoPensionTitular(titular, modeloCotizacion, tablasMortalidad, gratificacion, factorReajuste, flujoTramos, ref fpx, ref flujosPension);
        }

        public void FlujoPensionTitular(Beneficiario titular, beDatosModalidad modeloCotizacion, List<Mortalidad> tablasMortalidad, List<int> gratificacion, List<double> factorReajuste, List<double> flujoTramos, ref List<double> fpx, ref List<double> flujosPension)
        {
            string indiceCobertura = modeloCotizacion.IndCob;
            string tipoPension = modeloCotizacion.Tippen;
            string tipoRenta = modeloCotizacion.TipRen;
            string tipoModalidad = modeloCotizacion.TipMod;

            int mesesNacimiento = titular.AniosFechaNacimiento * 12 + titular.MesesFechaNacimiento;
            int aniosFechaDevengue = Convert.ToInt32(modeloCotizacion.FecDev.Substring(0, 4));
            int mesesFechaDevengue = Convert.ToInt32(modeloCotizacion.FecDev.Substring(4, 2));
            int mesesDevengue = aniosFechaDevengue * 12 + mesesFechaDevengue;
            int finMortalidad = modeloCotizacion.FinTab;

            int sumaMesesGarantizadoDiferido = mesesDiferidos + mesesGarantizados;

            int limiteTotal = tipoRenta == "E" ? mesesGarantizados : sumaMesesGarantizadoDiferido;
            int limiteTotalGastoSepelio = tipoRenta == "E" ? 0 : mesesDiferidos;

            double gastoFunerario = modeloCotizacion.MtoGS;

            double porcentaje = 1;
            double porcentajePension = titular.PorcentajePension;
            double tpx;
            double qxt;
            double px;
            double qx = 0;
            double fqxt = 0;
            double ftpx = 0;

            decimal lxDinAux;
            decimal lxDinLimite;

            int edadMesesDevengue;
            int edadMesesDevengueLimite;
            int edadMesesDevengueAux;
            int limite;
            int contador;

            List<double> valores = new List<double>(); // Lista para obtener el valor mínimo o máximo
            List<double> flujosMesesConsumidos = new List<double>();

            if (indiceCobertura == "S")
            {
                porcentaje = tipoPension == "T" ? 0.7 : tipoPension == "P" ? 0.5 : 1;
                porcentajePension = titular.PorcentajePension * porcentaje;
            }

            edadMesesDevengue = mesesDevengue - mesesNacimiento;

            if (edadMesesDevengue <= 0 || edadMesesDevengue > finMortalidad)
            {
                Mensaje = "Error Edad es mayor a final de tabla Mortal y menor a 0.";
                throw new System.ArgumentException();
            }

            limite = finMortalidad - edadMesesDevengue - 1;
            valores.Add(sumaMesesGarantizadoDiferido);
            valores.Add(limite);

            limite = Convert.ToInt32(ObtenerMinimoMaximo(valores, "MAX"));
            valores.Clear(); // Se vacía la lista para futuras comparaciones

            flujosPension.Add(0);
            flujosMesesConsumidos.Add(0);

            try
            {
                fpx.Add(1);
                for (int i = 0; i <= limite; i++)
                {
                    contador = i + 1;
                    edadMesesDevengueLimite = edadMesesDevengue + i;
                    edadMesesDevengueAux = edadMesesDevengueLimite + 1;

                    valores.Add(edadMesesDevengueAux);
                    valores.Add(finMortalidad);

                    edadMesesDevengueAux = Convert.ToInt32(ObtenerMinimoMaximo(valores, "MIN"));
                    valores.Clear(); // Se vacía la lista para futuras comparaciones

                    if (i < mesesCosto)
                    {
                        tpx = 1;
                        px = 1;
                        qxt = 0;
                        fpx[i] = 1;
                        fqxt = 1;
                    }
                    else
                    {
                        lxDinAux = (from m in tablasMortalidad where m.IdBeneficiario == titular.IdBeneficiario && m.Mes == edadMesesDevengueAux select m.LxDin).FirstOrDefault();
                        lxDinLimite = (from m in tablasMortalidad where m.IdBeneficiario == titular.IdBeneficiario && m.Mes == edadMesesDevengueLimite select m.LxDin).FirstOrDefault();

                        qxt = lxDinAux == 0 ? 1 : Convert.ToDouble((1 - (lxDinAux / lxDinLimite)));

                        if (i == 0)
                        {
                            tpx = 1;
                            px = 1;
                        }
                        else
                        {
                            tpx = ftpx * (1 - fqxt);
                            px = i < limiteTotal ? 1 : tpx;
                        }
                    }
                    
                    flujosPension.Add(flujosPension[contador] + px * porcentajePension * gratificacion[contador] * factorReajuste[contador] * flujoTramos[contador]);

                    //saca el gasto de Sepelio
                    if (i > 0)
                        qx = (i < limiteTotalGastoSepelio) ? 0 : ftpx - tpx;
                    
                    flujosMesesConsumidos.Add(flujosMesesConsumidos[contador] + gastoFunerario * qx);

                    ftpx = tpx;
                    fqxt = qxt;
                    fpx.Add(tpx);

                    if (tipoModalidad == "S" && edadMesesDevengueAux + 1 == finMortalidad)
                        break;
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Problemas en los Flujos de Titular de la Rutina.";

                throw;
            }
        }
       
        public void FlujoPensionNoTitular(List<Beneficiario> beneficiarios, double sumaMesesGarantizadoDiferido, beDatosModalidad modeloCotizacion)
        {  
            Mortalidad lxDin = new Mortalidad();
            Mortalidad lxDinSiguiente = new Mortalidad ();

            double pensionBeneficiario = 
           
            Penben[j] = porcbe_ori[j]; /// new_prc;
            edabe = FechaDv - (Nanbe[j] * 12 + Nmnbe[j]);
            
           foreach(var item in beneficiarios)
           {
               if (edabe > Fintab)
                {
                    msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                    throw new System.ArgumentException();
                }
                if (Ncorbe[j] == 10 || Ncorbe[j] == 11 || Ncorbe[j] == 20 ||
                    Ncorbe[j] == 21 || Ncorbe[j] == 41 || Ncorbe[j] == 42 ||
                    ((Ncorbe[j] >= 30 && Ncorbe[j] < 40) && (Coinb[j] != "N")))
                {
                    edacai = 0;
                    limite = Fintab - edabe - 1;
                    limite = (long)amax0(sumaMesesGarantizadoDiferido, limite);
                    nmax = (long)amax0(nmax, limite);
                    int a = 0;
                    try
                    {
                        for (int i = 0; i <= limite - 1; i++)
                        {
                            
                            edalbe = edabe + i;
                            edalbe = (int)amin0(edalbe, Fintab);
                            edacai = edalbe + 1;
                            edacai = (int)amin0(edacai, Fintab);

                            if (i < mescosto)
                            {
                                tpx = 1;
                                qxt = 0;
                                py = 0;
                                fpy[i] = 1;
                                fqxy[i] = 1;
                            }
                            else
                            {
                                if (LxDin[j, edalbe] == 0)
                                {
                                    qxt = 1;
                                }
                                else
                                {
                                    qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
                                }

                                if (i == 0)
                                {
                                    tpx = 1;
                                    py = tpx;
                                }
                                else
                                {
                                    tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                    if (i < ltot)
                                    {
                                        py = 0 : tpx;
                                    }
                                    else
                                    {
                                        py = ;
                                    }
                                }

                            }
                            fpy[i] = tpx;
                            fqxy[i] = qxt;
                            valotemp[imas1] = py * (1 - fpx[imas1]) * facgratif[imas1] * Penben[j] * vl_FactorReajuste[imas1] * fTramos[imas1];
                            Flupen[imas1] = Flupen[imas1] + valotemp[imas1];
                        }
                    }
                    catch (Exception ex)
                    {
                        msj = "Problemas en los Flujos de Conyugue, Padres o hijos Invalidos de la Rutina. ";
                        return ListaResultador;
                    }
                }
                else
                {
                    if (Ncorbe[j] >= 30 && Ncorbe[j] < 40)
                    {
                        //ActualizaXMLDET(pathD, j + 1, "PRC_PENSIONREP", CStr(Penben(j) * 100))
                        if (edabe > EdaLim)
                        {
                            //NO HACE NADA
                        }
                        else
                        {
                            mdif = EdaLim - edabe;
                            if (edabe < 1) { edabe = 1; };
                            nmdif = mdif;
                            limite2 = Fintab - edadPensionado;
                            limite = (long)amin0(nmdif, limite2) - 1;
                            nmax = (long)amax0(nmax, limite);

                            try
                            {
                                for (i = 0; i <= mdif - 1; i++)
                                {
                                    imas1 = i + 1;
                                    edalbe = edabe + i;
                                    edacai = edalbe + 1;
                                    edacai = (int)amin0(edacai, Fintab);

                                    if (edalbe < 0)
                                    {
                                        edalbe = 1;
                                        edacai = 1;
                                        valPY[i] = 0;
                                        fpy[i] = 1;
                                        fqxy[i] = 0;
                                        valotemp[imas1] = 0;
                                        Flupen[imas1] = Flupen[imas1] + 0;
                                    }
                                    else
                                    {
                                        if (i < mescosto)
                                        {
                                            tpx = 1;
                                            qxt = 0;
                                            py = 0;
                                            fpy[i] = 1;
                                            fqxy[i] = 1;
                                        }
                                        else
                                        {
                                            //qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);
                                            qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
                                            if (i == 0)
                                            {
                                                tpx = 1;
                                            }
                                            else
                                            {
                                                tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                                if (i < ltot)
                                                {
                                                    py = 0;
                                                }
                                                else
                                                {
                                                    py = tpx;
                                                }
                                            }

                                        }
                                        fpy[i] = tpx;
                                        fqxy[i] = qxt;
                                        valotemp[imas1] = py * (1 - fpx[imas1]) * facgratif[imas1] * Penben[j] * vl_FactorReajuste[imas1] * fTramos[imas1];
                                        Flupen[imas1] = Flupen[imas1] + valotemp[imas1];
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                msj = "Problemas en los Flujos de hijos Sanos de la Rutina. ";
                                return ListaResultador;
                            }
                        }
                    }
                }
           }
            
        }
        
        public void FlujoPensionSupervivencia(List<Beneficiario> beneficiario, double sumaMesesGarantizadoDiferido, beDatosModalidad modeloCotizacion)
        {
            long limiteA = 0;
            for (j = 0; j <= Nben; j++)
            {
                for (long g = 0; g <= 1332; g++)
                {
                    fpy[g] = 0;
                    fqxy[g] = 0;
                    valotemp[g] = 0;
                }
                
                    #region "no titular"
                    mesesGarantizados = pergar;
                    if (TipMod == "S") { mesesGarantizados = 0; };
                    sumaMesesGarantizadoDiferido = mesesDiferidos + mesesGarantizados;

                    Penben[j] = Porcbe[j];
                    numbep = numbep + 1;
                    Fechan = Nanbe[j] * 12 + Nmnbe[j];
                    edabe = Fechap - Fechan;
                    edaberv = Fechrv - Fechan;
                    edadedv = FechaDv - Fechan;

                    if (edabe > Fintab)
                    {
                        msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                        return ListaResultador;
                    }
                    //'calculo de renta vitalicias
                    if (Ncorbe[j] == 10 || Ncorbe[j] == 11 || Ncorbe[j] == 20 || Ncorbe[j] == 21 || Ncorbe[j] == 41 || Ncorbe[j] == 42 || ((Ncorbe[j] >= 30 && Ncorbe[j] < 40) && (Coinb[j]) != "N"))
                    {
                        limite = Fintab - edabe - 1;
                        limite = (long)amax0(sumaMesesGarantizadoDiferido, limite);
                        nmax = (long)amax0(nmax, limite);
                        tpx = 1;
                        for (i = 0; i <= limite; i++)
                        {
                            imas1 = i + 1;
                            edalbe = edabe + i;
                            edalbe = (int)amin0(edalbe, Fintab);
                            edacai = edalbe + 1;
                            edacai = (int)amin0(edacai, Fintab);

                            if (i < mescosto)
                            {
                                tpx = 1;
                                qxt = 0;
                                py = 1;
                                fpy[i] = 1;
                                fqxy[i] = 1;
                            }
                            else
                            {
                                if (LxDin[j, edalbe] == 0)
                                {
                                    qxt = 1;
                                }
                                else
                                {
                                    qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
                                }

                                if (i == 0)
                                {
                                    tpx = 1;
                                    py = tpx;
                                }
                                else
                                {
                                    tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                    if (i < ltot)
                                    {
                                        if (mesesGarantizados > 0)
                                        {
                                            py = 1;
                                        }
                                        else
                                        {
                                            py = tpx;
                                        }
                                        //py = 1;
                                        //tpx = 1;
                                        //qxt = 0;
                                    }
                                    else
                                    {
                                        py = tpx;
                                        if (i >= Mesdif && i < sumaMesesGarantizadoDiferido) { py = 1; };
                                    }
                                }
                            }

                            fpy[i] = tpx;
                            fqxy[i] = qxt;
                            valPY[imas1] = py;
                            valotemp[imas1] = py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                            Flupen[imas1] = Flupen[imas1] + py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                        }
                    }
                    else
                    {
                        if (Ncorbe[j] >= 30 && Ncorbe[j] < 40)
                        {
                            if (edaberv > (EdaLim + sumaMesesGarantizadoDiferido) && Coinb[j] == "N")
                            {
                                Penben[j] = 0;

                                //ActualizaXMLDET(pathB, j + 1, "PRC_PENSIONSOBDIF", "0")
                            }
                            else
                            {
                                mdif = EdaLim - edabe;
                                //if (edabe < 1) { edabe = 1; };
                                nmdif = mdif;// -1;
                                limiteA = nmdif;
                                limiteA = (long)amax0(sumaMesesGarantizadoDiferido, nmdif);
                                nmax = (long)amax0(limiteA, nmax);
                                if (TipMod == "G" && tipoPension == "S")
                                {
                                    limiteA = (long)amax0(sumaMesesGarantizadoDiferido, nmdif);
                                    nmax = (long)amax0(limiteA, nmax);
                                }
                                //***

                                for (i = 0; i <= limiteA - 1; i++)
                                {
                                    imas1 = i + 1;
                                    edalbe = edabe + i;
                                    edacai = edalbe + 1;
                                    edacai = (int)amin0(edacai, Fintab);
                                    if (edalbe < 0)
                                    {
                                        edalbe = 1;
                                        edacai = 1;
                                        valPY[i] = 0;
                                        fpy[i] = 1;
                                        fqxy[i] = 0;
                                        valotemp[imas1] = 0;
                                        Flupen[imas1] = Flupen[imas1] + 0;
                                    }
                                    else
                                    {
                                        if (i < mescosto)
                                        {
                                            tpx = 1;
                                            qxt = 0;
                                            py = 1;
                                            fpy[i] = 1;
                                            fqxy[i] = 1;
                                        }
                                        else
                                        {
                                            //qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);
                                            qxt = (double)(1 - (LxDin[j, edacai] / LxDin[j, edalbe]));
                                            if (i == 0)
                                            {
                                                tpx = 1;
                                                py = tpx;
                                            }
                                            else
                                            {
                                                tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                                if (i < ltot)
                                                {
                                                    if (mesesGarantizados > 0)
                                                    {
                                                        py = 1;
                                                    }
                                                    else
                                                    {
                                                        py = tpx;
                                                    }
                                                    //py = 1;
                                                    //tpx = 1;
                                                    //qxt = 0;
                                                }
                                                else
                                                {
                                                    py = tpx;
                                                    if (swg == "S" && i < sumaMesesGarantizadoDiferido) { py = 1; }
                                                    if (i < sumaMesesGarantizadoDiferido) { py = 1; }
                                                    if (i < mesesDiferidos) { py = 0; }

                                                }
                                            }
                                        }
                                        valPY[i] = py;
                                        fpy[i] = tpx;
                                        fqxy[i] = qxt;
                                        valotemp[imas1] = py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                                        Flupen[imas1] = Flupen[imas1] + py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                
                }
        }
      
        #endregion 
    
        // public void CalcularTasaVenta(beDatosTasasPar tasa)
        // {
        //    double tasatirc;
        //    double tasaVentamax;
        //    double tasaCalce;
        //    double tasaCalculoInicial;
        //    double tasaPromedio;

        //    double tirVenta = ((Math.Pow((1 + (tasa.PrcTas / 100)), (double)Exp)) - 1) + 0.00001;

        //    CalTva:
        //    tasatirc = 0;

        //    tirVenta = tirVenta - 0.00001;

        //    tasaVentamax = tirVenta;
        //    //tce = amin0(tci, tasaVentamax);
        //    tasaCalce = tasaVentamax <= tasaCalculoInicial ? , tasaPromedio
        //    salcta_eva = MtoCic;
        //    vppen = 0;
        //    vpcm = 0;
        //    vppenres = 0;
        //    vpcmres = 0;
        //    cr = 1;
        //    dflupag = 0;
        //    for (int ir = 0; ir <= Fintab; ir++)
        //    {
        //        fcru[ir] = 0;
        //    }
        //    for (i = 1; i <= nmax; i++)
        //    {
        //        if (i <= mescosto + 1)
        //        {
        //            fcru[i] = Flupen[i] * Math.Pow((1 / (1 + tasaVentamax)), (0));
        //            vppen = vppen + fcru[i];
        //        }
        //        else
        //        {
        //            // CRU PENSION
        //            fcru[i] = Flupen[i] * Math.Pow((1 / (1 + tasaVentamax)), (cr));
        //            vppen = vppen + fcru[i];
        //            vppenres = vppenres + Flupen[i] / Math.Pow((1 + tce), (cr));
        //            //vppenres = vppenres + Flupen[i] * Math.Pow((1 / (1 + tce)), (cr));

        //            // CRU GS
        //            fcruGS[i] = Flucm[i] * Math.Pow((1 / (1 + tasaVentamax)), (cr - 0.5));
        //            vpcm = vpcm + fcruGS[i];
        //            vpcmres = vpcmres + Flucm[i] / Math.Pow((1 + tce), (cr - 0.5));
        //            //vpcmres = vpcmres + Flucm[i] * Math.Pow((1 / (1 + tce)), (cr));

        //            cr = cr + 1;
        //        }

        //    }
        //    penanu = (salcta_eva - vpcm) / vppen;
        //    mesdiftmp = Mesdif;
        //    mesdif1 = mesdifc;

        //    if (tipoPension == "S")
        //    {
        //        if (TipRen == "D")
        //        {
        //            //''SOBREVIVENCIA DIFERIDA
        //            if (DerGratificacion == "S")
        //            {
        //                mesdif1 = mesdif1 + ((mesdif1 / 12) * 2);
        //            }
        //            //if (mescon <= 0)
        //            //{
        //            //    mesdiftmp = mesdif1;
        //            //}
        //            //else
        //            //{
        //            //    mesdiftmp = mesdif1 - mescon;
        //            //}
        //            mesdiftmp = mesdif1 - mescon;
        //            if (mesdiftmp < 0)
        //            {
        //                mesdiftmp = 0;
        //            }
        //            ival = ((Math.Pow((1 + PrcAfp), (double)Exp)) - 1);
        //            if (mescon > mesdif1)
        //            {
        //                mesconTmp = mesdif1;
        //            }
        //            else
        //            {
        //                mesconTmp = mescon;
        //                //mesconTmp = 0;
        //            }
        //            vppfactor = ival / ((ival * mesconTmp) + (1 - Math.Pow((1 + ival), -(mesdiftmp))) * (1 + ival));
        //            Vpptem = (1 / vppfactor) * sumaporcsob;
        //            penanu = (salcta_eva - vpcm) / (vppen + (Vpptem / PrcTaf));
        //            Add_porc_be = Totpofr;

        //            if (salcta_eva > 0)
        //            {
        //                if (Mone == "NS")
        //                {
        //                    if (Vpptem == 0 || PrcTaf == 0)
        //                    {
        //                        Rete_sim = 0;
        //                    }
        //                    else
        //                    {
        //                        Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
        //                    }
        //                }
        //                else
        //                {
        //                    if (Vpptem == 0 || PrcTaf == 0)
        //                    {
        //                        Rete_sim = 0;
        //                    }
        //                    else
        //                    {
        //                        Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
        //                    }
        //                }
        //            }
        //            sumapenben = 0;
        //            //for (i = 0; i <= Nben; i++)
        //            //{
        //            //PensionBenef[i] = Porcbe[i] * (penanu / PrcTaf);
        //            //sumapenben = sumapenben + PensionBenef[i];
        //            //}
        //            sumapenben = (penanu / PrcTaf);
        //            vld_saldo = Rete_sim;
        //            if (vppen > 0)
        //            {
        //                salcta_eva = (salcta_eva - vld_saldo);
        //            }
        //            pensim = 0;
        //            if (vppen > 0)
        //            {
        //                penanu = (salcta_eva - vpcm) / vppen;
        //            }

        //        }
        //    }
        //    else
        //    {
        //        if (TipRen == "D")
        //        {
        //            if (DerGratificacion == "S")
        //            {
        //                mesdif1 = mesdif1 + ((mesdif1 / 12) * 2);
        //            }
        //            //if (mescon <= 0)
        //            //{
        //            //    mesdiftmp = mesdif1;
        //            //}
        //            //else
        //            //{
        //            //    mesdiftmp = mesdif1 - mescon;
        //            //}
        //            mesdiftmp = mesdif1 - mescon;
        //            if (mesdiftmp < 0) { mesdiftmp = 0; }
        //            ival = ((Math.Pow((1 + (PrcAfp)), (double)Exp)) - 1);
        //            if (mescon > mesdif1)
        //            {
        //                mesconTmp = mesdif1;
        //            }
        //            else
        //            {
        //                mesconTmp = mescon;
        //                //mesconTmp = 0;
        //            }
        //            vppfactor = ival / ((ival * mesconTmp) + (1 - Math.Pow((1 + ival), -(mesdiftmp))) * (1 + ival));
        //            Vpptem = (1 / vppfactor) * new_prc;
        //            penanu = (salcta_eva - vpcm) / (vppen + (Vpptem / PrcTaf));
        //            Add_porc_be = Totpofr;

        //            if (salcta_eva > 0)
        //            {
        //                if (Mone == "NS")
        //                {
        //                    if (Vpptem == 0 || PrcTaf == 0)
        //                    {
        //                        Rete_sim = 0;
        //                    }
        //                    else
        //                    {
        //                        Rete_sim = (penanu / PrcTaf) * Vpptem;
        //                        //Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
        //                    }
        //                }
        //                else
        //                {
        //                    if (Vpptem == 0 || PrcTaf == 0)
        //                    {
        //                        Rete_sim = 0;
        //                    }
        //                    else
        //                    {
        //                        Rete_sim = (penanu / PrcTaf) * Vpptem;
        //                        //Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
        //                    }
        //                }
        //            }
        //            sumapenben = 0;
        //            PensionBenef[1] = (penanu / PrcTaf);
        //            sumapenben = sumapenben + PensionBenef[1];

        //            vld_saldo = Rete_sim;
        //            if (vppen > 0)
        //            {
        //                salcta_eva = (salcta_eva - vld_saldo);
        //            }
        //            pensim = 0;
        //            if (vppen > 0)
        //            {
        //                penanu = (salcta_eva - vpcm) / vppen;
        //            }
        //        }
        //    }

        //    for (int ir = 0; ir <= Fintab; ir++)
        //    {
        //        Exced[ir] = 0;
        //    }
        //    if (tipoPension != "S")
        //    {
        //        sumaporcsob = 1;
        //    }
        //    reserva = 0;
        //    PERDI = 0;
        //    dflupag = 0;
        //    flupag = 0;
        //    //TRAE RESERVAS DE MESES CONSUMIDOS
        //    for (int a = 0; a <= mescosto + 1; a++)
        //    {
        //        dflupag = dflupag + (penanu * Flupen[a] + Flucm[a]);/// (Math.Pow((1 + tce), a)));
        //    }
        //    //TRAE RESERVAS
        //    at = 1;
        //    for (int a = (int)mescosto + 1; a <= nmax; a++)
        //    {
        //        resfin = resfin + ((penanu * Flupen[a + 1] + Flucm[a + 1]) / (Math.Pow((1 + tce), at)));
        //        at = at + 1;
        //        //dflupag = dflupag + (penanu * Flupen[a] + Flucm[a]);/// (Math.Pow((1 + tce), a)));
        //    }
        //    reserva = resfin + dflupag;

        //    //reserva = penanu * (vppenres) + vpcmres + dflupag;
        //    PERDI = ((reserva / salcta_eva) - 1) * 100;

        //    dComis = (salcta_eva * PrcCom);
        //    //dComis = (salcta_eva * (2.6/100) * 1.42);
        //    dGasMan = (gastos / 12);
        //    dVarRes = reserva;
        //    dUtilImp = salcta_eva - (dComis + gasemi + dGasMan + dflupag + dVarRes);
        //    dvarCap = dVarRes * dMarSol;
        //    //PRIMER FLUJO EXDENTES
        //    Exced[1] = dUtilImp - dImp - dvarCap;

        //    dvarCapAnt = dvarCap;
        //    resfinAnt = dVarRes;
        //    vlContarMaximo = nmax;

        //    for (i = 2; i <= nmax; i++)
        //    {
        //        relres = 1;
        //        resfin = 0;
        //        at = 1;
        //        iRes = i + mescosto;
        //        if (iRes == 1332) { goto CalExd; }
        //        //if (iRes >= 1332) 
        //        //{ 
        //        //    iRes = i; 
        //        //}
        //        flupag = penanu * Flupen[iRes] + Flucm[iRes];
        //        for (long a = iRes; a <= nmax; a++)
        //        {
        //            resfin = resfin + ((penanu * Flupen[a + 1] + Flucm[a + 1]) / (Math.Pow((1 + tce), at)));
        //            at = at + 1;
        //        }
        //        resfin = flupag + resfin;
        //        dGasMan = (gastos / 12) * (resfin / reserva);
        //        dPagos = flupag;

        //        //if (i == 2)
        //        //{
        //        //    dPagos = (dPagos + dflupag);
        //        //}
        //        dVarRes = (resfin - resfinAnt);
        //        dProdInv = (resfinAnt + dvarCapAnt) * Prodin[i];
        //        dUtilImp = -(dGasMan + dPagos + dVarRes) + dProdInv;

        //        dCapit = resfin * dMarSol;
        //        dvarCap = dCapit - dvarCapAnt;
        //        dImp = 0;
        //        if (dUtilImp - dProdInv > 0)
        //        {
        //            dImp = (dUtilImp - dProdInv) * 0.30;
        //        }

        //        Exced[i] = Math.Round(dUtilImp - dImp - dvarCap, 4);

        //        dvarCapAnt = dCapit;
        //        resfinAnt = resfin;

        //    }

        //    CalExd:
        //    sumaex = 0;
        //    for (i = 1; i <= vlContarMaximo; i++)
        //    {
        //        sumaex = sumaex + Exced[i] / Math.Pow((1 + tasatirc), i);
        //    }
        //    if (sumaex >= 0)
        //    {
        //        tasatirc = tasatirc + tasanc;
        //        goto CalExd;
        //    }
        //    tasatirc = (Math.Pow((1 + tasatirc), 12)) - 1;

        //    if (Math.Round(tasatirc, 5) < tasac)
        //    {
        //        TitMayor = false;
        //        goto CalTva;
        //    }

        //    if (sumaex >= 0)
        //    {
        //        tirVenta = tirVenta + TINC1;
        //        if (tirVenta > 100)
        //        {
        //            break;
        //            //sale funcion
        //        }
        //        sumaex1 = sumaex;
        //        sumaex = 0;
        //        goto CalTva;
        //    }

        //    //if (Math.Round(tasatirc, 2) > tasac)
        //    //{
        //    //    //tirVenta = tirVenta + TINC1;
        //    //    //if (tirVenta > 100)
        //    //    //{
        //    //    //    break;
        //    //    //    //sale funcion
        //    //    //}
        //    //    //sumaex1 = sumaex;
        //    //    //sumaex = 0;
        //    //    TitMayor = true;
        //    //    goto CalTva;
        //    //}
        //    tirmax = tirVenta;
        //    tirmax_ori = tirmax;
        //    TTirMax = tirmax;
        //    //CalPer:
        //    if (PERDI > PERMAX)
        //    {
        //        goto CalTva;

        //        //TTirMax = TTirMax - 0.00001;//TINC1;

        //        //vppen = 0;
        //        //vpcm = 0;
        //        //RM = 0;
        //        //for (int ir = 0; ir <= Fintab; ir++)
        //        //{
        //        //    fcru[ir] = 0;
        //        //}
        //        //cr = 1;
        //        //for (i = 1; i <= nmax; i++)
        //        //{
        //        //    if (i <= mescosto + 1)
        //        //    {
        //        //        fcru[i] = Flupen[i] * Math.Pow((1 / (1 + TTirMax)), (0));
        //        //        vppen = vppen + fcru[i];
        //        //    }
        //        //    else
        //        //    {
        //        //        fcru[i] = Flupen[i] * Math.Pow((1 / (1 + TTirMax)), (cr));
        //        //        vppen = vppen + fcru[i];
        //        //        vpcm = vpcm + Flucm[i] / Math.Pow((1 + TTirMax), (cr - 0.5));
        //        //        cr = cr + 1;
        //        //    }
        //        //}
        //        //NewPen = (salcta_eva - vpcm) / vppen;
        //        //RM = (NewPen * vppenres) + vpcmres;
        //        //PERDI = ((RM / salcta_eva) - 1) * 100;
        //        // goto CalPer;
        //    }
        //    //tirmax = TTirMax;
        //    if (PERDI < 0)
        //    {
        //        PERDI = 0;
        //    }


        //    tirmax = ((Math.Pow((1 + tirmax), 12)) - 1) * 100;
        //    tirmax = Math.Round(tirmax, 2);
        //    if (tirmax > penmax) { tirmax = penmax; }


        //    //OBTIENE LOS VALORES FINALES CON LA TASA CALCULADA//
        //    salcta_eva = MtoCic;
        //    tirmax = ((Math.Pow((1 + (tirmax / 100)), (double)Exp)) - 1);
        //    tasaVentamax = tirmax;
        //    //tce = amin1(tasaVentamax, tci, tpr);
        //    vppen = 0;
        //    vpcm = 0;
        //    for (int ir = 0; ir <= Fintab; ir++)
        //    {
        //        fcru[ir] = 0;
        //    }
        //    cr = 1;
        //    for (i = 1; i <= nmax; i++)
        //    {
        //        if (i <= mescosto + 1)
        //        {
        //            fcru[i] = Flupen[i] * Math.Pow((1 / (1 + tasaVentamax)), (0));
        //            vppen = vppen + fcru[i];
        //        }
        //        else
        //        {
        //            // CRU PENSION
        //            fcru[i] = Flupen[i] * Math.Pow((1 / (1 + tasaVentamax)), (cr));
        //            vppen = vppen + fcru[i];

        //            // CRU GS
        //            fcruGS[i] = Flucm[i] * Math.Pow((1 / (1 + tasaVentamax)), (cr - 0.5));
        //            vpcm = vpcm + fcruGS[i];

        //            cr = cr + 1;
        //        }
        //    }
        //    penanu = (salcta_eva - vpcm) / vppen;
        //    //mesdiftmp = Mesdif;
        //    mesdif1 = mesdifc;
        //    sumapenben = 0;
        //    if (tipoPension == "S")
        //    {
        //        if (TipRen == "D")
        //        {
        //            //''SOBREVIVENCIA DIFERIDA
        //            if (DerGratificacion == "S")
        //            {
        //                mesdif1 = mesdif1 + ((mesdif1 / 12) * 2);
        //            }
        //            //if (mescon <= 0)
        //            //{
        //            //    mesdiftmp = mesdif1;
        //            //}
        //            //else
        //            //{
        //            //    mesdiftmp = mesdif1 - mescon;
        //            //}
        //            mesdiftmp = mesdif1 - mescon;
        //            if (mesdiftmp < 0)
        //            {
        //                mesdiftmp = 0;
        //            }
        //            ival = ((Math.Pow((1 + PrcAfp), (double)Exp)) - 1);
        //            if (mescon > mesdif1)
        //            {
        //                mesconTmp = mesdif1;
        //            }
        //            else
        //            {
        //                mesconTmp = mescon;
        //                //mesconTmp = 0;
        //            }
        //            vppfactor = ival / ((ival * mesconTmp) + (1 - Math.Pow((1 + ival), -(mesdiftmp))) * (1 + ival));
        //            Vpptem = (1 / vppfactor) * sumaporcsob;
        //            penanu = (salcta_eva - vpcm) / (vppen + (Vpptem / PrcTaf));
        //            Add_porc_be = Totpofr;

        //            if (salcta_eva > 0)
        //            {
        //                if (Mone == "NS")
        //                {
        //                    if (Vpptem == 0 || PrcTaf == 0)
        //                    {
        //                        Rete_sim = 0;
        //                    }
        //                    else
        //                    {
        //                        Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
        //                    }
        //                }
        //                else
        //                {
        //                    if (Vpptem == 0 || PrcTaf == 0)
        //                    {
        //                        Rete_sim = 0;
        //                    }
        //                    else
        //                    {
        //                        Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
        //                    }
        //                }
        //            }
        //            sumapenben = 0;
        //            //for (i = 0; i <= Nben; i++)
        //            //{
        //            //    PensionBenef[i] = Porcbe[i] * (penanu / PrcTaf);
        //            //    sumapenben = sumapenben + PensionBenef[i];
        //            //}
        //            sumapenben = (penanu / PrcTaf);
        //            vld_saldo = Rete_sim;
        //            if (vppen > 0)
        //            {
        //                salcta_eva = (salcta_eva - vld_saldo);
        //            }
        //            pensim = 0;
        //            if (vppen > 0)
        //            {
        //                penanu = (salcta_eva - vpcm) / vppen;
        //            }

        //        }
        //    }
        //    else
        //    {
        //        if (TipRen == "D")
        //        {
        //            if (DerGratificacion == "S")
        //            {
        //                mesdif1 = mesdif1 + ((mesdif1 / 12) * 2);
        //            }
        //            //if (mescon <= 0)
        //            //{
        //            //    mesdiftmp = mesdif1;
        //            //}
        //            //else
        //            //{
        //            //    mesdiftmp = mesdif1 - mescon;
        //            //}
        //            mesdiftmp = mesdif1 - mescon;
        //            if (mesdiftmp < 0) { mesdiftmp = 0; }
        //            ival = ((Math.Pow((1 + (PrcAfp)), (double)Exp)) - 1);
        //            if (mescon > mesdif1)
        //            {
        //                mesconTmp = mesdif1;
        //            }
        //            else
        //            {
        //                mesconTmp = mescon;
        //                //mesconTmp = 0;
        //            }
        //            vppfactor = ival / ((ival * mesconTmp) + (1 - Math.Pow((1 + ival), -(mesdiftmp))) * (1 + ival));
        //            Vpptem = (1 / vppfactor) * new_prc;
        //            penanu = (salcta_eva - vpcm) / (vppen + (Vpptem / PrcTaf));
        //            Add_porc_be = Totpofr;

        //            if (salcta_eva > 0)
        //            {
        //                if (Mone == "NS")
        //                {
        //                    if (Vpptem == 0 || PrcTaf == 0)
        //                    {
        //                        Rete_sim = 0;
        //                    }
        //                    else
        //                    {
        //                        Rete_sim = (penanu / PrcTaf) * Vpptem;
        //                        //Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
        //                    }
        //                }
        //                else
        //                {
        //                    if (Vpptem == 0 || PrcTaf == 0)
        //                    {
        //                        Rete_sim = 0;
        //                    }
        //                    else
        //                    {
        //                        Rete_sim = (penanu / PrcTaf) * Vpptem;
        //                        //Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
        //                    }
        //                }
        //            }
        //            sumapenben = 0;
        //            PensionBenef[1] = (penanu / PrcTaf);
        //            sumapenben = sumapenben + PensionBenef[1];

        //            vld_saldo = Rete_sim;
        //            if (vppen > 0)
        //            {
        //                salcta_eva = (salcta_eva - vld_saldo);
        //            }
        //            pensim = 0;
        //            if (vppen > 0)
        //            {
        //                penanu = (salcta_eva - vpcm) / vppen;
        //            }
        //        }
        //    }

        //    for (int ir = 0; ir <= Fintab; ir++)
        //    {
        //        Exced[ir] = 0;
        //    }
        //    if (tipoPension != "S")
        //    {
        //        sumaporcsob = 1;
        //    }

        //    //calcula la tci de nuevo para la nueva tasa por si se necesita en algun momento
        //    tci = 0;
        //    tce = 0;
        //    vpte = 0;
        //    vpte2 = 0;
        //    difres = 0;
        //    difre1 = 0;
        //    tir = 0;
        //    tinc = 0.00001;

        //    CalTce2:
        //    //EMPIEZA LA RUTINA DEL CALCULO DE TCE 
        //    //Tasa = (Math.Pow((1 + tir / 100), (double)Exp) - 1);
        //    Tasa = (tir / 100);
        //    i = 1;
        //    cr = 1;
        //    for (i = 0; i <= nmax; i++)
        //        for (i = 0; i <= nmax; i++)
        //        {
        //            if (i < mescosto)
        //            {
        //                //vpte2 = vpte2 + ((Flupen[i + 1] * penanu) / 1);
        //            }
        //            else
        //            {
        //                if (i == 850)
        //                {
        //                    r = 1;
        //                }
        //                fpagosRes[cr] = (Flupen[i + 1] * penanu + Flucm[i + 1]) / Math.Pow((1 + Tasa), cr);
        //                vpte = vpte + ((Flupen[i + 1] * penanu + Flucm[i + 1]) / Math.Pow((1 + Tasa), cr));
        //                vpte2 = vpte2 + (((Flupen[i + 1] * penanu) + Flucm[i + 1]) / factual[cr]);
        //                cr = cr + 1;
        //            }
        //        }
        //    //vpte = vpte;
        //    difres = vpte - vpte2;
        //    if (difres >= 0)
        //    {
        //        tir = tir + tinc;
        //        if (tir > 100)
        //        {
        //            msj = "Tasa TIR mayor o igual a 100%. ";
        //            return ListaResultador;
        //        }
        //        difre1 = difres;
        //        vpte = 0;
        //        vpte2 = 0;
        //        goto CalTce2;
        //    }
        //    tci = (tir / 100);

        //    //tci = tir + tinc * (difres / (difre1 - difres));
        //    //tci = (Math.Pow((1 + (tci / 100)), (double)Exp)) - 1;


        //    //minimo de tasas
        //    tce = amin1(tasaVentamax, tci, tpr);

        //    dflupag = 0;
        //    //TRAE RESERVAS DE MESES CONSUMIDOS
        //    for (int a = 0; a <= mescosto + 1; a++)
        //    {
        //        dflupag = dflupag + (penanu * Flupen[a] + Flucm[a]);// / (Math.Pow((1 + tce), a)));
        //    }

        //    //TRAE RESERVAS
        //    at = 1;
        //    for (int a = (int)mescosto + 1; a <= nmax; a++)
        //    {
        //        fpagosRes[at] = (penanu * Flupen[a + 1] + Flucm[a + 1]);
        //        resfin = resfin + ((penanu * Flupen[a + 1] + Flucm[a + 1]) / (Math.Pow((1 + tce), at)));
        //        at = at + 1;
        //        //dflupag = dflupag + (penanu * Flupen[a] + Flucm[a]);/// (Math.Pow((1 + tce), a)));
        //    }
        //    reserva = resfin + dflupag;
        //    //reserva = penanu * (vppenres) + vpcmres + dflupag;
        //    PERDI = ((reserva / salcta_eva) - 1) * 100;

        //    dUtilImp = 0;
        //    dCapit = 0;
        //    dvarCap = 0;
        //    dComis = 0;
        //    dGasMan = 0;
        //    dPagos = 0;
        //    dVarRes = 0;
        //    dProdInv = 0;
        //    dImp = 0;
        //    dMarSol = (6.75 / 100);
        //    dvarCapAnt = 0;
        //    resfinAnt = 0;

        //    dComis = (salcta_eva * PrcCom);
        //    //dComis = (salcta_eva * (2.6 / 100) * 1.42);
        //    dGasMan = (gastos / 12);
        //    dVarRes = reserva;
        //    dUtilImp = salcta_eva - (dComis + gasemi + dGasMan + dflupag + dVarRes);
        //    dvarCap = dVarRes * dMarSol;

        //    Exced[1] = dUtilImp - dImp - dvarCap;

        //    dvarCapAnt = dvarCap;
        //    resfinAnt = reserva;
        //    vlContarMaximo = nmax;

        //    for (i = 2; i <= nmax; i++)
        //    {

        //        relres = 1;
        //        resfin = 0;
        //        at = 1;
        //        iRes = i + mescosto;
        //        if (iRes == 1332) { goto CalSalExd; }
        //        flupag = penanu * Flupen[iRes] + Flucm[iRes];
        //        for (long a = iRes; a <= nmax; a++)
        //        {
        //            resfin = resfin + ((penanu * Flupen[a + 1] + Flucm[a + 1]) / (Math.Pow((1 + tce), at)));
        //            at = at + 1;
        //        }
        //        resfin = flupag + resfin;
        //        //if (resfin < 0) { goto CalSalExd; };

        //        dGasMan = (gastos / 12) * (resfin / reserva);
        //        dPagos = flupag;

        //        //if (i == 2)
        //        //{
        //        //    dPagos = (dPagos + dflupag);
        //        //}
        //        dVarRes = (resfin - resfinAnt);
        //        dProdInv = (resfinAnt + dvarCapAnt) * Prodin[i];
        //        dUtilImp = -(dGasMan + dPagos + dVarRes) + dProdInv;

        //        dCapit = resfin * dMarSol;
        //        dvarCap = dCapit - dvarCapAnt;
        //        dImp = 0;
        //        if (dUtilImp - dProdInv > 0)
        //        {
        //            dImp = (dUtilImp - dProdInv) * 0.30;
        //        }

        //        Exced[i] = Math.Round(dUtilImp - dImp - dvarCap, 4);

        //        dvarCapAnt = dCapit;
        //        resfinAnt = resfin;
        //    }

        //    CalSalExd:
        //    tasatirc = 0;
        //    sumaex = 0;
        //    CalExd2:
        //    sumaex = 0;
        //    for (i = 1; i <= vlContarMaximo; i++)
        //    {
        //        sumaex = sumaex + Exced[i] / Math.Pow((1 + tasatirc), i);
        //    }
        //    if (sumaex >= 0)
        //    {
        //        tasatirc = tasatirc + tasanc;
        //        goto CalExd2;
        //    }

        //    tasatirc = (Math.Pow((1 + tasatirc), 12)) - 1;
        //    if (tasatirc > 100)
        //    {
        //        break;
        //    }
        //    //if (Math.Round(tasatirc, 4) <= tasac)
        //    //{
        //    //    goto CalTva;
        //    //}
        //    //if (sumaex >= 0)
        //    //{
        //    //    tirVenta = tirVenta + TINC1;
        //    //    if (tirVenta > 100)
        //    //    {
        //    //        break;
        //    //        //sale funcion
        //    //    }
        //    //    sumaex1 = sumaex;
        //    //    sumaex = 0;
        //    //    goto CalTva;
        //    //}

        //    PERDI = ((reserva / salcta_eva) - 1) * 100;

        //    if (PERDI < 0)
        //    {
        //        PERDI = 0;
        //    }
        //    perdis = PERDI;
        //    tce = ((Math.Pow((1 + tce), 12)) - 1) * 100;
        //    tci = ((Math.Pow((1 + tci), 12)) - 1) * 100;
        //    tpr = ((Math.Pow((1 + tpr), 12)) - 1) * 100;
        //    //tasatirc = (Math.Pow((1 + tasatirc), 12)) - 1;
        //    tasatirc = tasatirc * 100;
        //    tirmax = (Math.Pow((1 + tirmax), 12)) - 1;
        //    tirmax = tirmax * 100;
        //    tasa_tir = Math.Round((tasatirc), 2);
        //    tasa_vta = Math.Round(tirmax, 2);
        //    tasa_tce = Math.Round(tce, 2);
        //    tasa_tci = Math.Round(tci, 2);
        //    Tasa_pro = Math.Round(tpr, 2);
        //    tprc_per = Math.Round(perdis, 2);
        // }

        #region FuncionesPropias
      
        public double ObtenerMinimoMaximo(List<double> valores, string tipo)
        {
            return tipo == "MIN" ? valores.Min() : valores.Max();
        }

        #endregion
       
        #region Funciones Auxiliares
        
        /// <summary>
        /// Josué Gutierrez / Antonio Quezada
        /// 2019-08-13
        /// Actualiza las variables meses diferidos, meses garantizados y meses costo
        /// </summary>
        /// <param name="modeloCotizacion"> Modalidad </param>
       
        public void ObtenerMesesCotizacion(beDatosModalidad modeloCotizacion)
        {
            int aniosFechaCotizacion = Convert.ToInt32(modeloCotizacion.FecCot.Substring(0, 4));
            int mesesFechaCotizacion = Convert.ToInt32(modeloCotizacion.FecCot.Substring(4, 2));

            int aniosFechaDevengue = Convert.ToInt32(modeloCotizacion.FecDev.Substring(0, 4));
            int mesesFechaDevengue = Convert.ToInt32(modeloCotizacion.FecDev.Substring(4, 2));

            mesesDiferidos = modeloCotizacion.MesDif;
            mesesGarantizados = modeloCotizacion.MesGar;
            mesesCosto = ((aniosFechaCotizacion * 12) + mesesFechaCotizacion) - ((aniosFechaDevengue * 12) + mesesFechaDevengue);

            if (mesesCosto < 0) { mesesCosto = 0; }
            if (mesesCosto > mesesDiferidos){ mesesDiferidos = 0; }
           
            //OBTIENE LOS MESESCOSTOS
            if (mesesCosto > (mesesDiferidos + mesesGarantizados))
                mesesCosto = mesesCosto - (mesesDiferidos + mesesGarantizados);
            
            //'Periodo Diferido
            if (mesesDiferidos == mesesCosto)
                mesesDiferidos = (mesesCosto - mesesDiferidos);
            
            //'Periodo Garantizado
            if (mesesCosto > (mesesDiferidos + mesesGarantizados))
            {
                mesesGarantizados = 0;
            }
            else
            {
                if (mesesCosto > mesesDiferidos)
                {
                    mesesGarantizados = (mesesDiferidos + mesesGarantizados);
                }
            }
               
        }

        /// <summary>
        /// Josué Gutierrez
        /// 2019-08-15
        /// Obtener los registros de Gratificación
        /// </summary>
        /// <param name="modeloCotizacion"> Modalidad </param>
        /// <returns> Regresa una lista que contiene la información de la Gratificación </returns>
       
        public List<int> ObtenerGratificacion(beDatosModalidad modeloCotizacion)
        {
            List<int> gratificacion = new List<int>();

            int anios = Convert.ToInt32(modeloCotizacion.FecCot.Substring(0, 4));
            int meses = Convert.ToInt32(modeloCotizacion.FecCot.Substring(4, 2));
            int finMortalidad = modeloCotizacion.FinTab;

            string derechoGratificacion = modeloCotizacion.DerGra;

            DateTime fechaCotizacion = new DateTime(anios, meses, 1);

            gratificacion.Add(0);

            for (int mes = 1; mes <= finMortalidad - 1; mes++)
            {
                if ((fechaCotizacion.Month == 7 || fechaCotizacion.Month == 12) && derechoGratificacion == "S")
                    gratificacion.Add(2);
                else
                    gratificacion.Add(1);

                fechaCotizacion = fechaCotizacion.AddMonths(1);
            }

            return gratificacion;
        }

        #endregion
    }
}
