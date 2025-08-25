namespace DGE.Core
{
    using System.Collections.Generic;
    using DGE.Utils;
    using DGE.Utils.core;

    public class UpdateManager : SingletonPersistant<UpdateManager>
    {
        public static readonly List<IUpdatable> All = new List<IUpdatable>();

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            UpdateAll();
        }

        void UpdateAll()
        {
            for (int i = 0; i < All.Count; i++)
            {
                All[i].OnUpdate();
            }
        }

        public static void Add(IUpdatable _target)
        {
            if (All.Contains(_target)) return;
            All.Add(_target);
        }
        public static void Remove(IUpdatable _target)
        {
            if (!All.Contains(_target)) return;
            All.Remove(_target);
        }
    }
}
