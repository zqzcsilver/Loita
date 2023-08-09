using System;
using System.Collections.Generic;

namespace Loita.Components
{
    internal abstract class ComponentBase : IComponent
    {
        public IEntity Entity { get => _entity; set => _entity = value; }
        private IEntity _entity;

        public ComponentBase(IEntity entity)
        {
            Entity = entity;
        }

        public virtual IComponent Clone(IEntity cloneEntity)
        {
            return PrimitiveClone(cloneEntity);
        }

        public virtual List<Type> GetDependComponents()
        {
            return null;
        }

        public abstract void Load();

        public virtual void OnEntityComponentsAdd(List<IComponent> entityComponents)
        {
        }

        public virtual IComponent PrimitiveClone(IEntity cloneEntity)
        {
            return (IComponent)Activator.CreateInstance(GetType(), cloneEntity);
        }

        public virtual IComponent TotallyClone(IEntity cloneEntity)
        {
            return Clone(cloneEntity);
        }

        public virtual void UnLoad()
        {
        }
    }
}