//
// MaxLengthAttribute.cs
//
// Author:
//	Atsushi Enomoto <atsushi@ximian.com>
//	Marek Habersack <grendel@twistedcode.net>
//
// Copyright (C) 2008-2010 Novell Inc. http://novell.com
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#if NET_4_5
using System;
using System.ComponentModel;

namespace System.ComponentModel.DataAnnotations
{
	[AttributeUsage (AttributeTargets.Parameter|AttributeTargets.Property|AttributeTargets.Field, AllowMultiple = false)]
	public class MaxLengthAttribute : ValidationAttribute
	{
		public int MaximumLength { get; private set; }

		public MaxLengthAttribute (int maximumLength)
			: base (GetDefaultErrorMessage)
		{
			MaximumLength = maximumLength;
		}

		static string GetDefaultErrorMessage ()
		{
			return "The field {0} must be a string or an array with a maximum length of {1}.";
		}

		public override string FormatErrorMessage (string name)
		{
			return String.Format (ErrorMessageString, name, MaximumLength);
		}

		public override bool IsValid (object value)
		{
			if (value == null)
				return true;

			int len;
			if (value is string)
			{
				string str = (string) value;
				len = str.Length;
			} else if (value is Array)
			{
				Array array = (Array) value;
				if (array.Rank > 1)
					throw new InvalidOperationException ("Only one dimenstional arrays are supported.");
				len = array.GetLength(0);
			} else
				return false;

			int max = MaximumLength;

			if (max < 0)
				throw new InvalidOperationException ("The maximum length must be a nonnegative integer.");

			return len <= max;
		}
	}
}
#endif
