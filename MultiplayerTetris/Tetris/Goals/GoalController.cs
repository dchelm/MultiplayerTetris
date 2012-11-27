using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerTetris.Tetris.Goals;

namespace MultiplayerTetris.Tetris
{
    class GoalController
    {
        private List<GoalI> completed;
        private List<GoalI> pending;
        private int concurrentGoals;
        private Windows.UI.Xaml.Controls.TextBlock[] texts;


        public GoalController(Windows.UI.Xaml.Controls.TextBlock[] texts)
        {
            this.texts = texts;
            concurrentGoals = texts.Length-1;
            this.init();
        }

        private void init()
        {
            if(completed == null || pending == null)
            {
                completed = new List<GoalI>();
                pending = new List<GoalI>();
                this.initGoals();
                updateTexts();
            }
        }

        private void initGoals()
        {
            pending.Add(new GoalLine(10,0));
            pending.Add(new GoalMin(1));
            pending.Add(new GoalCombo(3));
            pending.Add(new GoalTetris(1));
            pending.Add(new GoalLine(20, 0));
            pending.Add(new GoalApm(400,2));
            pending.Add(new GoalMin(2));
            pending.Add(new GoalLine(30, 0));
            pending.Add(new GoalCombo(4));
            pending.Add(new GoalTetris(3));
            pending.Add(new GoalMin(3));
            pending.Add(new GoalLine(30, 1));
            pending.Add(new GoalCombo(5));
            pending.Add(new GoalTetris(5));
            pending.Add(new GoalMin(4));
            pending.Add(new GoalLine(40, 1));
        }

        public void updateTexts()
        {
            int goals = Math.Min(pending.Count, concurrentGoals);
            texts[0].Text = "Goals ("+completed.Count+" out of "+(pending.Count+completed.Count)+")";
            for (int i = 0; i < this.concurrentGoals; i++)
                if(i<pending.Count)
                    texts[i+1].Text = pending[i].ToString();
                else
                    texts[i + 1].Text = "";
        }

        public void updatePending(int lines, int totalAPM, int tetris, int min, int combo)
        {
            List<GoalI> toRemove = new List<GoalI>();
            for (int i = 0; i < Math.Min(pending.Count, concurrentGoals); i++)
            {
                if (pending[i].isCompleted(lines, totalAPM, tetris, min, combo))
                    toRemove.Add(pending[i]);
            }
            foreach (GoalI g in toRemove)
            {
                pending.Remove(g);
                completed.Add(g);
            }
            if (toRemove.Count > 0)
                this.updateTexts();
        }
    }
}
