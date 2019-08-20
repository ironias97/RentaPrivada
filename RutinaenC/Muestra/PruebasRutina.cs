using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RutinaenC.Modelos;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
namespace RutinaenC.Muestra
{
    class PruebasRutina
    {


        [STAThread]
        static void Main()
        {
            List<beResultados> ResultadoCot = new List<beResultados>();
            RutinaAct RActuarial = new RutinaAct();
            RutinaActuarial rutinaActuarial = new RutinaActuarial();
            RutinaActMej RActuarialMej = new RutinaActMej();
            //SqlConnection conexion = new SqlConnection("Data Source=DESKTOP-JS5B7VJ\\SERVERPRS2012;Initial Catalog=SeguroRV;Integrated Security=True");
            SqlConnection conexion = new SqlConnection("Data Source=192.168.0.149;Initial Catalog=SeguroRV;User ID=sa;Password=Eisei_dgo");
            //SqlConnection conexion = new SqlConnection("Data Source=150.100.10.30,1433;Initial Catalog=SeguroRV;User ID=usersegrv;Password=USERSEG01");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;



            //#region CUSPP 123456DDDDD4

            //string CodAfp = "245";
            //double Cic = 240522.88;  // c5  y  c20
            //string FecCal = "20181210"; //c16   T.C. 3.331
            //string FecDev = "20180801"; //c21
            //string TipPen = "08"; // 04 y 05 jubilaciones (J), 06 inv total (IT) 07 inv parcial (IP), 08 sobrevivencia (S) c18
            //string TipRen = "1"; // 1 INMEDIATA  c49='', 2 DIFERIDO ES TEMPORAL c49<>'', 6 ESCALONADA c46<>''
            //string TipMod = "3"; // 1 simples sin gartizado  c50='', 3 garantizadas  c50<>''
            //string CodMon = "NS"; // monedad NS soles , US doalares  c48
            //string DerGra = "N"; // GRATIFIACION S O N C51
            //int TipRea = 2; // 2 ajustado (solo usan estas)  c48, 1 indexado  c48 , 0 dolares nomilanes oses sin ajuste  c48
            //double PrcCom = 3.69; //COMISION  c115
            //int MesDif = 0;  // AÑOS DIFERIDOS QUE SOLO S PUEDES USAR CUANDO EL  TipRen=2 LO CONTRAIO tipRen=1 es igual 0 o TipRen=6 es igual >0 años del primer traomo c46
            //int MesGar = 15;  // SI TipMod=3 entoce 10 o 15 de lo contrario 0  c50
            //int RenTmp = 0;  // si tipren=2 es 50 lo contrario tipren=6 valor segundo tramo 50 o 75% c47  tipren=1 es 0 
            //#endregion

            //241	HORIZONTE	4.06
            //242	INTEGRA	4.06
            //243	PROFUTURO	4.10
            //244	UNION VIDA	NULL
            //245	PRIMA	3.19
            //246	HABITAT	3.85

            #region 542641AGEAR4
            string CodAfp = "242";
            double Cic = 1155023.26;
            string FecCal = "20190601";
            string FecDev = "20181122";
            string DevSol = "20181122";
            string cobertura = "S";
            string TipPen = "08";
            //string TipRen = "1";
            string TipRen = "I";
            //string TipMod = "1";
            string TipMod = "S";
            string CodMon = "NS";
            string DerGra = "N";
            int TipRea = 1;
            double PrcCom = 3.69;
            int MesDif = 0;
            int MesGar = 0;
            int RenTmp = 0;
            #endregion

            #region 208800VMGTT5
            //string CodAfp = "243";
            //double Cic = 89658.76;
            //string FecCal = "20190606";
            //string FecDev = "20180701";
            //string DevSol = "20180701";
            //string TipPen = "04";
            //string TipRen = "6";
            //string TipMod = "1";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 7.952;
            //int MesDif = 5;
            //int MesGar = 0;
            //int RenTmp = 50;
            #endregion


            #region 244340PGRZS1 (Op: 147237)
            //string CodAfp = "245";
            //double Cic = 152054.31;
            //string FecCal = "20190604";
            //string FecDev = "20190401";
            //string DevSol = "20190401";
            //string TipPen = "05";
            //string TipRen = "1";
            //string TipMod = "1";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 0;
            //int MesGar = 0;
            //int RenTmp = 0;
            #endregion


            #region 566200BAPIE0
            //string CodAfp = "243";
            //double Cic = 167537.36;
            //string FecCal = "20190528";
            //string FecDev = "20190112";
            //string DevSol = "20180112";
            //string TipPen = "06";
            //string TipRen = "1";
            //string TipMod = "1";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 0;
            //int MesGar = 0;
            //int RenTmp = 0;
            #endregion

            #region 123456ABCDE3 Caso TM JUBILACION FUERA DE LA TM DESCUADRA PENSIÓN(Op: 900115)
            //string CodAfp = "246";
            //double Cic = 617070.35;
            //string FecCal = "20190527";
            //string FecDev = "20190122";
            //string DevSol = "20190122";
            //string TipPen = "04";
            //string TipRen = "2";
            //string TipMod = "3";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 5.11;
            //int MesDif = 5;
            //int MesGar = 15;
            //int RenTmp = 50;
            #endregion

            #region 543070JTVBE3 Caso con TV negativa (Op: 147273)
            //string CodAfp = "242";
            //double Cic = 85822.61;
            //string FecCal = "20190524";
            //string FecDev = "20160114";
            //string DevSol = "20160114";
            //string TipPen = "08";
            //string TipRen = "1";
            //string TipMod = "1";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 5.11;
            //int MesDif = 0;
            //int MesGar = 0;
            //int RenTmp = 0;
            #endregion

            //NO CUADRA LA TIR Y LA PERDIDA
            #region 603220KBGAN6 
            //string CodAfp = "242";
            //double Cic = 459959.92;
            //string FecCal = "20190312";
            //string FecDev = "20181004";
            //string DevSol = "20170404";
            //string TipPen = "07";
            //string TipRen = "2";
            //string TipMod = "3";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 3;
            //int MesGar = 15;
            //int RenTmp = 50;
            #endregion

            #region 593450LCDEÑ2
            //string CodAfp = "246";
            //double Cic = 102590.6;
            //string FecCal = "20190313";
            //string FecDev = "20180302";
            //string DevSol = "20180302";
            //string TipPen = "08";
            //string TipRen = "2";
            //string TipMod = "1";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 1;
            //int MesGar = 0;
            //int RenTmp = 50;
            #endregion

            #region 196871GUMRG7
            ////Jubilación
            //string CodAfp = "246";
            //double Cic = 111701.81;
            //string FecCal = "20190313";
            //string FecDev = "20190326";
            //string DevSol = "20180326";
            //string TipPen = "04";
            //string TipRen = "2";
            //string TipMod = "1";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 1;
            //int MesGar = 0;
            //int RenTmp = 50;
            #endregion
            //LA PERDIDA SE PASA DEL LÍMITE Y PASA CON LAS INMEDIATAS Y CON EL CASO 0 DIF 15 GARANTIZADO
            #region 518480MQSSI8 
            //string CodAfp = "245";
            //double Cic = 1047114.93;
            //string FecCal = "20190307";
            //string FecDev = "20190120";
            //string DevSol = "20160120";
            //string TipPen = "06";
            //string TipRen = "2";
            //string TipMod = "3";
            //string CodMon = "US";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 3;
            //int MesGar = 15;
            //int RenTmp = 50;
            #endregion

            #region 123456LLLLL2 
            //string CodAfp = "246";
            //double Cic = 117070.35;
            //string FecCal = "20190221";
            //string FecDev = "20151122";
            //string DevSol = "20151122";
            //string TipPen = "04";
            //string TipRen = "2";
            //string TipMod = "1";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 5;
            //int MesGar = 0;
            //int RenTmp = 50;
            #endregion

            #region 123456KKKKK2 
            //string CodAfp = "246";
            //double Cic = 417070.35;
            //string FecCal = "20190215";
            //string FecDev = "20111122";
            //string DevSol = "20111122";
            //string TipPen = "04";
            //string TipRen = "6";
            //string TipMod = "1";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 5;
            //int MesGar = 0;
            //int RenTmp = 50;
            #endregion

            #region 513511JLCNP6 
            //string CodAfp = "243";
            //double Cic = 5550.47;
            //string FecCal = "20190218";
            //string FecDev = "20140414";
            //string DevSol = "20140414";
            //string TipPen = "08";
            //string TipRen = "1";
            //string TipMod = "1";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 0;
            //int MesGar = 0;
            //int RenTmp = 0;
            #endregion

            #region 208800VMGTT5 
            //string CodAfp = "243";
            //double Cic = 89658.76;
            //string FecCal = "20190220";
            //string FecDev = "20180701";
            //string DevSol = "20180701";
            //string TipPen = "04";
            //string TipRen = "6";
            //string TipMod = "3";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 15;
            //int MesGar = 15;
            //int RenTmp = 50;
            #endregion

            #region 196871GUMRG7 
            //string CodAfp = "246";
            //double Cic = 111701.81;
            //string FecCal = "20190220";
            //string FecDev = "20180326";
            //string DevSol = "20180326";
            //string TipPen = "04";
            //string TipRen = "6";
            //string TipMod = "3";
            //string CodMon = "NS";
            //string DerGra = "N";
            //int TipRea = 2;
            //double PrcCom = 3.69;
            //int MesDif = 15;
            //int MesGar = 15;
            //int RenTmp = 50;
            #endregion

            #region Carga Matriz de Tablas de Mortalidad

            //string Query = "";
            //int MortalVit_F, MortalTot_F, MortalPar_F, MortalBen_F, MortalVit_M, MortalTot_M, MortalPar_M, MortalBen_M;

            //RutinaMortalidad RMortal = new RutinaMortalidad();
            //List<beMortalidad> ListaMor = new List<beMortalidad>();
            //List<beMortalVar> LisTab = new List<beMortalVar>();
            //try
            //{

            //    Query = "SELECT GLS_NOMBRE,NUM_CORRELATIVO,COD_TIPTABMOR,COD_SEXO,COD_TIPOPER FROM MA_TVAL_MORTAL " +
            //                   "WHERE FEC_INI<= " + FecCal + " AND FEC_TER>= " + FecCal + " AND  COD_TIPTABMOR<>'IND' AND COD_TIPOPER='M'";
            //    cmd.CommandText = Query;
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Connection = conexion;
            //    //Console.WriteLine("{0}", "iNICIA");
            //    conexion.Open();
            //    reader = cmd.ExecuteReader();

            //    if (reader.HasRows ==  true)
            //    {
            //        while (reader.Read())
            //        {
            //            beMortalVar Tab = new beMortalVar();

            //            Tab.GLS_NOMBRE = reader.GetString(0);
            //            Tab.NUM_CORRELATIVO = reader.GetInt32(1);
            //            Tab.COD_TIPTABMOR = reader.GetString(2);
            //            Tab.COD_SEXO = reader.GetString(3);
            //            Tab.COD_TIPOPER = reader.GetString(4);
            //            LisTab.Add(Tab);
            //            //Console.WriteLine("{0}", reader.GetString(0));
            //        }
            //    }
            //    reader.Close();
            //    MortalVit_F = LisTab.Where(x => x.COD_TIPTABMOR == "RV" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalVit_M = LisTab.Where(x => x.COD_TIPTABMOR == "RV" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalTot_F = LisTab.Where(x => x.COD_TIPTABMOR == "MIT" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalTot_M = LisTab.Where(x => x.COD_TIPTABMOR == "MIT" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalPar_F = LisTab.Where(x => x.COD_TIPTABMOR == "MIP" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalPar_M = LisTab.Where(x => x.COD_TIPTABMOR == "MIP" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalBen_F = LisTab.Where(x => x.COD_TIPTABMOR == "B" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalBen_M = LisTab.Where(x => x.COD_TIPTABMOR == "B" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    conexion.Close();
            //    //OBTIENE EL DETALLE DE LAS TABLAS DE MORTALIDAD

            //    Query = "Select num_correlativo as numCor,num_edad AS edad,mto_lx from MA_TVAL_MORDET order by num_edad";
            //    cmd.CommandText = Query;
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Connection = conexion;
            //    conexion.Open();
            //    reader = cmd.ExecuteReader();


            //    List<beMortalidadDet> LisTabMD = new List<beMortalidadDet>();

            //    if (reader != null)
            //    {
            //        while (reader.Read())
            //        {
            //            beMortalidadDet TabMD = new beMortalidadDet();
            //            TabMD.numCor = reader.GetInt32(0);
            //            TabMD.edad = reader.GetInt32(1);
            //            TabMD.mto_lx = reader.GetDecimal(2);
            //            LisTabMD.Add(TabMD);
            //            //Console.WriteLine("{0}", reader.GetString(2));
            //        }
            //        reader.Close();
            //    }
            //    conexion.Close();
            //    //llamo al listado de Tablas de Mortalidad

            //    ListaMor = RMortal.TablaMortalidad(LisTabMD, MortalVit_F, MortalTot_F, MortalPar_F, MortalBen_F, MortalVit_M, MortalTot_M, MortalPar_M, MortalBen_M);

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("{0}", ex.Message);
            //    conexion.Close();

            //}


            #endregion

            #region Carga Tabla Mortaliadad Dinamicas

            string Query = "";
            int Mortal_M_S, Mortal_F_S, Mortal_M_I, Mortal_F_I;

            RutinaMortalidadDin RMortal = new RutinaMortalidadDin();
            List<beMortalidadDin> LisTabDin = new List<beMortalidadDin>();
            List<beMortalidadDinVal> ListaMor = new List<beMortalidadDinVal>();
            try
            {

                Query = "SELECT GLS_DESCRIPCION, NUM_CORRELATIVO, COD_SEXO, COD_INVALIDEZ, NUM_ANNO FROM MA_TMAE_MORTAL_DIN " +
                        "WHERE FEC_INIVIG<='" + FecCal + "' AND FEC_FINVIG>='" + FecCal + "'";

                cmd.CommandText = Query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conexion;
                conexion.Open();
                reader = cmd.ExecuteReader();

                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        beMortalidadDin Tab = new beMortalidadDin();

                        Tab.GLS_DESCRIPCION = reader.GetString(0);
                        Tab.NUM_CORRELATIVO = reader.GetInt32(1);
                        Tab.COD_SEXO = reader.GetString(2);
                        Tab.COD_INVALIDEZ = reader.GetString(3);
                        Tab.NUM_ANNO = reader.GetInt32(4);
                        LisTabDin.Add(Tab);
                    }
                }
                reader.Close();
                Mortal_M_S = LisTabDin.Where(x => x.COD_INVALIDEZ == "S" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_F_S = LisTabDin.Where(x => x.COD_INVALIDEZ == "S" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_M_I = LisTabDin.Where(x => x.COD_INVALIDEZ == "I" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_F_I = LisTabDin.Where(x => x.COD_INVALIDEZ == "I" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                conexion.Close();
                //OBTIENE EL DETALLE DE LAS TABLAS DE MORTALIDAD

                Query = "SELECT NUM_CORRELATIVO, NUM_EDAD, MTO_QX, MTO_AXX FROM  MA_TMAE_DETMOR_DIN ORDER BY 1,2";
                cmd.CommandText = Query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conexion;
                conexion.Open();
                reader = cmd.ExecuteReader();

                List<beMortalidadDinDet> LisTabMD = new List<beMortalidadDinDet>();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        beMortalidadDinDet TabMD = new beMortalidadDinDet();
                        TabMD.numCor = reader.GetInt32(0);
                        TabMD.edad = reader.GetInt32(1);
                        TabMD.mto_lx = reader.GetDecimal(2);
                        TabMD.mto_ax = reader.GetDecimal(3);
                        LisTabMD.Add(TabMD);
                    }
                    reader.Close();
                }
                conexion.Close();
                //llamo al listado de Tablas de Mortalidad
                ListaMor = RMortal.TablaMortalidad(LisTabMD, Mortal_M_S, Mortal_F_S, Mortal_M_I, Mortal_F_I);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                conexion.Close();

            }

            #endregion

            #region Carga Parametros

            beDatosModalidad TablaPar = new beDatosModalidad();
            List<beDatosModalidad> ListaPar = new List<beDatosModalidad>();

            TablaPar.NumCot = "9999999999";
            TablaPar.FinTab = 1332;
            TablaPar.Cobertura = cobertura;
            TablaPar.Tippen = TipPen;
            TablaPar.TipRen = TipRen;
            TablaPar.TipMod = TipMod;
            TablaPar.MesGar = MesGar * 12;
            TablaPar.FecCot = FecCal;
            TablaPar.MtoPri = Cic;
            TablaPar.MesDif = MesDif * 12;
            TablaPar.FecDev = FecDev;
            TablaPar.DevSol = DevSol;
            TablaPar.TipSex = "F";
            TablaPar.FecNac = "19660721";
            //obtiene EL GASTO DE SEPELIO DEL MES
            Query = "SELECT MTO_CUOMOR FROM MA_TVAL_CUOMOR WHERE " + FecCal + " BETWEEN FEC_INICUOMOR AND FEC_TERCUOMOR";
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            conexion.Open();

            reader = cmd.ExecuteReader();

            if (reader != null)
            {
                while (reader.Read())
                {
                    TablaPar.MtoGS = (double)reader.GetDecimal(0);
                }
                reader.Close();
            }
            conexion.Close();
            //
            //OBTIENE LA AFP RENTAB
            Query = "SELECT MTO_ELEMENTO FROM MA_TPAR_TABCOD WHERE COD_TABLA='AF' AND COD_ELEMENTO=" + CodAfp;
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            conexion.Open();

            reader = cmd.ExecuteReader();

            if (reader != null)
            {
                while (reader.Read())
                {
                    TablaPar.RenAfp = (double)reader.GetDecimal(0);
                }
                reader.Close();
            }
            conexion.Close();
            //

            TablaPar.RenTmp = RenTmp;
            TablaPar.NumCor = 1;
            TablaPar.PrcTas = 0;
            TablaPar.PrcCom = PrcCom;
            TablaPar.CodMon = CodMon;
            TablaPar.DerCre = "N";
            TablaPar.DerGra = DerGra;
            if (TipPen == "04" || TipPen == "05")
            {
                TablaPar.IndCob = "N";
            }
            else
            {
                TablaPar.IndCob = "S";
            }

            //obtiene la region de la tasa
            Query = "SELECT COD_REGION FROM MA_TPAR_REGION WHERE " + Cic.ToString() + " BETWEEN MTO_MINIMO AND MTO_MAXIMO";
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            conexion.Open();

            reader = cmd.ExecuteReader();

            if (reader != null)
            {
                while (reader.Read())
                {
                    TablaPar.CodReg = reader.GetString(0); ;
                }
                reader.Close();
            }
            conexion.Close();
            //
            TablaPar.RegEst = "14";
            TablaPar.TipRea = TipRea;
            TablaPar.PrcMen = 0.16515813;
            TablaPar.PrcTri = 0.49629316;
            TablaPar.PrcAnu = 2;
            //SE AGREGO RRR 21/12/2018
            int EdaLim = 0;
            if (DateTime.ParseExact(DevSol, "yyyyMMdd", CultureInfo.InvariantCulture) < DateTime.ParseExact("20130801", "yyyyMMdd", CultureInfo.InvariantCulture))
            {
                EdaLim = 216;
            }
            else
            {
                EdaLim = 336;
            };
            TablaPar.EdaLim = EdaLim;
            TablaPar.MinRC = 0;
            TablaPar.RepRC = 0;
            TablaPar.ValCam = 3.274;
            ListaPar.Add(TablaPar);


            #endregion

            #region Carga Beneficiario
            //S	1/08/1990	M	T

            #region 542641AGEAR4 descuadre SISCO
            ////////agrega Titular
            beDatosBen TablaBen = new beDatosBen();
            List<beDatosBen> ListaBen = new List<beDatosBen>();
            TablaBen.NumOrd = 1;
            TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            TablaBen.FecNac = "19660608"; // C33 
            TablaBen.GruFam = "00";
            TablaBen.TipSex = "M"; //D33
            //(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            TablaBen.FecInv = "";
            TablaBen.DerPen = "99";
            TablaBen.PrcPen = 0;
            TablaBen.PrcLeg = 0;
            TablaBen.PrcGar = 0;
            TablaBen.NacHM = "";
            TablaBen.FacFal = FecDev; // SOLO SE LLENA SI ES TIPPEN=08
            TablaBen.derCre = "N";
            ListaBen.Add(TablaBen);

            ////N	1/08/2015	M	HJ
            ////agrega Beneficiarios 
            beDatosBen TablaBen2 = new beDatosBen();
            TablaBen2.NumOrd = 2;
            TablaBen2.CodPar = "11"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            TablaBen2.FecNac = "19640702"; // FECHA NACEMINTO
            TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            TablaBen2.TipSex = "F"; // SEXO
            TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            TablaBen2.FecInv = "";
            TablaBen2.DerPen = "99";
            TablaBen2.PrcPen = 0;
            TablaBen2.PrcLeg = 0;
            TablaBen2.PrcGar = 0;
            TablaBen2.NacHM = "";
            TablaBen2.FacFal = "";
            TablaBen2.derCre = "N";
            ListaBen.Add(TablaBen2);

            ////////N	1/08/2015	M	HJ
            ////////agrega Beneficiarios 
            beDatosBen TablaBen3 = new beDatosBen();
            TablaBen3.NumOrd = 3;
            TablaBen3.CodPar = "30"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            TablaBen3.FecNac = "20021128"; // FECHA NACEMINTO
            TablaBen3.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            TablaBen3.TipSex = "F"; // SEXO
            TablaBen3.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            TablaBen3.FecInv = "";
            TablaBen3.DerPen = "99";
            TablaBen3.PrcPen = 0;
            TablaBen3.PrcLeg = 0;
            TablaBen3.PrcGar = 0;
            TablaBen3.NacHM = "";
            TablaBen3.FacFal = "";
            TablaBen3.derCre = "N";
            ListaBen.Add(TablaBen3);


            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            beDatosBen TablaBen4 = new beDatosBen();
            TablaBen4.NumOrd = 4;
            TablaBen4.CodPar = "30"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            TablaBen4.FecNac = "20040827"; // FECHA NACEMINTO
            TablaBen4.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            TablaBen4.TipSex = "M"; // SEXO
            TablaBen4.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            TablaBen4.FecInv = "";
            TablaBen4.DerPen = "99";
            TablaBen4.PrcPen = 0;
            TablaBen4.PrcLeg = 0;
            TablaBen4.PrcGar = 0;
            TablaBen4.NacHM = "";
            TablaBen4.FacFal = "";
            TablaBen4.derCre = "N";
            ListaBen.Add(TablaBen4);
            #endregion

            #region 208800VMGTT5
            //////////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19570303"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "F"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);
            #endregion

            #region 244340PGRZS1 Caso con TV negativa (Op: 147237)
            //////////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19661125"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "F"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            #endregion

            #region 566200BAPIE0
            ////////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19670530"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "F"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "T"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = FecDev;
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "10"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "19650227"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "M"; // SEXO
            //TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen3 = new beDatosBen();
            //TablaBen3.NumOrd = 3;
            //TablaBen3.CodPar = "42"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen3.FecNac = "19400727"; // FECHA NACEMINTO
            //TablaBen3.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen3.TipSex = "F"; // SEXO
            //TablaBen3.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen3.FecInv = "";
            //TablaBen3.DerPen = "99";
            //TablaBen3.PrcPen = 0;
            //TablaBen3.PrcLeg = 0;
            //TablaBen3.PrcGar = 0;
            //TablaBen3.NacHM = "";
            //TablaBen3.FacFal = "";
            //TablaBen3.derCre = "N";
            //ListaBen.Add(TablaBen3);

            #endregion

            #region 123456ABCDE4 Caso TM (Op: 900114)
            ////////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19190811"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "M"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "10"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "19200117"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "F"; // SEXO
            //TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);

            #endregion

            #region 567441JFEAI0 Caso con TV negativa (Op: 142719)
            //////////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19660721"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "F"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = FecDev; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "30"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "20001217"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "F"; // SEXO
            //TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);

            #endregion

            #region 196871GUMRG7
            ////////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19531126"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "M"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "11"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "19490715"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "F"; // SEXO
            //TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);

            ////////N	1/08/2015	M	HJ
            ////////agrega Beneficiarios 
            //beDatosBen TablaBen3 = new beDatosBen();
            //TablaBen3.NumOrd = 3;
            //TablaBen3.CodPar = "30"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen3.FecNac = "20050608"; // FECHA NACEMINTO
            //TablaBen3.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen3.TipSex = "F"; // SEXO
            //TablaBen3.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen3.FecInv = "";
            //TablaBen3.DerPen = "99";
            //TablaBen3.PrcPen = 0;
            //TablaBen3.PrcLeg = 0;
            //TablaBen3.PrcGar = 0;
            //TablaBen3.NacHM = "";
            //TablaBen3.FacFal = "";
            //TablaBen3.derCre = "N";
            //ListaBen.Add(TablaBen3);

            #endregion

            #region 593450LCDEÑ2
            ////////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19800506"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "F"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = FecDev; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "41"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "19470520"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "M"; // SEXO
            //TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);


            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen3 = new beDatosBen();
            //TablaBen3.NumOrd = 3;
            //TablaBen3.CodPar = "30"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen3.FecNac = "20050608"; // FECHA NACEMINTO
            //TablaBen3.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen3.TipSex = "F"; // SEXO
            //TablaBen3.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen3.FecInv = "";
            //TablaBen3.DerPen = "99";
            //TablaBen3.PrcPen = 0;
            //TablaBen3.PrcLeg = 0;
            //TablaBen3.PrcGar = 0;
            //TablaBen3.NacHM = "";
            //TablaBen3.FacFal = "";
            //TablaBen3.derCre = "N";
            //ListaBen.Add(TablaBen3);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen4 = new beDatosBen();
            //TablaBen4.NumOrd = 4;
            //TablaBen4.CodPar = "11"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen4.FecNac = "19760824"; // FECHA NACEMINTO
            //TablaBen4.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen4.TipSex = "M"; // SEXO
            //TablaBen4.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen4.FecInv = "";
            //TablaBen4.DerPen = "99";
            //TablaBen4.PrcPen = 0;
            //TablaBen4.PrcLeg = 0;
            //TablaBen4.PrcGar = 0;
            //TablaBen4.NacHM = "";
            //TablaBen4.FacFal = "";
            //TablaBen4.derCre = "N";
            //ListaBen.Add(TablaBen4);

            #endregion

            #region 603220KBGAN6
            ////////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19830108"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "F"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "P"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = FecDev;
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "30"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "20120721"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "M"; // SEXO
            //TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);
            #endregion

            #region 518480MQSSI8 
            ////////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19591027"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "F"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "T"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = FecDev;
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "30"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "20030512"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "M"; // SEXO
            //TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);
            #endregion

            #region 123456LLLLL2 
            //////agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19760811"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "M"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "11"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "19840901"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "F"; // SEXO
            //TablaBen2.TipInv = "T"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);

            //////N	16/09/2002	F	HI
            ////agrega Beneficiarios
            //beDatosBen TablaBen3 = new beDatosBen();
            //TablaBen3.NumOrd = 3;
            //TablaBen3.CodPar = "30";  //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen3.FecNac = "20010709";
            //TablaBen3.GruFam = "01";
            //TablaBen3.TipSex = "F";
            //TablaBen3.TipInv = "N";
            //TablaBen3.FecInv = "";
            //TablaBen3.DerPen = "99";
            //TablaBen3.PrcPen = 0;
            //TablaBen3.PrcLeg = 0;
            //TablaBen3.PrcGar = 0;
            //TablaBen3.NacHM = "";
            //TablaBen3.FacFal = "";
            //TablaBen3.derCre = "N";
            //ListaBen.Add(TablaBen3);

            //////////////N	1/08/1990	F	CY
            //////////////agrega Beneficiarios
            //beDatosBen TablaBen4 = new beDatosBen();
            //TablaBen4.NumOrd = 4;
            //TablaBen4.CodPar = "42";
            //TablaBen4.FecNac = "19520217";
            //TablaBen4.GruFam = "01";
            //TablaBen4.TipSex = "F";
            //TablaBen4.TipInv = "N";
            //TablaBen4.FecInv = "";
            //TablaBen4.DerPen = "99";
            //TablaBen4.PrcPen = 0;
            //TablaBen4.PrcLeg = 0;
            //TablaBen4.PrcGar = 0;
            //TablaBen4.NacHM = "";
            //TablaBen4.FacFal = "";
            //TablaBen4.derCre = "N";
            //ListaBen.Add(TablaBen4);
            #endregion

            #region 123456KKKKK2 
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19760801"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "M"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "11"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "19840901"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "F"; // SEXO
            //TablaBen2.TipInv = "T"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);

            //////N	16/09/2002	F	HI
            ////agrega Beneficiarios
            //beDatosBen TablaBen3 = new beDatosBen();
            //TablaBen3.NumOrd = 3;
            //TablaBen3.CodPar = "30";  //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen3.FecNac = "20050709";
            //TablaBen3.GruFam = "01";
            //TablaBen3.TipSex = "F";
            //TablaBen3.TipInv = "N";
            //TablaBen3.FecInv = "";
            //TablaBen3.DerPen = "99";
            //TablaBen3.PrcPen = 0;
            //TablaBen3.PrcLeg = 0;
            //TablaBen3.PrcGar = 0;
            //TablaBen3.NacHM = "";
            //TablaBen3.FacFal = "";
            //TablaBen3.derCre = "N";
            //ListaBen.Add(TablaBen3);

            //////////////N	1/08/1990	F	CY
            //////////////agrega Beneficiarios
            //beDatosBen TablaBen4 = new beDatosBen();
            //TablaBen4.NumOrd = 4;
            //TablaBen4.CodPar = "42";
            //TablaBen4.FecNac = "19520217";
            //TablaBen4.GruFam = "01";
            //TablaBen4.TipSex = "F";
            //TablaBen4.TipInv = "N";
            //TablaBen4.FecInv = "";
            //TablaBen4.DerPen = "99";
            //TablaBen4.PrcPen = 0;
            //TablaBen4.PrcLeg = 0;
            //TablaBen4.PrcGar = 0;
            //TablaBen4.NacHM = "";
            //TablaBen4.FacFal = "";
            //TablaBen4.derCre = "N";
            //ListaBen.Add(TablaBen4);
            #endregion

            #region 513511JLCNP6
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19580617"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "M"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = FecDev; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "10"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "19640413"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "F"; // SEXO
            //TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);
            #endregion

            #region 208800VMGTT5 
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19570303"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "F"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "T"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);
            #endregion

            #region 196871GUMRG7 
            //agrega Titular
            //beDatosBen TablaBen = new beDatosBen();
            //List<beDatosBen> ListaBen = new List<beDatosBen>();
            //TablaBen.NumOrd = 1;
            //TablaBen.CodPar = "99"; //E33 SI ES T 99, SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen.FecNac = "19531126"; // C33 
            //TablaBen.GruFam = "00";
            //TablaBen.TipSex = "M"; //D33
            ////(Con invalidez S o T no cuadra la pensión ¿Como se debe enviar el titular invalido?)
            //TablaBen.TipInv = "N"; //B33 = S ES invalida en el excel SIEMPRE Y CUANDO si tippen= 06 es T (total) O si es =07 P (parcial), SI B33 = N TIENES Q PONER N (NO IVALIDO)  
            //TablaBen.FecInv = "";
            //TablaBen.DerPen = "99";
            //TablaBen.PrcPen = 0;
            //TablaBen.PrcLeg = 0;
            //TablaBen.PrcGar = 0;
            //TablaBen.NacHM = "";
            //TablaBen.FacFal = ""; // SOLO SE LLENA SI ES TIPPEN=08
            //TablaBen.derCre = "N";
            //ListaBen.Add(TablaBen);

            //////N	1/08/2015	M	HJ
            //////agrega Beneficiarios 
            //beDatosBen TablaBen2 = new beDatosBen();
            //TablaBen2.NumOrd = 2;
            //TablaBen2.CodPar = "10"; //SI ES H 30 SI ES C ES 10 SI NO TIENE HIJOS Y SI TIENE ES 11,  P ES 41 PADRE 42 MADRE
            //TablaBen2.FecNac = "19490715"; // FECHA NACEMINTO
            //TablaBen2.GruFam = "01"; // FIJO PARA TOSDOS LOS BEN QUE NO SON TITULAT
            //TablaBen2.TipSex = "F"; // SEXO
            //TablaBen2.TipInv = "N"; // S SI , N NO IGUAL QUE EXCEL
            //TablaBen2.FecInv = "";
            //TablaBen2.DerPen = "99";
            //TablaBen2.PrcPen = 0;
            //TablaBen2.PrcLeg = 0;
            //TablaBen2.PrcGar = 0;
            //TablaBen2.NacHM = "";
            //TablaBen2.FacFal = "";
            //TablaBen2.derCre = "N";
            //ListaBen.Add(TablaBen2);

            ////////////////agrega Beneficiarios
            //beDatosBen TablaBen5 = new beDatosBen();
            //TablaBen5.NumOrd = 5;
            //TablaBen5.CodPar = "30";
            //TablaBen5.FecNac = "20010803";
            //TablaBen5.GruFam = "01";
            //TablaBen5.TipSex = "M";
            //TablaBen5.TipInv = "N";
            //TablaBen5.FecInv = "";
            //TablaBen5.DerPen = "99";
            //TablaBen5.PrcPen = 0;
            //TablaBen5.PrcLeg = 0;
            //TablaBen5.PrcGar = 0;
            //TablaBen5.NacHM = "";
            //TablaBen5.FacFal = "";
            //TablaBen5.derCre = "N";
            //ListaBen.Add(TablaBen5);

            ////////////////agrega Beneficiarios
            //beDatosBen TablaBen6 = new beDatosBen();
            //TablaBen6.NumOrd = 6;
            //TablaBen6.CodPar = "30";
            //TablaBen6.FecNac = "20040322";
            //TablaBen6.GruFam = "01";
            //TablaBen6.TipSex = "F";
            //TablaBen6.TipInv = "N";
            //TablaBen6.FecInv = "";
            //TablaBen6.DerPen = "99";
            //TablaBen6.PrcPen = 0;
            //TablaBen6.PrcLeg = 0;
            //TablaBen6.PrcGar = 0;
            //TablaBen6.NacHM = "";
            //TablaBen6.FacFal = "";
            //TablaBen6.derCre = "N";
            //ListaBen.Add(TablaBen6);

            ////////////agrega Beneficiarios
            //beDatosBen TablaBen7 = new beDatosBen();
            //TablaBen7.NumOrd = 7;
            //TablaBen7.CodPar = "30";
            //TablaBen7.FecNac = "20120505";
            //TablaBen7.GruFam = "01";
            //TablaBen7.TipSex = "F";
            //TablaBen7.TipInv = "N";
            //TablaBen7.FecInv = "";
            //TablaBen7.DerPen = "99";
            //TablaBen7.PrcPen = 0;
            //TablaBen7.PrcLeg = 0;
            //TablaBen7.PrcGar = 0;
            //TablaBen7.NacHM = "";
            //TablaBen7.FacFal = "";
            //TablaBen7.derCre = "N";
            //ListaBen.Add(TablaBen7);
            #endregion


            RutinaPorcentaje RP = new RutinaPorcentaje();
            Query = "Select cod_par, cod_sitinv, cod_sexo, prc_pension from ma_tval_porpar where '" + FecCal + "' between fec_inivigpor and fec_tervigpor;";
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            conexion.Open();
            reader = cmd.ExecuteReader();

            List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    bePorcenLegales TabPL = new bePorcenLegales();
                    TabPL.COD_PAR = int.Parse(reader.GetString(0));
                    TabPL.COD_SITINV = reader.GetString(1);
                    TabPL.COD_SEXO = reader.GetString(2);
                    TabPL.PRC_PENSION = (double)reader.GetDecimal(3);
                    LisTabPL.Add(TabPL);
                    //Console.WriteLine("{0}", reader.GetString(2));
                }
                reader.Close();
            }
            conexion.Close();
            //obtiene los porcentajes de Pension de Cada Beneficiario
            ListaBen = RP.PorcentajeBen(ListaBen, FecDev, "S", TipPen, EdaLim, LisTabPL);

            foreach (beDatosBen be in ListaBen)
            {
                Console.WriteLine("{0} - {1} - {2}% - {3}", be.CodPar, be.FecNac, be.PrcPen, be.NumOrd);
            }

            Console.WriteLine("{0}", RP.msj);

            #endregion

            #region Carga Gastos Tasas Indicadores


            Query = "SELECT G.COD_MONEDA,TV.COD_TIPPENSION ,T.COD_REGION ,G.MTO_GASADM,G.MTO_GASEMI, G.PRC_ENDEUDA,G.PRC_GASCTRSUP, " +
                    "(SELECT MTO_IMP from MA_TVAL_IMPUESTO where FEC_INIIMP<=  '" + FecCal + "' and FEC_TERIMP >= '" + FecCal + "' ) as MTO_IMP, " +
                    "P.PRC_MAXIMO  , TV.PRC_MAXIMO AS TASAV ,TV.PRC_MINIMO AS TASAVMIN , T.PRC_MINIMO, convert(integer, T.COD_TIPREAJUSTE) as COD_TIPREAJUSTE " +
                    "FROM PT_TVAL_GASTO G " +
                    "join PT_TVAL_MINMAXTAS_INI TV on G.COD_MONEDA=TV.COD_MONEDA AND " +
                    "TV.COD_TIPREAJUSTE=G.COD_TIPREAJUSTE " +
                    "join PT_TVAL_MINMAXPER_INI P on " +
                    "TV.COD_MONEDA=P.COD_MONEDA AND " +
                    "TV.COD_TIPREAJUSTE=P.COD_TIPREAJUSTE AND " +
                    "TV.COD_REGION=P.COD_REGION " +
                    "join PT_TVAL_MINMAXTIR_INI T on " +
                    "TV.COD_MONEDA=T.COD_MONEDA AND " +
                    "TV.COD_TIPREAJUSTE=T.COD_TIPREAJUSTE AND " +
                    "TV.COD_REGION=T.COD_REGION " +
                    "WHERE  G.FEC_INIVIG<='" + FecCal + "' " +
                    "AND G.FEC_TERVIG>= '" + FecCal + "' " +
                    "AND P.FEC_INIMINMAX<='" + FecCal + "' " +
                    "AND P.FEC_TERMINMAX>= '" + FecCal + "' " +
                    "AND TV.COD_TIPPENSION= '" + TipPen + "' " +
                    "AND TV.FEC_INIMINMAX <= '" + FecCal + "' " +
                    "AND TV.FEC_TERMINMAX >= '" + FecCal + "' " +
                    "AND T.FEC_INIMINMAX <=  '" + FecCal + "' " +
                    "AND T.FEC_TERMINMAX >=  '" + FecCal + "' " +
                    "ORDER BY G.COD_MONEDA,TV.COD_TIPPENSION,T.COD_REGION ";

            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            //Console.WriteLine("{0}", "iNICIA");
            conexion.Open();

            reader = cmd.ExecuteReader();


            List<beDatosTasasPar> ListaTas = new List<beDatosTasasPar>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    beDatosTasasPar tablaTas = new beDatosTasasPar();
                    tablaTas.CodMon = reader.GetString(0);
                    tablaTas.TipPen = reader.GetString(1);
                    tablaTas.CodReg = reader.GetString(2);
                    tablaTas.PriMin = 0;
                    tablaTas.PriMax = 99999999.99;
                    tablaTas.MtoGad = (double)reader.GetDecimal(3);
                    tablaTas.MtoGem = (double)reader.GetDecimal(4);
                    tablaTas.PrcDeu = (double)reader.GetDecimal(5);
                    tablaTas.MtoImp = (double)reader.GetDecimal(6);
                    tablaTas.PrcPer = (double)reader.GetDecimal(8);
                    tablaTas.PrcTas = (double)reader.GetDecimal(9);
                    tablaTas.PrcTir = (double)reader.GetDecimal(11);
                    tablaTas.TipRea = reader.GetInt32(12);

                    //TablaPar.TipRea = reader.GetInt32(12);
                    ListaTas.Add(tablaTas);
                    //Console.WriteLine("{0}", reader.GetString(0));
                }
                reader.Close();
            }
            conexion.Close();
            #endregion

            #region Carga Tasa Mercado

            string Mes = FecCal.Substring(4, 2).Replace("0", "").Trim();

            Query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE, PRC_MES" + Mes + " AS PRC_MES FROM MA_TVAL_TASATM WHERE NUM_ANNO=" + FecCal.Substring(0, 4);
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            //Console.WriteLine("{0}", "iNICIA");
            conexion.Open();

            reader = cmd.ExecuteReader();
            List<beTasaMercado> ListaTM = new List<beTasaMercado>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    beTasaMercado tablaTM = new beTasaMercado();
                    tablaTM.CodMon = reader.GetString(0);
                    tablaTM.TipRea = (Int32)reader.GetDecimal(1);
                    tablaTM.PrcVal = (double)reader.GetDecimal(2);
                    ListaTM.Add(tablaTM);
                    //Console.WriteLine("{0}", reader.GetString(0));
                }
                reader.Close();
            }
            conexion.Close();
            #endregion

            #region Carga Tasa Anclaje

            Query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE, PRC_TASA FROM ma_tval_tasaanclaje WHERE " + FecCal + " BETWEEN fec_inivig AND fec_tervig ";
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            //Console.WriteLine("{0}", "iNICIA");
            conexion.Open();

            reader = cmd.ExecuteReader();
            List<beTasaAnclaje> ListaTA = new List<beTasaAnclaje>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    beTasaAnclaje tablaTA = new beTasaAnclaje();
                    tablaTA.CodMon = reader.GetString(0);
                    tablaTA.TipRea = (Int32)reader.GetDecimal(1);
                    tablaTA.PrcVal = (double)reader.GetDecimal(2);
                    ListaTA.Add(tablaTA);
                    //Console.WriteLine("{0}", reader.GetString(0));
                }
                reader.Close();
            }
            conexion.Close();
            #endregion

            #region Carga Factor VAC
            List<beTasaFacVac> ListaVac = new List<beTasaFacVac>();
            //se debe crear la tabla de Factores Vacpara indexados
            Query = "SELECT convert(datetime, FEC_IPC) FEC_IPC, MTO_IPC, PRC_IPC FROM MA_TVAL_IPC";
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            //Console.WriteLine("{0}", "iNICIA");
            conexion.Open();

            reader = cmd.ExecuteReader();
            //List<beTasaFacVac> ListaVac = new List<beTasaFacVac>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    beTasaFacVac tablaVac = new beTasaFacVac();
                    tablaVac.FEC_IPC = reader.GetDateTime(0);
                    tablaVac.MTO_IPC = (double)reader.GetDecimal(1);
                    tablaVac.PRC_IPC = (double)reader.GetDecimal(2);
                    ListaVac.Add(tablaVac);
                    //Console.WriteLine("{0}", reader.GetString(0));
                }
                reader.Close();
            }
            conexion.Close();
            #endregion

            #region Carga CPK's

            Query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE,PRC_CPK,NUM_ANNO FROM PT_TVAL_CALCE WHERE FEC_INIVIG<=" + FecCal + " AND FEC_TERVIG>=" + FecCal + " ORDER BY NUM_ANNO";
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            //Console.WriteLine("{0}", "iNICIA");
            conexion.Open();

            reader = cmd.ExecuteReader();
            List<beCPK> ListaCPK = new List<beCPK>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    beCPK tablaCPK = new beCPK();
                    tablaCPK.COD_MONEDA = reader.GetString(0);
                    tablaCPK.COD_TIPREAJUSTE = (Int32)reader.GetDecimal(1);
                    tablaCPK.PRC_CPK = (double)reader.GetDecimal(2);
                    tablaCPK.NUM_ANNO = reader.GetInt32(3);
                    ListaCPK.Add(tablaCPK);
                    //Console.WriteLine("{0}", reader.GetString(0));
                }
                reader.Close();
            }
            conexion.Close();
            #endregion

            #region Carga Rentabilidad
            //Query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE,PRC_TASAREN,NUM_ANNO FROM PT_TVAL_RENTABILIDAD WHERE FEC_INIVIG<=" + FecCal + " AND FEC_TERVIG>=" + FecCal + " ORDER BY NUM_ANNO";
            Query = "SELECT COD_MONEDA,CONVERT(INTEGER,COD_TIPREAJUSTE) COD_TIPREAJUSTE,PRC_TASAREN,NUM_ANNO FROM PT_TVAL_RENTABILIDAD WHERE '" + FecCal + "' BETWEEN FEC_INIVIG AND FEC_TERVIG ORDER BY NUM_ANNO";
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            //Console.WriteLine("{0}", "iNICIA");
            conexion.Open();

            reader = cmd.ExecuteReader();
            List<beRentabilidad> ListaRen = new List<beRentabilidad>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    beRentabilidad tablaRen = new beRentabilidad();
                    tablaRen.COD_MONEDA = reader.GetString(0);
                    tablaRen.COD_TIPREAJUSTE = reader.GetInt32(1);
                    tablaRen.PRC_TASAREN = (double)reader.GetDecimal(2);
                    tablaRen.NUM_ANNO = reader.GetInt32(3);
                    ListaRen.Add(tablaRen);
                    //Console.WriteLine("{0}", reader.GetString(0));
                }
                reader.Close();
            }
            conexion.Close();
            #endregion

            #region Tasas Promedio 
            //Query = "SELECT COD_MONEDA, CONVERT(INT,COD_TIPPREAJUSTE) COD_TIPPREAJUSTE, MTO_VTAPROM FROM PT_TVAL_VTAPROM WHERE FEC_VTAPROM='" + FecCal.Substring(0, 6) + "01' AND COD_TIPPENSION='" + TipPen + "'";

            Query = "SELECT COD_MONEDA, CONVERT(INT,COD_TIPPREAJUSTE) COD_TIPPREAJUSTE, MTO_VTAPROM FROM PT_TVAL_VTAPROM A";
            Query = Query + " WHERE FEC_VTAPROM=(SELECT MAX(FEC_VTAPROM) FROM PT_TVAL_VTAPROM WHERE COD_MONEDA = A.COD_MONEDA AND COD_TIPPREAJUSTE=A.COD_TIPPREAJUSTE AND COD_TIPPENSION=A.COD_TIPPENSION)";
            Query = Query + " AND COD_TIPPENSION='" + TipPen + "'";


            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            //Console.WriteLine("{0}", "iNICIA");
            conexion.Open();

            reader = cmd.ExecuteReader();
            List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    beTasasPromedio tablaTasProm = new beTasasPromedio();
                    tablaTasProm.COD_MONEDA = reader.GetString(0);
                    tablaTasProm.COD_TIPPREAJUSTE = reader.GetInt32(1);
                    tablaTasProm.MTO_VTAPROM = (double)reader.GetDecimal(2);
                    ListaTasProm.Add(tablaTasProm);
                }
                reader.Close();
            }
            conexion.Close();
            #endregion

            #region Curva Tasas
            Query = "SELECT COD_MONEDA, CONVERT(INT,COD_TIPREAJUSTE) COD_TIPPREAJUSTE, NUM_MES, MTO_VALOR FROM PT_TVAL_CURVA_TASAS WHERE '" + FecCal + "' BETWEEN FEC_INIVIG AND FEC_TERVIG";
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexion;
            //Console.WriteLine("{0}", "iNICIA");
            conexion.Open();

            reader = cmd.ExecuteReader();
            List<beCurvaTasas> ListaCurvaTasas = new List<beCurvaTasas>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    beCurvaTasas tablaCurvaTasas = new beCurvaTasas();
                    tablaCurvaTasas.COD_MONEDA = reader.GetString(0);
                    tablaCurvaTasas.COD_TIPPREAJUSTE = reader.GetInt32(1);
                    tablaCurvaTasas.NUM_MES = reader.GetInt32(2);
                    tablaCurvaTasas.MTO_VALOR = (double)reader.GetDecimal(3);
                    ListaCurvaTasas.Add(tablaCurvaTasas);
                }
                reader.Close();
            }
            conexion.Close();
            #endregion

            ////LLAMA LISTADO PAA OBTENER LOS DATOS DE COTIZACION
            try
            {
                //descomentar para rutina oficial
                //ResultadoCot = RActuarial.RutinaPension(ListaPar, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas).ToList();
                //descomentar para rutina mejoradas
                //se crearon dos variables comision y tir como parmetror en la funcion
                //ResultadoCot = RActuarialMej.RutinaPension_mej(ListaPar, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, 3.69, 1, 7, 597.74).ToList();
                //ResultadoCot = RActuarialMej.RutinaPension_mej(ListaPar, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, 3.69, 1, 8, 0).ToList();
                //ResultadoCot = RActuarialMej.RutinaPension_mej(ListaPar, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, 7.952, 14.02, 8, 0).ToList();

                //ResultadoCot = RActuarial.RutinaPension(ListaPar, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas).ToList();
                ResultadoCot = rutinaActuarial.RutinaPension(ListaPar[0], ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas).ToList();

                Console.WriteLine("{0}", RActuarial.msj);
                foreach (var item in ResultadoCot)
                {
                    Console.WriteLine("{0}", "Tasa: " + item.PRC_TASAVTA);
                    Console.WriteLine("{0}", "Tir: " + item.PRC_TASATIR);
                    Console.WriteLine("{0}", "Perdida: " + item.PRC_PERCON);
                    Console.WriteLine("{0}", "Pension: " + item.MTO_PENSION);
                    Console.WriteLine("{0}", "PensionAFP: " + item.MTO_RENTATMPAFP);
                    Console.WriteLine("{0}", "Tasa Costo: " + item.PRC_TASATCE);
                    Console.WriteLine("{0}", "Tasa TCI: " + item.PRC_TASATCI);
                    Console.WriteLine("{0}", "MTO_PRIUNIDIF: " + item.MTO_PRIUNIDIF);
                    Console.WriteLine("{0}", "MTO_RENTA: " + item.MTO_RENTA);
                    Console.WriteLine("{0}", "MTO_PRIUNISIM: " + item.MTO_PRIUNISIM);
                    Console.WriteLine("{0}", "MTO_PRIMA: " + item.MTO_PRIMA);
                    Console.WriteLine("{0}", "PRIMA_UNICA: " + item.PRIMA_UNICA);

                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", "Error en la Rutina");
            }
        }
    }
}