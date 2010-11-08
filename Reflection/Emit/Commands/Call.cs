﻿/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Reflection.Emit.Interfaces;
using System.Reflection;
using Utilities.Reflection.Emit.Commands;
using System.Reflection.Emit;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Call command
    /// </summary>
    public class Call : ICommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ObjectCallingOn">Object calling on</param>
        /// <param name="Method">Method builder</param>
        /// <param name="MethodCalling">Method calling on the object</param>
        /// <param name="Parameters">List of parameters to send in</param>
        public Call(IMethodBuilder Method, IVariable ObjectCallingOn, MethodInfo MethodCalling, List<IVariable> Parameters)
        {
            this.ObjectCallingOn = ObjectCallingOn;
            this.MethodCalling = MethodCalling;
            this.Parameters = Parameters;
            this.MethodCallingFrom = Method;
            if (MethodCalling.ReturnType != null && MethodCalling.ReturnType != typeof(void))
            {
                TempObject = Method.CreateLocal(MethodCalling.Name + "ReturnObject", MethodCalling.ReturnType);
            }
            ObjectCallingOn.Load(Method.Generator);
            foreach (IVariable Parameter in Parameters)
            {
                Parameter.Load(Method.Generator);
            }
            if (ObjectCallingOn.Name == "this" && Method.Name == MethodCalling.Name)
            {
                Method.Generator.EmitCall(OpCodes.Call, MethodCalling, null);
                if (MethodCalling.ReturnType != null && MethodCalling.ReturnType != typeof(void))
                {
                    Method.Generator.Emit(OpCodes.Stloc, ((LocalBuilder)TempObject).Builder);
                }
                return;
            }
            Method.Generator.EmitCall(OpCodes.Callvirt, MethodCalling, null);
            if (MethodCalling.ReturnType != null && MethodCalling.ReturnType != typeof(void))
            {
                Method.Generator.Emit(OpCodes.Stloc, ((LocalBuilder)TempObject).Builder);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Temp return object from the call
        /// </summary>
        protected virtual IVariable TempObject { get; set; }

        /// <summary>
        /// Object calling on
        /// </summary>
        protected virtual IVariable ObjectCallingOn { get; set; }

        /// <summary>
        /// Method calling
        /// </summary>
        protected virtual MethodInfo MethodCalling { get; set; }

        /// <summary>
        /// Parameters sent in
        /// </summary>
        protected virtual List<IVariable> Parameters { get; set; }

        /// <summary>
        /// Method calling from
        /// </summary>
        protected virtual IMethodBuilder MethodCallingFrom { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the object that temporarily stores the new object
        /// </summary>
        /// <returns>The new object</returns>
        public virtual IVariable GetObject()
        {
            return TempObject;
        }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            if (TempObject != null)
            {
                Output.Append(TempObject).Append(" = ");
            }
            Output.Append(ObjectCallingOn).Append(".");
            if (ObjectCallingOn.Name == "this" && MethodCallingFrom.Name == MethodCalling.Name)
            {
                Output.Append("base").Append("(");
            }
            else
            {
                Output.Append(MethodCalling.Name).Append("(");
            }
            string Seperator = "";
            foreach (IVariable Variable in Parameters)
            {
                Output.Append(Seperator).Append(Variable.ToString());
                Seperator = ",";
            }
            Output.Append(");\n");
            return Output.ToString();
        }

        #endregion
    }
}