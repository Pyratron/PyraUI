﻿using System;
using System.ComponentModel;
using Pyratron.UI.States;
using PyraUI;

namespace Pyratron.UI.Controls
{
    /// <summary>
    /// A visual UI element.
    /// </summary>
    public class Visual : Element
    {
        /// <summary>
        /// The opacity of the visual, from 0 to 1.
        /// </summary>
        public float Alpha { get; }

        /// <summary>
        /// The color of the control.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// The display state of the element.
        /// </summary>
        public Visibility Visibility;

        private Element parent;

        /// <summary>
        /// Returns if the element is visible.
        /// </summary>
        [Browsable(false)]
        public bool IsVisible => Visibility == Visibility.Visible;

        public override Element Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                // If parent is a visual, set the child visibility to the parent's.
                var visual = parent as Visual;
                if (visual != null)
                    Visibility = visual.Visibility;
            }
        }

        public Visual(Manager manager) : base(manager)
        {
            Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Draws the component.
        /// </summary>
        /// <param name="delta">Elapsed seconds since the last frame.</param>
        public virtual void Draw(float delta)
        {
            Manager.Renderer.DrawRectangle(ExtendedArea, Color.Red);
            Manager.Renderer.DrawRectangle(BorderArea, Color.Blue);
            var boxColor = Color.White;
            if (Box == Box.Block)
                boxColor = new Color(255, 255, 172);
            if (Box == Box.Inline)
                boxColor = new Color(172, 255, 255);
            Manager.Renderer.DrawRectangle(ContentArea, boxColor);

            DrawChildren(delta);
        }

        /// <summary>
        /// Draws all of the children elements.
        /// </summary>
        private void DrawChildren(float delta)
        {
            for (var i = 0; i < Elements.Count; i++)
            {
                var element = Elements[i];
                var visual = element as Visual;
                visual?.Draw(delta);
            }
        }
    }
}