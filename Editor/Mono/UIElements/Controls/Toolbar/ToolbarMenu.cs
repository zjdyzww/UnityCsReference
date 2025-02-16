// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using Unity.Properties;
using UnityEngine.UIElements;

namespace UnityEditor.UIElements
{
    /// <summary>
    /// A drop-down menu for the toolbar. For more information, refer to [[wiki:UIE-uxml-element-ToolbarMenu|UXML element ToolbarMenu]].
    /// </summary>
    public class ToolbarMenu : TextElement, IToolbarMenuElement
    {
        internal static readonly BindingId menuProperty = nameof(menu);
        internal static readonly BindingId variantProperty = nameof(variant);

        [UnityEngine.Internal.ExcludeFromDocs, Serializable]
        public new class UxmlSerializedData : TextElement.UxmlSerializedData
        {
            public override object CreateInstance() => new ToolbarMenu();
        }

        /// <summary>
        /// Instantiates a <see cref="ToolbarMenu"/> using the data read from a UXML file.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<ToolbarMenu, UxmlTraits> {}
        /// <summary>
        /// Defines <see cref="UxmlTraits"/> for the <see cref="ToolbarMenu"/>.
        /// </summary>
        public new class UxmlTraits : TextElement.UxmlTraits {}

        /// <summary>
        /// Display styles for the menu.
        /// </summary>
        public enum Variant
        {
            /// <summary>
            /// Display the menu using the default style.
            /// </summary>
            Default,
            /// <summary>
            /// Display the menu using the popup style.
            /// </summary>
            Popup
        }

        /// <summary>
        /// The menu.
        /// </summary>
        [CreateProperty(ReadOnly = true)]
        public DropdownMenu menu { get; }
        public override string text
        {
            get { return base.text; }
            set { m_TextElement.text = value; base.text = value; }
        }

        /// <summary>
        /// USS class name of elements of this type.
        /// </summary>
        public new static readonly string ussClassName = "unity-toolbar-menu";
        /// <summary>
        /// USS class name of elements of this type, when they are displayed as popup menu.
        /// </summary>
        public static readonly string popupVariantUssClassName = ussClassName + "--popup";
        /// <summary>
        /// USS class name of text elements in elements of this type.
        /// </summary>
        public static readonly string textUssClassName = ussClassName + "__text";
        /// <summary>
        /// USS class name of arrow indicators in elements of this type.
        /// </summary>
        public static readonly string arrowUssClassName = ussClassName + "__arrow";

        private TextElement m_TextElement;
        private VisualElement m_ArrowElement;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ToolbarMenu()
        {
            Toolbar.SetToolbarStyleSheet(this);
            generateVisualContent = null;

            this.AddManipulator(new Clickable(this.ShowMenu));
            menu = new DropdownMenu();

            AddToClassList(ussClassName);

            m_TextElement = new TextElement();
            m_TextElement.AddToClassList(textUssClassName);
            m_TextElement.pickingMode = PickingMode.Ignore;
            Add(m_TextElement);

            m_ArrowElement = new VisualElement();
            m_ArrowElement.AddToClassList(arrowUssClassName);
            m_ArrowElement.pickingMode = PickingMode.Ignore;
            Add(m_ArrowElement);
        }

        Variant m_Variant;
        /// <summary>
        /// The display styles that you can use when creating menus.
        /// </summary>
        [CreateProperty]
        public Variant variant
        {
            get { return m_Variant; }
            set
            {
                var previous = m_Variant;
                m_Variant = value;
                EnableInClassList(popupVariantUssClassName, value == Variant.Popup);

                if (previous != m_Variant)
                    NotifyPropertyChanged(variantProperty);
            }
        }
    }
}
