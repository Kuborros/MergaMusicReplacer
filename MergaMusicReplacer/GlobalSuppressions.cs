﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Method called by mod loader", Scope = "member", Target = "~M:MergaMusicReplacer.Plugin.Awake")]
[assembly: SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "Cheapest way to idle", Scope = "member", Target = "~M:Patch.Prefix(UnityEngine.AudioClip@)")]
