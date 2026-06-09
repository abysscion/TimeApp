using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Modules.TimeMachine.UI.Views
{
	public class AnalogClockArrowView : MonoBehaviour, IPointerDownHandler, IDragHandler
	{
		private RectTransform _parentRt;
		private float _dragInitialMouseAngle;
		private float _dragInitialArrowAngle;
		private float _currentAngle;

		public event Action<float> AbsoluteAngleChanged;

		public bool ShouldBeDraggable { get; set; }

		private void Awake()
		{
			_parentRt = transform.parent as RectTransform;
		}

		public void SetAngle(float angle)
		{
			_currentAngle = NormalizeAngle(angle);
			transform.localRotation = Quaternion.Euler(0, 0, -_currentAngle);
		}

		public float GetAngle() => _currentAngle;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!ShouldBeDraggable)
				return;
			_dragInitialArrowAngle = _currentAngle;
			_dragInitialMouseAngle = GetMouseAngle(eventData.position);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (!ShouldBeDraggable)
				return;
			var currentMouseAngle = GetMouseAngle(eventData.position);
			var newAngle = _dragInitialArrowAngle + Mathf.DeltaAngle(_dragInitialMouseAngle, currentMouseAngle);
			SetAngle(NormalizeAngle(newAngle));
			AbsoluteAngleChanged?.Invoke(newAngle);
		}

		private float GetMouseAngle(Vector2 screenPosition)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRt, screenPosition, null, out Vector2 localPoint);
			return NormalizeAngle(Mathf.Atan2(localPoint.x, localPoint.y) * Mathf.Rad2Deg);
		}

		private float NormalizeAngle(float angle) => (angle % 360f + 360f) % 360f;
	}
}
