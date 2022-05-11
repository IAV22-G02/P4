using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace es.ucm.fdi.iav.rts.G02 {
    public class InfluenceCellBehaviour : MonoBehaviour {
        public Team team;

        [HideInInspector]
        public int currMiliPrio = 0;

        [HideInInspector]
        public int fil { get; set; }
        public int col { get; set; }
        //  Columna que representa a esta casilla en vector de mapManager

        private int defensaAzul = 0;
        private int defensaAmarilla = 0;

        private CasillaPrioAtaque casillaPrioAtq;
        private CasillaPrioDefensa casillaPrioDef;

        //UNITIES
        private List<UnitType> fremenUnits = new List<UnitType>();
        public int fremenPriority = 0;

        private List<UnitType> grabenUnits = new List<UnitType>();
        private int grabenPriority = 0;

        private List<UnitType> harkonnenUnits = new List<UnitType>();
        public int harkonnenPriority = 0;

        // Start is called before the first frame update
        void Start(){
            currMiliPrio = 0;
            team = Team.VOID;

            ChangeCellColor();

            //Datos de prioridad y ataque IComparer
            casillaPrioAtq = new CasillaPrioAtaque(this);
            casillaPrioDef = new CasillaPrioDefensa(this);
        }

        // Update is called once per frame
        void Update(){
        
        }

        public int getDefPriority(){
            if (team.Equals(Team.Harkonnen)) return defensaAzul;

            return defensaAmarilla;
        }

        private void ChangeCellColor()
        {
            Color cl = Color.red;
            switch (team)
            {
                case Team.Fremen:
                    cl = Color.yellow;
                    break;
                case Team.Harkonnen:
                    cl = Color.blue;
                    break;
                case Team.Graben:
                    cl = Color.green;
                    break;
                case Team.VOID:
                    cl = Color.white;
                    break;
                default:
                    break;
            }

            cl.a = 0.2f;
            gameObject.GetComponent<MeshRenderer>().material.color = cl;
        }


        public void UnityOnOut(UnitType unit_, int influ)
        {
            switch (unit_.team)
            {
                case Team.Fremen:
                    fremenUnits.Remove(unit_);
                    if (unit_.unit.Equals(UnitPurpose.Attack))
                    {
                        fremenPriority -= influ;
                    }
                    break;
                case Team.Graben:
                    grabenUnits.Remove(unit_);
                    if (unit_.unit.Equals(UnitPurpose.Attack))
                    {
                        grabenPriority -= influ;
                    }
                    break;
                case Team.Harkonnen:
                    harkonnenUnits.Remove(unit_);
                    if (unit_.unit.Equals(UnitPurpose.Attack))
                    {
                        harkonnenPriority -= influ;
                    }
                    break;
                default:
                    break;
            }

            ModificaInfluenciaAlSalir(unit_.team, unit_.unit, influ);
            ChangeCellColor();
        }

        public void UnidadEntraCasilla(UnitType unit_, int influ)
        {
            switch (unit_.team)
            {
                case Team.Fremen:
                    fremenUnits.Add(unit_);
                    if (unit_.unit.Equals(UnitPurpose.Attack))
                    {
                        fremenPriority += influ;
                    }
                    break;
                case Team.Graben:
                    grabenUnits.Add(unit_);
                    if (unit_.unit.Equals(UnitPurpose.Attack))
                    {
                        grabenPriority += influ;
                    }
                    break;
                case Team.Harkonnen:
                    harkonnenUnits.Add(unit_);
                    if (unit_.unit.Equals(UnitPurpose.Attack))
                    {
                        harkonnenPriority += influ;
                    }
                    break;
                default:
                    currMiliPrio = 0;
                    break;
            }
            ModificaInfluenciaAlEntrar(unit_.team, unit_.unit, influ);
            ChangeCellColor();

        }

        public CasillaPrioAtaque GetCasillaPrioMilitar()
        {
            return casillaPrioAtq;
        }

        public CasillaPrioDefensa getCasillaPrioDefensa()
        {
            return casillaPrioDef;
        }

        private Team GetMayorPrio()
        {

            if (grabenPriority == 0 && harkonnenPriority == 0 && fremenPriority == 0)
            {
                return Team.VOID;
            }
            else if (fremenPriority > harkonnenPriority && fremenPriority > grabenPriority)
            {
                return Team.Fremen;
            }
            else if (harkonnenPriority > fremenPriority && harkonnenPriority > grabenPriority)
            {
                return Team.Harkonnen;
            }
            else if (grabenPriority == 0 && harkonnenPriority == fremenPriority)
            {
                return Team.None;
            }
            else
            {
                return Team.Graben;
            }
        }

        private void ActualizaPrioridadCasilla(Team dominanUnit)
        {
            switch (dominanUnit)
            {
                case Team.Fremen:
                    currMiliPrio = fremenPriority;
                    break;
                case Team.Harkonnen:
                    currMiliPrio = harkonnenPriority;
                    break;
                case Team.Graben:
                    currMiliPrio = grabenPriority;
                    break;
                case Team.VOID:
                    currMiliPrio = 0;
                    break;
                case Team.None:
                    currMiliPrio = 0;
                    break;
                default:
                    currMiliPrio = 0;
                    break;
            }
        }

        //Actualiza la influencia de la casilla cuando ha entrado una nueva unidad
        private void ModificaInfluenciaAlEntrar(Team teamType_, UnitPurpose unit_, int infl_)
        {
            if (currMiliPrio < 0)
            {
                currMiliPrio = 0;
            }

            if (fremenPriority < 0) fremenPriority = 0;
            if (harkonnenPriority < 0) harkonnenPriority = 0;
            if (grabenPriority < 0) grabenPriority = 0;


            //Si es del mismo tipo que la casilla, la casilla es neutral o está vacía
            if (teamType_.Equals(team) || team.Equals(Team.None) || team.Equals(Team.VOID))
            {
                //Si es una unidad militar
                if (unit_.Equals(UnitPurpose.Attack))
                {
                    //La casilla es del equipo de la unidad entrante
                    team = teamType_;

                    //Actualizamos valor de la prioridadMilitar
                    ActualizaPrioridadCasilla(team);
                }
                //Si es una unidad de defensa
                else
                {
                    switch (teamType_)
                    {
                        case Team.Fremen:
                            defensaAmarilla += infl_;
                            break;
                        case Team.Harkonnen:
                            defensaAzul += infl_;
                            break;

                    }
                }

            }
            //Si no es del mismo equipo
            else if (!teamType_.Equals(team))
            {
                //es una unidad Militar
                if (unit_.Equals(UnitPurpose.Attack))
                {
                    //cogemos el team con mayor influencia en la casilla
                    team = GetMayorPrio();

                    //si la casilla esta vacia o es neutral la prioridad militar es cero
                    if (team.Equals(Team.VOID) || team.Equals(Team.None))
                    {
                        currMiliPrio = 0;
                    }
                    else ActualizaPrioridadCasilla(team);
                }
                else
                    switch (teamType_)
                    {
                        case Team.Fremen:
                            defensaAmarilla += infl_;
                            break;
                        case Team.Harkonnen:
                            defensaAzul += infl_;
                            break;

                    }
            }
        }

        //Actualiza la influencia de la casilla cuando ha salida una unidad
        private void ModificaInfluenciaAlSalir(Team teamType_, UnitPurpose unit_, int infl_)
        {
            if (currMiliPrio < 0)
            {
                currMiliPrio = 0;
            }

            if (fremenPriority < 0) fremenPriority = 0;
            if (harkonnenPriority < 0) harkonnenPriority = 0;
            if (grabenPriority < 0) grabenPriority = 0;

            // si salgo en una casilla de mi equipo o neutral
            if (teamType_.Equals(team) || team.Equals(Team.None))
            {
                //si es militar
                if (unit_.Equals(UnitPurpose.Attack))
                {
                    team = GetMayorPrio();

                    if (team.Equals(Team.None) || teamType_.Equals(Team.VOID))
                    {
                        currMiliPrio = 0;
                    }
                    else
                    {
                        ActualizaPrioridadCasilla(team);
                    }
                }
                else// si soy de defensa
                    switch (teamType_)
                    {
                        case Team.Fremen:
                            defensaAmarilla -= infl_;
                            break;
                        case Team.Harkonnen:
                            defensaAzul -= infl_;
                            break;

                    }
            }
            //si salgo de una casilla que no es de mi equipo
            else if (!teamType_.Equals(team))
            {
                //si soy de defensa
                if (!unit_.Equals(UnitPurpose.Attack))
                {
                    switch (teamType_)
                    {
                        case Team.Fremen:
                            defensaAmarilla -= infl_;
                            break;
                        case Team.Harkonnen:
                            defensaAzul -= infl_;
                            break;

                    }
                }

            }

        }
    }
}
