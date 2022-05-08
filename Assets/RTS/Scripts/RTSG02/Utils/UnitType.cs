using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts.G02
{

    public class UnitType : MonoBehaviour
    {
        [Tooltip("Due�o de esta unidad")]
        public Team team;
        [Tooltip("Tipo de unidad")]
        public Unit unit;
        [Tooltip("Cantidad de puntos de influencia de esta unidad")]
        public int influencia;
        [Tooltip("Rango de influencia de esta unidad")]
        public int rango = 0;

        //Referencia a la casilla en la que nos encontrabamos en la iteracion anterior
        private InfluenceCellBehaviour curCasilla;
        //Referencia a la casilla en la que nos encontramos en la iteracion actual
        private InfluenceCellBehaviour prevCasilla;


        //  Constructor por copia
        public UnitType(UnitType unitCopy)
        {
            team = unitCopy.team;
            unit = unitCopy.unit;
            influencia = unitCopy.influencia;
            rango = unitCopy.rango;
        }

        public Team getUnitTeam()
        {
            return team;
        }

        //Gesti�n del movimiento de las unidades para actualizar el mapa de influencia
        private void Update()
        {
            curCasilla = InfluenceGenerator.GetInstance().GetCasillaCercana(transform);

            //Ha habido cambio de casilla
            if (prevCasilla != null && curCasilla != prevCasilla)
            {
                InfluenceGenerator.GetInstance().ActualizaPrioridadAlSalir(prevCasilla, this);
                InfluenceGenerator.GetInstance().ActualizaPrioridadAlEntrar(curCasilla, this);
            }
            //Si la prevCasilla es null, significa que estamos en la primera iteraci�n del bucle
            else if (prevCasilla == null) InfluenceGenerator.GetInstance().ActualizaPrioridadAlEntrar(curCasilla, this);

            prevCasilla = curCasilla;

        }

        private void OnDestroy()
        {
            if (prevCasilla)
            {
                //Cuando se destruye esta entidad hay que quitar los valores de influencia de la misma en el mapa
                InfluenceGenerator.GetInstance().ActualizaPrioridadAlSalir(prevCasilla, this);
            }
        }
    }
}
