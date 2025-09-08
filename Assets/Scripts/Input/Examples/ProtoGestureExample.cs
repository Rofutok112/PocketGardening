using Proto.Core;
using Proto.Input.Gestures;
using ProtoInput.Core;
using UnityEngine;

namespace ProtoInput.Examples
{
    /// <summary>
    ///     ジェスチャー認識の使用例
    ///     各種ジェスチャーの検出と処理方法を示す
    /// </summary>
    public class ProtoGestureExample : MonoBehaviour
    {
        [Header("Input Manager")] [SerializeField]
        private ProtoInputManager inputManager;

        [Header("Debug")] [SerializeField] private bool enableDebugLog = true;

        // カメラの移動範囲を定義
        [Header("Camera Move Limits")]
        [SerializeField] private float minX = -10f;
        [SerializeField] private float maxX = 10f;
        [SerializeField] private float minY = -10f;
        [SerializeField] private float maxY = 10f;
        [SerializeField] private float moveSpeed = 1f;

        private void Start()
        {
            // InputManagerが設定されていない場合、自動で探す
            if (inputManager == null)
            {
                inputManager = FindObjectOfType<ProtoInputManager>();
                if (inputManager == null)
                {
                    ProtoLogger.LogWarning("InputManager not found! GestureExample will not work.", this);
                    return;
                }
            }

            // ジェスチャーコールバックを登録
            RegisterGestureCallbacks();
        }

        private void OnDestroy()
        {
            // コールバックを解除（メモリリーク防止）
            if (inputManager != null)
            {
                inputManager.UnregisterGestureCallback(ProtoGestureType.Tap, OnTap);
                inputManager.UnregisterGestureCallback(ProtoGestureType.Swipe, OnSwipe);
                inputManager.UnregisterGestureCallback(ProtoGestureType.Pinch, OnPinch);
                inputManager.UnregisterGestureCallback(ProtoGestureType.LongPress, OnLongPress);
            }
        }

        private void RegisterGestureCallbacks()
        {
            // タップコールバック
            inputManager.RegisterGestureCallback(ProtoGestureType.Tap, OnTap);

            // スワイプコールバック
            inputManager.RegisterGestureCallback(ProtoGestureType.Swipe, OnSwipe);

            // ピンチコールバック
            inputManager.RegisterGestureCallback(ProtoGestureType.Pinch, OnPinch);

            // 長押しコールバック
            inputManager.RegisterGestureCallback(ProtoGestureType.LongPress, OnLongPress);

            if (enableDebugLog) ProtoLogger.LogGesture("Gesture callbacks registered successfully!", this);
        }

        #region Gesture Callbacks

        /// <summary>
        ///     タップ時の処理例
        /// </summary>
        private void OnTap(ProtoGestureData gestureData)
        {
            if (enableDebugLog) ProtoLogger.LogGesture($"Tap detected at position: {gestureData.position}", this);

            // 例: オブジェクトをクリック
            // GameObject clickedObject = GetObjectAtPosition(gestureData.position);
            // if (clickedObject != null)
            // {
            //     clickedObject.GetComponent<IClickable>()?.OnClick();
            // }
        }

        /// <summary>
        ///     スワイプ時の処理例
        /// </summary>
        private void OnSwipe(ProtoGestureData gestureData)
        {
            if (enableDebugLog)
                ProtoLogger.LogGesture(
                    $"Swipe detected: {gestureData.swipeDirection} at position: {gestureData.position}", this);

            //カメラの現在位置を取得
            Vector3 newPosition = Camera.main.transform.position;
            // メインカメラの移動
            switch (gestureData.swipeDirection)
            {
                case ProtoSwipeDirection.Up:
                    newPosition += Vector3.up * moveSpeed;
                    break;
                case ProtoSwipeDirection.Down:
                    newPosition -= Vector3.down * moveSpeed;
                    break;
                case ProtoSwipeDirection.Left:
                    newPosition -= Vector3.left * moveSpeed;
                    break;
                case ProtoSwipeDirection.Right:
                    newPosition += Vector3.right * moveSpeed;
                    break;
            }
            //新しい位置を範囲内に制限
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            //カメラの位置を更新
            Camera.main.transform.position = newPosition;
        }

        /// <summary>
        ///     ピンチ時の処理例
        /// </summary>
        private void OnPinch(ProtoGestureData gestureData)
        {
            if (enableDebugLog)
                ProtoLogger.LogGesture(
                    $"Pinch detected: delta={gestureData.pinchDelta}, distance={gestureData.pinchDistance}", this);

            // カメラのズーム
            if (gestureData.pinchDelta > 0)
            {
                Camera.main.orthographicSize -= gestureData.pinchDelta * 0.1f;
            }
            else
            {
                Camera.main.orthographicSize += Mathf.Abs(gestureData.pinchDelta) * 0.1f;
            }
        }

        /// <summary>
        ///     長押し時の処理例
        /// </summary>
        private void OnLongPress(ProtoGestureData gestureData)
        {
            if (enableDebugLog)
                ProtoLogger.LogGesture($"Long press detected at position: {gestureData.position}", this);

            // 例: コンテキストメニューの表示
            // ShowContextMenu(gestureData.position);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        ///     指定位置のオブジェクトを取得（例）
        /// </summary>
        private GameObject GetObjectAtPosition(Vector2 screenPosition)
        {
            var ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) return hit.collider.gameObject;

            return null;
        }

        /// <summary>
        ///     プレイヤー移動（例）
        /// </summary>
        private void MovePlayer(Vector2 direction)
        {
            // プレイヤーの移動処理
            // playerController.Move(direction);
        }

        /// <summary>
        ///     コンテキストメニュー表示（例）
        /// </summary>
        private void ShowContextMenu(Vector2 position)
        {
            // コンテキストメニューの表示処理
            // contextMenu.ShowAt(position);
        }

        #endregion
    }
}