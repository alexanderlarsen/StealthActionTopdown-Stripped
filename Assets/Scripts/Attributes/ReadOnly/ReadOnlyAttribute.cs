using System;
using UnityEngine;

/// <summary>
/// Disables editing on an inspector field/property.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute
{ }