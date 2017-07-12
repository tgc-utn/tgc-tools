using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TGC.Tools.Utils.Modifiers
{
    public class TgcModifiers
    {
        private readonly Dictionary<string, TgcModifierPanel> modifiers;
        private readonly Panel modifiersPanel;

        public TgcModifiers(Panel modifiersPanel)
        {
            this.modifiersPanel = modifiersPanel;
            modifiers = new Dictionary<string, TgcModifierPanel>();
        }

        public object this[string varName]
        {
            get { return modifiers[varName].getValue(); }
        }

        public void add(TgcModifierPanel modifier)
        {
            if (modifiersPanel.Controls.Count > 0)
            {
                modifiersPanel.Controls.RemoveAt(modifiersPanel.Controls.Count - 1);
            }

            modifiers.Add(modifier.VarName, modifier);
            modifier.MainPanel.Dock = DockStyle.Fill;
            modifiersPanel.Controls.Add(modifier.MainPanel);
        }

        public object getValue(string varName)
        {
            return modifiers[varName].getValue();
        }

        internal void variableChanged()
        {
        }

        public void clear()
        {
            modifiersPanel.Controls.Clear();
            modifiers.Clear();
        }

        #region Facilitadores de Modifiers

        /// <summary>
        ///     Modificador para valores Float
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="minValue">Valor minimo</param>
        /// <param name="maxValue">Valor maximo</param>
        /// <param name="defaultValue">Valor default</param>
        public void addFloat(string varName, float minValue, float maxValue, float defaultValue)
        {
            add(new TgcFloatModifier(varName, minValue, maxValue, defaultValue));
        }

        /// <summary>
        ///     Modificador para valores Int
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="minValue">Valor minimo</param>
        /// <param name="maxValue">Valor maximo</param>
        /// <param name="defaultValue">Valor default</param>
        public void addInt(string varName, int minValue, int maxValue, int defaultValue)
        {
            add(new TgcIntModifier(varName, minValue, maxValue, defaultValue));
        }

        /// <summary>
        ///     Modificador para valores Boolean
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="text">Descripcion</param>
        /// <param name="defaultValue">Valor default</param>
        public void addBoolean(string varName, string text, bool defaultValue)
        {
            add(new TgcBooleanModifier(varName, text, defaultValue));
        }

        /// <summary>
        ///     Modificador para elegir un color
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="defaultValue">Valor default</param>
        public void addColor(string varName, Color defaultValue)
        {
            add(new TgcColorModifier(varName, defaultValue));
        }

        /// <summary>
        ///     Modificador para un intervalo discreto de valores
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="values">Array de valores discretos</param>
        /// <param name="defaultIndex">Indice default del array</param>
        public void addInterval(string varName, object[] values, int defaultIndex)
        {
            add(new TgcIntervalModifier(varName, values, defaultIndex));
        }

        /// <summary>
        ///     Modificador para un intervalo discreto de valores creados con una estructura Enum
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="enumType">tipo del Enum a utilizar. Se obtiene con typeof(MyEnum)</param>
        /// <param name="defaultValue">variable del Enum que se carga como default. Ejemplo MyEnum.OpcionA</param>
        public void addEnum(string varName, Type enumType, object defaultValue)
        {
            add(new TgcEnumModifier(varName, enumType, defaultValue));
        }

        /// <summary>
        ///     Modificador para elegir un archivo del FileSystem
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="defaultPath">path del archivo default</param>
        /// <param name="fileFilter">
        ///     string que especifca el filtro de archivos.
        ///     Ejemplo: .Imagenes JPG|*.jpg|Archivos XML|*.xml
        ///     Enviar null si no se quiere filtro
        /// </param>
        public void addFile(string varName, string defaultPath, string fileFilter)
        {
            add(new TgcFileModifier(varName, defaultPath, fileFilter));
        }

        /// <summary>
        ///     Modificador para elegir una textura
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="values">path de la textura default</param>
        public void addTexture(string varName, string defaultPath)
        {
            add(new TgcTextureModifier(varName, defaultPath));
        }

        /// <summary>
        ///     Modificador para valores floats (X,Y,Z) de un vertice
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="minValue">Valor minimo</param>
        /// <param name="maxValue">Valor maximo</param>
        /// <param name="defaultValue">Valor default</param>
        public void addVertex3f(string varName, Vector3 minValue, Vector3 maxValue, Vector3 defaultValue)
        {
            add(new TgcVertex3fModifier(varName, minValue, maxValue, defaultValue));
        }

        /// <summary>
        ///     Modificador para valores floats (X,Y) o (U,V) de un vertice
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="minValue">Valor minimo</param>
        /// <param name="maxValue">Valor maximo</param>
        /// <param name="defaultValue">Valor default</param>
        public void addVertex2f(string varName, Vector2 minValue, Vector2 maxValue, Vector2 defaultValue)
        {
            add(new TgcVertex2fModifier(varName, minValue, maxValue, defaultValue));
        }

        /// <summary>
        ///     Modificador que agrega un Boton
        /// </summary>
        /// <param name="varName">Nombre del modificador</param>
        /// <param name="text">Descripcion</param>
        /// <param name="defaultValue">Evento para manejer el click</param>
        public void addButton(string varName, string text, EventHandler clickEventHandler)
        {
            add(new TgcButtonModifier(varName, text, clickEventHandler));
        }

        #endregion Facilitadores de Modifiers
    }
}