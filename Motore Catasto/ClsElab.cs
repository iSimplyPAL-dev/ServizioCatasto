using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using log4net;
using System.IO;
using CatastoInterface;

namespace Motore_Catasto
{
    /// <summary>
    /// Classe che incapsula tutte le costanti
    /// </summary>
    public static class RouteConfig
    {
        public static string PathConfLog4Net
        {
            get
            {
                if (ConfigurationManager.AppSettings["pathfileconflog4net"] != null)
                    return ConfigurationManager.AppSettings["pathfileconflog4net"].ToString();
                else
                    return string.Empty;
            }
        }
        #region DB
        public static string TypeDB
        {
            get
            {
                if (ConfigurationManager.AppSettings["TypeDB"] != null)
                    return ConfigurationManager.AppSettings["TypeDB"].ToString();
                else
                    return "SQL";
            }
        }
        public static string StringConnection
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["CatastoContext"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["CatastoContext"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string StringConnectionICI
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["ICIContext"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["ICIContext"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
        #region Path
        public static string PathDaAcquisire
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathDaAcquisire"] != null)
                {
                    return ConfigurationManager.AppSettings["PathDaAcquisire"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string PathInLavorazione
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathInLavorazione"] != null)
                {
                    return ConfigurationManager.AppSettings["PathInLavorazione"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string PathAcquisiti
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathAcquisiti"] != null)
                {
                    return ConfigurationManager.AppSettings["PathAcquisiti"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string PathScartati
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathScartati"] != null)
                {
                    return ConfigurationManager.AppSettings["PathScartati"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string PathRibaltaDaAcquisire
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathRibaltaDaAcquisire"] != null)
                {
                    return ConfigurationManager.AppSettings["PathRibaltaDaAcquisire"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string PathRibaltaInLavorazione
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathRibaltaInLavorazione"] != null)
                {
                    return ConfigurationManager.AppSettings["PathRibaltaInLavorazione"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string PathRibaltaAcquisiti
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathRibaltaAcquisiti"] != null)
                {
                    return ConfigurationManager.AppSettings["PathRibaltaAcquisiti"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string PathRibaltaScartati
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathRibaltaScartati"] != null)
                {
                    return ConfigurationManager.AppSettings["PathRibaltaScartati"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// Classe per la lavorazione dei flussi.
    /// Il servizio è il cuore del sistema ed è indipendente da qualsiasi gestionale tributario. È diviso nelle 3 funzioni principali:
    /// 1.	Acquisizione flussi
    /// 2.	Conversione di CATASTO in CATWORK
    /// 3.	Incrocio CATWORK con DICHIARAZIONI con conseguente generazione di DICHWORK.
    /// </summary>
    public class ImportFiles
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ImportFiles));
        /// <summary>
        /// Il servizio controlla ogni 10 minuti la presenza di flussi da acquisire al percorso \DA_ACQUISIRE.
        /// Se al percorso sono presenti dei flussi e non c’è nessuna ELABORAZIONE in corso il sistema inizia l’importazione registrandone l’evento in apposita tabella.Un’ELABORAZIONE è in corso fintanto che non è registrato l’esito di tutte le sue fasi; va da sé che se una la fase non è andata a buon fine l’esito delle fasi successive non potrà essere che KO ma comunque registrato.
        /// </summary>
        public void CheckStart()
        {//controllo che ci siano file e che non ci siano elaborazioni in corso
            Log.Debug("CheckStart.start");
            try
            {
                Elaborazione myElab = new Elaborazione();
                string IdCatastale = GetIdEnteToImport();
                if (IdCatastale != string.Empty)
                {
                    if (!CheckElabInCorso(IdCatastale, out myElab))
                    {
                        StartElab(IdCatastale, ref myElab);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("CheckStart.errore::", ex);
            }
        }
        /// <summary>
        ///controllo che ci siano file e che non ci siano elaborazioni in corso
        /// </summary>
        public void CheckRibalta()
        {
            Log.Debug("CheckRibalta.start");
            try
            {
                Elaborazione myElab = new Elaborazione();
                string IdEnte = GetIdEnteToRibalta();
                if (IdEnte != string.Empty)
                {
                    if (CheckRibaltaInCorso(IdEnte, out myElab))
                    {
                        StartRibalta(IdEnte, myElab);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("CheckRibalta.errore::", ex);
            }
        }
        /// <summary>
        ///restituisco i primi 4chr del primo file che trovo avente nome più lungo di 4chr e estensione valida
        /// </summary>
        /// <returns></returns>
        private string GetIdEnteToImport()
        {
            Log.Debug("GetIdEnteToImport.start");
            try
            {
                string CodCatastale = string.Empty;
                string[] ListFiles = Directory.GetFiles(RouteConfig.PathDaAcquisire);
                foreach (string myItem in ListFiles)
                {
                    string[] myFile = myItem.Split(char.Parse("."));
                    myFile[0] = myFile[0].Replace(RouteConfig.PathDaAcquisire, string.Empty);
                    myFile[1] = myFile[1].ToUpper();
                    if (myFile[0].Length > 4 && (myFile[1] == ElaborazioneFile.Estensioni.Fabbricati || myFile[1] == ElaborazioneFile.Estensioni.Storico || myFile[1] == ElaborazioneFile.Estensioni.Terreni || myFile[1] == ElaborazioneFile.Estensioni.Soggetti || myFile[1] == ElaborazioneFile.Estensioni.Titoli || myFile[1] == ElaborazioneFile.Estensioni.Dichiarazioni))
                    {
                        CodCatastale = myFile[0].Substring(0, 4);
                        break;
                    }
                }
                Log.Debug("GetIdEnteToImport.ente->" + CodCatastale);
                return CodCatastale;
            }
            catch (Exception ex)
            {
                Log.Debug("GetIdEnteToImport.errore::", ex);
                return "";
            }
        }
        /// <summary>
        ///restituisco i primi 4chr del primo file che trovo avente nome più lungo di 4chr e estensione valida
        /// </summary>
        /// <returns></returns>
        private string GetIdEnteToRibalta()
        {
            try
            {
                string IdEnte = string.Empty;
                string[] ListFiles = Directory.GetFiles(RouteConfig.PathRibaltaDaAcquisire);
                foreach (string myItem in ListFiles)
                {
                    string[] myFile = myItem.Split(char.Parse("."));
                    myFile[0] = myFile[0].Replace(RouteConfig.PathRibaltaDaAcquisire, string.Empty);
                    myFile[1] = myFile[1].ToUpper();
                    if (myFile[0].Length > 26 && myFile[1] == ElaborazioneFile.Estensioni.Dichiarazioni)
                    {
                        IdEnte = myFile[0].Substring(19, 6);
                        break;
                    }
                }
                return IdEnte;
            }
            catch (Exception ex)
            {
                Log.Debug("GetIdEnteToRibalta.errore::", ex);
                return "";
            }
        }
        /// <summary>
        ///se l'ultima fase è ok allora non ci sono elaborazioni in corso
        /// </summary>
        /// <param name="IdCatastale"></param>
        /// <param name="myElab"></param>
        /// <returns></returns>
        private bool CheckElabInCorso(string IdCatastale, out Elaborazione myElab)
        {
            Log.Debug("CheckElabInCorso.start");
            bool InCorso = true;
            myElab = new Elaborazione();
            try
            {
                List<Elaborazione> listElab = new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).LoadLastElaborazione(IdCatastale);
                if (listElab.Count > 0)
                {
                    Log.Debug("CheckElabInCorso.ho elaborazioni");
                    foreach (Elaborazione myItem in listElab)
                    {
                        myElab = myItem;
                        if (myItem.InizioImport.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                        {
                            if (myItem.EsitoEstrazioneComunioneMancante == Elaborazione.Esito.OK)
                            {
                                Log.Debug("CheckElabInCorso.ho EsitoEstrazioneComunioneMancante=OK");
                                InCorso = false;
                            }
                        }
                        else
                        {
                            Log.Debug("CheckElabInCorso.ho InizioImport=DateTime.MaxValue");
                            InCorso = false;
                        }
                    }
                }
                else
                {
                    Log.Debug("CheckElabInCorso.non ho elaborazioni");
                    InCorso = false;
                }
                return InCorso;
            }
            catch (Exception ex)
            {
                Log.Debug("CheckElabInCorso.errore::", ex);
                return true;
            }
        }
        private bool CheckRibaltaInCorso(string IdEnte, out Elaborazione myElab)
        {//se l'ultima fase è ok allora non ci sono elaborazioni in corso
            bool InCorso = true;
            myElab = new Elaborazione();
            try
            {
                List<Elaborazione> listElab = new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnectionICI).LoadLastElaborazione(IdEnte);
                if (listElab.Count > 0)
                {
                    foreach (Elaborazione myItem in listElab)
                    {
                        myElab = myItem;
                        if (myItem.InizioImport.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                        {
                            if (myItem.EsitoImport == Elaborazione.Esito.OK)
                            {
                                InCorso = false;
                            }
                        }
                    }
                }
                else
                {
                    InCorso = false;
                }
                return InCorso;
            }
            catch (Exception ex)
            {
                Log.Debug("CheckRibaltaInCorso.errore::", ex);
                return true;
            }
        }
        private void StartElab(string IdEnte, ref Elaborazione myElab)
        {
            Log.Debug("StartElab.start");
            try
            {
                if (new ClsImport().StartImport(IdEnte, ref myElab))
                {
                    if (new ClsCatasto().CreateCatastoWork(ref myElab))
                    {
                        new ClsDichiarazioni().CreateDichWork(myElab);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("StartElab.errore::", ex);
                myElab = new Elaborazione();
            }
        }
        private void StartRibalta(string IdEnte, Elaborazione myElab)
        {
            try
            {
                new ClsRibalta().StartImport(IdEnte,myElab);
            }
            catch (Exception ex)
            {
                Log.Debug("StartRibalta.errore::", ex);
            }
        }
    }
    /// <summary>
    /// L’importazione inizia con lo svuotamento delle tabelle e lo spostamento dei flussi dalla directory \DA_ACQUISIRE alla directory \INLAVORAZIONE; da qui i flussi verranno letti, senza nessun ordine specifico, riga per riga ed importati in tabelle piatte aventi la stessa struttura dei flussi. Se l’importazione del singolo file termina con successo il sistema ne registra il nome nell’apposita tabella e lo sposta nella directory \ACQUISITI altrimenti non sarà registrato nella tabella e sarà messo nella directory \SCARTATI.
    /// </summary>
    public class ClsImport
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsImport));

        public bool StartImport(string IdCatastale, ref Elaborazione myElab)
        {
            Log.Debug("StartImport.start");
            try
            {
                //svuoto tabelle
                if (new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).DeleteElaborazionePrec(IdCatastale))
                {
                    //inizio elaborazione
                    myElab.IDCatastale = IdCatastale;
                    myElab.InizioImport = DateTime.Now;
                    new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                    if (myElab.ID > 0)
                    {//inizio importazione
                        return ImportFlussi(ref myElab);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("StartImport.errore::", ex);
                return false;
            }
        }
        private bool ImportFlussi(ref Elaborazione myElab)
        {
            Log.Debug("ImportFlussi.start");
            try
            {
                ElaborazioneFile mySingleFile = new ElaborazioneFile();
                string[] ListFiles = Directory.GetFiles(RouteConfig.PathDaAcquisire);
                foreach (string myItem in ListFiles)
                {
                    string myNameFile = myItem.Replace(RouteConfig.PathDaAcquisire, string.Empty);
                    string[] myFile = myNameFile.Split(char.Parse("."));
                    myFile[1] = myFile[1].ToUpper();
                    if (myFile[0].Length > 4)
                    {
                        if (myFile[0].Substring(0, 4) == myElab.IDCatastale && (myFile[1] == ElaborazioneFile.Estensioni.Fabbricati || myFile[1] == ElaborazioneFile.Estensioni.Storico || myFile[1] == ElaborazioneFile.Estensioni.Terreni || myFile[1] == ElaborazioneFile.Estensioni.Soggetti || myFile[1] == ElaborazioneFile.Estensioni.Titoli || myFile[1] == ElaborazioneFile.Estensioni.Dichiarazioni))
                        {
                            //sposto file
                            if (File.Exists(RouteConfig.PathInLavorazione + myNameFile))
                            {
                                File.Delete(RouteConfig.PathInLavorazione + myNameFile);
                            }
                            File.Move(RouteConfig.PathDaAcquisire + myNameFile, RouteConfig.PathInLavorazione + myNameFile);
                            //importo
                            switch (myFile[1])
                            {
                                case "FAB":
                                case "STO":
                                    mySingleFile = new ElaborazioneFile();
                                    mySingleFile.IDElaborazione = myElab.ID;
                                    mySingleFile.NameFile = myNameFile;
                                    mySingleFile.InizioImport = DateTime.Now;
                                    List<Fabbricato> ListFab = new ClsImportFAB().ReadFAB(RouteConfig.PathInLavorazione + myNameFile, myElab.ID);
                                    mySingleFile.FineImport = DateTime.Now;
                                    if (ListFab.Count > 0)
                                    {
                                        if (!new ClsManageDB().SaveFAB(ListFab, (myFile[1] == ElaborazioneFile.Estensioni.Storico ? "_STORICO" : string.Empty)))
                                        {
                                            if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathScartati + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                            myElab.ListFiles.Add(mySingleFile);
                                            myElab.FineImport = DateTime.Now;
                                            myElab.EsitoImport = "Errore in ImportFlussi.Errore Inserimento Flusso FAB";
                                            myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                            new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                            return false;
                                        }
                                        else
                                        {
                                            if (File.Exists(RouteConfig.PathAcquisiti + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathAcquisiti + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathAcquisiti + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.OK;
                                            myElab.ListFiles.Add(mySingleFile);
                                        }
                                    }
                                    else
                                    {
                                        if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                        {
                                            File.Delete(RouteConfig.PathScartati + myNameFile);
                                        }
                                        File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                        mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                        myElab.ListFiles.Add(mySingleFile);
                                        myElab.FineImport = DateTime.Now;
                                        myElab.EsitoImport = "Errore in ImportFlussi.Errore Lettura Flusso FAB";
                                        myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                        new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                        return false;
                                    }
                                    break;
                                case "TER":
                                    mySingleFile = new ElaborazioneFile();
                                    mySingleFile.IDElaborazione = myElab.ID;
                                    mySingleFile.NameFile = myNameFile;
                                    mySingleFile.InizioImport = DateTime.Now;
                                    List<Terreno> ListTer = new ClsImportTER().ReadTER(RouteConfig.PathInLavorazione + myNameFile, myElab.ID);
                                    mySingleFile.FineImport = DateTime.Now;
                                    if (ListTer.Count > 0)
                                    {
                                        if (!new ClsManageDB().SaveTER(ListTer))
                                        {
                                            if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathScartati + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                            myElab.ListFiles.Add(mySingleFile);
                                            myElab.FineImport = DateTime.Now;
                                            myElab.EsitoImport = "Errore in ImportFlussi.Errore Inserimento Flusso TER";
                                            myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                            new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                            return false;
                                        }
                                        else
                                        {
                                            if (File.Exists(RouteConfig.PathAcquisiti + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathAcquisiti + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathAcquisiti + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.OK;
                                            myElab.ListFiles.Add(mySingleFile);
                                        }
                                    }
                                    else
                                    {
                                        if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                        {
                                            File.Delete(RouteConfig.PathScartati + myNameFile);
                                        }
                                        File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                        mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                        myElab.ListFiles.Add(mySingleFile);
                                        myElab.FineImport = DateTime.Now;
                                        myElab.EsitoImport = "Errore in ImportFlussi.Errore Lettura Flusso TER";
                                        myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                        new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                        return false;
                                    }
                                    break;
                                case "TIT":
                                    mySingleFile = new ElaborazioneFile();
                                    mySingleFile.IDElaborazione = myElab.ID;
                                    mySingleFile.NameFile = myNameFile;
                                    mySingleFile.InizioImport = DateTime.Now;
                                    List<Titoli> ListTit = new ClsImportTIT().ReadTIT(RouteConfig.PathInLavorazione + myNameFile, myElab.ID);
                                    mySingleFile.FineImport = DateTime.Now;
                                    if (ListTit.Count > 0)
                                    {
                                        if (!new ClsManageDB().SaveTIT(ListTit))
                                        {
                                            if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathScartati + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                            myElab.ListFiles.Add(mySingleFile);
                                            myElab.FineImport = DateTime.Now;
                                            myElab.EsitoImport = "Errore in ImportFlussi.Errore Inserimento Flusso TIT";
                                            myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                            new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                            return false;
                                        }
                                        else
                                        {
                                            if (File.Exists(RouteConfig.PathAcquisiti + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathAcquisiti + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathAcquisiti + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.OK;
                                            myElab.ListFiles.Add(mySingleFile);
                                        }
                                    }
                                    else
                                    {
                                        if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                        {
                                            File.Delete(RouteConfig.PathScartati + myNameFile);
                                        }
                                        File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                        mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                        myElab.ListFiles.Add(mySingleFile);
                                        myElab.FineImport = DateTime.Now;
                                        myElab.EsitoImport = "Errore in ImportFlussi.Errore Lettura Flusso TIT";
                                        myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                        new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                        return false;
                                    }
                                    break;
                                case "SOG":
                                    mySingleFile = new ElaborazioneFile();
                                    mySingleFile.IDElaborazione = myElab.ID;
                                    mySingleFile.NameFile = myNameFile;
                                    mySingleFile.InizioImport = DateTime.Now;
                                    List<Soggetto> ListSog = new ClsImportSOG().ReadSOG(RouteConfig.PathInLavorazione + myNameFile, myElab.ID);
                                    mySingleFile.FineImport = DateTime.Now;
                                    if (ListSog.Count > 0)
                                    {
                                        if (!new ClsManageDB().SaveSOG(ListSog))
                                        {
                                            if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathScartati + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                            myElab.ListFiles.Add(mySingleFile);
                                            myElab.FineImport = DateTime.Now;
                                            myElab.EsitoImport = "Errore in ImportFlussi.Errore Inserimento Flusso SOG";
                                            myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                            new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                            return false;
                                        }
                                        else
                                        {
                                            if (File.Exists(RouteConfig.PathAcquisiti + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathAcquisiti + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathAcquisiti + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.OK;
                                            myElab.ListFiles.Add(mySingleFile);
                                        }
                                    }
                                    else
                                    {
                                        if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                        {
                                            File.Delete(RouteConfig.PathScartati + myNameFile);
                                        }
                                        File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                        mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                        myElab.ListFiles.Add(mySingleFile);
                                        myElab.FineImport = DateTime.Now;
                                        myElab.EsitoImport = "Errore in ImportFlussi.Errore Lettura Flusso SOG";
                                        myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                        new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                        return false;
                                    }
                                    break;
                                case "CSV":
                                    mySingleFile = new ElaborazioneFile();
                                    mySingleFile.IDElaborazione = myElab.ID;
                                    mySingleFile.NameFile = myNameFile;
                                    mySingleFile.InizioImport = DateTime.Now;
                                    List<Dichiarazione> ListDic = new ClsImportDIC().ReadDIC(RouteConfig.PathInLavorazione + myNameFile, myElab.ID);
                                    mySingleFile.FineImport = DateTime.Now;
                                    if (ListDic.Count > 0)
                                    {
                                        if (!new ClsManageDB().SaveDIC(ListDic, DBModel.Ambiente_Catasto))
                                        {
                                            if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathScartati + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                            myElab.ListFiles.Add(mySingleFile);
                                            myElab.FineImport = DateTime.Now;
                                            myElab.EsitoImport = "Errore in ImportFlussi.Errore Inserimento Flusso CSV";
                                            myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                            new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                            return false;
                                        }
                                        else
                                        {
                                            if (File.Exists(RouteConfig.PathAcquisiti + myNameFile))
                                            {
                                                File.Delete(RouteConfig.PathAcquisiti + myNameFile);
                                            }
                                            File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathAcquisiti + myNameFile);
                                            mySingleFile.EsitoImport = Elaborazione.Esito.OK;
                                            myElab.ListFiles.Add(mySingleFile);
                                        }
                                    }
                                    else
                                    {
                                        if (File.Exists(RouteConfig.PathScartati + myNameFile))
                                        {
                                            File.Delete(RouteConfig.PathScartati + myNameFile);
                                        }
                                        File.Move(RouteConfig.PathInLavorazione + myNameFile, RouteConfig.PathScartati + myNameFile);
                                        mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                        myElab.ListFiles.Add(mySingleFile);
                                        myElab.FineImport = DateTime.Now;
                                        myElab.EsitoImport = "Errore in ImportFlussi.Errore Lettura Flusso CSV";
                                        myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                                        new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                                        return false;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                myElab.FineImport = DateTime.Now;
                myElab.EsitoImport = Elaborazione.Esito.OK;
                myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("ImportFlussi.errore::", ex);
                myElab.FineImport = DateTime.Now;
                myElab.EsitoImport = "Errore in ImportFlussi:" + ex.Message;
                myElab.EsitoConvert = myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                return false;
            }
        }
    }
    /// <summary>
    /// Se sto importando il flusso dei fabbricati la riga da inserire nel database sarà il risultato della somma di tutti i campi previsti per ogni tipologia record presente.
    /// </summary>
    public class ClsImportFAB
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsImportFAB));
        public List<Fabbricato> ReadFAB(string myFile, int IDElaborazione)
        {
            try
            {
                string line = null;
                Fabbricato myFab = new Fabbricato();
                List<Fabbricato> ListFab = new List<Fabbricato>();
                byte[] filetoRead = System.IO.File.ReadAllBytes(myFile);
                MemoryStream ms = new MemoryStream(filetoRead, 0, filetoRead.Length);
                StreamReader sr = new StreamReader(ms);

                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() == string.Empty)
                            continue;
                        string[] ListColFile = line.Split(char.Parse("|"));
                        //se non cambia l'id immobile ciclo sui tipi record altrimenti inserisco nuova posizione
                        if (myFab.IDImmobile != ListColFile[2])
                        {
                            if (myFab.IDImmobile != string.Empty)
                            {
                                ListFab.Add(myFab);
                            }
                            myFab = new Fabbricato();
                            myFab.IDElaborazione = IDElaborazione;
                        }
                        switch (ListColFile[5])
                        {
                            case "1":
                                myFab = ReadFAB_TR1(ListColFile, myFab);
                                break;
                            case "2":
                                myFab = ReadFAB_TR2(ListColFile, myFab);
                                break;
                            case "3":
                                myFab = ReadFAB_TR3(ListColFile, myFab);
                                break;
                            case "4":
                                myFab = ReadFAB_TR4(ListColFile, myFab);
                                break;
                            case "5":
                                myFab = ReadFAB_TR5(ListColFile, myFab);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("ReadFAB.errore::", ex);
                }
                finally
                {
                    sr.Close();
                }
                return ListFab;
            }
            catch (Exception err)
            {
                Log.Debug("ReadFAB.errore::", err);
                return new List<Fabbricato>();
            }
        }
        private Fabbricato ReadFAB_TR1(string[] ListColFile, Fabbricato Fab)
        {
            Fabbricato myItem = Fab;
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.Progressivo = ListColFile[x];
                x++;
                x++;
                myItem.Zona = ListColFile[x];
                x++;
                myItem.Categoria = ListColFile[x];
                x++;
                myItem.Classe = ListColFile[x];
                x++;
                myItem.Consistenza = ListColFile[x];
                x++;
                myItem.Superficie = ListColFile[x];
                x++;
                myItem.RenditaLire = ListColFile[x];
                x++;
                myItem.RenditaEuro = ListColFile[x];
                x++;
                myItem.Lotto = ListColFile[x];
                x++;
                myItem.Edificio = ListColFile[x];
                x++;
                myItem.Scala = ListColFile[x];
                x++;
                myItem.Interno1 = ListColFile[x];
                x++;
                myItem.Interno2 = ListColFile[x];
                x++;
                myItem.Piano1 = ListColFile[x];
                x++;
                myItem.Piano2 = ListColFile[x];
                x++;
                myItem.Piano3 = ListColFile[x];
                x++;
                myItem.Piano4 = ListColFile[x];
                x++;
                myItem.DataInizioEfficacia = ListColFile[x];
                x++;
                myItem.DataInizioRegistrazioneAtti = ListColFile[x];
                x++;
                myItem.TipoNotaInizio = ListColFile[x];
                x++;
                myItem.NumeroNotaInizio = ListColFile[x];
                x++;
                myItem.ProgressivoNotaInizio = ListColFile[x];
                x++;
                myItem.AnnoNotaInizio = ListColFile[x];
                x++;
                myItem.DataFineEfficacia = ListColFile[x];
                x++;
                myItem.DataFineRegistrazioneAtti = ListColFile[x];
                x++;
                myItem.TipoNotaFine = ListColFile[x];
                x++;
                myItem.NumeroNotaFine = ListColFile[x];
                x++;
                myItem.ProgressivoNotaFine = ListColFile[x];
                x++;
                myItem.AnnoNotaFine = ListColFile[x];
                x++;
                myItem.Partita = ListColFile[x];
                x++;
                myItem.Annotazione = ListColFile[x];
                x++;
                myItem.IDMutazioneIniziale = ListColFile[x];
                x++;
                myItem.IDMutazioneFinale = ListColFile[x];
                x++;
                myItem.ProtocolloNotifica = ListColFile[x];
                x++;
                myItem.DataNotifica = ListColFile[x];
                x++;
                myItem.CodiceCausaleAttoGenerante = ListColFile[x];
                x++;
                myItem.DescrizioneAttoGenerante = ListColFile[x];
                x++;
                myItem.CodiceCausaleAttoConclusivo = ListColFile[x];
                x++;
                myItem.DescrizioneAttoConclusivo = ListColFile[x];
                x++;
                myItem.FlagClassamento = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadFAB_TR.TipoRecord->1.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
        private Fabbricato ReadFAB_TR2(string[] ListColFile, Fabbricato Fab)
        {
            Fabbricato myItem = Fab;
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.Progressivo = ListColFile[x];
                x++;
                x++;
                myItem.SezioneUrbana1 = ListColFile[x];
                x++;
                myItem.Foglio1 = ListColFile[x];
                x++;
                myItem.Numero1 = ListColFile[x];
                x++;
                myItem.Denominatore1 = ListColFile[x];
                x++;
                myItem.Subalterno1 = ListColFile[x];
                x++;
                myItem.Edificialita1 = ListColFile[x];
                x++;
                myItem.SezioneUrbana2 = ListColFile[x];
                x++;
                myItem.Foglio2 = ListColFile[x];
                x++;
                myItem.Numero2 = ListColFile[x];
                x++;
                myItem.Denominatore2 = ListColFile[x];
                x++;
                myItem.Subalterno2 = ListColFile[x];
                x++;
                myItem.Edificialita2 = ListColFile[x];
                x++;
                myItem.SezioneUrbana3 = ListColFile[x];
                x++;
                myItem.Foglio3 = ListColFile[x];
                x++;
                myItem.Numero3 = ListColFile[x];
                x++;
                myItem.Denominatore3 = ListColFile[x];
                x++;
                myItem.Subalterno3 = ListColFile[x];
                x++;
                myItem.Edificialita3 = ListColFile[x];
                x++;
                myItem.SezioneUrbana4 = ListColFile[x];
                x++;
                myItem.Foglio4 = ListColFile[x];
                x++;
                myItem.Numero4 = ListColFile[x];
                x++;
                myItem.Denominatore4 = ListColFile[x];
                x++;
                myItem.Subalterno4 = ListColFile[x];
                x++;
                myItem.Edificialita4 = ListColFile[x];
                x++;
                myItem.SezioneUrbana5 = ListColFile[x];
                x++;
                myItem.Foglio5 = ListColFile[x];
                x++;
                myItem.Numero5 = ListColFile[x];
                x++;
                myItem.Denominatore5 = ListColFile[x];
                x++;
                myItem.Subalterno5 = ListColFile[x];
                x++;
                myItem.Edificialita5 = ListColFile[x];
                x++;
                myItem.SezioneUrbana6 = ListColFile[x];
                x++;
                myItem.Foglio6 = ListColFile[x];
                x++;
                myItem.Numero6 = ListColFile[x];
                x++;
                myItem.Denominatore6 = ListColFile[x];
                x++;
                myItem.Subalterno6 = ListColFile[x];
                x++;
                myItem.Edificialita6 = ListColFile[x];
                x++;
                myItem.SezioneUrbana7 = ListColFile[x];
                x++;
                myItem.Foglio7 = ListColFile[x];
                x++;
                myItem.Numero7 = ListColFile[x];
                x++;
                myItem.Denominatore7 = ListColFile[x];
                x++;
                myItem.Subalterno7 = ListColFile[x];
                x++;
                myItem.Edificialita7 = ListColFile[x];
                x++;
                myItem.SezioneUrbana8 = ListColFile[x];
                x++;
                myItem.Foglio8 = ListColFile[x];
                x++;
                myItem.Numero8 = ListColFile[x];
                x++;
                myItem.Denominatore8 = ListColFile[x];
                x++;
                myItem.Subalterno8 = ListColFile[x];
                x++;
                myItem.Edificialita8 = ListColFile[x];
                x++;
                myItem.SezioneUrbana9 = ListColFile[x];
                x++;
                myItem.Foglio9 = ListColFile[x];
                x++;
                myItem.Numero9 = ListColFile[x];
                x++;
                myItem.Denominatore9 = ListColFile[x];
                x++;
                myItem.Subalterno9 = ListColFile[x];
                x++;
                myItem.Edificialita9 = ListColFile[x];
                x++;
                myItem.SezioneUrbana10 = ListColFile[x];
                x++;
                myItem.Foglio10 = ListColFile[x];
                x++;
                myItem.Numero10 = ListColFile[x];
                x++;
                myItem.Denominatore10 = ListColFile[x];
                x++;
                myItem.Subalterno10 = ListColFile[x];
                x++;
                myItem.Edificialita10 = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadFAB_TR.TipoRecord->2.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
        private Fabbricato ReadFAB_TR3(string[] ListColFile, Fabbricato Fab)
        {
            Fabbricato myItem = Fab;
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.Progressivo = ListColFile[x];
                x++;
                x++;
                myItem.Toponimo1 = ListColFile[x];
                x++;
                myItem.Indirizzo1 = ListColFile[x];
                x++;
                myItem.Civico11 = ListColFile[x];
                x++;
                myItem.Civico21 = ListColFile[x];
                x++;
                myItem.Civico31 = ListColFile[x];
                x++;
                myItem.CodiceStrada1 = ListColFile[x];
                x++;
                myItem.Toponimo2 = ListColFile[x];
                x++;
                myItem.Indirizzo2 = ListColFile[x];
                x++;
                myItem.Civico12 = ListColFile[x];
                x++;
                myItem.Civico22 = ListColFile[x];
                x++;
                myItem.Civico32 = ListColFile[x];
                x++;
                myItem.CodiceStrada2 = ListColFile[x];
                x++;
                myItem.Toponimo3 = ListColFile[x];
                x++;
                myItem.Indirizzo3 = ListColFile[x];
                x++;
                myItem.Civico13 = ListColFile[x];
                x++;
                myItem.Civico23 = ListColFile[x];
                x++;
                myItem.Civico33 = ListColFile[x];
                x++;
                myItem.CodiceStrada3 = ListColFile[x];
                x++;
                myItem.Toponimo4 = ListColFile[x];
                x++;
                myItem.Indirizzo4 = ListColFile[x];
                x++;
                myItem.Civico14 = ListColFile[x];
                x++;
                myItem.Civico24 = ListColFile[x];
                x++;
                myItem.Civico34 = ListColFile[x];
                x++;
                myItem.CodiceStrada4 = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadFAB_TR.TipoRecord->3.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
        private Fabbricato ReadFAB_TR4(string[] ListColFile, Fabbricato Fab)
        {
            Fabbricato myItem = Fab;
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.Progressivo = ListColFile[x];
                x++;
                x++;
                myItem.UC_SezioneUrbana1 = ListColFile[x];
                x++;
                myItem.UC_Foglio1 = ListColFile[x];
                x++;
                myItem.UC_Numero1 = ListColFile[x];
                x++;
                myItem.UC_Denominatore1 = ListColFile[x];
                x++;
                myItem.UC_Subalterno1 = ListColFile[x];
                x++;
                myItem.UC_SezioneUrbana2 = ListColFile[x];
                x++;
                myItem.UC_Foglio2 = ListColFile[x];
                x++;
                myItem.UC_Numero2 = ListColFile[x];
                x++;
                myItem.UC_Denominatore2 = ListColFile[x];
                x++;
                myItem.UC_Subalterno2 = ListColFile[x];
                x++;
                myItem.UC_SezioneUrbana3 = ListColFile[x];
                x++;
                myItem.UC_Foglio3 = ListColFile[x];
                x++;
                myItem.UC_Numero3 = ListColFile[x];
                x++;
                myItem.UC_Denominatore3 = ListColFile[x];
                x++;
                myItem.UC_Subalterno3 = ListColFile[x];
                x++;
                myItem.UC_SezioneUrbana4 = ListColFile[x];
                x++;
                myItem.UC_Foglio4 = ListColFile[x];
                x++;
                myItem.UC_Numero4 = ListColFile[x];
                x++;
                myItem.UC_Denominatore4 = ListColFile[x];
                x++;
                myItem.UC_Subalterno4 = ListColFile[x];
                x++;
                myItem.UC_SezioneUrbana5 = ListColFile[x];
                x++;
                myItem.UC_Foglio5 = ListColFile[x];
                x++;
                myItem.UC_Numero5 = ListColFile[x];
                x++;
                myItem.UC_Denominatore5 = ListColFile[x];
                x++;
                myItem.UC_Subalterno5 = ListColFile[x];
                x++;
                myItem.UC_SezioneUrbana6 = ListColFile[x];
                x++;
                myItem.UC_Foglio6 = ListColFile[x];
                x++;
                myItem.UC_Numero6 = ListColFile[x];
                x++;
                myItem.UC_Denominatore6 = ListColFile[x];
                x++;
                myItem.UC_Subalterno6 = ListColFile[x];
                x++;
                myItem.UC_SezioneUrbana7 = ListColFile[x];
                x++;
                myItem.UC_Foglio7 = ListColFile[x];
                x++;
                myItem.UC_Numero7 = ListColFile[x];
                x++;
                myItem.UC_Denominatore7 = ListColFile[x];
                x++;
                myItem.UC_Subalterno7 = ListColFile[x];
                x++;
                myItem.UC_SezioneUrbana8 = ListColFile[x];
                x++;
                myItem.UC_Foglio8 = ListColFile[x];
                x++;
                myItem.UC_Numero8 = ListColFile[x];
                x++;
                myItem.UC_Denominatore8 = ListColFile[x];
                x++;
                myItem.UC_Subalterno8 = ListColFile[x];
                x++;
                myItem.UC_SezioneUrbana9 = ListColFile[x];
                x++;
                myItem.UC_Foglio9 = ListColFile[x];
                x++;
                myItem.UC_Numero9 = ListColFile[x];
                x++;
                myItem.UC_Denominatore9 = ListColFile[x];
                x++;
                myItem.UC_Subalterno9 = ListColFile[x];
                x++;
                myItem.UC_SezioneUrbana10 = ListColFile[x];
                x++;
                myItem.UC_Foglio10 = ListColFile[x];
                x++;
                myItem.UC_Numero10 = ListColFile[x];
                x++;
                myItem.UC_Denominatore10 = ListColFile[x];
                x++;
                myItem.UC_Subalterno10 = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadFAB_TR.TipoRecord->4.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
        private Fabbricato ReadFAB_TR5(string[] ListColFile, Fabbricato Fab)
        {
            Fabbricato myItem = Fab;
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.Progressivo = ListColFile[x];
                x++;
                x++;
                myItem.CodiceRiserva1 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva1 = ListColFile[x];
                x++;
                myItem.CodiceRiserva2 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva2 = ListColFile[x];
                x++;
                myItem.CodiceRiserva3 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva3 = ListColFile[x];
                x++;
                myItem.CodiceRiserva4 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva4 = ListColFile[x];
                x++;
                myItem.CodiceRiserva5 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva5 = ListColFile[x];
                x++;
                myItem.CodiceRiserva6 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva6 = ListColFile[x];
                x++;
                myItem.CodiceRiserva7 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva7 = ListColFile[x];
                x++;
                myItem.CodiceRiserva8 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva8 = ListColFile[x];
                x++;
                myItem.CodiceRiserva9 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva9 = ListColFile[x];
                x++;
                myItem.CodiceRiserva10 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva10 = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadFAB_TR.TipoRecord->5.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
    }
    public class ClsImportTER
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsImportTER));
        public List<Terreno> ReadTER(string myFile, int IDElaborazione)
        {
            try
            {
                string line = null;
                Terreno myTer = new Terreno();
                List<Terreno> ListTer = new List<Terreno>();
                byte[] filetoRead = System.IO.File.ReadAllBytes(myFile);
                MemoryStream ms = new MemoryStream(filetoRead, 0, filetoRead.Length);
                StreamReader sr = new StreamReader(ms);

                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() == string.Empty)
                            continue;
                        string[] ListColFile = line.Split(char.Parse("|"));
                        //se non cambia l'id immobile ciclo sui tipi record altrimenti inserisco nuova posizione
                        if (myTer.IDImmobile != ListColFile[2])
                        {
                            if (myTer.IDImmobile != string.Empty)
                            {
                                ListTer.Add(myTer);
                            }
                            myTer = new Terreno();
                            myTer.IDElaborazione = IDElaborazione;
                        }
                        switch (ListColFile[5])
                        {
                            case "1":
                                myTer = ReadTER_TR1(ListColFile, myTer);
                                break;
                            case "2":
                                myTer = ReadTER_TR2(ListColFile, myTer);
                                break;
                            case "3":
                                myTer = ReadTER_TR3(ListColFile, myTer);
                                break;
                            case "4":
                                myTer = ReadTER_TR4(ListColFile, myTer);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("ReadTER.errore::", ex);
                }
                finally
                {
                    sr.Close();
                }
                return ListTer;
            }
            catch (Exception err)
            {
                Log.Debug("ReadTER.errore::", err);
                return new List<Terreno>();
            }
        }
        private Terreno ReadTER_TR1(string[] ListColFile, Terreno Ter)
        {
            Terreno myItem = Ter;
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.Progressivo = ListColFile[x];
                x++;
                x++;
                myItem.Foglio = ListColFile[x];
                x++;
                myItem.Numero = ListColFile[x];
                x++;
                myItem.Denominatore = ListColFile[x];
                x++;
                myItem.Subalterno = ListColFile[x];
                x++;
                myItem.Edificialita = ListColFile[x];
                x++;
                myItem.Qualita = ListColFile[x];
                x++;
                myItem.Classe = ListColFile[x];
                x++;
                myItem.Ettari = ListColFile[x];
                x++;
                myItem.Are = ListColFile[x];
                x++;
                myItem.Centiare = ListColFile[x];
                x++;
                myItem.FlagReddito = ListColFile[x];
                x++;
                myItem.FlagPorzione = ListColFile[x];
                x++;
                myItem.FlagDeduzioni = ListColFile[x];
                x++;
                myItem.RedditoDominicaleLire = ListColFile[x];
                x++;
                myItem.RedditoAgrarioLire = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuro = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuro = ListColFile[x];
                x++;
                myItem.DataInizioEfficacia = ListColFile[x];
                x++;
                myItem.DataInizioRegistrazioneAtti = ListColFile[x];
                x++;
                myItem.TipoNotaInizio = ListColFile[x];
                x++;
                myItem.NumeroNotaInizio = ListColFile[x];
                x++;
                myItem.ProgressivoNotaInizio = ListColFile[x];
                x++;
                myItem.AnnoNotaInizio = ListColFile[x];
                x++;
                myItem.DataFineEfficacia = ListColFile[x];
                x++;
                myItem.DataFineRegistrazioneAtti = ListColFile[x];
                x++;
                myItem.TipoNotaFine = ListColFile[x];
                x++;
                myItem.NumeroNotaFine = ListColFile[x];
                x++;
                myItem.ProgressivoNotaFine = ListColFile[x];
                x++;
                myItem.AnnoNotaFine = ListColFile[x];
                x++;
                myItem.Partita = ListColFile[x];
                x++;
                myItem.Annotazione = ListColFile[x];
                x++;
                myItem.IDMutazioneIniziale = ListColFile[x];
                x++;
                myItem.IDMutazioneFinale = ListColFile[x];
                x++;
                myItem.CodiceCausaleAttoGenerante = ListColFile[x];
                x++;
                myItem.DescrizioneAttoGenerante = ListColFile[x];
                x++;
                myItem.CodiceCausaleAttoConclusivo = ListColFile[x];
                x++;
                myItem.DescrizioneAttoConclusivo = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadTER_TR.TipoRecord->1.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
        private Terreno ReadTER_TR2(string[] ListColFile, Terreno Ter)
        {
            Terreno myItem = Ter;
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.Progressivo = ListColFile[x];
                x++;
                x++;
                myItem.SimboloDeduzione1 = ListColFile[x];
                x++;
                myItem.SimboloDeduzione2 = ListColFile[x];
                x++;
                myItem.SimboloDeduzione3 = ListColFile[x];
                x++;
                myItem.SimboloDeduzione4 = ListColFile[x];
                x++;
                myItem.SimboloDeduzione5 = ListColFile[x];
                x++;
                myItem.SimboloDeduzione6 = ListColFile[x];
                x++;
                myItem.SimboloDeduzione7 = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadTER_TR.TipoRecord->2.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
        private Terreno ReadTER_TR3(string[] ListColFile, Terreno Ter)
        {
            Terreno myItem = Ter;
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.Progressivo = ListColFile[x];
                x++;
                x++;
                myItem.CodiceRiserva1 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva1 = ListColFile[x];
                x++;
                myItem.CodiceRiserva2 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva2 = ListColFile[x];
                x++;
                myItem.CodiceRiserva3 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva3 = ListColFile[x];
                x++;
                myItem.CodiceRiserva4 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva4 = ListColFile[x];
                x++;
                myItem.CodiceRiserva5 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva5 = ListColFile[x];
                x++;
                myItem.CodiceRiserva6 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva6 = ListColFile[x];
                x++;
                myItem.CodiceRiserva7 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva7 = ListColFile[x];
                x++;
                myItem.CodiceRiserva8 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva8 = ListColFile[x];
                x++;
                myItem.CodiceRiserva9 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva9 = ListColFile[x];
                x++;
                myItem.CodiceRiserva10 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva10 = ListColFile[x];
                x++;
                myItem.CodiceRiserva11 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva11 = ListColFile[x];
                x++;
                myItem.CodiceRiserva12 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva12 = ListColFile[x];
                x++;
                myItem.CodiceRiserva13 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva13 = ListColFile[x];
                x++;
                myItem.CodiceRiserva14 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva14 = ListColFile[x];
                x++;
                myItem.CodiceRiserva15 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva15 = ListColFile[x];
                x++;
                myItem.CodiceRiserva16 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva16 = ListColFile[x];
                x++;
                myItem.CodiceRiserva17 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva17 = ListColFile[x];
                x++;
                myItem.CodiceRiserva18 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva18 = ListColFile[x];
                x++;
                myItem.CodiceRiserva19 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva19 = ListColFile[x];
                x++;
                myItem.CodiceRiserva20 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva20 = ListColFile[x];
                x++;
                myItem.CodiceRiserva21 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva21 = ListColFile[x];
                x++;
                myItem.CodiceRiserva22 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva22 = ListColFile[x];
                x++;
                myItem.CodiceRiserva23 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva23 = ListColFile[x];
                x++;
                myItem.CodiceRiserva24 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva24 = ListColFile[x];
                x++;
                myItem.CodiceRiserva25 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva25 = ListColFile[x];
                x++;
                myItem.CodiceRiserva26 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva26 = ListColFile[x];
                x++;
                myItem.CodiceRiserva27 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva27 = ListColFile[x];
                x++;
                myItem.CodiceRiserva28 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva28 = ListColFile[x];
                x++;
                myItem.CodiceRiserva29 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva29 = ListColFile[x];
                x++;
                myItem.CodiceRiserva30 = ListColFile[x];
                x++;
                myItem.PartitaIscrizioneRiserva30 = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadTER_TR.TipoRecord->3.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
        private Terreno ReadTER_TR4(string[] ListColFile, Terreno Ter)
        {
            Terreno myItem = Ter;
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.Progressivo = ListColFile[x];
                x++;
                x++;
                myItem.IdentificativoPorzione1 = ListColFile[x];
                x++;
                myItem.QualitaPorzione1 = ListColFile[x];
                x++;
                myItem.ClassePorzione1 = ListColFile[x];
                x++;
                myItem.EttariPorzione1 = ListColFile[x];
                x++;
                myItem.ArePorzione1 = ListColFile[x];
                x++;
                myItem.CentiarePorzione1 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione1 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione1 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione2 = ListColFile[x];
                x++;
                myItem.QualitaPorzione2 = ListColFile[x];
                x++;
                myItem.ClassePorzione2 = ListColFile[x];
                x++;
                myItem.EttariPorzione2 = ListColFile[x];
                x++;
                myItem.ArePorzione2 = ListColFile[x];
                x++;
                myItem.CentiarePorzione2 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione2 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione2 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione3 = ListColFile[x];
                x++;
                myItem.QualitaPorzione3 = ListColFile[x];
                x++;
                myItem.ClassePorzione3 = ListColFile[x];
                x++;
                myItem.EttariPorzione3 = ListColFile[x];
                x++;
                myItem.ArePorzione3 = ListColFile[x];
                x++;
                myItem.CentiarePorzione3 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione3 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione3 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione4 = ListColFile[x];
                x++;
                myItem.QualitaPorzione4 = ListColFile[x];
                x++;
                myItem.ClassePorzione4 = ListColFile[x];
                x++;
                myItem.EttariPorzione4 = ListColFile[x];
                x++;
                myItem.ArePorzione4 = ListColFile[x];
                x++;
                myItem.CentiarePorzione4 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione4 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione4 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione5 = ListColFile[x];
                x++;
                myItem.QualitaPorzione5 = ListColFile[x];
                x++;
                myItem.ClassePorzione5 = ListColFile[x];
                x++;
                myItem.EttariPorzione5 = ListColFile[x];
                x++;
                myItem.ArePorzione5 = ListColFile[x];
                x++;
                myItem.CentiarePorzione5 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione5 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione5 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione6 = ListColFile[x];
                x++;
                myItem.QualitaPorzione6 = ListColFile[x];
                x++;
                myItem.ClassePorzione6 = ListColFile[x];
                x++;
                myItem.EttariPorzione6 = ListColFile[x];
                x++;
                myItem.ArePorzione6 = ListColFile[x];
                x++;
                myItem.CentiarePorzione6 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione6 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione6 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione7 = ListColFile[x];
                x++;
                myItem.QualitaPorzione7 = ListColFile[x];
                x++;
                myItem.ClassePorzione7 = ListColFile[x];
                x++;
                myItem.EttariPorzione7 = ListColFile[x];
                x++;
                myItem.ArePorzione7 = ListColFile[x];
                x++;
                myItem.CentiarePorzione7 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione7 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione7 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione8 = ListColFile[x];
                x++;
                myItem.QualitaPorzione8 = ListColFile[x];
                x++;
                myItem.ClassePorzione8 = ListColFile[x];
                x++;
                myItem.EttariPorzione8 = ListColFile[x];
                x++;
                myItem.ArePorzione8 = ListColFile[x];
                x++;
                myItem.CentiarePorzione8 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione8 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione8 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione9 = ListColFile[x];
                x++;
                myItem.QualitaPorzione9 = ListColFile[x];
                x++;
                myItem.ClassePorzione9 = ListColFile[x];
                x++;
                myItem.EttariPorzione9 = ListColFile[x];
                x++;
                myItem.ArePorzione9 = ListColFile[x];
                x++;
                myItem.CentiarePorzione9 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione9 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione9 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione10 = ListColFile[x];
                x++;
                myItem.QualitaPorzione10 = ListColFile[x];
                x++;
                myItem.ClassePorzione10 = ListColFile[x];
                x++;
                myItem.EttariPorzione10 = ListColFile[x];
                x++;
                myItem.ArePorzione10 = ListColFile[x];
                x++;
                myItem.CentiarePorzione10 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione10 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione10 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione11 = ListColFile[x];
                x++;
                myItem.QualitaPorzione11 = ListColFile[x];
                x++;
                myItem.ClassePorzione11 = ListColFile[x];
                x++;
                myItem.EttariPorzione11 = ListColFile[x];
                x++;
                myItem.ArePorzione11 = ListColFile[x];
                x++;
                myItem.CentiarePorzione11 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione11 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione11 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione12 = ListColFile[x];
                x++;
                myItem.QualitaPorzione12 = ListColFile[x];
                x++;
                myItem.ClassePorzione12 = ListColFile[x];
                x++;
                myItem.EttariPorzione12 = ListColFile[x];
                x++;
                myItem.ArePorzione12 = ListColFile[x];
                x++;
                myItem.CentiarePorzione12 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione12 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione12 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione13 = ListColFile[x];
                x++;
                myItem.QualitaPorzione13 = ListColFile[x];
                x++;
                myItem.ClassePorzione13 = ListColFile[x];
                x++;
                myItem.EttariPorzione13 = ListColFile[x];
                x++;
                myItem.ArePorzione13 = ListColFile[x];
                x++;
                myItem.CentiarePorzione13 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione13 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione13 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione14 = ListColFile[x];
                x++;
                myItem.QualitaPorzione14 = ListColFile[x];
                x++;
                myItem.ClassePorzione14 = ListColFile[x];
                x++;
                myItem.EttariPorzione14 = ListColFile[x];
                x++;
                myItem.ArePorzione14 = ListColFile[x];
                x++;
                myItem.CentiarePorzione14 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione14 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione14 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione15 = ListColFile[x];
                x++;
                myItem.QualitaPorzione15 = ListColFile[x];
                x++;
                myItem.ClassePorzione15 = ListColFile[x];
                x++;
                myItem.EttariPorzione15 = ListColFile[x];
                x++;
                myItem.ArePorzione15 = ListColFile[x];
                x++;
                myItem.CentiarePorzione15 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione15 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione15 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione16 = ListColFile[x];
                x++;
                myItem.QualitaPorzione16 = ListColFile[x];
                x++;
                myItem.ClassePorzione16 = ListColFile[x];
                x++;
                myItem.EttariPorzione16 = ListColFile[x];
                x++;
                myItem.ArePorzione16 = ListColFile[x];
                x++;
                myItem.CentiarePorzione16 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione16 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione16 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione17 = ListColFile[x];
                x++;
                myItem.QualitaPorzione17 = ListColFile[x];
                x++;
                myItem.ClassePorzione17 = ListColFile[x];
                x++;
                myItem.EttariPorzione17 = ListColFile[x];
                x++;
                myItem.ArePorzione17 = ListColFile[x];
                x++;
                myItem.CentiarePorzione17 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione17 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione17 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione18 = ListColFile[x];
                x++;
                myItem.QualitaPorzione18 = ListColFile[x];
                x++;
                myItem.ClassePorzione18 = ListColFile[x];
                x++;
                myItem.EttariPorzione18 = ListColFile[x];
                x++;
                myItem.ArePorzione18 = ListColFile[x];
                x++;
                myItem.CentiarePorzione18 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione18 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione18 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione19 = ListColFile[x];
                x++;
                myItem.QualitaPorzione19 = ListColFile[x];
                x++;
                myItem.ClassePorzione19 = ListColFile[x];
                x++;
                myItem.EttariPorzione19 = ListColFile[x];
                x++;
                myItem.ArePorzione19 = ListColFile[x];
                x++;
                myItem.CentiarePorzione19 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione19 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione19 = ListColFile[x];
                x++;
                myItem.IdentificativoPorzione20 = ListColFile[x];
                x++;
                myItem.QualitaPorzione20 = ListColFile[x];
                x++;
                myItem.ClassePorzione20 = ListColFile[x];
                x++;
                myItem.EttariPorzione20 = ListColFile[x];
                x++;
                myItem.ArePorzione20 = ListColFile[x];
                x++;
                myItem.CentiarePorzione20 = ListColFile[x];
                x++;
                myItem.RedditoDominicaleEuroPorzione20 = ListColFile[x];
                x++;
                myItem.RedditoAgrarioEuroPorzione20 = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadTER_TR.TipoRecord->4.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
    }
    public class ClsImportTIT
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsImportTIT));
        public List<Titoli> ReadTIT(string myFile, int IDElaborazione)
        {
            try
            {
                string line = null;
                Titoli myTit = new Titoli();
                List<Titoli> ListTit = new List<Titoli>();
                byte[] filetoRead = System.IO.File.ReadAllBytes(myFile);
                MemoryStream ms = new MemoryStream(filetoRead, 0, filetoRead.Length);
                StreamReader sr = new StreamReader(ms);

                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() == string.Empty)
                            continue;
                        string[] ListColFile = line.Split(char.Parse("|"));
                        myTit = ReadLineTIT(ListColFile);
                        myTit.IDElaborazione = IDElaborazione;
                        ListTit.Add(myTit);
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("ReadTIT.errore::", ex);
                }
                finally
                {
                    sr.Close();
                }
                return ListTit;
            }
            catch (Exception err)
            {
                Log.Debug("ReadTIT.errore::", err);
                return new List<Titoli>();
            }
        }
        private Titoli ReadLineTIT(string[] ListColFile)
        {
            Titoli myItem = new Titoli();
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDSoggetto = ListColFile[x];
                x++;
                myItem.TipoSoggetto = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.TipoImmobile = ListColFile[x];
                x++;
                myItem.CodiceDiritto = ListColFile[x];
                x++;
                myItem.TitoloNonCodificato = ListColFile[x];
                x++;
                myItem.QuotaNumeratore = ListColFile[x];
                x++;
                myItem.QuotaDenominatore = ListColFile[x];
                x++;
                myItem.Regime = ListColFile[x];
                x++;
                myItem.SoggettoDiRiferimento = ListColFile[x];
                x++;
                myItem.DataInizioEfficacia = ListColFile[x];
                x++;
                myItem.TipoNotaInizio = ListColFile[x];
                x++;
                myItem.NumeroNotaInizio = ListColFile[x];
                x++;
                myItem.ProgressivoNotaInizio = ListColFile[x];
                x++;
                myItem.AnnoNotaInizio = ListColFile[x];
                x++;
                myItem.DataInizioRegistrazioneAtti = ListColFile[x];
                x++;
                myItem.Partita = ListColFile[x];
                x++;
                myItem.DataFineEfficacia = ListColFile[x];
                x++;
                myItem.TipoNotaFine = ListColFile[x];
                x++;
                myItem.NumeroNotaFine = ListColFile[x];
                x++;
                myItem.ProgressivoNotaFine = ListColFile[x];
                x++;
                myItem.AnnoNotaFine = ListColFile[x];
                x++;
                myItem.DataFineRegistrazioneAtti = ListColFile[x];
                x++;
                myItem.IDMutazioneIniziale = ListColFile[x];
                x++;
                myItem.IDMutazioneFinale = ListColFile[x];
                x++;
                myItem.IDTitolarita = ListColFile[x];
                x++;
                myItem.CodiceCausaleAttoGenerante = ListColFile[x];
                x++;
                myItem.DescrizioneAttoGenerante = ListColFile[x];
                x++;
                myItem.CodicecausaleAttoConclusivo = ListColFile[x];
                x++;
                myItem.DescrizioneAttoConclusivo = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadLineTIT.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
    }
    public class ClsImportSOG
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsImportSOG));
        public List<Soggetto> ReadSOG(string myFile, int IDElaborazione)
        {
            try
            {
                string line = null;
                Soggetto mySog = new Soggetto();
                List<Soggetto> ListSog = new List<Soggetto>();
                byte[] filetoRead = System.IO.File.ReadAllBytes(myFile);
                MemoryStream ms = new MemoryStream(filetoRead, 0, filetoRead.Length);
                StreamReader sr = new StreamReader(ms);

                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() == string.Empty)
                            continue;
                        string[] ListColFile = line.Split(char.Parse("|"));
                        mySog = ReadLineSOG(ListColFile);
                        mySog.IDElaborazione = IDElaborazione;
                        ListSog.Add(mySog);
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("ReadSOG.errore::", ex);
                }
                finally
                {
                    sr.Close();
                }
                return ListSog;
            }
            catch (Exception err)
            {
                Log.Debug("ReadSOG.errore::", err);
                return new List<Soggetto>();
            }
        }
        private Soggetto ReadLineSOG(string[] ListColFile)
        {
            Soggetto myItem = new Soggetto();
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Sezione = ListColFile[x];
                x++;
                myItem.IDSoggetto = ListColFile[x];
                x++;
                myItem.TipoSoggetto = ListColFile[x];
                switch (myItem.TipoSoggetto)
                {
                    case "P":
                        x++;
                        myItem.Cognome = ListColFile[x];
                        x++;
                        myItem.Nome = ListColFile[x];
                        x++;
                        myItem.Sesso = ListColFile[x];
                        x++;
                        myItem.DataNascita = ListColFile[x];
                        x++;
                        myItem.LuogoNascita = ListColFile[x];
                        x++;
                        myItem.CodFiscalePIVA = ListColFile[x];
                        break;
                    case "G":
                        x++;
                        myItem.Denominazione = ListColFile[x];
                        x++;
                        myItem.Sede = ListColFile[x];
                        x++;
                        myItem.CodFiscalePIVA = ListColFile[x];
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("ReadLineSOG.TipoRecord->" + myItem.TipoSoggetto + ".Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
    }
    public class ClsImportDIC
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsImportDIC));
        #region Banca Dati Tributaria per Catasto
        public List<Dichiarazione> ReadDIC(string myFile, int IDElaborazione)
        {
            try
            {
                string line = null;
                Dichiarazione myDich = new Dichiarazione();
                List<Dichiarazione> ListDich = new List<Dichiarazione>();
                byte[] filetoRead = System.IO.File.ReadAllBytes(myFile);
                MemoryStream ms = new MemoryStream(filetoRead, 0, filetoRead.Length);
                StreamReader sr = new StreamReader(ms);

                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() == string.Empty)
                            continue;
                        string[] ListColFile = line.Split(char.Parse(";"));
                        if (ListColFile[0].Length == 4)
                        {//importo solo se il codice ente è valido
                            myDich = ReadLineDIC(ListColFile);
                            myDich.IDElaborazione = IDElaborazione;
                            ListDich.Add(myDich);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("ReadDIC.errore::", ex);
                }
                finally
                {
                    sr.Close();
                }
                return ListDich;
            }
            catch (Exception err)
            {
                Log.Debug("ReadDIC.errore::", err);
                return new List<Dichiarazione>();
            }
        }
        private Dichiarazione ReadLineDIC(string[] ListColFile)
        {
            Dichiarazione myItem = new Dichiarazione();
            int x = 0;

            try
            {
                x = 0;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.Cognome = ListColFile[x];
                x++;
                myItem.Nome = ListColFile[x];
                x++;
                myItem.CodFiscalePIVA = ListColFile[x];
                x++;
                myItem.NumeroDichiarazione = ListColFile[x];
                x++;
                myItem.DataDichiarazione = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.IDStrada = ListColFile[x];
                x++;
                myItem.Indirizzo = ListColFile[x];
                x++;
                myItem.Civico = ListColFile[x];
                x++;
                myItem.Scala = ListColFile[x];
                x++;
                myItem.Piano = ListColFile[x];
                x++;
                myItem.Interno = ListColFile[x];
                x++;
                myItem.Foglio = ListColFile[x];
                x++;
                myItem.Numero = ListColFile[x];
                x++;
                myItem.Subalterno = ListColFile[x];
                x++;
                myItem.DataInizio = ListColFile[x];
                x++;
                myItem.DataFine = ListColFile[x];
                x++;
                myItem.MesiPossesso = ListColFile[x];
                x++;
                myItem.TipoPossesso = ListColFile[x];
                x++;
                myItem.TipoUtilizzo = ListColFile[x];
                x++;
                myItem.QuotaPossesso = ListColFile[x];
                x++;
                myItem.FlagPrincipale = ListColFile[x];
                x++;
                myItem.FlagPertinenza = ListColFile[x];
                x++;
                myItem.TipoRendita = ListColFile[x];
                x++;
                myItem.Zona = ListColFile[x];
                x++;
                myItem.Categoria = ListColFile[x];
                x++;
                myItem.Classe = ListColFile[x];
                x++;
                myItem.Valore = ListColFile[x];
                x++;
                myItem.Rendita = ListColFile[x];
                x++;
                myItem.Consistenza = ListColFile[x];
                x++;
                myItem.FlagEsente = ListColFile[x];
                x++;
                myItem.FlagRiduzione = ListColFile[x];
                x++;
                myItem.NUtilizzatori = ListColFile[x];
                x++;
                myItem.FlagColDir = ListColFile[x];
                x++;
                myItem.NFigliMinori26Anni = ListColFile[x];
                x++;
                myItem.Note = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadLineDIC.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
        #endregion
        #region Dichiarazioni per Banca Dati Tributaria
        public List<Dichiarazione> ReadDICCAT(string myFile, int IDElaborazione, ref int IDElabCat)
        {
            try
            {
                string line = null;
                Dichiarazione myDich = new Dichiarazione();
                List<Dichiarazione> ListDich = new List<Dichiarazione>();
                byte[] filetoRead = System.IO.File.ReadAllBytes(myFile);
                MemoryStream ms = new MemoryStream(filetoRead, 0, filetoRead.Length);
                StreamReader sr = new StreamReader(ms);

                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() == string.Empty)
                            continue;
                        string[] ListColFile = line.Split(char.Parse(";"));
                        if (ListColFile[0].Length == 6)
                        {//importo solo se il codice ente è valido
                            myDich = ReadLineDICCAT(ListColFile);
                            IDElabCat = myDich.IDElaborazione;
                            myDich.IDElaborazione = IDElaborazione;
                            ListDich.Add(myDich);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("ReadDICCAT.errore::", ex);
                }
                finally
                {
                    sr.Close();
                }
                return ListDich;
            }
            catch (Exception err)
            {
                Log.Debug("ReadDICCAT.errore::", err);
                return new List<Dichiarazione>();
            }
        }
        private Dichiarazione ReadLineDICCAT(string[] ListColFile)
        {
            Dichiarazione myItem = new Dichiarazione();
            int x = 0;

            try
            {
                x = 0;
                x++;
                myItem.ID = int.Parse(ListColFile[x]);
                x++;
                myItem.IDElaborazione = int.Parse(ListColFile[x]);
                x++;
                myItem.IDCatastale = ListColFile[x];
                x++;
                myItem.IDSoggetto = ListColFile[x];
                x++;
                myItem.Cognome = ListColFile[x];
                x++;
                myItem.Nome = ListColFile[x];
                x++;
                myItem.CodFiscalePIVA = ListColFile[x].Replace("'","");
                x++;
                myItem.IDImmobileCat = ListColFile[x];
                x++;
                myItem.IDImmobile = ListColFile[x];
                x++;
                myItem.IDStrada = ListColFile[x];
                x++;
                myItem.Indirizzo = ListColFile[x];
                x++;
                myItem.Civico = ListColFile[x];
                x++;
                myItem.Scala = ListColFile[x];
                x++;
                myItem.Piano = ListColFile[x];
                x++;
                myItem.Interno = ListColFile[x];
                x++;
                myItem.Foglio = ListColFile[x];
                x++;
                myItem.Numero = ListColFile[x];
                x++;
                myItem.Subalterno = ListColFile[x];
                x++;
                myItem.DataInizio = ListColFile[x];
                x++;
                myItem.DataFine = ListColFile[x];
                x++;
                myItem.TipoPossesso = ListColFile[x];
                x++;
                myItem.RegimePossesso = ListColFile[x];
                x++;
                myItem.QuotaPossesso = ListColFile[x];
                x++;
                myItem.Zona = ListColFile[x];
                x++;
                myItem.Categoria = ListColFile[x];
                x++;
                myItem.Classe = ListColFile[x];
                x++;
                myItem.Consistenza = ListColFile[x];
                x++;
                myItem.Rendita = ListColFile[x];
                x++;
                myItem.Note = ListColFile[x];
                x++;
                myItem.NumeroDichiarazione = ListColFile[x];
                x++;
                myItem.DataDichiarazione = ListColFile[x];
                x++;
                myItem.MesiPossesso = ListColFile[x];
                x++;
                myItem.TipoUtilizzo = ListColFile[x];
                x++;
                myItem.FlagPrincipale = ListColFile[x];
                x++;
                myItem.FlagPertinenza = ListColFile[x];
                x++;
                myItem.TipoRendita = ListColFile[x];
                x++;
                myItem.Valore = ListColFile[x];
                x++;
                myItem.FlagEsente = ListColFile[x];
                x++;
                myItem.FlagRiduzione = ListColFile[x];
                x++;
                myItem.NUtilizzatori = ListColFile[x];
                x++;
                myItem.FlagColDir = ListColFile[x];
                x++;
                myItem.NFigliMinori26Anni = ListColFile[x];
                x++;
                myItem.Azione = ListColFile[x];
            }
            catch (Exception ex)
            {
                Log.Debug("ReadLineDICCAT.Ultimo indice trovato=" + x.ToString() + ".errore::", ex);
            }
            return myItem;
        }
        #endregion
    }
    /// <summary>
    /// Si compone di 3 fasi:
    /// 1.	Pulizia del file.FAB
    /// 2.	Selezione di tutti gli immobili di.FAB Storico presenti in .TIT e non in .FAB
    /// 3.	Unione di .TIT con .FAB
    /// Queste fasi sono indispensabili per poter ragionare sulle date corrette degli immobili.
    /// Queste fasi sono implementate in STORED PROCEDURE richiamate in sequenza, in questo modo sono facilmente modificabili senza dover dipendere da un rilascio dell’applicativo.
    /// </summary>
    public class ClsCatasto
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsCatasto));
        public bool CreateCatastoWork(ref Elaborazione myElab)
        {
            Log.Debug("CreateCatastoWork.start");
            try
            {
                myElab.InizioConvert = DateTime.Now;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                //vado a ripulire la situazione di ogni immobile per considerare solo le registrazioni che hanno effettivamente generato modifiche
                if (!new ClsManageDB().PuliziaFAB(myElab.IDCatastale))
                {
                    myElab.FineConvert = DateTime.Now;
                    myElab.EsitoConvert = Elaborazione.Esito.KO;
                    myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                    new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                    return false;
                }
                else
                {
                    if (!new ClsManageDB().GetFABMancanti(myElab.IDCatastale))
                    {
                        myElab.FineConvert = DateTime.Now;
                        myElab.EsitoConvert = Elaborazione.Esito.KO;
                        myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                        new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                        return false;
                    }
                    else
                    {
                        if (!new ClsManageDB().JoinTIT(myElab.IDCatastale))
                        {
                            myElab.FineConvert = DateTime.Now;
                            myElab.EsitoConvert = Elaborazione.Esito.KO;
                            myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                            new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                            return false;
                        }
                    }
                }
                myElab.FineConvert = DateTime.Now;
                myElab.EsitoConvert = Elaborazione.Esito.OK;
                myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("CreateCatastoWork.errore::", ex);
                myElab.FineConvert = DateTime.Now;
                myElab.EsitoConvert = Elaborazione.Esito.KO;
                myElab.EsitoIncrocio = myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                return false;
            }
        }
    }
    /// <summary>
    /// Partendo dai riferimenti presenti in CATASTOWORK si creano i movimenti di dichiarazioni.
    /// Nella tabella DICHWORK saranno anche inseriti anche tutti i riferimenti di DICHIARAZIONI non presenti in CATASTOWORK con la nota “non in aggiornamento”.
    /// Nella tabella DICHWORK saranno anche inseriti anche tutti i riferimenti di DICHIARAZIONI non presenti in .FAB e neanche in .STO con la nota “non a catasto”.
    /// Nella tabella DICHWORK sarà presente un campo che evidenzia l’attività da fare(chiudere UI, inserire UI, aprire UI a nuova data, aggiornare UI, aggiungere nota UI).
    /// Anche questa fase è implementata in STORED PROCEDURE, in questo modo è facilmente modificabile senza dover dipendere da un rilascio dell’applicativo.
    /// </summary>
    public class ClsDichiarazioni
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsDichiarazioni));

        public bool CreateDichWork(Elaborazione myElab)
        {
            Log.Debug("CreateDichWork.start");
            try
            {
                myElab.InizioIncrocio = DateTime.Now;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                //vado ad incrociare CATASTOWORK con DICHIARAZIONI per creare DICHWORK
                if (!new ClsManageDB().JoinCatDic(myElab.IDCatastale))
                {
                    myElab.FineIncrocio = DateTime.Now;
                    myElab.EsitoIncrocio = Elaborazione.Esito.KO;
                    myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                    new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                    return false;
                }
                myElab.FineIncrocio = DateTime.Now;
                myElab.EsitoIncrocio = Elaborazione.Esito.OK;
                myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("CreateDichWork.errore::", ex);
                myElab.FineIncrocio = DateTime.Now;
                myElab.EsitoIncrocio = Elaborazione.Esito.KO;
                myElab.EsitoEstrazioneDichWork = myElab.EsitoEstrazioneTitVSSog = myElab.EsitoEstrazioneSogVSTit = myElab.EsitoEstrazioneTitVSFab = myElab.EsitoEstrazioneFabVSTit = myElab.EsitoEstrazioneTitVSTer = myElab.EsitoEstrazioneTerVSTit = myElab.EsitoEstrazioneDirittoMancante = myElab.EsitoEstrazionePossMancante = myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.KO;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnection).SetElaborazione(myElab);
                return false;
            }
        }
    }
    public class ClsRibalta
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsRibalta));

        public bool StartImport(string IdEnte, Elaborazione myElab)
        {
            try
            {

                //inizio elaborazione
                myElab.InizioImport = DateTime.Now;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnectionICI).SetElaborazione(myElab);
                if (myElab.ID > 0)
                {//inizio importazione
                    return ImportFlussi(myElab);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("ClsRibalta.StartImport.errore::", ex);
                return false;
            }
        }
        private bool ImportFlussi(Elaborazione myElab)
        {
            try
            {
                ElaborazioneFile mySingleFile = new ElaborazioneFile();
                string[] ListFiles = Directory.GetFiles(RouteConfig.PathRibaltaDaAcquisire);
                foreach (string myItem in ListFiles)
                {
                    string myNameFile = myItem.Replace(RouteConfig.PathRibaltaDaAcquisire, string.Empty);
                    string[] myFile = myNameFile.Split(char.Parse("."));
                    myFile[1] = myFile[1].ToUpper();
                    if (myFile[0].Length > 26)
                    {
                        if (myFile[0].Substring(19, 6) == myElab.IDEnte && myFile[1] == ElaborazioneFile.Estensioni.Dichiarazioni)
                        {
                            //sposto file
                            if (File.Exists(RouteConfig.PathRibaltaInLavorazione + myNameFile))
                            {
                                File.Delete(RouteConfig.PathRibaltaInLavorazione + myNameFile);
                            }
                            File.Move(RouteConfig.PathRibaltaDaAcquisire + myNameFile, RouteConfig.PathRibaltaInLavorazione + myNameFile);
                            //importo
                            mySingleFile = new ElaborazioneFile();
                            mySingleFile.IDElaborazione = myElab.ID;
                            mySingleFile.NameFile = myNameFile;
                            mySingleFile.InizioImport = DateTime.Now;
                            int IDElabCat = 0;
                            List<Dichiarazione> ListDic = new ClsImportDIC().ReadDICCAT(RouteConfig.PathRibaltaInLavorazione + myNameFile, myElab.ID, ref IDElabCat);
                            mySingleFile.FineImport = DateTime.Now;
                            if (ListDic.Count > 0)
                            {
                                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnectionICI).SetElaborazioneCat(myElab.ID, IDElabCat);
                                //Storicizzo tutte le dichiarazioni che saranno toccate
                                if (!new ClsManageDB().StoricizzaDIC(ListDic, DBModel.Ambiente_Verticale))
                                {
                                    if (File.Exists(RouteConfig.PathRibaltaScartati + myNameFile))
                                    {
                                        File.Delete(RouteConfig.PathRibaltaScartati + myNameFile);
                                    }
                                    File.Move(RouteConfig.PathRibaltaInLavorazione + myNameFile, RouteConfig.PathRibaltaScartati + myNameFile);
                                    mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                    myElab.ListFiles.Add(mySingleFile);
                                    myElab.FineImport = DateTime.Now;
                                    myElab.EsitoImport = "Errore in ImportFlussi.Errore Storicizzazione dichirazioni";
                                    new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnectionICI).SetElaborazione(myElab);
                                    return false;
                                }
                                else
                                {
                                //Inserisco i movimenti da catasto
                                if (!new ClsManageDB().SaveDIC(ListDic, DBModel.Ambiente_Verticale))
                                {
                                    if (File.Exists(RouteConfig.PathRibaltaScartati + myNameFile))
                                    {
                                        File.Delete(RouteConfig.PathRibaltaScartati + myNameFile);
                                    }
                                    File.Move(RouteConfig.PathRibaltaInLavorazione + myNameFile, RouteConfig.PathRibaltaScartati + myNameFile);
                                    mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                    myElab.ListFiles.Add(mySingleFile);
                                    myElab.FineImport = DateTime.Now;
                                    myElab.EsitoImport = "Errore in ImportFlussi.Errore Inserimento Flusso CSV";
                                    new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnectionICI).SetElaborazione(myElab);
                                    return false;
                                }
                                else
                                {
                                    if (File.Exists(RouteConfig.PathRibaltaAcquisiti + myNameFile))
                                    {
                                        File.Delete(RouteConfig.PathRibaltaAcquisiti + myNameFile);
                                    }
                                    File.Move(RouteConfig.PathRibaltaInLavorazione + myNameFile, RouteConfig.PathRibaltaAcquisiti + myNameFile);
                                    mySingleFile.EsitoImport = Elaborazione.Esito.OK;
                                    myElab.ListFiles.Add(mySingleFile);
                                }
                                }
                            }
                            else
                            {
                                if (File.Exists(RouteConfig.PathRibaltaScartati + myNameFile))
                                {
                                    File.Delete(RouteConfig.PathRibaltaScartati + myNameFile);
                                }
                                File.Move(RouteConfig.PathRibaltaInLavorazione + myNameFile, RouteConfig.PathRibaltaScartati + myNameFile);
                                mySingleFile.EsitoImport = Elaborazione.Esito.KO;
                                myElab.ListFiles.Add(mySingleFile);
                                myElab.FineImport = DateTime.Now;
                                myElab.EsitoImport = "Errore in ImportFlussi.Errore Lettura Flusso CSV";
                                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnectionICI).SetElaborazione(myElab);
                                return false;
                            }
                        }
                    }
                }
                myElab.FineImport = DateTime.Now;
                myElab.EsitoImport = Elaborazione.Esito.OK;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnectionICI).SetElaborazione(myElab);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("ClsRibalta.ImportFlussi.errore::", ex);
                myElab.FineImport = DateTime.Now;
                myElab.EsitoImport = "Errore in ImportFlussi:" + ex.Message;
                new Utility.DichManagerCatasto(RouteConfig.TypeDB, RouteConfig.StringConnectionICI).SetElaborazione(myElab);
                return false;
            }
        }
    }
}