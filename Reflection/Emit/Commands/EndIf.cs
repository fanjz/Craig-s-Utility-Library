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
using Utilities.Reflection.Emit.Enums;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// End If command
    /// </summary>
    public class EndIf : ICommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Method">Method builder</param>
        /// <param name="ComparisonType">Comparison type</param>
        /// <param name="LeftHandSide">Left hand side</param>
        /// <param name="RightHandSide">Right hand side</param>
        public EndIf(IMethodBuilder Method, If IfCommand)
        {
            this.Method = Method;
            Method.Generator.MarkLabel(IfCommand.EndIfLabel);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Method builder
        /// </summary>
        protected virtual IMethodBuilder Method { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append("}\n");
            return Output.ToString();
        }

        #endregion
    }
}
