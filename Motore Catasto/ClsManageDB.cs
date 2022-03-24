using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using log4net;
using CatastoInterface;

namespace Motore_Catasto
{
    /// <summary>
    /// Classe di modellazione database
    /// </summary>
    public class DBModel : IDisposable
    {
        void IDisposable.Dispose() { }
        public void Dispose() { }
        public DbContext ContextDB { get; private set; }
            public const string Ambiente_Catasto = "Catasto";
            public const string Ambiente_Verticale = "Verticale";

        public DBModel(string Ambiente)
        {
            if (RouteConfig.TypeDB == "MySQL")
                if (Ambiente==Ambiente_Verticale)
                    ContextDB = new ctxICIMySQL();
            else
            ContextDB = new ctxCatastoMySQL();
            else
                if (Ambiente == Ambiente_Verticale)
                ContextDB = new ctxICISQL();
            else
                ContextDB = new ctxCatastoSQL();
        }
        private static string ToExecSP
        {
            get
            {
                if (RouteConfig.TypeDB == "MySQL")
                    return "CALL ";
                else
                    return "EXEC ";
            }
        }
        private static string PrefVarSP
        {
            get
            {
                if (RouteConfig.TypeDB == "MySQL")
                    return "@var";
                else
                    return "@";
            }
        }
        #region "Method"
        public string GetSQL(string sSQL, params string[] myParam)
        {
            string sRet = "";
            foreach (string myItem in myParam)
            {
                if (sRet != string.Empty)
                    sRet += ",";
                sRet += PrefVarSP + myItem;
            }

            if (RouteConfig.TypeDB == "MySQL")
                sRet = ToExecSP + sSQL + " (" + sRet + ")";
            else
                sRet = ToExecSP + sSQL + " " + sRet;
            return sRet;
        }
        public object GetParam(string Name, object Value)
        {
            if (RouteConfig.TypeDB == "MySQL")
            {
                DateTime d;
                if (DateTime.TryParse(Value.ToString(), out d))
                {
                    if (d.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                        Value = DateTime.MinValue;
                }
                return new MySqlParameter(PrefVarSP + Name, Value);
            }
            else
                return new SqlParameter(PrefVarSP + Name, Value);

        }
        #endregion
    }
    /// <summary>
    /// Classe che inizializza una connessione ad un db per il catasto in formato SQL
    /// </summary>
    public class ctxCatastoSQL : DbContext
    {
        public ctxCatastoSQL()
            : base("name=CatastoContext")
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 1200;
        }
    }
    /// <summary>
    /// Classe che inizializza una connessione ad un db per il catasto in formato MySQL
    /// </summary>
    [System.Data.Entity.DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ctxCatastoMySQL : DbContext
    {
        public ctxCatastoMySQL()
            : base("name=CatastoContext")
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 1200;
        }
    }
    /// <summary>
    /// Classe che inizializza una connessione ad un db per IMU in formato SQL
    /// </summary>
    public class ctxICISQL : DbContext
    {
        public ctxICISQL()
            : base("name=ICIContext")
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 1200;
        }
    }
    /// <summary>
    /// Classe che inizializza una connessione ad un db IMU in formato MySQL
    /// </summary>
    [System.Data.Entity.DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ctxICIMySQL : DbContext
    {
        public ctxICIMySQL()
            : base("name=ICIContext")
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 1200;
        }
    }
    /// <summary>
    /// Classe per l'interfacciamento con il database
    /// </summary>
    class ClsManageDB
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClsManageDB));

        public bool SaveFAB(List<Fabbricato> ListItem, string PrefissoTab)
        {
            try
            {
                using (DBModel ctx = new DBModel(DBModel.Ambiente_Catasto))
                {
                    foreach (Fabbricato myItem in ListItem)
                    {
                        string sSQL = ctx.GetSQL("prc_CATASTO_FAB"+PrefissoTab+"_IU", "ID"
                                , "IDELABORAZIONE"
                                , "IDCATASTALE"
                                , "SEZIONE"
                                , "IDIMMOBILE"
                                , "TIPOIMMOBILE"
                                , "PROGRESSIVO"
                                , "ZONA"
                                , "CATEGORIA"
                                , "CLASSE"
                                , "CONSISTENZA"
                                , "SUPERFICIE"
                                , "RENDITALIRE"
                                , "RENDITAEURO"
                                , "LOTTO"
                                , "EDIFICIO"
                                , "SCALA"
                                , "INTERNO1"
                                , "INTERNO2"
                                , "PIANO1"
                                , "PIANO2"
                                , "PIANO3"
                                , "PIANO4"
                                , "DATAINIZIOEFFICACIA"
                                , "DATAINIZIOREGISTRAZIONEINATTI"
                                , "TIPONOTAINIZIO"
                                , "NUMERONOTAINIZIO"
                                , "PROGRESSIVONOTAINIZIO"
                                , "ANNONOTAINIZIO"
                                , "DATAFINEEFFICACIA"
                                , "DATAFINEREGISTRAZIONEATTI"
                                , "TIPONOTAFINE"
                                , "NUMERONOTAFINE"
                                , "PROGRESSIVONOTAFINE"
                                , "ANNONOTAFINE"
                                , "PARTITA"
                                , "ANNOTAZIONE"
                                , "IDENTIFICATIVOMUTAZIONEINIZIALE"
                                , "IDENTIFICATIVOMUTAZIONEFINALE"
                                , "PROTOCOLLONOTIFICA"
                                , "DATANOTIFICA"
                                , "CODICECAUSALEATTOGENERANTE"
                                , "DESCRIZIONEATTOGENERANTE"
                                , "CODICECAUSALEATTOCONCLUSIVO"
                                , "DESCRIZIONEATTOCONCLUSIVO"
                                , "FLAGCLASSAMENTO"
                                , "SEZIONEURBANA1"
                                , "FOGLIO1"
                                , "NUMERO1"
                                , "DENOMINATORE1"
                                , "SUBALTERNO1"
                                , "EDIFICIALITA1"
                                , "SEZIONEURBANA2"
                                , "FOGLIO2"
                                , "NUMERO2"
                                , "DENOMINATORE2"
                                , "SUBALTERNO2"
                                , "EDIFICIALITA2"
                                , "SEZIONEURBANA3"
                                , "FOGLIO3"
                                , "NUMERO3"
                                , "DENOMINATORE3"
                                , "SUBALTERNO3"
                                , "EDIFICIALITA3"
                                , "SEZIONEURBANA4"
                                , "FOGLIO4"
                                , "NUMERO4"
                                , "DENOMINATORE4"
                                , "SUBALTERNO4"
                                , "EDIFICIALITA4"
                                , "SEZIONEURBANA5"
                                , "FOGLIO5"
                                , "NUMERO5"
                                , "DENOMINATORE5"
                                , "SUBALTERNO5"
                                , "EDIFICIALITA5"
                                , "SEZIONEURBANA6"
                                , "FOGLIO6"
                                , "NUMERO6"
                                , "DENOMINATORE6"
                                , "SUBALTERNO6"
                                , "EDIFICIALITA6"
                                , "SEZIONEURBANA7"
                                , "FOGLIO7"
                                , "NUMERO7"
                                , "DENOMINATORE7"
                                , "SUBALTERNO7"
                                , "EDIFICIALITA7"
                                , "SEZIONEURBANA8"
                                , "FOGLIO8"
                                , "NUMERO8"
                                , "DENOMINATORE8"
                                , "SUBALTERNO8"
                                , "EDIFICIALITA8"
                                , "SEZIONEURBANA9"
                                , "FOGLIO9"
                                , "NUMERO9"
                                , "DENOMINATORE9"
                                , "SUBALTERNO9"
                                , "EDIFICIALITA9"
                                , "SEZIONEURBANA10"
                                , "FOGLIO10"
                                , "NUMERO10"
                                , "DENOMINATORE10"
                                , "SUBALTERNO10"
                                , "EDIFICIALITA10"
                                , "TOPONIMO1"
                                , "INDIRIZZO1"
                                , "CIVICO11"
                                , "CIVICO21"
                                , "CIVICO31"
                                , "CODICESTRADA1"
                                , "TOPONIMO2"
                                , "INDIRIZZO2"
                                , "CIVICO12"
                                , "CIVICO22"
                                , "CIVICO32"
                                , "CODICESTRADA2"
                                , "TOPONIMO3"
                                , "INDIRIZZO3"
                                , "CIVICO13"
                                , "CIVICO23"
                                , "CIVICO33"
                                , "CODICESTRADA3"
                                , "TOPONIMO4"
                                , "INDIRIZZO4"
                                , "CIVICO14"
                                , "CIVICO24"
                                , "CIVICO34"
                                , "CODICESTRADA4"
                                , "UC_SEZIONEURBANA1"
                                , "UC_FOGLIO1"
                                , "UC_NUMERO1"
                                , "UC_DENOMINATORE1"
                                , "UC_SUBALTERNO1"
                                , "UC_SEZIONEURBANA2"
                                , "UC_FOGLIO2"
                                , "UC_NUMERO2"
                                , "UC_DENOMINATORE2"
                                , "UC_SUBALTERNO2"
                                , "UC_SEZIONEURBANA3"
                                , "UC_FOGLIO3"
                                , "UC_NUMERO3"
                                , "UC_DENOMINATORE3"
                                , "UC_SUBALTERNO3"
                                , "UC_SEZIONEURBANA4"
                                , "UC_FOGLIO4"
                                , "UC_NUMERO4"
                                , "UC_DENOMINATORE4"
                                , "UC_SUBALTERNO4"
                                , "UC_SEZIONEURBANA5"
                                , "UC_FOGLIO5"
                                , "UC_NUMERO5"
                                , "UC_DENOMINATORE5"
                                , "UC_SUBALTERNO5"
                                , "UC_SEZIONEURBANA6"
                                , "UC_FOGLIO6"
                                , "UC_NUMERO6"
                                , "UC_DENOMINATORE6"
                                , "UC_SUBALTERNO6"
                                , "UC_SEZIONEURBANA7"
                                , "UC_FOGLIO7"
                                , "UC_NUMERO7"
                                , "UC_DENOMINATORE7"
                                , "UC_SUBALTERNO7"
                                , "UC_SEZIONEURBANA8"
                                , "UC_FOGLIO8"
                                , "UC_NUMERO8"
                                , "UC_DENOMINATORE8"
                                , "UC_SUBALTERNO8"
                                , "UC_SEZIONEURBANA9"
                                , "UC_FOGLIO9"
                                , "UC_NUMERO9"
                                , "UC_DENOMINATORE9"
                                , "UC_SUBALTERNO9"
                                , "UC_SEZIONEURBANA10"
                                , "UC_FOGLIO10"
                                , "UC_NUMERO10"
                                , "UC_DENOMINATORE10"
                                , "UC_SUBALTERNO10"
                                , "CODICERISERVA1"
                                , "PARTITAISCRIZIONERISERVA1"
                                , "CODICERISERVA2"
                                , "PARTITAISCRIZIONERISERVA2"
                                , "CODICERISERVA3"
                                , "PARTITAISCRIZIONERISERVA3"
                                , "CODICERISERVA4"
                                , "PARTITAISCRIZIONERISERVA4"
                                , "CODICERISERVA5"
                                , "PARTITAISCRIZIONERISERVA5"
                                , "CODICERISERVA6"
                                , "PARTITAISCRIZIONERISERVA6"
                                , "CODICERISERVA7"
                                , "PARTITAISCRIZIONERISERVA7"
                                , "CODICERISERVA8"
                                , "PARTITAISCRIZIONERISERVA8"
                                , "CODICERISERVA9"
                                , "PARTITAISCRIZIONERISERVA9"
                                , "CODICERISERVA10"
                                , "PARTITAISCRIZIONERISERVA10"
                            );
                        myItem.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", myItem.ID)
                                , ctx.GetParam("IDELABORAZIONE", myItem.IDElaborazione)
                                , ctx.GetParam("IDCATASTALE", myItem.IDCatastale)
                                , ctx.GetParam("SEZIONE", myItem.Sezione)
                                , ctx.GetParam("IDIMMOBILE", myItem.IDImmobile)
                                , ctx.GetParam("TIPOIMMOBILE", myItem.TipoImmobile)
                                , ctx.GetParam("PROGRESSIVO", myItem.Progressivo)
                                , ctx.GetParam("ZONA", myItem.Zona)
                                , ctx.GetParam("CATEGORIA", myItem.Categoria)
                                , ctx.GetParam("CLASSE", myItem.Classe)
                                , ctx.GetParam("CONSISTENZA", myItem.Consistenza)
                                , ctx.GetParam("SUPERFICIE", myItem.Superficie)
                                , ctx.GetParam("RENDITALIRE", myItem.RenditaLire)
                                , ctx.GetParam("RENDITAEURO", myItem.RenditaEuro)
                                , ctx.GetParam("LOTTO", myItem.Lotto)
                                , ctx.GetParam("EDIFICIO", myItem.Edificio)
                                , ctx.GetParam("SCALA", myItem.Scala)
                                , ctx.GetParam("INTERNO1", myItem.Interno1)
                                , ctx.GetParam("INTERNO2", myItem.Interno2)
                                , ctx.GetParam("PIANO1", myItem.Piano1)
                                , ctx.GetParam("PIANO2", myItem.Piano2)
                                , ctx.GetParam("PIANO3", myItem.Piano3)
                                , ctx.GetParam("PIANO4", myItem.Piano4)
                                , ctx.GetParam("DATAINIZIOEFFICACIA", myItem.DataInizioEfficacia)
                                , ctx.GetParam("DATAINIZIOREGISTRAZIONEINATTI", myItem.DataInizioRegistrazioneAtti)
                                , ctx.GetParam("TIPONOTAINIZIO", myItem.TipoNotaInizio)
                                , ctx.GetParam("NUMERONOTAINIZIO", myItem.NumeroNotaInizio)
                                , ctx.GetParam("PROGRESSIVONOTAINIZIO", myItem.ProgressivoNotaInizio)
                                , ctx.GetParam("ANNONOTAINIZIO", myItem.AnnoNotaInizio)
                                , ctx.GetParam("DATAFINEEFFICACIA", myItem.DataFineEfficacia)
                                , ctx.GetParam("DATAFINEREGISTRAZIONEATTI", myItem.DataFineRegistrazioneAtti)
                                , ctx.GetParam("TIPONOTAFINE", myItem.TipoNotaFine)
                                , ctx.GetParam("NUMERONOTAFINE", myItem.NumeroNotaFine)
                                , ctx.GetParam("PROGRESSIVONOTAFINE", myItem.ProgressivoNotaFine)
                                , ctx.GetParam("ANNONOTAFINE", myItem.AnnoNotaFine)
                                , ctx.GetParam("PARTITA", myItem.Partita)
                                , ctx.GetParam("ANNOTAZIONE", myItem.Annotazione)
                                , ctx.GetParam("IDENTIFICATIVOMUTAZIONEINIZIALE", myItem.IDMutazioneIniziale)
                                , ctx.GetParam("IDENTIFICATIVOMUTAZIONEFINALE", myItem.IDMutazioneFinale)
                                , ctx.GetParam("PROTOCOLLONOTIFICA", myItem.ProtocolloNotifica)
                                , ctx.GetParam("DATANOTIFICA", myItem.DataNotifica)
                                , ctx.GetParam("CODICECAUSALEATTOGENERANTE", myItem.CodiceCausaleAttoGenerante)
                                , ctx.GetParam("DESCRIZIONEATTOGENERANTE", myItem.DescrizioneAttoGenerante)
                                , ctx.GetParam("CODICECAUSALEATTOCONCLUSIVO", myItem.CodiceCausaleAttoConclusivo)
                                , ctx.GetParam("DESCRIZIONEATTOCONCLUSIVO", myItem.DescrizioneAttoConclusivo)
                                , ctx.GetParam("FLAGCLASSAMENTO", myItem.FlagClassamento)
                                , ctx.GetParam("SEZIONEURBANA1", myItem.SezioneUrbana1)
                                , ctx.GetParam("FOGLIO1", myItem.Foglio1)
                                , ctx.GetParam("NUMERO1", myItem.Numero1)
                                , ctx.GetParam("DENOMINATORE1", myItem.Denominatore1)
                                , ctx.GetParam("SUBALTERNO1", myItem.Subalterno1)
                                , ctx.GetParam("EDIFICIALITA1", myItem.Edificialita1)
                                , ctx.GetParam("SEZIONEURBANA2", myItem.SezioneUrbana2)
                                , ctx.GetParam("FOGLIO2", myItem.Foglio2)
                                , ctx.GetParam("NUMERO2", myItem.Numero2)
                                , ctx.GetParam("DENOMINATORE2", myItem.Denominatore2)
                                , ctx.GetParam("SUBALTERNO2", myItem.Subalterno2)
                                , ctx.GetParam("EDIFICIALITA2", myItem.Edificialita2)
                                , ctx.GetParam("SEZIONEURBANA3", myItem.SezioneUrbana3)
                                , ctx.GetParam("FOGLIO3", myItem.Foglio3)
                                , ctx.GetParam("NUMERO3", myItem.Numero3)
                                , ctx.GetParam("DENOMINATORE3", myItem.Denominatore3)
                                , ctx.GetParam("SUBALTERNO3", myItem.Subalterno3)
                                , ctx.GetParam("EDIFICIALITA3", myItem.Edificialita3)
                                , ctx.GetParam("SEZIONEURBANA4", myItem.SezioneUrbana4)
                                , ctx.GetParam("FOGLIO4", myItem.Foglio4)
                                , ctx.GetParam("NUMERO4", myItem.Numero4)
                                , ctx.GetParam("DENOMINATORE4", myItem.Denominatore4)
                                , ctx.GetParam("SUBALTERNO4", myItem.Subalterno4)
                                , ctx.GetParam("EDIFICIALITA4", myItem.Edificialita4)
                                , ctx.GetParam("SEZIONEURBANA5", myItem.SezioneUrbana5)
                                , ctx.GetParam("FOGLIO5", myItem.Foglio5)
                                , ctx.GetParam("NUMERO5", myItem.Numero5)
                                , ctx.GetParam("DENOMINATORE5", myItem.Denominatore5)
                                , ctx.GetParam("SUBALTERNO5", myItem.Subalterno5)
                                , ctx.GetParam("EDIFICIALITA5", myItem.Edificialita5)
                                , ctx.GetParam("SEZIONEURBANA6", myItem.SezioneUrbana6)
                                , ctx.GetParam("FOGLIO6", myItem.Foglio6)
                                , ctx.GetParam("NUMERO6", myItem.Numero6)
                                , ctx.GetParam("DENOMINATORE6", myItem.Denominatore6)
                                , ctx.GetParam("SUBALTERNO6", myItem.Subalterno6)
                                , ctx.GetParam("EDIFICIALITA6", myItem.Edificialita6)
                                , ctx.GetParam("SEZIONEURBANA7", myItem.SezioneUrbana7)
                                , ctx.GetParam("FOGLIO7", myItem.Foglio7)
                                , ctx.GetParam("NUMERO7", myItem.Numero7)
                                , ctx.GetParam("DENOMINATORE7", myItem.Denominatore7)
                                , ctx.GetParam("SUBALTERNO7", myItem.Subalterno7)
                                , ctx.GetParam("EDIFICIALITA7", myItem.Edificialita7)
                                , ctx.GetParam("SEZIONEURBANA8", myItem.SezioneUrbana8)
                                , ctx.GetParam("FOGLIO8", myItem.Foglio8)
                                , ctx.GetParam("NUMERO8", myItem.Numero8)
                                , ctx.GetParam("DENOMINATORE8", myItem.Denominatore8)
                                , ctx.GetParam("SUBALTERNO8", myItem.Subalterno8)
                                , ctx.GetParam("EDIFICIALITA8", myItem.Edificialita8)
                                , ctx.GetParam("SEZIONEURBANA9", myItem.SezioneUrbana9)
                                , ctx.GetParam("FOGLIO9", myItem.Foglio9)
                                , ctx.GetParam("NUMERO9", myItem.Numero9)
                                , ctx.GetParam("DENOMINATORE9", myItem.Denominatore9)
                                , ctx.GetParam("SUBALTERNO9", myItem.Subalterno9)
                                , ctx.GetParam("EDIFICIALITA9", myItem.Edificialita9)
                                , ctx.GetParam("SEZIONEURBANA10", myItem.SezioneUrbana10)
                                , ctx.GetParam("FOGLIO10", myItem.Foglio10)
                                , ctx.GetParam("NUMERO10", myItem.Numero10)
                                , ctx.GetParam("DENOMINATORE10", myItem.Denominatore10)
                                , ctx.GetParam("SUBALTERNO10", myItem.Subalterno10)
                                , ctx.GetParam("EDIFICIALITA10", myItem.Edificialita10)
                                , ctx.GetParam("TOPONIMO1", myItem.Toponimo1)
                                , ctx.GetParam("INDIRIZZO1", myItem.Indirizzo1)
                                , ctx.GetParam("CIVICO11", myItem.Civico11)
                                , ctx.GetParam("CIVICO21", myItem.Civico21)
                                , ctx.GetParam("CIVICO31", myItem.Civico31)
                                , ctx.GetParam("CODICESTRADA1", myItem.CodiceStrada1)
                                , ctx.GetParam("TOPONIMO2", myItem.Toponimo2)
                                , ctx.GetParam("INDIRIZZO2", myItem.Indirizzo2)
                                , ctx.GetParam("CIVICO12", myItem.Civico12)
                                , ctx.GetParam("CIVICO22", myItem.Civico22)
                                , ctx.GetParam("CIVICO32", myItem.Civico32)
                                , ctx.GetParam("CODICESTRADA2", myItem.CodiceStrada2)
                                , ctx.GetParam("TOPONIMO3", myItem.Toponimo3)
                                , ctx.GetParam("INDIRIZZO3", myItem.Indirizzo3)
                                , ctx.GetParam("CIVICO13", myItem.Civico13)
                                , ctx.GetParam("CIVICO23", myItem.Civico23)
                                , ctx.GetParam("CIVICO33", myItem.Civico33)
                                , ctx.GetParam("CODICESTRADA3", myItem.CodiceStrada3)
                                , ctx.GetParam("TOPONIMO4", myItem.Toponimo4)
                                , ctx.GetParam("INDIRIZZO4", myItem.Indirizzo4)
                                , ctx.GetParam("CIVICO14", myItem.Civico14)
                                , ctx.GetParam("CIVICO24", myItem.Civico24)
                                , ctx.GetParam("CIVICO34", myItem.Civico34)
                                , ctx.GetParam("CODICESTRADA4", myItem.CodiceStrada4)
                                , ctx.GetParam("UC_SEZIONEURBANA1", myItem.UC_SezioneUrbana1)
                                , ctx.GetParam("UC_FOGLIO1", myItem.UC_Foglio1)
                                , ctx.GetParam("UC_NUMERO1", myItem.UC_Numero1)
                                , ctx.GetParam("UC_DENOMINATORE1", myItem.UC_Denominatore1)
                                , ctx.GetParam("UC_SUBALTERNO1", myItem.UC_Subalterno1)
                                , ctx.GetParam("UC_SEZIONEURBANA2", myItem.UC_SezioneUrbana2)
                                , ctx.GetParam("UC_FOGLIO2", myItem.UC_Foglio2)
                                , ctx.GetParam("UC_NUMERO2", myItem.UC_Numero2)
                                , ctx.GetParam("UC_DENOMINATORE2", myItem.UC_Denominatore2)
                                , ctx.GetParam("UC_SUBALTERNO2", myItem.UC_Subalterno2)
                                , ctx.GetParam("UC_SEZIONEURBANA3", myItem.UC_SezioneUrbana3)
                                , ctx.GetParam("UC_FOGLIO3", myItem.UC_Foglio3)
                                , ctx.GetParam("UC_NUMERO3", myItem.UC_Numero3)
                                , ctx.GetParam("UC_DENOMINATORE3", myItem.UC_Denominatore3)
                                , ctx.GetParam("UC_SUBALTERNO3", myItem.UC_Subalterno3)
                                , ctx.GetParam("UC_SEZIONEURBANA4", myItem.UC_SezioneUrbana4)
                                , ctx.GetParam("UC_FOGLIO4", myItem.UC_Foglio4)
                                , ctx.GetParam("UC_NUMERO4", myItem.UC_Numero4)
                                , ctx.GetParam("UC_DENOMINATORE4", myItem.UC_Denominatore4)
                                , ctx.GetParam("UC_SUBALTERNO4", myItem.UC_Subalterno4)
                                , ctx.GetParam("UC_SEZIONEURBANA5", myItem.UC_SezioneUrbana5)
                                , ctx.GetParam("UC_FOGLIO5", myItem.UC_Foglio5)
                                , ctx.GetParam("UC_NUMERO5", myItem.UC_Numero5)
                                , ctx.GetParam("UC_DENOMINATORE5", myItem.UC_Denominatore5)
                                , ctx.GetParam("UC_SUBALTERNO5", myItem.UC_Subalterno5)
                                , ctx.GetParam("UC_SEZIONEURBANA6", myItem.UC_SezioneUrbana6)
                                , ctx.GetParam("UC_FOGLIO6", myItem.UC_Foglio6)
                                , ctx.GetParam("UC_NUMERO6", myItem.UC_Numero6)
                                , ctx.GetParam("UC_DENOMINATORE6", myItem.UC_Denominatore6)
                                , ctx.GetParam("UC_SUBALTERNO6", myItem.UC_Subalterno6)
                                , ctx.GetParam("UC_SEZIONEURBANA7", myItem.UC_SezioneUrbana7)
                                , ctx.GetParam("UC_FOGLIO7", myItem.UC_Foglio7)
                                , ctx.GetParam("UC_NUMERO7", myItem.UC_Numero7)
                                , ctx.GetParam("UC_DENOMINATORE7", myItem.UC_Denominatore7)
                                , ctx.GetParam("UC_SUBALTERNO7", myItem.UC_Subalterno7)
                                , ctx.GetParam("UC_SEZIONEURBANA8", myItem.UC_SezioneUrbana8)
                                , ctx.GetParam("UC_FOGLIO8", myItem.UC_Foglio8)
                                , ctx.GetParam("UC_NUMERO8", myItem.UC_Numero8)
                                , ctx.GetParam("UC_DENOMINATORE8", myItem.UC_Denominatore8)
                                , ctx.GetParam("UC_SUBALTERNO8", myItem.UC_Subalterno8)
                                , ctx.GetParam("UC_SEZIONEURBANA9", myItem.UC_SezioneUrbana9)
                                , ctx.GetParam("UC_FOGLIO9", myItem.UC_Foglio9)
                                , ctx.GetParam("UC_NUMERO9", myItem.UC_Numero9)
                                , ctx.GetParam("UC_DENOMINATORE9", myItem.UC_Denominatore9)
                                , ctx.GetParam("UC_SUBALTERNO9", myItem.UC_Subalterno9)
                                , ctx.GetParam("UC_SEZIONEURBANA10", myItem.UC_SezioneUrbana10)
                                , ctx.GetParam("UC_FOGLIO10", myItem.UC_Foglio10)
                                , ctx.GetParam("UC_NUMERO10", myItem.UC_Numero10)
                                , ctx.GetParam("UC_DENOMINATORE10", myItem.UC_Denominatore10)
                                , ctx.GetParam("UC_SUBALTERNO10", myItem.UC_Subalterno10)
                                , ctx.GetParam("CODICERISERVA1", myItem.CodiceRiserva1)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA1", myItem.PartitaIscrizioneRiserva1)
                                , ctx.GetParam("CODICERISERVA2", myItem.CodiceRiserva2)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA2", myItem.PartitaIscrizioneRiserva2)
                                , ctx.GetParam("CODICERISERVA3", myItem.CodiceRiserva3)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA3", myItem.PartitaIscrizioneRiserva3)
                                , ctx.GetParam("CODICERISERVA4", myItem.CodiceRiserva4)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA4", myItem.PartitaIscrizioneRiserva4)
                                , ctx.GetParam("CODICERISERVA5", myItem.CodiceRiserva5)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA5", myItem.PartitaIscrizioneRiserva5)
                                , ctx.GetParam("CODICERISERVA6", myItem.CodiceRiserva6)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA6", myItem.PartitaIscrizioneRiserva6)
                                , ctx.GetParam("CODICERISERVA7", myItem.CodiceRiserva7)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA7", myItem.PartitaIscrizioneRiserva7)
                                , ctx.GetParam("CODICERISERVA8", myItem.CodiceRiserva8)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA8", myItem.PartitaIscrizioneRiserva8)
                                , ctx.GetParam("CODICERISERVA9", myItem.CodiceRiserva9)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA9", myItem.PartitaIscrizioneRiserva9)
                                , ctx.GetParam("CODICERISERVA10", myItem.CodiceRiserva10)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA10", myItem.PartitaIscrizioneRiserva10)
                            ).First<int>();
                        if (myItem.ID <= 0)
                        {
                            Log.Debug("Motore_Catasto.ClsManageDB.SaveFAB::errore in inserimento");
                            return false;
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.SaveFAB.errore::", ex);
                return false;
            }
        }
        public bool SaveTER(List<Terreno> ListItem)
        {
            try
            {
                using (DBModel ctx = new DBModel(DBModel.Ambiente_Catasto))
                {
                    foreach (Terreno myItem in ListItem)
                    {
                        string sSQL = ctx.GetSQL("prc_CATASTO_TER_IU", "ID"
                                , "IDELABORAZIONE"
                                , "IDCATASTALE"
                                , "SEZIONE"
                                , "IDIMMOBILE"
                                , "TIPOIMMOBILE"
                                , "PROGRESSIVO"
                                , "FOGLIO"
                                , "NUMERO"
                                , "DENOMINATORE"
                                , "SUBALTERNO"
                                , "EDIFICIALITA"
                                , "QUALITA"
                                , "CLASSE"
                                , "ETTARI"
                                , "ARE"
                                , "CENTIARE"
                                , "FLAGREDDITO"
                                , "FLAGPORZIONE"
                                , "FLAGDEDUZIONI"
                                , "REDDITODOMINICALELIRE"
                                , "REDDITOAGRARIOLIRE"
                                , "REDDITODOMINICALEEURO"
                                , "REDDITOAGRARIOEURO"
                                , "DATAINIZIOEFFICACIA"
                                , "DATAINIZIOREGISTRAZIONEATTI"
                                , "TIPONOTAINIZIO"
                                , "NUMERONOTAINIZIO"
                                , "PROGRESSIVONOTAINIZIO"
                                , "ANNONOTAINIZIO"
                                , "DATAFINEEFFICACIA"
                                , "DATAFINEREGISTRAZIONEATTI"
                                , "TIPONOTAFINE"
                                , "NUMERONOTAFINE"
                                , "PROGRESSIVONOTAFINE"
                                , "ANNONOTAFINE"
                                , "PARTITA"
                                , "ANNOTAZIONE"
                                , "IDMUTAZIONEINIZIALE"
                                , "IDMUTAZIONEFINALE"
                                , "CODICECAUSALEATTOGENERANTE"
                                , "DESCRIZIONEATTOGENERANTE"
                                , "CODICECAUSALEATTOCONCLUSIVO"
                                , "DESCRIZIONEATTOCONCLUSIVO"
                                , "SIMBOLODEDUZIONE1"
                                , "SIMBOLODEDUZIONE2"
                                , "SIMBOLODEDUZIONE3"
                                , "SIMBOLODEDUZIONE4"
                                , "SIMBOLODEDUZIONE5"
                                , "SIMBOLODEDUZIONE6"
                                , "SIMBOLODEDUZIONE7"
                                , "CODICERISERVA1"
                                , "PARTITAISCRIZIONERISERVA1"
                                , "CODICERISERVA2"
                                , "PARTITAISCRIZIONERISERVA2"
                                , "CODICERISERVA3"
                                , "PARTITAISCRIZIONERISERVA3"
                                , "CODICERISERVA4"
                                , "PARTITAISCRIZIONERISERVA4"
                                , "CODICERISERVA5"
                                , "PARTITAISCRIZIONERISERVA5"
                                , "CODICERISERVA6"
                                , "PARTITAISCRIZIONERISERVA6"
                                , "CODICERISERVA7"
                                , "PARTITAISCRIZIONERISERVA7"
                                , "CODICERISERVA8"
                                , "PARTITAISCRIZIONERISERVA8"
                                , "CODICERISERVA9"
                                , "PARTITAISCRIZIONERISERVA9"
                                , "CODICERISERVA10"
                                , "PARTITAISCRIZIONERISERVA10"
                                , "CODICERISERVA11"
                                , "PARTITAISCRIZIONERISERVA11"
                                , "CODICERISERVA12"
                                , "PARTITAISCRIZIONERISERVA12"
                                , "CODICERISERVA13"
                                , "PARTITAISCRIZIONERISERVA13"
                                , "CODICERISERVA14"
                                , "PARTITAISCRIZIONERISERVA14"
                                , "CODICERISERVA15"
                                , "PARTITAISCRIZIONERISERVA15"
                                , "CODICERISERVA16"
                                , "PARTITAISCRIZIONERISERVA16"
                                , "CODICERISERVA17"
                                , "PARTITAISCRIZIONERISERVA17"
                                , "CODICERISERVA18"
                                , "PARTITAISCRIZIONERISERVA18"
                                , "CODICERISERVA19"
                                , "PARTITAISCRIZIONERISERVA19"
                                , "CODICERISERVA20"
                                , "PARTITAISCRIZIONERISERVA20"
                                , "CODICERISERVA21"
                                , "PARTITAISCRIZIONERISERVA21"
                                , "CODICERISERVA22"
                                , "PARTITAISCRIZIONERISERVA22"
                                , "CODICERISERVA23"
                                , "PARTITAISCRIZIONERISERVA23"
                                , "CODICERISERVA24"
                                , "PARTITAISCRIZIONERISERVA24"
                                , "CODICERISERVA25"
                                , "PARTITAISCRIZIONERISERVA25"
                                , "CODICERISERVA26"
                                , "PARTITAISCRIZIONERISERVA26"
                                , "CODICERISERVA27"
                                , "PARTITAISCRIZIONERISERVA27"
                                , "CODICERISERVA28"
                                , "PARTITAISCRIZIONERISERVA28"
                                , "CODICERISERVA29"
                                , "PARTITAISCRIZIONERISERVA29"
                                , "CODICERISERVA30"
                                , "PARTITAISCRIZIONERISERVA30"
                                , "IDENTIFICATIVOPORZIONE1"
                                , "QUALITAPORZIONE1"
                                , "CLASSEPORZIONE1"
                                , "ETTARIPORZIONE1"
                                , "AREPORZIONE1"
                                , "CENTIAREPORZIONE1"
                                , "REDDITODOMINICALEEUROPORZIONE1"
                                , "REDDITOAGRARIOEUROPORZIONE1"
                                , "IDENTIFICATIVOPORZIONE2"
                                , "QUALITAPORZIONE2"
                                , "CLASSEPORZIONE2"
                                , "ETTARIPORZIONE2"
                                , "AREPORZIONE2"
                                , "CENTIAREPORZIONE2"
                                , "REDDITODOMINICALEEUROPORZIONE2"
                                , "REDDITOAGRARIOEUROPORZIONE2"
                                , "IDENTIFICATIVOPORZIONE3"
                                , "QUALITAPORZIONE3"
                                , "CLASSEPORZIONE3"
                                , "ETTARIPORZIONE3"
                                , "AREPORZIONE3"
                                , "CENTIAREPORZIONE3"
                                , "REDDITODOMINICALEEUROPORZIONE3"
                                , "REDDITOAGRARIOEUROPORZIONE3"
                                , "IDENTIFICATIVOPORZIONE4"
                                , "QUALITAPORZIONE4"
                                , "CLASSEPORZIONE4"
                                , "ETTARIPORZIONE4"
                                , "AREPORZIONE4"
                                , "CENTIAREPORZIONE4"
                                , "REDDITODOMINICALEEUROPORZIONE4"
                                , "REDDITOAGRARIOEUROPORZIONE4"
                                , "IDENTIFICATIVOPORZIONE5"
                                , "QUALITAPORZIONE5"
                                , "CLASSEPORZIONE5"
                                , "ETTARIPORZIONE5"
                                , "AREPORZIONE5"
                                , "CENTIAREPORZIONE5"
                                , "REDDITODOMINICALEEUROPORZIONE5"
                                , "REDDITOAGRARIOEUROPORZIONE5"
                                , "IDENTIFICATIVOPORZIONE6"
                                , "QUALITAPORZIONE6"
                                , "CLASSEPORZIONE6"
                                , "ETTARIPORZIONE6"
                                , "AREPORZIONE6"
                                , "CENTIAREPORZIONE6"
                                , "REDDITODOMINICALEEUROPORZIONE6"
                                , "REDDITOAGRARIOEUROPORZIONE6"
                                , "IDENTIFICATIVOPORZIONE7"
                                , "QUALITAPORZIONE7"
                                , "CLASSEPORZIONE7"
                                , "ETTARIPORZIONE7"
                                , "AREPORZIONE7"
                                , "CENTIAREPORZIONE7"
                                , "REDDITODOMINICALEEUROPORZIONE7"
                                , "REDDITOAGRARIOEUROPORZIONE7"
                                , "IDENTIFICATIVOPORZIONE8"
                                , "QUALITAPORZIONE8"
                                , "CLASSEPORZIONE8"
                                , "ETTARIPORZIONE8"
                                , "AREPORZIONE8"
                                , "CENTIAREPORZIONE8"
                                , "REDDITODOMINICALEEUROPORZIONE8"
                                , "REDDITOAGRARIOEUROPORZIONE8"
                                , "IDENTIFICATIVOPORZIONE9"
                                , "QUALITAPORZIONE9"
                                , "CLASSEPORZIONE9"
                                , "ETTARIPORZIONE9"
                                , "AREPORZIONE9"
                                , "CENTIAREPORZIONE9"
                                , "REDDITODOMINICALEEUROPORZIONE9"
                                , "REDDITOAGRARIOEUROPORZIONE9"
                                , "IDENTIFICATIVOPORZIONE10"
                                , "QUALITAPORZIONE10"
                                , "CLASSEPORZIONE10"
                                , "ETTARIPORZIONE10"
                                , "AREPORZIONE10"
                                , "CENTIAREPORZIONE10"
                                , "REDDITODOMINICALEEUROPORZIONE10"
                                , "REDDITOAGRARIOEUROPORZIONE10"
                                , "IDENTIFICATIVOPORZIONE11"
                                , "QUALITAPORZIONE11"
                                , "CLASSEPORZIONE11"
                                , "ETTARIPORZIONE11"
                                , "AREPORZIONE11"
                                , "CENTIAREPORZIONE11"
                                , "REDDITODOMINICALEEUROPORZIONE11"
                                , "REDDITOAGRARIOEUROPORZIONE11"
                                , "IDENTIFICATIVOPORZIONE12"
                                , "QUALITAPORZIONE12"
                                , "CLASSEPORZIONE12"
                                , "ETTARIPORZIONE12"
                                , "AREPORZIONE12"
                                , "CENTIAREPORZIONE12"
                                , "REDDITODOMINICALEEUROPORZIONE12"
                                , "REDDITOAGRARIOEUROPORZIONE12"
                                , "IDENTIFICATIVOPORZIONE13"
                                , "QUALITAPORZIONE13"
                                , "CLASSEPORZIONE13"
                                , "ETTARIPORZIONE13"
                                , "AREPORZIONE13"
                                , "CENTIAREPORZIONE13"
                                , "REDDITODOMINICALEEUROPORZIONE13"
                                , "REDDITOAGRARIOEUROPORZIONE13"
                                , "IDENTIFICATIVOPORZIONE14"
                                , "QUALITAPORZIONE14"
                                , "CLASSEPORZIONE14"
                                , "ETTARIPORZIONE14"
                                , "AREPORZIONE14"
                                , "CENTIAREPORZIONE14"
                                , "REDDITODOMINICALEEUROPORZIONE14"
                                , "REDDITOAGRARIOEUROPORZIONE14"
                                , "IDENTIFICATIVOPORZIONE15"
                                , "QUALITAPORZIONE15"
                                , "CLASSEPORZIONE15"
                                , "ETTARIPORZIONE15"
                                , "AREPORZIONE15"
                                , "CENTIAREPORZIONE15"
                                , "REDDITODOMINICALEEUROPORZIONE15"
                                , "REDDITOAGRARIOEUROPORZIONE15"
                                , "IDENTIFICATIVOPORZIONE16"
                                , "QUALITAPORZIONE16"
                                , "CLASSEPORZIONE16"
                                , "ETTARIPORZIONE16"
                                , "AREPORZIONE16"
                                , "CENTIAREPORZIONE16"
                                , "REDDITODOMINICALEEUROPORZIONE16"
                                , "REDDITOAGRARIOEUROPORZIONE16"
                                , "IDENTIFICATIVOPORZIONE17"
                                , "QUALITAPORZIONE17"
                                , "CLASSEPORZIONE17"
                                , "ETTARIPORZIONE17"
                                , "AREPORZIONE17"
                                , "CENTIAREPORZIONE17"
                                , "REDDITODOMINICALEEUROPORZIONE17"
                                , "REDDITOAGRARIOEUROPORZIONE17"
                                , "IDENTIFICATIVOPORZIONE18"
                                , "QUALITAPORZIONE18"
                                , "CLASSEPORZIONE18"
                                , "ETTARIPORZIONE18"
                                , "AREPORZIONE18"
                                , "CENTIAREPORZIONE18"
                                , "REDDITODOMINICALEEUROPORZIONE18"
                                , "REDDITOAGRARIOEUROPORZIONE18"
                                , "IDENTIFICATIVOPORZIONE19"
                                , "QUALITAPORZIONE19"
                                , "CLASSEPORZIONE19"
                                , "ETTARIPORZIONE19"
                                , "AREPORZIONE19"
                                , "CENTIAREPORZIONE19"
                                , "REDDITODOMINICALEEUROPORZIONE19"
                                , "REDDITOAGRARIOEUROPORZIONE19"
                                , "IDENTIFICATIVOPORZIONE20"
                                , "QUALITAPORZIONE20"
                                , "CLASSEPORZIONE20"
                                , "ETTARIPORZIONE20"
                                , "AREPORZIONE20"
                                , "CENTIAREPORZIONE20"
                                , "REDDITODOMINICALEEUROPORZIONE20"
                                , "REDDITOAGRARIOEUROPORZIONE20"
                            );
                        myItem.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", myItem.ID)
                                , ctx.GetParam("IDELABORAZIONE", myItem.IDElaborazione)
                                , ctx.GetParam("IDCATASTALE", myItem.IDCatastale)
                                , ctx.GetParam("SEZIONE", myItem.Sezione)
                                , ctx.GetParam("IDIMMOBILE", myItem.IDImmobile)
                                , ctx.GetParam("TIPOIMMOBILE", myItem.TipoImmobile)
                                , ctx.GetParam("PROGRESSIVO", myItem.Progressivo)
                                , ctx.GetParam("FOGLIO", myItem.Foglio)
                                , ctx.GetParam("NUMERO", myItem.Numero)
                                , ctx.GetParam("DENOMINATORE", myItem.Denominatore)
                                , ctx.GetParam("SUBALTERNO", myItem.Subalterno)
                                , ctx.GetParam("EDIFICIALITA", myItem.Edificialita)
                                , ctx.GetParam("QUALITA", myItem.Qualita)
                                , ctx.GetParam("CLASSE", myItem.Classe)
                                , ctx.GetParam("ETTARI", myItem.Ettari)
                                , ctx.GetParam("ARE", myItem.Are)
                                , ctx.GetParam("CENTIARE", myItem.Centiare)
                                , ctx.GetParam("FLAGREDDITO", myItem.FlagReddito)
                                , ctx.GetParam("FLAGPORZIONE", myItem.FlagPorzione)
                                , ctx.GetParam("FLAGDEDUZIONI", myItem.FlagDeduzioni)
                                , ctx.GetParam("REDDITODOMINICALELIRE", myItem.RedditoDominicaleLire)
                                , ctx.GetParam("REDDITOAGRARIOLIRE", myItem.RedditoAgrarioLire)
                                , ctx.GetParam("REDDITODOMINICALEEURO", myItem.RedditoDominicaleEuro)
                                , ctx.GetParam("REDDITOAGRARIOEURO", myItem.RedditoAgrarioEuro)
                                , ctx.GetParam("DATAINIZIOEFFICACIA", myItem.DataInizioEfficacia)
                                , ctx.GetParam("DATAINIZIOREGISTRAZIONEATTI", myItem.DataInizioRegistrazioneAtti)
                                , ctx.GetParam("TIPONOTAINIZIO", myItem.TipoNotaInizio)
                                , ctx.GetParam("NUMERONOTAINIZIO", myItem.NumeroNotaInizio)
                                , ctx.GetParam("PROGRESSIVONOTAINIZIO", myItem.ProgressivoNotaInizio)
                                , ctx.GetParam("ANNONOTAINIZIO", myItem.AnnoNotaInizio)
                                , ctx.GetParam("DATAFINEEFFICACIA", myItem.DataFineEfficacia)
                                , ctx.GetParam("DATAFINEREGISTRAZIONEATTI", myItem.DataFineRegistrazioneAtti)
                                , ctx.GetParam("TIPONOTAFINE", myItem.TipoNotaFine)
                                , ctx.GetParam("NUMERONOTAFINE", myItem.NumeroNotaFine)
                                , ctx.GetParam("PROGRESSIVONOTAFINE", myItem.ProgressivoNotaFine)
                                , ctx.GetParam("ANNONOTAFINE", myItem.AnnoNotaFine)
                                , ctx.GetParam("PARTITA", myItem.Partita)
                                , ctx.GetParam("ANNOTAZIONE", myItem.Annotazione)
                                , ctx.GetParam("IDMUTAZIONEINIZIALE", myItem.IDMutazioneIniziale)
                                , ctx.GetParam("IDMUTAZIONEFINALE", myItem.IDMutazioneFinale)
                                , ctx.GetParam("CODICECAUSALEATTOGENERANTE", myItem.CodiceCausaleAttoGenerante)
                                , ctx.GetParam("DESCRIZIONEATTOGENERANTE", myItem.DescrizioneAttoGenerante)
                                , ctx.GetParam("CODICECAUSALEATTOCONCLUSIVO", myItem.CodiceCausaleAttoConclusivo)
                                , ctx.GetParam("DESCRIZIONEATTOCONCLUSIVO", myItem.DescrizioneAttoConclusivo)
                                , ctx.GetParam("SIMBOLODEDUZIONE1", myItem.SimboloDeduzione1)
                                , ctx.GetParam("SIMBOLODEDUZIONE2", myItem.SimboloDeduzione2)
                                , ctx.GetParam("SIMBOLODEDUZIONE3", myItem.SimboloDeduzione3)
                                , ctx.GetParam("SIMBOLODEDUZIONE4", myItem.SimboloDeduzione4)
                                , ctx.GetParam("SIMBOLODEDUZIONE5", myItem.SimboloDeduzione5)
                                , ctx.GetParam("SIMBOLODEDUZIONE6", myItem.SimboloDeduzione6)
                                , ctx.GetParam("SIMBOLODEDUZIONE7", myItem.SimboloDeduzione7)
                                , ctx.GetParam("CODICERISERVA1", myItem.CodiceRiserva1)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA1", myItem.PartitaIscrizioneRiserva1)
                                , ctx.GetParam("CODICERISERVA2", myItem.CodiceRiserva2)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA2", myItem.PartitaIscrizioneRiserva2)
                                , ctx.GetParam("CODICERISERVA3", myItem.CodiceRiserva3)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA3", myItem.PartitaIscrizioneRiserva3)
                                , ctx.GetParam("CODICERISERVA4", myItem.CodiceRiserva4)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA4", myItem.PartitaIscrizioneRiserva4)
                                , ctx.GetParam("CODICERISERVA5", myItem.CodiceRiserva5)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA5", myItem.PartitaIscrizioneRiserva5)
                                , ctx.GetParam("CODICERISERVA6", myItem.CodiceRiserva6)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA6", myItem.PartitaIscrizioneRiserva6)
                                , ctx.GetParam("CODICERISERVA7", myItem.CodiceRiserva7)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA7", myItem.PartitaIscrizioneRiserva7)
                                , ctx.GetParam("CODICERISERVA8", myItem.CodiceRiserva8)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA8", myItem.PartitaIscrizioneRiserva8)
                                , ctx.GetParam("CODICERISERVA9", myItem.CodiceRiserva9)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA9", myItem.PartitaIscrizioneRiserva9)
                                , ctx.GetParam("CODICERISERVA10", myItem.CodiceRiserva10)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA10", myItem.PartitaIscrizioneRiserva10)
                                , ctx.GetParam("CODICERISERVA11", myItem.CodiceRiserva11)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA11", myItem.PartitaIscrizioneRiserva11)
                                , ctx.GetParam("CODICERISERVA12", myItem.CodiceRiserva12)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA12", myItem.PartitaIscrizioneRiserva12)
                                , ctx.GetParam("CODICERISERVA13", myItem.CodiceRiserva13)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA13", myItem.PartitaIscrizioneRiserva13)
                                , ctx.GetParam("CODICERISERVA14", myItem.CodiceRiserva14)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA14", myItem.PartitaIscrizioneRiserva14)
                                , ctx.GetParam("CODICERISERVA15", myItem.CodiceRiserva15)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA15", myItem.PartitaIscrizioneRiserva15)
                                , ctx.GetParam("CODICERISERVA16", myItem.CodiceRiserva16)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA16", myItem.PartitaIscrizioneRiserva16)
                                , ctx.GetParam("CODICERISERVA17", myItem.CodiceRiserva17)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA17", myItem.PartitaIscrizioneRiserva17)
                                , ctx.GetParam("CODICERISERVA18", myItem.CodiceRiserva18)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA18", myItem.PartitaIscrizioneRiserva18)
                                , ctx.GetParam("CODICERISERVA19", myItem.CodiceRiserva19)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA19", myItem.PartitaIscrizioneRiserva19)
                                , ctx.GetParam("CODICERISERVA20", myItem.CodiceRiserva20)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA20", myItem.PartitaIscrizioneRiserva20)
                                , ctx.GetParam("CODICERISERVA21", myItem.CodiceRiserva21)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA21", myItem.PartitaIscrizioneRiserva21)
                                , ctx.GetParam("CODICERISERVA22", myItem.CodiceRiserva22)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA22", myItem.PartitaIscrizioneRiserva22)
                                , ctx.GetParam("CODICERISERVA23", myItem.CodiceRiserva23)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA23", myItem.PartitaIscrizioneRiserva23)
                                , ctx.GetParam("CODICERISERVA24", myItem.CodiceRiserva24)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA24", myItem.PartitaIscrizioneRiserva24)
                                , ctx.GetParam("CODICERISERVA25", myItem.CodiceRiserva25)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA25", myItem.PartitaIscrizioneRiserva25)
                                , ctx.GetParam("CODICERISERVA26", myItem.CodiceRiserva26)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA26", myItem.PartitaIscrizioneRiserva26)
                                , ctx.GetParam("CODICERISERVA27", myItem.CodiceRiserva27)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA27", myItem.PartitaIscrizioneRiserva27)
                                , ctx.GetParam("CODICERISERVA28", myItem.CodiceRiserva28)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA28", myItem.PartitaIscrizioneRiserva28)
                                , ctx.GetParam("CODICERISERVA29", myItem.CodiceRiserva29)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA29", myItem.PartitaIscrizioneRiserva29)
                                , ctx.GetParam("CODICERISERVA30", myItem.CodiceRiserva30)
                                , ctx.GetParam("PARTITAISCRIZIONERISERVA30", myItem.PartitaIscrizioneRiserva30)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE1", myItem.IdentificativoPorzione1)
                                , ctx.GetParam("QUALITAPORZIONE1", myItem.QualitaPorzione1)
                                , ctx.GetParam("CLASSEPORZIONE1", myItem.ClassePorzione1)
                                , ctx.GetParam("ETTARIPORZIONE1", myItem.EttariPorzione1)
                                , ctx.GetParam("AREPORZIONE1", myItem.ArePorzione1)
                                , ctx.GetParam("CENTIAREPORZIONE1", myItem.CentiarePorzione1)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE1", myItem.RedditoDominicaleEuroPorzione1)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE1", myItem.RedditoAgrarioEuroPorzione1)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE2", myItem.IdentificativoPorzione2)
                                , ctx.GetParam("QUALITAPORZIONE2", myItem.QualitaPorzione2)
                                , ctx.GetParam("CLASSEPORZIONE2", myItem.ClassePorzione2)
                                , ctx.GetParam("ETTARIPORZIONE2", myItem.EttariPorzione2)
                                , ctx.GetParam("AREPORZIONE2", myItem.ArePorzione2)
                                , ctx.GetParam("CENTIAREPORZIONE2", myItem.CentiarePorzione2)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE2", myItem.RedditoDominicaleEuroPorzione2)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE2", myItem.RedditoAgrarioEuroPorzione2)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE3", myItem.IdentificativoPorzione3)
                                , ctx.GetParam("QUALITAPORZIONE3", myItem.QualitaPorzione3)
                                , ctx.GetParam("CLASSEPORZIONE3", myItem.ClassePorzione3)
                                , ctx.GetParam("ETTARIPORZIONE3", myItem.EttariPorzione3)
                                , ctx.GetParam("AREPORZIONE3", myItem.ArePorzione3)
                                , ctx.GetParam("CENTIAREPORZIONE3", myItem.CentiarePorzione3)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE3", myItem.RedditoDominicaleEuroPorzione3)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE3", myItem.RedditoAgrarioEuroPorzione3)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE4", myItem.IdentificativoPorzione4)
                                , ctx.GetParam("QUALITAPORZIONE4", myItem.QualitaPorzione4)
                                , ctx.GetParam("CLASSEPORZIONE4", myItem.ClassePorzione4)
                                , ctx.GetParam("ETTARIPORZIONE4", myItem.EttariPorzione4)
                                , ctx.GetParam("AREPORZIONE4", myItem.ArePorzione4)
                                , ctx.GetParam("CENTIAREPORZIONE4", myItem.CentiarePorzione4)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE4", myItem.RedditoDominicaleEuroPorzione4)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE4", myItem.RedditoAgrarioEuroPorzione4)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE5", myItem.IdentificativoPorzione5)
                                , ctx.GetParam("QUALITAPORZIONE5", myItem.QualitaPorzione5)
                                , ctx.GetParam("CLASSEPORZIONE5", myItem.ClassePorzione5)
                                , ctx.GetParam("ETTARIPORZIONE5", myItem.EttariPorzione5)
                                , ctx.GetParam("AREPORZIONE5", myItem.ArePorzione5)
                                , ctx.GetParam("CENTIAREPORZIONE5", myItem.CentiarePorzione5)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE5", myItem.RedditoDominicaleEuroPorzione5)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE5", myItem.RedditoAgrarioEuroPorzione5)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE6", myItem.IdentificativoPorzione6)
                                , ctx.GetParam("QUALITAPORZIONE6", myItem.QualitaPorzione6)
                                , ctx.GetParam("CLASSEPORZIONE6", myItem.ClassePorzione6)
                                , ctx.GetParam("ETTARIPORZIONE6", myItem.EttariPorzione6)
                                , ctx.GetParam("AREPORZIONE6", myItem.ArePorzione6)
                                , ctx.GetParam("CENTIAREPORZIONE6", myItem.CentiarePorzione6)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE6", myItem.RedditoDominicaleEuroPorzione6)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE6", myItem.RedditoAgrarioEuroPorzione6)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE7", myItem.IdentificativoPorzione7)
                                , ctx.GetParam("QUALITAPORZIONE7", myItem.QualitaPorzione7)
                                , ctx.GetParam("CLASSEPORZIONE7", myItem.ClassePorzione7)
                                , ctx.GetParam("ETTARIPORZIONE7", myItem.EttariPorzione7)
                                , ctx.GetParam("AREPORZIONE7", myItem.ArePorzione7)
                                , ctx.GetParam("CENTIAREPORZIONE7", myItem.CentiarePorzione7)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE7", myItem.RedditoDominicaleEuroPorzione7)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE7", myItem.RedditoAgrarioEuroPorzione7)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE8", myItem.IdentificativoPorzione8)
                                , ctx.GetParam("QUALITAPORZIONE8", myItem.QualitaPorzione8)
                                , ctx.GetParam("CLASSEPORZIONE8", myItem.ClassePorzione8)
                                , ctx.GetParam("ETTARIPORZIONE8", myItem.EttariPorzione8)
                                , ctx.GetParam("AREPORZIONE8", myItem.ArePorzione8)
                                , ctx.GetParam("CENTIAREPORZIONE8", myItem.CentiarePorzione8)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE8", myItem.RedditoDominicaleEuroPorzione8)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE8", myItem.RedditoAgrarioEuroPorzione8)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE9", myItem.IdentificativoPorzione9)
                                , ctx.GetParam("QUALITAPORZIONE9", myItem.QualitaPorzione9)
                                , ctx.GetParam("CLASSEPORZIONE9", myItem.ClassePorzione9)
                                , ctx.GetParam("ETTARIPORZIONE9", myItem.EttariPorzione9)
                                , ctx.GetParam("AREPORZIONE9", myItem.ArePorzione9)
                                , ctx.GetParam("CENTIAREPORZIONE9", myItem.CentiarePorzione9)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE9", myItem.RedditoDominicaleEuroPorzione9)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE9", myItem.RedditoAgrarioEuroPorzione9)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE10", myItem.IdentificativoPorzione10)
                                , ctx.GetParam("QUALITAPORZIONE10", myItem.QualitaPorzione10)
                                , ctx.GetParam("CLASSEPORZIONE10", myItem.ClassePorzione10)
                                , ctx.GetParam("ETTARIPORZIONE10", myItem.EttariPorzione10)
                                , ctx.GetParam("AREPORZIONE10", myItem.ArePorzione10)
                                , ctx.GetParam("CENTIAREPORZIONE10", myItem.CentiarePorzione10)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE10", myItem.RedditoDominicaleEuroPorzione10)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE10", myItem.RedditoAgrarioEuroPorzione10)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE11", myItem.IdentificativoPorzione11)
                                , ctx.GetParam("QUALITAPORZIONE11", myItem.QualitaPorzione11)
                                , ctx.GetParam("CLASSEPORZIONE11", myItem.ClassePorzione11)
                                , ctx.GetParam("ETTARIPORZIONE11", myItem.EttariPorzione11)
                                , ctx.GetParam("AREPORZIONE11", myItem.ArePorzione11)
                                , ctx.GetParam("CENTIAREPORZIONE11", myItem.CentiarePorzione11)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE11", myItem.RedditoDominicaleEuroPorzione11)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE11", myItem.RedditoAgrarioEuroPorzione11)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE12", myItem.IdentificativoPorzione12)
                                , ctx.GetParam("QUALITAPORZIONE12", myItem.QualitaPorzione12)
                                , ctx.GetParam("CLASSEPORZIONE12", myItem.ClassePorzione12)
                                , ctx.GetParam("ETTARIPORZIONE12", myItem.EttariPorzione12)
                                , ctx.GetParam("AREPORZIONE12", myItem.ArePorzione12)
                                , ctx.GetParam("CENTIAREPORZIONE12", myItem.CentiarePorzione12)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE12", myItem.RedditoDominicaleEuroPorzione12)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE12", myItem.RedditoAgrarioEuroPorzione12)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE13", myItem.IdentificativoPorzione13)
                                , ctx.GetParam("QUALITAPORZIONE13", myItem.QualitaPorzione13)
                                , ctx.GetParam("CLASSEPORZIONE13", myItem.ClassePorzione13)
                                , ctx.GetParam("ETTARIPORZIONE13", myItem.EttariPorzione13)
                                , ctx.GetParam("AREPORZIONE13", myItem.ArePorzione13)
                                , ctx.GetParam("CENTIAREPORZIONE13", myItem.CentiarePorzione13)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE13", myItem.RedditoDominicaleEuroPorzione13)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE13", myItem.RedditoAgrarioEuroPorzione13)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE14", myItem.IdentificativoPorzione14)
                                , ctx.GetParam("QUALITAPORZIONE14", myItem.QualitaPorzione14)
                                , ctx.GetParam("CLASSEPORZIONE14", myItem.ClassePorzione14)
                                , ctx.GetParam("ETTARIPORZIONE14", myItem.EttariPorzione14)
                                , ctx.GetParam("AREPORZIONE14", myItem.ArePorzione14)
                                , ctx.GetParam("CENTIAREPORZIONE14", myItem.CentiarePorzione14)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE14", myItem.RedditoDominicaleEuroPorzione14)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE14", myItem.RedditoAgrarioEuroPorzione14)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE15", myItem.IdentificativoPorzione15)
                                , ctx.GetParam("QUALITAPORZIONE15", myItem.QualitaPorzione15)
                                , ctx.GetParam("CLASSEPORZIONE15", myItem.ClassePorzione15)
                                , ctx.GetParam("ETTARIPORZIONE15", myItem.EttariPorzione15)
                                , ctx.GetParam("AREPORZIONE15", myItem.ArePorzione15)
                                , ctx.GetParam("CENTIAREPORZIONE15", myItem.CentiarePorzione15)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE15", myItem.RedditoDominicaleEuroPorzione15)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE15", myItem.RedditoAgrarioEuroPorzione15)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE16", myItem.IdentificativoPorzione16)
                                , ctx.GetParam("QUALITAPORZIONE16", myItem.QualitaPorzione16)
                                , ctx.GetParam("CLASSEPORZIONE16", myItem.ClassePorzione16)
                                , ctx.GetParam("ETTARIPORZIONE16", myItem.EttariPorzione16)
                                , ctx.GetParam("AREPORZIONE16", myItem.ArePorzione16)
                                , ctx.GetParam("CENTIAREPORZIONE16", myItem.CentiarePorzione16)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE16", myItem.RedditoDominicaleEuroPorzione16)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE16", myItem.RedditoAgrarioEuroPorzione16)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE17", myItem.IdentificativoPorzione17)
                                , ctx.GetParam("QUALITAPORZIONE17", myItem.QualitaPorzione17)
                                , ctx.GetParam("CLASSEPORZIONE17", myItem.ClassePorzione17)
                                , ctx.GetParam("ETTARIPORZIONE17", myItem.EttariPorzione17)
                                , ctx.GetParam("AREPORZIONE17", myItem.ArePorzione17)
                                , ctx.GetParam("CENTIAREPORZIONE17", myItem.CentiarePorzione17)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE17", myItem.RedditoDominicaleEuroPorzione17)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE17", myItem.RedditoAgrarioEuroPorzione17)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE18", myItem.IdentificativoPorzione18)
                                , ctx.GetParam("QUALITAPORZIONE18", myItem.QualitaPorzione18)
                                , ctx.GetParam("CLASSEPORZIONE18", myItem.ClassePorzione18)
                                , ctx.GetParam("ETTARIPORZIONE18", myItem.EttariPorzione18)
                                , ctx.GetParam("AREPORZIONE18", myItem.ArePorzione18)
                                , ctx.GetParam("CENTIAREPORZIONE18", myItem.CentiarePorzione18)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE18", myItem.RedditoDominicaleEuroPorzione18)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE18", myItem.RedditoAgrarioEuroPorzione18)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE19", myItem.IdentificativoPorzione19)
                                , ctx.GetParam("QUALITAPORZIONE19", myItem.QualitaPorzione19)
                                , ctx.GetParam("CLASSEPORZIONE19", myItem.ClassePorzione19)
                                , ctx.GetParam("ETTARIPORZIONE19", myItem.EttariPorzione19)
                                , ctx.GetParam("AREPORZIONE19", myItem.ArePorzione19)
                                , ctx.GetParam("CENTIAREPORZIONE19", myItem.CentiarePorzione19)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE19", myItem.RedditoDominicaleEuroPorzione19)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE19", myItem.RedditoAgrarioEuroPorzione19)
                                , ctx.GetParam("IDENTIFICATIVOPORZIONE20", myItem.IdentificativoPorzione20)
                                , ctx.GetParam("QUALITAPORZIONE20", myItem.QualitaPorzione20)
                                , ctx.GetParam("CLASSEPORZIONE20", myItem.ClassePorzione20)
                                , ctx.GetParam("ETTARIPORZIONE20", myItem.EttariPorzione20)
                                , ctx.GetParam("AREPORZIONE20", myItem.ArePorzione20)
                                , ctx.GetParam("CENTIAREPORZIONE20", myItem.CentiarePorzione20)
                                , ctx.GetParam("REDDITODOMINICALEEUROPORZIONE20", myItem.RedditoDominicaleEuroPorzione20)
                                , ctx.GetParam("REDDITOAGRARIOEUROPORZIONE20", myItem.RedditoAgrarioEuroPorzione20)
                            ).First<int>();
                        if (myItem.ID <= 0)
                        {
                            Log.Debug("Motore_Catasto.ClsManageDB.SaveTER::errore in inserimento");
                            return false;
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.SaveTER.errore::", ex);
                return false;
            }
        }
        public bool SaveTIT(List<Titoli> ListItem)
        {
            try
            {
                using (DBModel ctx = new DBModel(DBModel.Ambiente_Catasto))
                {
                    foreach (Titoli myItem in ListItem)
                    {
                        string sSQL = ctx.GetSQL("prc_CATASTO_TIT_IU", "ID"
                                , "IDELABORAZIONE"
                                , "IDCATASTALE"
                                , "SEZIONE"
                                , "IDENTIFICATIVOSOGGETTO"
                                , "TIPOSOGGETTO"
                                , "IDENTIFICATIVOIMMOBILE"
                                , "TIPOIMMOBILE"
                                , "CODICEDIRITTO"
                                , "TITOLONONCODIFICATO"
                                , "QUOTANUMERATORE"
                                , "QUOTADENOMINATORE"
                                , "REGIME"
                                , "SOGGETTODIRIFERIMENTO"
                                , "DATAINIZIOEFFICACIA"
                                , "TIPONOTAINIZIO"
                                , "NUMERONOTAINIZIO"
                                , "PROGRESSIVONOTAINIZIO"
                                , "ANNONOTAINIZIO"
                                , "DATAINIZIOREGISTRAZIONEINATTI"
                                , "PARTITA"
                                , "DATAFINEEFFICACIA"
                                , "TIPONOTAFINE"
                                , "NUMERONOTAFINE"
                                , "PROGRESSIVONOTAFINE"
                                , "ANNONOTAFINE"
                                , "DATAFINEREGISTRAZIONEINATTI"
                                , "IDENTIFICATIVOMUTAZIONEINIZIALE"
                                , "IDENTIFICATIVOMUTAZIONEFINALE"
                                , "IDENTIFICATIVOTITOLARITA"
                                , "CODICECAUSALEATTOGENERANTE"
                                , "DESCRIZIONEATTOGENERANTE"
                                , "CODICECAUSALEATTOCONCLUSIVO"
                                , "DESCRIZIONEATTOCONCLUSIVO"
                            );
                        myItem.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", myItem.ID)
                                , ctx.GetParam("IDELABORAZIONE", myItem.IDElaborazione)
                                , ctx.GetParam("IDCATASTALE", myItem.IDCatastale)
                                , ctx.GetParam("SEZIONE", myItem.Sezione)
                                , ctx.GetParam("IDENTIFICATIVOSOGGETTO", myItem.IDSoggetto)
                                , ctx.GetParam("TIPOSOGGETTO", myItem.TipoSoggetto)
                                , ctx.GetParam("IDENTIFICATIVOIMMOBILE", myItem.IDImmobile)
                                , ctx.GetParam("TIPOIMMOBILE", myItem.TipoImmobile)
                                , ctx.GetParam("CODICEDIRITTO", myItem.CodiceDiritto)
                                , ctx.GetParam("TITOLONONCODIFICATO", myItem.TitoloNonCodificato)
                                , ctx.GetParam("QUOTANUMERATORE", myItem.QuotaNumeratore)
                                , ctx.GetParam("QUOTADENOMINATORE", myItem.QuotaDenominatore)
                                , ctx.GetParam("REGIME", myItem.Regime)
                                , ctx.GetParam("SOGGETTODIRIFERIMENTO", myItem.SoggettoDiRiferimento)
                                , ctx.GetParam("DATAINIZIOEFFICACIA", myItem.DataInizioEfficacia)
                                , ctx.GetParam("TIPONOTAINIZIO", myItem.TipoNotaInizio)
                                , ctx.GetParam("NUMERONOTAINIZIO", myItem.NumeroNotaInizio)
                                , ctx.GetParam("PROGRESSIVONOTAINIZIO", myItem.ProgressivoNotaInizio)
                                , ctx.GetParam("ANNONOTAINIZIO", myItem.AnnoNotaInizio)
                                , ctx.GetParam("DATAINIZIOREGISTRAZIONEINATTI", myItem.DataInizioRegistrazioneAtti)
                                , ctx.GetParam("PARTITA", myItem.Partita)
                                , ctx.GetParam("DATAFINEEFFICACIA", myItem.DataFineEfficacia)
                                , ctx.GetParam("TIPONOTAFINE", myItem.TipoNotaFine)
                                , ctx.GetParam("NUMERONOTAFINE", myItem.NumeroNotaFine)
                                , ctx.GetParam("PROGRESSIVONOTAFINE", myItem.ProgressivoNotaFine)
                                , ctx.GetParam("ANNONOTAFINE", myItem.AnnoNotaFine)
                                , ctx.GetParam("DATAFINEREGISTRAZIONEINATTI", myItem.DataFineRegistrazioneAtti)
                                , ctx.GetParam("IDENTIFICATIVOMUTAZIONEINIZIALE", myItem.IDMutazioneIniziale)
                                , ctx.GetParam("IDENTIFICATIVOMUTAZIONEFINALE", myItem.IDMutazioneFinale)
                                , ctx.GetParam("IDENTIFICATIVOTITOLARITA", myItem.IDTitolarita)
                                , ctx.GetParam("CODICECAUSALEATTOGENERANTE", myItem.CodiceCausaleAttoGenerante)
                                , ctx.GetParam("DESCRIZIONEATTOGENERANTE", myItem.DescrizioneAttoGenerante)
                                , ctx.GetParam("CODICECAUSALEATTOCONCLUSIVO", myItem.CodicecausaleAttoConclusivo)
                                , ctx.GetParam("DESCRIZIONEATTOCONCLUSIVO", myItem.DescrizioneAttoConclusivo)
                            ).First<int>();
                        if (myItem.ID <= 0)
                        {
                            Log.Debug("Motore_Catasto.ClsManageDB.SaveTIT::errore in inserimento");
                            return false;
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.SaveTIT.errore::", ex);
                return false;
            }
        }
        public bool SaveSOG(List<Soggetto> ListItem)
        {
            try
            {
                using (DBModel ctx = new DBModel(DBModel.Ambiente_Catasto))
                {
                    foreach (Soggetto myItem in ListItem)
                    {
                        string sSQL = ctx.GetSQL("prc_CATASTO_SOG_IU", "ID"
                                , "IDELABORAZIONE"
                                , "IDCATASTALE"
                                , "SEZIONE"
                                , "IDSOGGETTO"
                                , "TIPOSOGGETTO"
                                , "COGNOME"
                                , "NOME"
                                , "SESSO"
                                , "DATANASCITA"
                                , "LUOGONASCITA"
                                , "CODFISCALEPIVA"
                                , "NOTE"
                                , "DENOMINAZIONE"
                                , "SEDE"
                            );
                        myItem.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", myItem.ID)
                                , ctx.GetParam("IDELABORAZIONE", myItem.IDElaborazione)
                                , ctx.GetParam("IDCATASTALE", myItem.IDCatastale)
                                , ctx.GetParam("SEZIONE", myItem.Sezione)
                                , ctx.GetParam("IDSOGGETTO", myItem.IDSoggetto)
                                , ctx.GetParam("TIPOSOGGETTO", myItem.TipoSoggetto)
                                , ctx.GetParam("COGNOME", myItem.Cognome)
                                , ctx.GetParam("NOME", myItem.Nome)
                                , ctx.GetParam("SESSO", myItem.Sesso)
                                , ctx.GetParam("DATANASCITA", myItem.DataNascita)
                                , ctx.GetParam("LUOGONASCITA", myItem.LuogoNascita)
                                , ctx.GetParam("CODFISCALEPIVA", myItem.CodFiscalePIVA)
                                , ctx.GetParam("NOTE", myItem.Note)
                                , ctx.GetParam("DENOMINAZIONE", myItem.Denominazione)
                                , ctx.GetParam("SEDE", myItem.Sede)
                            ).First<int>();
                        if (myItem.ID <= 0)
                        {
                            Log.Debug("Motore_Catasto.ClsManageDB.SaveSOG::errore in inserimento");
                            return false;
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.SaveSOG.errore::", ex);
                return false;
            }
        }
        public bool StoricizzaDIC(List<Dichiarazione> ListItem, string Ambiente)
        {
            int x = 0;
            try
            {
                using (DBModel ctx = new DBModel(Ambiente))
                {
                    foreach (Dichiarazione myItem in ListItem)
                    {
                        int nRet = 0;
                        if (myItem.IDImmobile != string.Empty)
                        {
                            string sSQL = ctx.GetSQL("prc_TBLOGGETTI_D", "ID");
                            nRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", myItem.IDImmobile)).First<int>();
                            if (nRet <= 0)
                            {
                                Log.Debug("Motore_Catasto.ClsManageDB.StoricizzaDIC::errore in storicizzazione");
                                Log.Debug("Motore_Catasto.ClsManageDB.StoricizzaDIC.exec prc_TBLOGGETTI_D @ID=" + myItem.ID.ToString());
                            }
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.StoricizzaDIC.errore::" + x.ToString(), ex);
                return false;
            }
        }
        public bool SaveDIC(List<Dichiarazione> ListItem, string Ambiente)
        {
            int x = 0;
            try
            {
                using (DBModel ctx = new DBModel(Ambiente))
                {
                    foreach (Dichiarazione myItem in ListItem)
                    {
             int nRet = 0;
                       string sSQL = ctx.GetSQL("prc_DICHIARAZIONI_IU", "ID"
                                , "IDELABORAZIONE"
                                , "IDCATASTALE"
                                , "COGNOME"
                                , "NOME"
                                , "CODFISCALEPIVA"
                                , "NUMERODICHIARAZIONE"
                                , "DATADICHIARAZIONE"
                                , "IDIMMOBILE"
                                , "IDSTRADA"
                                , "INDIRIZZO"
                                , "CIVICO"
                                , "SCALA"
                                , "PIANO"
                                , "INTERNO"
                                , "FOGLIO"
                                , "NUMERO"
                                , "SUBALTERNO"
                                , "DATAINIZIO"
                                , "DATAFINE"
                                , "MESIPOSSESSO"
                                , "TIPOPOSSESSO"
                                , "TIPOUTILIZZO"
                                , "QUOTAPOSSESSO"
                                , "FLAGPRINCIPALE"
                                , "FLAGPERTINENZA"
                                , "TIPORENDITA"
                                , "ZONA"
                                , "CATEGORIA"
                                , "CLASSE"
                                , "VALORE"
                                , "RENDITA"
                                , "CONSISTENZA"
                                , "FLAGESENTE"
                                , "FLAGRIDUZIONE"
                                , "NUTILIZZATORI"
                                , "FLAGCOLDIR"
                                , "NFIGLIMINORI26ANNI"
                                , "NOTE"
                                , "IDSOGGETTO"
                                , "IDIMMOBILECAT"
                                , "REGIMEPOSSESSO"
                                , "AZIONE"
                            );
                        nRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", myItem.ID)
                                , ctx.GetParam("IDELABORAZIONE", myItem.IDElaborazione)
                                , ctx.GetParam("IDCATASTALE", myItem.IDCatastale)
                                , ctx.GetParam("COGNOME", myItem.Cognome)
                                , ctx.GetParam("NOME", myItem.Nome)
                                , ctx.GetParam("CODFISCALEPIVA", myItem.CodFiscalePIVA)
                                , ctx.GetParam("NUMERODICHIARAZIONE", myItem.NumeroDichiarazione)
                                , ctx.GetParam("DATADICHIARAZIONE", myItem.DataDichiarazione)
                                , ctx.GetParam("IDIMMOBILE", myItem.IDImmobile)
                                , ctx.GetParam("IDSTRADA", myItem.IDStrada)
                                , ctx.GetParam("INDIRIZZO", myItem.Indirizzo)
                                , ctx.GetParam("CIVICO", myItem.Civico)
                                , ctx.GetParam("SCALA", myItem.Scala)
                                , ctx.GetParam("PIANO", myItem.Piano)
                                , ctx.GetParam("INTERNO", myItem.Interno)
                                , ctx.GetParam("FOGLIO", myItem.Foglio)
                                , ctx.GetParam("NUMERO", myItem.Numero)
                                , ctx.GetParam("SUBALTERNO", myItem.Subalterno)
                                , ctx.GetParam("DATAINIZIO", myItem.DataInizio)
                                , ctx.GetParam("DATAFINE", myItem.DataFine)
                                , ctx.GetParam("MESIPOSSESSO", myItem.MesiPossesso)
                                , ctx.GetParam("TIPOPOSSESSO", myItem.TipoPossesso)
                                , ctx.GetParam("TIPOUTILIZZO", myItem.TipoUtilizzo)
                                , ctx.GetParam("QUOTAPOSSESSO", myItem.QuotaPossesso)
                                , ctx.GetParam("FLAGPRINCIPALE", myItem.FlagPrincipale)
                                , ctx.GetParam("FLAGPERTINENZA", myItem.FlagPertinenza)
                                , ctx.GetParam("TIPORENDITA", myItem.TipoRendita)
                                , ctx.GetParam("ZONA", myItem.Zona)
                                , ctx.GetParam("CATEGORIA", myItem.Categoria)
                                , ctx.GetParam("CLASSE", myItem.Classe)
                                , ctx.GetParam("VALORE", myItem.Valore)
                                , ctx.GetParam("RENDITA", myItem.Rendita)
                                , ctx.GetParam("CONSISTENZA", myItem.Consistenza)
                                , ctx.GetParam("FLAGESENTE", myItem.FlagEsente)
                                , ctx.GetParam("FLAGRIDUZIONE", myItem.FlagRiduzione)
                                , ctx.GetParam("NUTILIZZATORI", myItem.NUtilizzatori)
                                , ctx.GetParam("FLAGCOLDIR", myItem.FlagColDir)
                                , ctx.GetParam("NFIGLIMINORI26ANNI", myItem.NFigliMinori26Anni)
                                , ctx.GetParam("NOTE", myItem.Note)
                                , ctx.GetParam("IDSOGGETTO", myItem.IDSoggetto)
                                , ctx.GetParam("IDIMMOBILECAT", myItem.IDImmobileCat)
                                , ctx.GetParam("REGIMEPOSSESSO", myItem.RegimePossesso)
                                , ctx.GetParam("AZIONE", myItem.Azione)
                            ).First<int>();
                        if (nRet <= 0)
                        {
                            Log.Debug("Motore_Catasto.ClsManageDB.SaveDIC::errore in inserimento");
                            Log.Debug("Motore_Catasto.ClsManageDB.SaveDIC.exec prc_DICHIARAZIONI_IU @ID=" + myItem.ID.ToString() + ",@IDELABORAZIONE= " + myItem.IDElaborazione.ToString() + ", @IDCATASTALE= " + myItem.IDCatastale.ToString() + ",@COGNOME= " + myItem.Cognome.ToString() + ", @NOME= " + myItem.Nome.ToString() + ",@CODFISCALEPIVA= " + myItem.CodFiscalePIVA.ToString() + ", @NUMERODICHIARAZIONE= " + myItem.NumeroDichiarazione.ToString() + ",@DATADICHIARAZIONE= " + myItem.DataDichiarazione.ToString() + ", @IDIMMOBILE= " + myItem.IDImmobile.ToString() + ",@IDSTRADA= " + myItem.IDStrada.ToString() + ", @INDIRIZZO= " + myItem.Indirizzo.ToString() + ",@CIVICO= " + myItem.Civico.ToString() + ", @SCALA= " + myItem.Scala.ToString() + ",@PIANO= " + myItem.Piano.ToString() + ", @INTERNO= " + myItem.Interno.ToString() + ",@FOGLIO= " + myItem.Foglio.ToString() + ", @NUMERO= " + myItem.Numero.ToString() + ",@SUBALTERNO= " + myItem.Subalterno.ToString() + ", @DATAINIZIO= " + myItem.DataInizio.ToString() + ",@DATAFINE= " + myItem.DataFine.ToString() + ", @MESIPOSSESSO= " + myItem.MesiPossesso.ToString() + ",@TIPOPOSSESSO= " + myItem.TipoPossesso.ToString() + ", @TIPOUTILIZZO= " + myItem.TipoUtilizzo.ToString() + ",@QUOTAPOSSESSO= " + myItem.QuotaPossesso.ToString() + ", @FLAGPRINCIPALE= " + myItem.FlagPrincipale.ToString() + ",@FLAGPERTINENZA= " + myItem.FlagPertinenza.ToString() + ", @TIPORENDITA= " + myItem.TipoRendita.ToString() + ",@ZONA= " + myItem.Zona.ToString() + ", @CATEGORIA= " + myItem.Categoria.ToString() + ",@CLASSE= " + myItem.Classe.ToString() + ", @VALORE= " + myItem.Valore.ToString() + ",@RENDITA= " + myItem.Rendita.ToString() + ", @CONSISTENZA= " + myItem.Consistenza.ToString() + ",@FLAGESENTE= " + myItem.FlagEsente.ToString() + ", @FLAGRIDUZIONE= " + myItem.FlagRiduzione.ToString() + ",@NUTILIZZATORI= " + myItem.NUtilizzatori.ToString() + ", @FLAGCOLDIR= " + myItem.FlagColDir.ToString() + ",@NFIGLIMINORI26ANNI= " + myItem.NFigliMinori26Anni.ToString() + ", @NOTE= " + myItem.Note.ToString() + ",@IDSOGGETTO= " + myItem.IDSoggetto.ToString() + ", @IDIMMOBILECAT= " + myItem.IDImmobileCat.ToString() + ",@REGIMEPOSSESSO= " + myItem.RegimePossesso.ToString() + ", @AZIONE= " + myItem.Azione.ToString());
                            //return false;
                        }
                        else
                            Log.Debug("Motore_Catasto.ClsManageDB.SaveDIC.posizione "+x.ToString()+"/"+ListItem.Count.ToString()+" inserita con id "+nRet.ToString()+ " - IDImmobile->"+ myItem.IDImmobile.ToString()+ "|IDImmobileCat->"+ myItem.IDImmobileCat.ToString()+ "|Azione->" + myItem.Azione);
                        x++;
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.SaveDIC.errore::"+x.ToString(), ex);
                return false;
            }
        }
        public bool PuliziaFAB(string IdEnte)
        {
            int myID = 0;
            try
            {
                using (DBModel ctx = new DBModel(DBModel.Ambiente_Catasto))
                {
                    string sSQL = ctx.GetSQL("prc_PuliziaFAB","IDENTE");
                    myID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTE", IdEnte)).First<int>();
                    if (myID <= 0)
                    {
                        Log.Debug("Motore_Catasto.ClsManageDB.PuliziaFAB::errore in pulizia");
                        return false;
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.PuliziaFAB.errore::", ex);
                return false;
            }
        }
        public bool GetFABMancanti(string IdEnte)
        {
            int myID = 0;
            try
            {
                using (DBModel ctx = new DBModel(DBModel.Ambiente_Catasto))
                {
                    string sSQL = ctx.GetSQL("prc_GetFABMancanti", "IDENTE");
                    myID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTE", IdEnte)).First<int>();
                    if (myID <= 0)
                    {
                        Log.Debug("Motore_Catasto.ClsManageDB.GetFABMancanti::errore in prelievo da storico");
                        return false;
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.GetFABMancanti.errore::", ex);
                return false;
            }
        }
        public bool JoinTIT(string IdEnte)
        {
            int myID = 0;
            try
            {
                using (DBModel ctx = new DBModel(DBModel.Ambiente_Catasto))
                {
                    string sSQL = ctx.GetSQL("prc_JoinTIT", "IDENTE");
                    myID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTE", IdEnte)).First<int>();
                    if (myID <= 0)
                    {
                        Log.Debug("Motore_Catasto.ClsManageDB.JoinTIT::errore in unione con TIT");
                        return false;
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.JoinTIT.errore::", ex);
                return false;
            }
        }
        public bool JoinCatDic(string IdEnte)
        {
            int myID = 0;
            try
            {
                using (DBModel ctx = new DBModel(DBModel.Ambiente_Catasto))
                {
                    string sSQL = ctx.GetSQL("prc_JoinCatDic", "IDENTE");
                    myID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTE", IdEnte)).First<int>();
                    if (myID <= 0)
                    {
                        Log.Debug("Motore_Catasto.ClsManageDB.JoinCatDic::errore in incrocio CATWORK vs DICWORK");
                        return false;
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Motore_Catasto.ClsManageDB.JoinCatDic.errore::", ex);
                return false;
            }
        }
    }
}
