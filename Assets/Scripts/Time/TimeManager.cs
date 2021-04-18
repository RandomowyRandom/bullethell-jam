using System;

namespace BulletHell.Time
{
    public class Timer
    {
        public event Action OnDone;

        private float _maxCooldown;
        private float _currentCooldown = 0;
        private bool _done = true;

        public float MaxCooldown { get => _maxCooldown; set => _maxCooldown = value; }
        public float CurrentCooldown => _currentCooldown;

        public Timer(float maxCooldown)
        {
            _maxCooldown = maxCooldown;
        }

        public void HandleTimer()
        {
            _currentCooldown -= UnityEngine.Time.unscaledDeltaTime;
            if(_currentCooldown <= 0 && !_done)
            {
                OnDone?.Invoke();
                _done = true;
            }
        }

        public void ForceDone()
        {
            _currentCooldown = -1;
        }

        public void ForceDoneNoEffect()
        {
            _currentCooldown = -1;
            _done = true;
        }
        
        public void Reset()
        {
            _currentCooldown = _maxCooldown;
            _done = false;
        }

        public bool IsDone()
        {
            return _done;
        }
    }
}