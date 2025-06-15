// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;

namespace Kalkatos
{
	public interface ISignal
	{
		event Action OnEmitted;
		void Emit ();
	}

	public interface ISignal<T> : ISignal
	{
		event Action<T> OnEmittedWithParam;
		T Value { get; set; }
		void EmitWithParam (T value);
	}
}
