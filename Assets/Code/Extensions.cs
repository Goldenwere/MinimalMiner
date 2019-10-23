using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinimalMiner
{
    /// <summary>
    /// Extensions to the Unity API
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Finds child components attached to gameobjects with a specified tag of a specified parent gameobject
        /// </summary>
        /// <typeparam name="T">Any class that inherits from the Component class</typeparam>
        /// <param name="parent">The parent object to search from</param>
        /// <param name="tag">The tag to search for</param>
        /// <returns>Collection of components with the provided tag</returns>
        /// <remarks>Extension provided by fafase: https://answers.unity.com/questions/893966/how-to-find-child-with-tag.html </remarks>
        public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
        {
            if (parent == null)
                throw new System.ArgumentNullException();

            if (string.IsNullOrEmpty(tag) == true)
                throw new System.ArgumentNullException();

            List<T> list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
            if (list.Count == 0)
                return null;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (!list[i].CompareTag(tag))
                    list.RemoveAt(i);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Finds a child component attached to a gameobject with a specified tag of a specified parent gameobject
        /// </summary>
        /// <typeparam name="T">Any class that inherits from the Component class</typeparam>
        /// <param name="parent">The parent object to search from</param>
        /// <param name="tag">The tag to search for</param>
        /// <returns>The component with the provided tag</returns>
        /// <remarks>Extension provided by fafase: https://answers.unity.com/questions/893966/how-to-find-child-with-tag.html </remarks>
        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
        {
            if (parent == null)
                throw new System.ArgumentNullException();

            if (string.IsNullOrEmpty(tag) == true)
                throw new System.ArgumentNullException();

            T[] list = parent.GetComponentsInChildren<T>(forceActive);
            foreach (T t in list)
            {
                if (t.CompareTag(tag))
                    return t;
            }

            return null;
        }
    }
}