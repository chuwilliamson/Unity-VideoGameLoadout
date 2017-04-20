namespace Isometric
{
    using System;
    using System.Collections;

    using Mask;

    using UnityEngine;

    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down,
    }

    [RequireComponent(typeof(IsometricTransform))]
    public class IsometricCharacter : MonoBehaviour, IStateEnterHandler
    {
        private enum FlyingState
        {
            None,
            TakingOff,
            Flying,
            Landing,
        }

        [SerializeField]
        private float m_MoveTime = 1f;
        [Space, SerializeField]
        private bool m_IsFlyingUnit;

        private IsometricTransform m_IsometricTransform;
        private Animator m_Animator;

        private bool m_IsMoving;
        private bool m_IsJumping;

        private FlyingState m_FlyingState;
        private float m_CurrentAnimationLength;

        public Animator animator { get { return m_Animator; } }

        private void OnValidate()
        {
            if (m_Animator == null)
                return;

            m_Animator.SetBool("Is Flying Unit", m_IsFlyingUnit);
        }

        private void Awake()
        {
            m_IsometricTransform = GetComponent<IsometricTransform>();
            m_Animator = GetComponentInChildren<Animator>();

            OnValidate();
        }

        public void Move(Direction direction)
        {
            if (m_IsMoving)
                return;

            var cartesianDirection =
                new Vector3(
                    direction == Direction.Left || direction == Direction.Up ? -1f : 1f,
                    0f,
                    direction == Direction.Up || direction == Direction.Right ? 1f : -1f);

            var isometricDirection =
                new Vector3(
                    direction == Direction.Left || direction == Direction.Right
                        ? direction == Direction.Left ? -1f : 1f : 0f,
                    0f,
                    direction == Direction.Up || direction == Direction.Down
                        ? direction == Direction.Up ? -1f : 1f : 0f);

            var currentMask =
                IsometricMaskManager.self.GetMask(
                    m_IsometricTransform.position.x,
                    m_IsometricTransform.position.z);
            var nextMask =
                IsometricMaskManager.self.GetMask(
                    m_IsometricTransform.position.x + isometricDirection.x,
                    m_IsometricTransform.position.z + isometricDirection.z);

            if (nextMask != null && nextMask.maskType == MaskType.Standard)
                StartCoroutine(StartMoving(direction, currentMask, nextMask));

            m_Animator.SetFloat("Normal X", cartesianDirection.x);
            m_Animator.SetFloat("Normal Z", cartesianDirection.z);
        }

        public void OnStateEnter(AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_CurrentAnimationLength = stateInfo.length;
            ++m_FlyingState;
        }

        private static bool CheckForJump(
            Direction direction,
            IsometricMask currentMask,
            IsometricMask nextMask)
        {
            var currentMaskEnd = currentMask.isometricTransform.position.y;
            var nextMaskBegin = nextMask.isometricTransform.position.y;

            switch (direction)
            {
            case Direction.None:
                return false;

            case Direction.Left:
                currentMaskEnd += currentMask.longitudinalTopography.Evaluate(0f);
                nextMaskBegin += nextMask.longitudinalTopography.Evaluate(1f);
                break;
            case Direction.Right:
                currentMaskEnd += currentMask.longitudinalTopography.Evaluate(1f);
                nextMaskBegin += nextMask.longitudinalTopography.Evaluate(0f);
                break;
            case Direction.Up:
                currentMaskEnd += currentMask.lateralTopography.Evaluate(0f);
                nextMaskBegin += nextMask.lateralTopography.Evaluate(1f);
                break;
            case Direction.Down:
                currentMaskEnd += currentMask.lateralTopography.Evaluate(1f);
                nextMaskBegin += nextMask.lateralTopography.Evaluate(0f);
                break;

            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
            }

            return currentMaskEnd != nextMaskBegin;
        }

        private IEnumerator StartMoving(
            Direction direction,
            IsometricMask currentMask,
            IsometricMask nextMask)
        {
            m_IsMoving = true;

            if (CheckForJump(direction, currentMask, nextMask))
                yield return FlyTowards(direction, currentMask, nextMask);
            else
                yield return MoveTowards(direction, currentMask, nextMask);

            m_IsometricTransform.position =
                new Vector3(
                    nextMask.isometricTransform.position.x,
                    nextMask.isometricTransform.position.y + nextMask.lateralTopography.Evaluate(0.5f),
                    nextMask.isometricTransform.position.z);

            m_IsMoving = false;
        }

        private IEnumerator MoveTowards(
            Direction direction,
            IsometricMask currentMask,
            IsometricMask nextMask)
        {
            var originalPosition =
                new Vector2(
                    m_IsometricTransform.position.x,
                    m_IsometricTransform.position.z);
            var finalPosition =
                new Vector2(
                    nextMask.isometricTransform.position.x,
                    nextMask.isometricTransform.position.z);

            var deltaTime = 0f;
            while (deltaTime <= m_MoveTime)
            {
                var progress = deltaTime / m_MoveTime;

                var currentPosition = Vector2.Lerp(originalPosition, finalPosition, progress);

                float height;
                if (progress <= 0.5f)
                {
                    height = currentMask.isometricTransform.position.y;

                    if (direction == Direction.Up)
                        height += currentMask.lateralTopography.Evaluate(0.5f - progress);
                    if (direction == Direction.Down)
                        height += currentMask.lateralTopography.Evaluate(0.5f + progress);

                    if (direction == Direction.Left)
                        height += currentMask.longitudinalTopography.Evaluate(0.5f - progress);
                    if (direction == Direction.Right)
                        height += currentMask.longitudinalTopography.Evaluate(0.5f + progress);
                }
                else
                {
                    height = nextMask.isometricTransform.position.y;

                    if (direction == Direction.Up)
                        height += nextMask.lateralTopography.Evaluate(1.5f - progress);
                    if (direction == Direction.Down)
                        height += nextMask.lateralTopography.Evaluate(progress - 0.5f);

                    if (direction == Direction.Left)
                        height += nextMask.longitudinalTopography.Evaluate(1.5f - progress);
                    if (direction == Direction.Right)
                        height += nextMask.longitudinalTopography.Evaluate(progress - 0.5f);
                }

                m_IsometricTransform.position = new Vector3(currentPosition.x, height, currentPosition.y);

                deltaTime += Time.deltaTime;

                yield return null;
            }
        }

        private IEnumerator FlyTowards(Direction direction, IsometricMask currentMask, IsometricMask nextMask)
        {
            m_Animator.SetBool("Jump", true);
            m_IsJumping = true;

            m_FlyingState = FlyingState.None;
            while (m_FlyingState == FlyingState.None)
                yield return null;

            var startPosition =
                new Vector3(
                    currentMask.isometricTransform.position.x,
                    currentMask.isometricTransform.position.y + currentMask.lateralTopography.Evaluate(0.5f),
                    currentMask.isometricTransform.position.z);
            var endPosition =
                new Vector3(
                    currentMask.isometricTransform.position.x,
                    Mathf.Max(
                        currentMask.isometricTransform.position.y
                        + currentMask.lateralTopography.Evaluate(0.5f),
                        nextMask.isometricTransform.position.y
                            + nextMask.lateralTopography.Evaluate(0.5f))
                        + 1f,
                    currentMask.isometricTransform.position.z);

            var deltaTime = 0f;
            while (m_FlyingState == FlyingState.TakingOff)
            {
                m_IsometricTransform.position =
                    Vector3.Lerp(startPosition, endPosition, deltaTime / m_CurrentAnimationLength);

                deltaTime += Time.deltaTime;

                yield return null;
            }

            endPosition =
                new Vector3(
                    nextMask.isometricTransform.position.x,
                    m_IsometricTransform.position.y,
                    nextMask.isometricTransform.position.z);

            while (m_IsometricTransform.position != endPosition)
            {
                m_IsometricTransform.position =
                    Vector3.MoveTowards(m_IsometricTransform.position, endPosition, 1f * Time.deltaTime);

                yield return null;
            }

            m_Animator.SetBool("Jump", false);
            while (m_FlyingState != FlyingState.Landing)
                yield return null;

            startPosition = m_IsometricTransform.position;
            endPosition =
                new Vector3(
                    nextMask.isometricTransform.position.x,
                    nextMask.isometricTransform.position.y + nextMask.lateralTopography.Evaluate(0.5f),
                    nextMask.isometricTransform.position.z);

            deltaTime = 0f;
            while (m_FlyingState == FlyingState.Landing && deltaTime <= m_CurrentAnimationLength)
            {
                m_IsometricTransform.position =
                    Vector3.Lerp(startPosition, endPosition, deltaTime / m_CurrentAnimationLength);

                deltaTime += Time.deltaTime;

                yield return null;
            }

            m_IsJumping = false;
            m_FlyingState = FlyingState.None;
        }
    }
}
