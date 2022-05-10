using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts.G02
{
    public enum Team { 
        Fremen, 
        Harkonnen,
        Graben,
        None,
        VOID
    }

    public enum Unit{
        Attack, Defense
    }

    public enum Command{
        AtaqueAlNexo,
        AtaqueMelangeMayorPrio,
        AtaqueMelangeMenorPrio,
        AtaqueFactoria,
        AtaqueMayorPrio,
        AtaqueNeutral,
        AtaqueMenorPrio,
        ataqueExtractor,
        DefiendeRecurso,
        DefiendeBase,
        DefiedeFactoria,
        DefiendeExtractor,
        Patrulla,
        Festivo,
    }

    public enum Strategy
    {
        Farming,
        Defensivo,
        Ofensivo,
        Guerrilla,
        Emergencia,
        //  
        NONE
    }
    
    public enum BuyTypeUnity
    {
        Extractor,
        Exploradores,
        Destructores,
        //no comprar nada
        Ahorrar,
        Emergencia
    }

    struct Extractor {
        ExtractionUnit extractor;
        public bool extrayendo;
        LimitedAccess melange;

        public Extractor(ExtractionUnit ext) {
            extractor = ext;
            extrayendo = false;
            melange = null;
        }

        public void extrayendoRecurso(LimitedAccess melange_) {
            melange = melange_;
            extrayendo = true;
        }

        public ExtractionUnit getExtractor() {
            return extractor;
        }

        public LimitedAccess getMelange()
        {
            return melange;
        }
    }

    //=====================================================================================
    public class CasillaPrioAtaque
    {
        private Team team;
        private int influencia;
        private InfluenceCellBehaviour casilla;

        public void ActualizaAtaque(){
            team = casilla.team;
            influencia = casilla.currMiliPrio;
        }

        public CasillaPrioAtaque(InfluenceCellBehaviour other)
        {
            casilla = other;
            team = casilla.team;
            influencia = casilla.currMiliPrio;
        }

        public int CompareTo(CasillaPrioAtaque other)
        {
            int result = casilla.currMiliPrio - other.casilla.currMiliPrio;
            if (this.Equals(other) && result == 0)
                return 0;
            else return result;
        }

        public bool Equals(CasillaPrioAtaque other)
        {
            return (this.casilla.Equals(other));
        }

        public override bool Equals(object obj)
        {
            CasillaPrioAtaque other = (CasillaPrioAtaque)obj;
            return Equals(other);
        }

        public InfluenceCellBehaviour GetCasilla()
        {
            return casilla;
        }
    }

    //Clase para el comparador de CasillaPrioAtaque
    public class ComparerAtaque : IComparer<CasillaPrioAtaque>
    {
        public int Compare(CasillaPrioAtaque x, CasillaPrioAtaque y)
        {
            int result = y.GetCasilla().currMiliPrio - x.GetCasilla().currMiliPrio;
            if (this.Equals(y) && result == 0)
                return 0;
            else return result;
        }
    }

    public class CasillaPrioDefensa
    {
        private Team team;
        private int influencia;
        private InfluenceCellBehaviour casilla;

        public void ActualizaDefensa()
        {
            team = casilla.team;
            influencia = casilla.getDefPriority();
        }

        public CasillaPrioDefensa(InfluenceCellBehaviour other)
        {
            casilla = other;
            team = casilla.team;
            influencia = casilla.getDefPriority();
        }

        public int CompareTo(CasillaPrioDefensa other)
        {
            int result = casilla.currMiliPrio - other.casilla.currMiliPrio;
            if (this.Equals(other) && result == 0)
                return 0;
            else return result;
        }

        public bool Equals(CasillaPrioDefensa other)
        {
            return (this.casilla.Equals(other) && this.casilla.Equals(other));
        }

        public override bool Equals(object obj)
        {
            CasillaPrioDefensa other = (CasillaPrioDefensa)obj;
            return Equals(other);
        }

        public InfluenceCellBehaviour GetCasilla()
        {
            return casilla;
        }
    }

    public class ComparerDef : IComparer<CasillaPrioDefensa>
    {
        public int Compare(CasillaPrioDefensa x, CasillaPrioDefensa y)
        {
            int result = y.GetCasilla().getDefPriority() - x.GetCasilla().getDefPriority();
            if (this.Equals(y) && result == 0)
                return 0;
            else return result;
        }
    }
}
