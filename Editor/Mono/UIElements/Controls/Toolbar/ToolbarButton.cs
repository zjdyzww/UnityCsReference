// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using UnityEngine.UIElements;

namespace UnityEditor.UIElements
{
    /// <summary>
    /// A button for the toolbar. For more information, refer to [[wiki:UIE-uxml-element-ToolbarButton|UXML element ToolbarButton]].
    /// </summary>
    public class ToolbarButton : Button
    {
        [UnityEngine.Internal.ExcludeFromDocs, Serializable]
        public new class UxmlSerializedData : Button.UxmlSerializedData
        {
            public override object CreateInstance() => new ToolbarButton();
        }

        /// <summary>
        /// Instantiates a <see cref="ToolbarButton"/> using the data read from a UXML file.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<ToolbarButton, UxmlTraits> {}
        /// <summary>
        /// Defines <see cref="UxmlTraits"/> for the <see cref="ToolbarButton"/>.
        /// </summary>
        public new class UxmlTraits : Button.UxmlTraits {}

        /// <summary>
        /// USS class name of elements of this type.
        /// </summary>
        public new static readonly string ussClassName = "unity-toolbar-button";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="clickEvent">The action to be called when the button is pressed.</param>
        public ToolbarButton(Action clickEvent) :
            base(clickEvent)
        {
            Toolbar.SetToolbarStyleSheet(this);
            RemoveFromClassList(Button.ussClassName);
            AddToClassList(ussClassName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ToolbarButton() : this(() => {})
        {
        }
    }
}
