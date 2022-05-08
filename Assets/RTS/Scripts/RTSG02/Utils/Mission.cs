using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts.G02
{
    public class Mission : IComparable<Mission> {

        int priority_;
        Command currMission;
        Transform target { get; set; }

        bool missionStatus = false;
        public bool misionCompletada() { return missionStatus; }

        public Strategy estrategia { get;}


        public Mission(Command command, Transform objetivo, int prio_, Strategy strategy_){
            this.currMission = command;
            this.target = objetivo;
            this.priority_ = prio_;
            this.estrategia = strategy_;
        }

        public int CompareTo(Mission other){
            int result = priority_ - other.priority_;

            if (this.currMission.Equals(other.currMission) && this.target.Equals(other.target) && result == 0)
                return 0;
            else return result;
        }

        public bool Equals(Mission other){
            return (this.currMission.Equals(other.currMission) && this.target.Equals(other.target) && this.priority_.Equals(other.priority_));
        }

        public override bool Equals(object obj)
        {
            Mission other = (Mission)obj;
            return Equals(other);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
