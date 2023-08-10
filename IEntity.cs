using Loita.Components;

using System;
using System.Collections.Generic;

namespace Loita
{
    internal interface IEntity
    {
        /// <summary>
        /// IEntity来源
        /// </summary>
        public object Source { get; set; }

        public Dictionary<string, List<IComponent>> CallOrder { get; }
        public Dictionary<string, Dictionary<IComponent, Delegate>> Hooks { get; }
        private static Dictionary<Type, List<IComponent>> Dependencies = new Dictionary<Type, List<IComponent>>();

        public object[] Call(string funcName, params object[] args)
        {
            if (!CallOrder.ContainsKey(funcName))
                return null;
            var co = CallOrder[funcName];
            object[] rts = new object[co.Count];
            int i = 0;
            co.ForEach(c => rts[i++] = Hooks[funcName][c].DynamicInvoke(args));
            return rts;
        }

        /// <summary>
        /// 装载组件
        /// </summary>
        /// <param name="component">等待被装载的组件</param>
        /// <returns>装载成功返回true，否则返回false</returns>
        public bool AddComponent(IComponent component)
        {
            List<IComponent> entityComponents = new List<IComponent>();
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                    if (!entityComponents.Contains(com))
                        entityComponents.Add(com);
            }
            var op = addComponent(component, entityComponents);
            if (op)
            {
                foreach (var com in entityComponents)
                {
                    com.OnEntityComponentsAdd(entityComponents);
                }
            }
            return op;
        }

        /// <summary>
        /// 装载组件
        /// </summary>
        /// <param name="component">等待被装载的组件</param>
        /// <returns>装载成功返回true，否则返回false</returns>
        private bool addComponent(IComponent component, List<IComponent> entityComponents)
        {
            var depends = component.GetDependComponents();
            bool hasAllDepend = true;
            if (depends != null)
            {
                foreach (var d in depends)
                {
                    hasAllDepend = false;
                    foreach (var c in entityComponents)
                    {
                        if (c.GetType() == d)
                        {
                            hasAllDepend = true;
                            break;
                        }
                    }
                    if (!hasAllDepend)
                        break;
                }
            }
            if (!entityComponents.Contains(component) && (depends == null || depends.Count == 0 || hasAllDepend))
            {
                component.Entity = this;
                component.Load();
                entityComponents.Add(component);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 装载组件（将自动创建组件实例并装载）
        /// </summary>
        /// <typeparam name="T">被装载组件的类型</typeparam>
        /// <returns></returns>
        public bool AddComponent<T>(params object[] args) where T : IComponent
        {
            T t = (T)Activator.CreateInstance(typeof(T), args);
            return AddComponent(t);
        }

        /// <summary>
        /// 按照依赖关系装载组件链
        /// </summary>
        /// <param name="components">等待被装载的组件组</param>
        /// <returns>装载成功返回true，否则返回false</returns>
        public bool AddComponents(List<IComponent> components)
        {
            List<IComponent> entityComponents = new List<IComponent>();
            var vComponents = new List<IComponent>(components);
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                    if (!entityComponents.Contains(com))
                        entityComponents.Add(com);
            }
            foreach (var com in components)
                if (!addComponents(com, components, entityComponents))
                    return false;

            foreach (var com in vComponents)
            {
                com.Entity = this;
                com.Load();
            }

            foreach (var com in entityComponents)
            {
                com.OnEntityComponentsAdd(entityComponents);
            }
            return true;
        }

        /// <summary>
        /// 装载组件链
        /// </summary>
        /// <param name="components">等待被装载的组件</param>
        /// <returns>装载成功返回true，否则返回false</returns>
        private bool addComponents(IComponent component, List<IComponent> components, List<IComponent> entityComponents)
        {
            if (component == null)
                return false;

            var depends = component.GetDependComponents();
            List<IComponent> dependComponents = new List<IComponent>();
            bool hasAllDepend = true;
            if (depends != null)
            {
                foreach (var d in depends)
                {
                    hasAllDepend = false;
                    foreach (var c in entityComponents)
                    {
                        if (c.GetType() == d)
                        {
                            hasAllDepend = true;
                            break;
                        }
                    }
                    if (!hasAllDepend)
                    {
                        foreach (var com in components)
                        {
                            if (com.GetType() == d)
                            {
                                dependComponents.Add(com);
                                hasAllDepend = true;
                                break;
                            }
                        }
                        if (!hasAllDepend)
                            return false;
                    }
                }
            }
            while (dependComponents.Count > 0)
            {
                if (!addComponents(component, dependComponents, entityComponents))
                    return false;
            }
            if (!entityComponents.Contains(component) && (depends == null || depends.Count == 0 || hasAllDepend))
            {
                components.Remove(component);
                entityComponents.Add(component);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 卸载组件与依赖它的组件
        /// </summary>
        /// <param name="type">被卸载组件的Type</param>
        /// <returns>卸载成功返回true，否则返回false</returns>
        public bool RemoveComponents(Type type)
        {
            List<IComponent> op = new List<IComponent>();
            foreach (var c in CallOrder.Values)
            {
                var f = c.FindAll(x => x.GetType() == type);
                foreach (var com in f)
                {
                    if (!op.Contains(com))
                        op.Add(com);
                }
            }
            return RemoveComponents(op);
        }

        /// <summary>
        /// 卸载组件与依赖它的组件
        /// </summary>
        /// <param name="type">被卸载组件的Type</param>
        /// <returns>卸载成功返回true，否则返回false</returns>
        public bool RemoveComponents(List<Type> types)
        {
            List<IComponent> op = new List<IComponent>();
            foreach (var type in types)
            {
                foreach (var c in CallOrder.Values)
                {
                    var f = c.FindAll(x => x.GetType() == type);
                    foreach (var com in f)
                    {
                        if (!op.Contains(com))
                            op.Add(com);
                    }
                }
            }
            return RemoveComponents(op);
        }

        /// <summary>
        /// 卸载组件与依赖它的组件
        /// </summary>
        /// <param name="component">被卸载组件的实例</param>
        /// <returns>卸载成功返回true，否则返回false</returns>
        public bool RemoveComponent(IComponent component)
        {
            if (component == null)
                return false;

            bool op = false;
            Dependencies.Clear();
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                {
                    com.GetDependComponents()?.ForEach(rc =>
                    {
                        if (!Dependencies.ContainsKey(rc))
                            Dependencies.Add(rc, new List<IComponent>());
                        Dependencies[rc].Add(com);
                    });
                }
            }

            var ct = component.GetType();
            foreach (var c in CallOrder.Values)
            {
                if (!op)
                    op = c.Remove(component);
                else
                    c.Remove(component);
            }
            if (op && Dependencies.ContainsKey(ct))
            {
                foreach (var c in CallOrder.Values)
                {
                    Dependencies[ct].ForEach(com =>
                    {
                        c.Remove(com);
                    });
                }
            }
            return op;
        }

        /// <summary>
        /// 卸载组件与依赖它的组件
        /// </summary>
        /// <param name="type">被卸载组件的Type</param>
        /// <returns>卸载成功返回true，否则返回false</returns>
        public bool RemoveComponents(List<IComponent> components)
        {
            bool op = false;

            Dependencies.Clear();
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                {
                    com.GetDependComponents()?.ForEach(rc =>
                    {
                        if (!Dependencies.ContainsKey(rc))
                            Dependencies.Add(rc, new List<IComponent>());
                        Dependencies[rc].Add(com);
                    });
                }
            }

            foreach (var component in components)
            {
                var ct = component.GetType();
                bool next = false;
                foreach (var c in CallOrder.Values)
                {
                    if (!op)
                        next = op = c.Remove(component);
                    else if (!next)
                        next = c.Remove(component);
                    else
                        c.Remove(component);
                }
                if (next && Dependencies.ContainsKey(ct))
                {
                    foreach (var c in CallOrder.Values)
                    {
                        Dependencies[ct].ForEach(com =>
                        {
                            c.Remove(com);
                        });
                    }
                }
            }
            return op;
        }

        /// <summary>
        /// 卸载组件与依赖它的组件
        /// </summary>
        /// <typeparam name="T">被卸载组件的类型</typeparam>
        /// <returns>卸载成功返回true，否则返回false</returns>
        public bool RemoveComponents<T>() where T : IComponent
        {
            return RemoveComponents(typeof(T));
        }

        public void RegisterHook(IComponent component, string hookName, Delegate hook)
        {
            if (!CallOrder.ContainsKey(hookName))
                CallOrder.Add(hookName, new List<IComponent>());
            if (CallOrder[hookName].Contains(component))
                return;
            CallOrder[hookName].Add(component);
            if (!Hooks.ContainsKey(hookName))
                Hooks.Add(hookName, new Dictionary<IComponent, Delegate>());
            Hooks[hookName].Add(component, hook);
        }

        /// <summary>
        /// 是否已装载组件
        /// </summary>
        /// <typeparam name="T">需要判断的组件类型</typeparam>
        /// <returns>如果已装载返回true，否则返回false</returns>
        public bool HasComponent<T>() where T : IComponent
        {
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                {
                    if (com is T)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否已装载组件
        /// </summary>
        /// <param name="type">组件的Type</param>
        /// <returns>如果已装载组件返回true，否则返回false</returns>
        public bool HasComponent(Type type)
        {
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                {
                    if (com.GetType() == type)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取第一个符合类型的组件
        /// </summary>
        /// <typeparam name="T">需要获取的组件的类型</typeparam>
        /// <returns>获取到的组件实例</returns>
        public T GetComponent<T>() where T : IComponent
        {
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                {
                    if (com is T)
                        return (T)com;
                }
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// 获取第一个符合类型的组件
        /// </summary>
        /// <param name="type">需要获取的组件的Type</param>
        /// <returns>获取到的组件的实例</returns>
        public IComponent GetComponent(Type type)
        {
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                {
                    if (com.GetType() == type)
                        return com;
                }
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// 获取符合类型的组件
        /// </summary>
        /// <param name="types">需要获取的组件的Type</param>
        /// <returns>获取到的组件的实例</returns>
        public List<IComponent> GetComponents(Type type)
        {
            List<IComponent> op = new List<IComponent>();
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                {
                    if (com.GetType() == type && !op.Contains(com))
                        op.Add(com);
                }
            }
            return op;
        }

        public List<IComponent> GetComponents()
        {
            List<IComponent> op = new List<IComponent>();
            foreach (var c in CallOrder.Values)
            {
                foreach (var com in c)
                {
                    if (!op.Contains(com))
                        op.Add(com);
                }
            }
            return op;
        }

        /// <summary>
        /// 获取符合类型的组件
        /// </summary>
        /// <param name="types">需要获取的组件的Type</param>
        /// <returns>获取到的组件的实例</returns>
        public List<IComponent> GetComponents(List<Type> types)
        {
            List<IComponent> op = new List<IComponent>();
            foreach (var type in types)
            {
                foreach (var c in CallOrder.Values)
                {
                    foreach (var com in c)
                    {
                        if (com.GetType() == type && !op.Contains(com))
                            op.Add(com);
                    }
                }
            }
            return op;
        }

        /// <summary>
        /// 克隆该IEntity与其已装载的组件
        /// </summary>
        /// <returns>克隆体</returns>
        public IEntity Clone(object source);

        /// <summary>
        /// 获得该实体的一个原始复制
        /// </summary>
        /// <returns>克隆体</returns>
        public IEntity PrimitiveClone(object source);

        /// <summary>
        /// 获得该实体的一个几乎一模一样的复制
        /// </summary>
        /// <returns>克隆体</returns>
        public IEntity TotallyClone(object source);

        public static bool ContainsKeys<TKey, TValue>(Dictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            foreach (var k in keys)
            {
                if (!dictionary.ContainsKey(k)) return false;
            }
            return true;
        }

        public static bool Contains<T>(List<T> me, List<T> list)
        {
            if (list == null || list.Count == 0) return true;
            foreach (var t in list)
            {
                if (!me.Contains(t)) return false;
            }
            return true;
        }
    }
}