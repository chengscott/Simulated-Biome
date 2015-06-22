using System;
using System.Collections.Generic;
using System.Drawing;

namespace SB
{
    class Map
    {
        public Creatures creature;
    }

    class Creatures
    {
        public Brush Colour() { return brush_[(int)(age_ / (double)span_ * brush_.Count)]; }
        public bool Dead() { return ++age_ >= span_ || cap_ == 0; }
        public int Cap { get { return cap_; } set { cap_ += value; } } // HACK: full_
        /// <summary>
        /// br_出生率(萬分之一)age_年齡span_壽命cap_ 飽食度appt_食量full_飽食量rspan_最低繁殖年齡rcap_最低繁殖飽食度sex_性別
        /// birth rate, age, life span, capacity, appetite, full, reproductive life span, reproductive capacity, sex
        /// </summary>
        public List<Brush> brush_ = new List<Brush>();
        protected int x_, y_, br_, age_, span_, cap_, appt_, full_, rspan_, rcap_;
        protected bool sex_;
    }

    class Animals : Creatures
    {
        public void Move(ref Map[] map)
        {
            List<Tuple<int, int>> mates = new List<Tuple<int, int>>();
            List<Tuple<int, int>> food = new List<Tuple<int, int>>();
            List<Tuple<int, int>> spaces = new List<Tuple<int, int>>();
            Random rnd = new Random();
            for (int i = -radius_; i <= radius_; ++i)
            {
                int flag = (int)Math.Sqrt(radius_ * radius_ - i * i);
                for (int j = -flag; j <= flag; ++j)
                {
                    int nx = x_ + i, ny = y_ + j, v = nx + ny * CST.kW;
                    if (nx < 0 || nx >= CST.kW || ny < 0 || ny >= CST.kH || (nx == x_ && ny == y_)) continue;
                    if (map[v].creature == null) { spaces.Add(Tuple.Create(nx, ny)); continue; }
                    Type type = map[v].creature.GetType().BaseType;
                    if (type == typeof(Plants) && (vore_ == vore.carni_ || vore_ == vore.herbi_)) food.Add(Tuple.Create(nx, ny));
                    else if (type == typeof(Animals))
                    {
                        Animals animal = (Animals)map[v].creature;
                        bool cond = this.GetType() == map[v].creature.GetType();
                        bool cond1 = animal.sex_ != sex_ && age_ >= rspan_ && cap_ >= rcap_;
                        bool cond2 = animal.age_ >= animal.rspan_ && animal.cap_ >= animal.rcap_;
                        if (cond && cond1 && rnd.Next(10001) <= br_ && cond2) mates.Add(Tuple.Create(nx, ny));
                        else if ((cond && cond1 && (vore_ == vore.omni_inter_ || vore_ == vore.carni_inter_)) || (!cond && (vore_ == vore.omni_ || vore_ == vore.carni_))) food.Add(Tuple.Create(nx, ny));
                    }
                }
            }
            int idx;
            // reproduce
            if (mates.Count > 0 && spaces.Count > 0)
            {
                idx = rnd.Next(mates.Count);
                Tuple<int, int> mate = mates[idx];
                Animals animal = (Animals)map[mate.Item1 + mate.Item2 * CST.kW].creature;
                animal.cap_ -= animal.rcap_; cap_ -= rcap_;
                idx = rnd.Next(spaces.Count);
                int nx = spaces[idx].Item1, ny = spaces[idx].Item2, v = nx + ny * CST.kW;
                map[v].creature = (Creatures)Activator.CreateInstance(this.GetType(), nx, ny, rnd.Next(2) == 1);
                spaces.RemoveAt(idx);
            }
            // grow (eat)
            if (food.Count > 0 && cap_ != full_) // HACK: else
            {
                idx = rnd.Next(food.Count);
                int nx = food[idx].Item1, ny = food[idx].Item2, v = nx + ny * CST.kW;
                int cap = (map[v].creature).Cap;
                if (cap <= appt_ && cap_ + cap <= full_)
                {
                    cap_ += cap;
                    map[x_ + y_ * CST.kW].creature = null;
                    // spaces.Add(Tuple.Create(x_, y_));
                    x_ = nx; y_ = ny;
                    map[v].creature = this;
                }
                else if (cap_ + appt_ <= full_) { cap -= appt_; cap_ += appt_; }
                else { cap -= (full_ - cap_); cap_ = full_; }
            }
            //move
            else
                while (spaces.Count > 0)
                {
                    idx = rnd.Next(spaces.Count);
                    int nx = spaces[idx].Item1, ny = spaces[idx].Item2, v = nx + ny * CST.kW;
                    int cost = appt_ * (int)Math.Sqrt((nx - x_) * (nx - x_) + (ny - y_) * (ny - y_)) / radius_;
                    if (cap_ >= cost)
                    {
                        cap_ -= cost;
                        map[x_ + y_ * CST.kW].creature = null;
                        x_ = nx; y_ = ny;
                        map[v].creature = this;
                        break;
                    }
                    else spaces.RemoveAt(idx);
                }
        }
        protected int radius_;
        protected vore vore_;
        protected enum vore { omni_, carni_, herbi_, omni_inter_, carni_inter_ }
    };

    class Plants : Creatures
    {
        public virtual void Grow()
        {
            int gs = new Random().Next(appt_);
            cap_ = cap_ + gs > full_ ? full_ : cap_ + gs;
        }

        public virtual void Reproduce(ref Map[] map)
        {
            List<Tuple<int, int>> spaces = new List<Tuple<int, int>>();
            for (int i = -radius_; i <= radius_; ++i)
            {
                int flag = (int)Math.Sqrt(radius_ * radius_ - i * i);
                for (int j = -flag; j <= flag; ++j)
                {
                    int nx = x_ + i, ny = y_ + j, v = nx + ny * CST.kW;
                    if (nx < 0 || nx >= CST.kW || ny < 0 || ny >= CST.kH || (nx == x_ && ny == y_)) continue;
                    if (map[v].creature == null) spaces.Add(Tuple.Create(nx, ny));
                }
            }
            Random rnd = new Random();
            if (spaces.Count != 0 && rnd.Next(10001) <= br_ && age_ >= rspan_ && cap_ >= rcap_)
            {
                int idx = rnd.Next(spaces.Count), nx = spaces[idx].Item1, ny = spaces[idx].Item2, v = nx + ny * CST.kW;
                map[v].creature = (Creatures)Activator.CreateInstance(this.GetType(), nx, ny, rnd.Next(2) == 1);
                cap_ -= appt_;
            }
        }
        protected int radius_;
    };
}
