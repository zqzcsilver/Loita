using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Loita.Components
{
    /// <summary>
    /// 组件接口
    /// </summary>
    internal interface IComponent
    {
        /// <summary>
        /// 在组件被装上Entity时将set该Entity
        /// </summary>
        public IEntity Entity { get; set; }

        /// <summary>
        /// 在组件被装上Entity时调用
        /// </summary>
        public void Load();

        /// <summary>
        /// 当组件被从Entity卸下时调用
        /// </summary>
        public void UnLoad();

        /// <summary>
        /// 获取前置组件
        /// </summary>
        /// <returns></returns>
        public List<Type> GetDependComponents();

        /// <summary>
        /// 当Entity组件有新增时调用
        /// </summary>
        /// <param name="entityComponents">组件们</param>
        public void OnEntityComponentsAdd(List<IComponent> entityComponents);

        /// <summary>
        /// 当Entity被克隆时组件一起跟着被克隆
        /// </summary>
        /// <returns>克隆体</returns>
        public IComponent Clone(IEntity cloneEntity);

        /// <summary>
        /// 获得该组件的一个原始复制
        /// </summary>
        /// <returns>克隆体</returns>
        public IComponent PrimitiveClone(IEntity cloneEntity);

        /// <summary>
        /// 获得该组件的一个几乎一模一样的复制
        /// </summary>
        /// <returns>克隆体</returns>
        public IComponent TotallyClone(IEntity cloneEntity);
    }
}