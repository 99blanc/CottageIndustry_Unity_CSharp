using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Component = UnityEngine.Component;

public class CharacterControl : MonoBehaviour
{
    private Character<Component> character;
    private Rigidbody2D rBody;
    private CapsuleCollider2D cCollider;
    private short currentJumpCount;
    private Vector2 moveInput;
    private bool isNearGround;

    private void Awake()
    {
        character = gameObject.GetComponentAssert<Character<Component>>();
        rBody = gameObject.GetComponentAssert<Rigidbody2D>();
        cCollider = gameObject.GetComponentAssert<CapsuleCollider2D>();
    }

    private void Start() => Init();

    public void Init()
    {
        BindInputAction(Managers.Config.actMap);
        this.FixedUpdateAsObservable().Subscribe(_ => { ApplyMovement(); ApplyFallGravity(); CheckGround(); }).AddTo(this);
    }

    private void OnMovePerformed(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext context) => moveInput = Vector2.zero;
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isNearGround && currentJumpCount == 0)
        {
            currentJumpCount = 1;
        }
        else
        {
            if (currentJumpCount >= character.stats.jumpCount.Value)
                return;

            ++currentJumpCount;
        }

        rBody.linearVelocity = new Vector2(rBody.linearVelocity.x, character.stats.jumpForce.Value);
        isNearGround = false;
    }

    private void ApplyMovement()
    {
        float targetSpeed = moveInput.x * character.stats.moveSpeed.Value;
        float currentXVelocity = rBody.linearVelocity.x;
        float lerpTime = moveInput.x != 0 ? 4f : 3.5f;
        float newXVelocity = Mathf.Lerp(currentXVelocity, targetSpeed, Time.fixedDeltaTime * lerpTime);
        rBody.linearVelocity = new Vector2(newXVelocity, rBody.linearVelocity.y);

        if (moveInput.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);
    }

    private void ApplyFallGravity()
    {
        if (rBody.linearVelocity.y < 0)
            rBody.linearVelocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.fixedDeltaTime;
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(cCollider.bounds.center, cCollider.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask(Define.Layer.GROUND));
        bool isGrounded = (hit.collider is not null && rBody.linearVelocity.y <= 0.025f) ? true : false;

        if (isGrounded)
        {
            currentJumpCount = 0;
            isNearGround = true;
        }
        else
        {
            RaycastHit2D nearHit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask(Define.Layer.GROUND));
            isNearGround = nearHit.collider is not null;
        }
    }

    private void OnDestroy()
    {
        Managers.Config.OnDestroy();
    }

    private void BindInputAction(InputActionMap map)
    {
        foreach (InputAction action in map.actions)
        {
            switch (action.name)
            {
                case Define.Input.ACTION_MOVE:
                    action.BindInputEvent(OnMovePerformed, OnMoveCanceled, this);
                    break;
                case Define.Input.ACTION_JUMP:
                    action.BindInputEvent(OnJump, this);
                    break;
            }
        }

        map.Enable();
    }

    public static void BindInputEvent(InputAction action, Action<InputAction.CallbackContext> performed, Component component) => action.OnPerformedAsObservable().Subscribe(performed).AddTo(component);

    public static void BindInputEvent(InputAction action, Action<InputAction.CallbackContext> performed, Action<InputAction.CallbackContext> canceled, Component component)
    {
        action.OnPerformedAsObservable().Subscribe(performed).AddTo(component);
        action.OnCanceledAsObservable().Subscribe(canceled).AddTo(component);
    }
}
