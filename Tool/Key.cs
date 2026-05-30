using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace MossLib.Tool;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class Key
{
    public static bool HasKey(string action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        if (string.IsNullOrWhiteSpace(action))
            return false;

        if (KeyBinds.binds.binds == null)
            KeyBinds.LoadBindsFromFile();

        return KeyBinds.binds.codeBinds != null
               && KeyBinds.binds.codeBinds.ContainsKey(action);
    }

    public static bool IsKey(string action)
    {
        var keyCode = ResolveKeyCode(action);
        return keyCode != KeyCode.None && Input.GetKey(keyCode);
    }

    public static bool IsKeyDown(string action)
    {
        var keyCode = ResolveKeyCode(action);
        return keyCode != KeyCode.None && Input.GetKeyDown(keyCode);
    }

    public static bool IsKeyUp(string action)
    {
        var keyCode = ResolveKeyCode(action);
        return keyCode != KeyCode.None && Input.GetKeyUp(keyCode);
    }

    private static Camera _mainCamera;
    private static bool _cameraSearched;

    public static Vector2 MouseWorldPosition()
    {
        if (!TryGetMainCamera(out var camera))
            return Vector2.zero;

        return camera.ScreenToWorldPoint(Input.mousePosition);
    }

    public static Vector2 LeftClickPosition()
    {
        return !IsKeyDown(InputAction.LeftClick) 
            ? Vector2.zero
            : MouseWorldPosition();
    }

    public static Vector2 RightClickPosition()
    {
        return !IsKeyDown(InputAction.RightClick)
            ? Vector2.zero 
            : MouseWorldPosition();
    }

    public static IEnumerator WaitForLeftClick(Action<Vector2> callback)
    {
        yield return new WaitUntil(() => IsKeyDown(InputAction.LeftClick));
        callback?.Invoke(MouseWorldPosition());
    }

    public static IEnumerator WaitForRightClick(Action<Vector2> callback)
    {
        yield return new WaitUntil(() => IsKeyDown(InputAction.RightClick));
        callback?.Invoke(MouseWorldPosition());
    }

    public static WaitForClickResult WaitForLeftClick()
    {
        return new WaitForClickResult(InputAction.LeftClick);
    }

    public static WaitForClickResult WaitForRightClick()
    {
        return new WaitForClickResult(InputAction.RightClick);
    }

    private static bool TryGetMainCamera(out Camera camera)
    {
        if (_mainCamera != null)
        {
            camera = _mainCamera;
            return true;
        }

        if (_cameraSearched)
        {
            camera = null;
            return false;
        }

        _mainCamera = Camera.main;
        _cameraSearched = true;

        if (_mainCamera != null)
        {
            camera = _mainCamera;
            return true;
        }

        camera = null;
        return false;
    }

    private static KeyCode ResolveKeyCode(string action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        if (string.IsNullOrWhiteSpace(action))
            return KeyCode.None;

        if (KeyBinds.binds.binds == null)
            KeyBinds.LoadBindsFromFile();

        return KeyBinds.binds.codeBinds != null
               && KeyBinds.binds.codeBinds.TryGetValue(action, out var keyCode)
            ? keyCode
            : KeyCode.None;
    }

    public static class InputAction
    {
        public const string LeftClick = "attack";
        public const string RightClick = "iteminteract";
    }

    public sealed class WaitForClickResult(string action) : CustomYieldInstruction
    {
        private readonly string _action = action ?? throw new ArgumentNullException(nameof(action));

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Vector2 Result { get; private set; }

        public override bool keepWaiting
        {
            get
            {
                if (field)
                    return false;

                if (!IsKeyDown(_action))
                    return true;

                Result = MouseWorldPosition();
                field = true;
                return false;
            }
        }
    }
}
