using R3;
using UnityEngine.InputSystem;

public static class ObserverExtensions
{
    public static Observable<InputAction.CallbackContext> OnPerformedAsObservable(this InputAction action) => Observable.FromEvent<InputAction.CallbackContext>(h => action.performed += h, h => action.performed -= h);

    public static Observable<InputAction.CallbackContext> OnCanceledAsObservable(this InputAction action) => Observable.FromEvent<InputAction.CallbackContext>(h => action.canceled += h, h => action.canceled -= h);

}